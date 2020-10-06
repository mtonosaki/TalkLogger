// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Tono;

namespace TalkLoggerWinform
{
    public class FeatureDummyAudio : FeatureAudioCaptureBase
    {
        public override void ParseParameter(string param)
        {
            base.ParseParameter(param);


            var rnd = new Random(DateTime.Now.Ticks.GetHashCode());
            var lines = param.Split(',').Select(a => a.Trim()).Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
            var ms = 1200;
            var sessionid = 1;
            var col = Color.DarkRed;
            var queue = new Queue<(int ms, SpeechEvent ev)>();

            Id fid = default;
            if (lines.FirstOrDefault() is string c1)
            {
                if (c1.ToLower().StartsWith("id="))
                {
                    try
                    {
                        var val = int.Parse(StrUtil.Mid(c1, 3));
                        fid = Id.From(val);
                    }
                    catch
                    {
                    }
                    lines.RemoveAt(0);
                }
            }
            if (fid == default)
            {
                if (GetRoot().FindChildFeatures(typeof(FeatureAudioLoopback2)).FirstOrDefault() is FeatureAudioLoopback2 f)
                {
                    fid = f.ID;
                }
                else
                {
                    fid = ID;
                }
            }
            Hot.AddRowID(fid.Value, fid.Value * 10, 42);            // Device Dummy
            Hot.AddRowID(0x8000 | fid.Value, fid.Value * 10 + 1, 4);    // Blank Space

            foreach (var line in lines)
            {
                if (line.StartsWith("+"))
                {
                    try
                    {
                        var val = int.Parse(StrUtil.Mid(line, 1));
                        ms += val;
                    }
                    catch
                    {
                    }
                    continue;
                }
                if (line.StartsWith("#"))
                {
                    try
                    {
                        var argb = Convert.ToInt32(StrUtil.Mid(line, 1), 16);
                        col = Color.FromArgb(argb);
                        if (col.A == 0)
                        {
                            col = Color.FromArgb(255, col.R, col.G, col.B);
                        }
                    }
                    catch
                    {
                    }
                    continue;
                }
                queue.Enqueue((ms, new SpeechEvent
                {
                    RowID = fid.Value,
                    Action = SpeechEvent.Actions.Start,
                    TimeGenerated = DateTime.Now,
                    SessionID = $"DUMMY-{sessionid:000}",
                }));
                queue.Enqueue((ms, new SpeechEvent
                {
                    RowID = fid.Value,
                    Action = SpeechEvent.Actions.SetColor,
                    TimeGenerated = DateTime.Now,
                    SessionID = $"DUMMY-{sessionid:000}",
                    Text = col.ToArgb().ToString(),
                }));
                ms += (int)(rnd.NextDouble() * 1000 + 200);
                var len = 1;
                var buildstr = "";
                for (var i = 0; i < line.Length; i += len)
                {
                    len = (int)(rnd.NextDouble() * 5 + 1);
                    buildstr += StrUtil.Mid(line, i, len) ?? "";
                    if (!string.IsNullOrWhiteSpace(buildstr))
                    {
                        queue.Enqueue((ms, new SpeechEvent
                        {
                            RowID = fid.Value,
                            Action = SpeechEvent.Actions.Recognizing,
                            TimeGenerated = DateTime.Now,
                            SessionID = $"DUMMY-{sessionid:000}",
                            Text = buildstr,
                        }));
                        ms += (int)(rnd.NextDouble() * 300 + 100);
                    }
                }
                ms += 500;
                queue.Enqueue((ms, new SpeechEvent
                {
                    RowID = fid.Value,
                    Action = SpeechEvent.Actions.Recognized,
                    TimeGenerated = DateTime.Now,
                    SessionID = $"DUMMY-{sessionid:000}",
                    Text = buildstr,
                }));
                sessionid++;
                ms += 1000;
            }
            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                Timer.AddTrigger(item.ms, () =>
                {
                    item.ev.TimeGenerated = DateTime.Now;
                    Hot.SpeechEventQueue.Enqueue(item.ev);
                    Token.Add(TokenSpeechEventQueued, this);
                    if (Pane.Control.InvokeRequired)
                    {
                        Pane.Control.Invoke(new InvokeMethod(() =>
                        {
                            GetRoot().FlushFeatureTriggers();
                        }));
                    }
                    else
                    {
                        GetRoot().FlushFeatureTriggers();
                    }
                });
            }
        }
        delegate void InvokeMethod();
    }
}
