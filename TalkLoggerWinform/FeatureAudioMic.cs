using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Tono;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class FeatureAudioMic : CoreFeatureBase, ITokenListener
    {
        public NamedId TokenTriggerID => TokenSettingsLoaded;
        private SpeechRecognizer recognizer = null;
        private string TalkID = null;
        private static readonly Color BarColor = Color.FromArgb(64, 160, 160, 255);

        public override void OnInitInstance()
        {
            base.OnInitInstance();

            Hot.AddRowID(ID.Value, 201, 32);   // Device 2 : Mic
            Pane.Control.FindForm().FormClosing += FeatureAudioMic_FormClosing;
        }
        bool isOnStarting = false;
        public override void Start(NamedId who)
        {
            base.Start(who);
            if (isOnStarting) return;

            isOnStarting = true;

            try
            {
                recognizer?.Dispose();

                var spconfig = SpeechConfig.FromSubscription(Hot.Setting.SubscriptionKey, Hot.Setting.ServiceRegion);
                spconfig.SpeechRecognitionLanguage = "ja-JP";
                recognizer = new SpeechRecognizer(spconfig);
                recognizer.Recognizing += OnRecognizing;
                recognizer.Recognized += OnRecognized;
                recognizer.Canceled += OnCancel;

                _ = recognizer.StartContinuousRecognitionAsync();   // No control task pool
            }
            catch (Exception ex)
            {
                LOG.WriteLineException(ex);
            }
            finally
            {
                isOnStarting = false;
            }
        }

        private void FeatureAudioMic_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            recognizer?.Dispose();
            recognizer = null;
        }

        private void OnRecognizing(object sender, SpeechRecognitionEventArgs e)
        {
            LOG.WriteLine(LLV.DEV, $"Mic.R'ing : {e.Result.Text}");
            if (TalkID == null)
            {
                TalkID = Guid.NewGuid().ToString();
                Hot.SpeechEventQueue.Enqueue(new SpeechEvent {
                    RowID = ID.Value,
                    Action = SpeechEvent.Actions.Start,
                    TimeGenerated = DateTime.Now,
                    SessionID = TalkID,
                });
                Hot.SpeechEventQueue.Enqueue(new SpeechEvent {
                    RowID = ID.Value,
                    Action = SpeechEvent.Actions.SetColor,
                    TimeGenerated = DateTime.Now,
                    SessionID = TalkID,
                    Text = Color.FromArgb(64, Color.DarkCyan).ToArgb().ToString(),
                });
            }

            Hot.SpeechEventQueue.Enqueue(new SpeechEvent {
                RowID = ID.Value,
                Action = SpeechEvent.Actions.Recognizing,
                TimeGenerated = DateTime.Now,
                SessionID = TalkID,
                Text = e.Result.Text,
            });
            Token.Add(TokenSpeechEventQueued, this);
            GetRoot().FlushFeatureTriggers();
        }

        private void OnRecognized(object sender, SpeechRecognitionEventArgs e)
        {
            LOG.WriteLine(LLV.DEV, $"Mic.R'ed : {e.Result.Text}");
            Hot.SpeechEventQueue.Enqueue(new SpeechEvent {
                RowID = ID.Value,
                Action = SpeechEvent.Actions.Recognized,
                TimeGenerated = DateTime.Now,
                SessionID = TalkID,
                Text = e.Result.Text,
            });
            TalkID = null;
            Token.Add(TokenSpeechEventQueued, this);
            GetRoot().FlushFeatureTriggers();
        }
        private void OnCancel(object sender, SpeechRecognitionEventArgs e)
        {
            LOG.WriteLine(LLV.DEV, $"Mic.OnCancel : {e.Result.Text}");
            Hot.SpeechEventQueue.Enqueue(new SpeechEvent {
                RowID = ID.Value,
                Action = SpeechEvent.Actions.Canceled,
                TimeGenerated = DateTime.Now,
                SessionID = TalkID,
                Text = e.Result.Text,
            });
            TalkID = null;
            Token.Add(TokenSpeechEventQueued, this);
            GetRoot().FlushFeatureTriggers();
        }
    }
}
