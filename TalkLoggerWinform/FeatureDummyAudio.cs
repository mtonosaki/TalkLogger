// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using System;
using System.Drawing;
using System.Linq;
using Tono;

namespace TalkLoggerWinform
{
    public class FeatureDummyAudio : CoreFeatureBase
    {
        public override void OnInitInstance()
        {
            base.OnInitInstance();

            Id fid;
            if (GetRoot().FindChildFeatures(typeof(FeatureAudioLoopback2)).FirstOrDefault() is FeatureAudioLoopback2 f)
            {
                fid = f.ID;
            }
            else
            {
                fid = ID;
                Hot.AddRowID(ID.Value, 911, 42);            // Device 2 : Mic
                Hot.AddRowID(0x8000 | ID.Value, 912, 4);    // Blank Space
            }

            Timer.AddTrigger(1200, () => {
                Hot.SpeechEventQueue.Enqueue(new SpeechEvent {
                    RowID = fid.Value,
                    Action = SpeechEvent.Actions.Start,
                    TimeGenerated = DateTime.Now - TimeSpan.FromSeconds(3.5),
                    SessionID = "DUMMY-001",
                });
                Hot.SpeechEventQueue.Enqueue(new SpeechEvent {
                    RowID = fid.Value,
                    Action = SpeechEvent.Actions.SetColor,
                    TimeGenerated = DateTime.Now - TimeSpan.FromSeconds(3.5),
                    SessionID = "DUMMY-001",
                    Text = Color.FromArgb(64, Color.DarkRed).ToArgb().ToString(),
                });
                Hot.SpeechEventQueue.Enqueue(new SpeechEvent {
                    RowID = fid.Value,
                    Action = SpeechEvent.Actions.Recognizing,
                    TimeGenerated = DateTime.Now - TimeSpan.FromSeconds(3.5),
                    SessionID = "DUMMY-001",
                    Text = "こんに",
                });
                Token.Add(TokenSpeechEventQueued, this);
                GetRoot().FlushFeatureTriggers();
            });
            Timer.AddTrigger(2500, () => {
                Hot.SpeechEventQueue.Enqueue(new SpeechEvent {
                    RowID = fid.Value,
                    Action = SpeechEvent.Actions.Recognizing,
                    TimeGenerated = DateTime.Now - TimeSpan.FromSeconds(1.0),
                    SessionID = "DUMMY-001",
                    Text = "こんには。ただいま",
                });
                Token.Add(TokenSpeechEventQueued, this);
                GetRoot().FlushFeatureTriggers();
            });
            Timer.AddTrigger(5000, () => {
                Hot.SpeechEventQueue.Enqueue(new SpeechEvent {
                    RowID = fid.Value,
                    Action = SpeechEvent.Actions.Recognizing,
                    TimeGenerated = DateTime.Now - TimeSpan.FromSeconds(0.3),
                    SessionID = "DUMMY-001",
                    Text = "こんには。只今マイクのテストをしております",
                });
                Token.Add(TokenSpeechEventQueued, this);
                GetRoot().FlushFeatureTriggers();
            });
            Timer.AddTrigger(5300, () => {
                Hot.SpeechEventQueue.Enqueue(new SpeechEvent {
                    RowID = fid.Value,
                    Action = SpeechEvent.Actions.Recognized,
                    TimeGenerated = DateTime.Now,
                    SessionID = "DUMMY-001",
                    Text = @"こんには。ただいまマイクのテストをしております。",
                });
                Token.Add(TokenSpeechEventQueued, this);
                GetRoot().FlushFeatureTriggers();
            });
        }
    }
}
