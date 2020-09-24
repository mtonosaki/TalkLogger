// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using System;
using System.Drawing;
using System.Linq;

namespace TalkLoggerWinform
{
    public class FeatureDummyAudio : CoreFeatureBase
    {
        public override void OnInitInstance()
        {
            base.OnInitInstance();

            var fid = GetRoot().FindChildFeatures(typeof(FeatureAudioLoopback2)).FirstOrDefault().ID;

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
