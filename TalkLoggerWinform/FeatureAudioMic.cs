// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using System;
using System.Drawing;
using Microsoft.CognitiveServices.Speech;
using Tono;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    /// <summary>
    /// Audio Input of Azure Speech to text default device (Not working collectly)
    /// </summary>
    public class FeatureAudioMic : CoreFeatureBase, ITokenListener
    {
        public NamedId TokenTriggerID => TokenSettingsLoaded;
        private SpeechRecognizer recognizer = null;
        private string TalkID = null;

        public override void OnInitInstance()
        {
            base.OnInitInstance();

            Hot.AddRowID(ID.Value, 211, 42);   // Device 2 : Mic
            Hot.AddRowID(0x8000 | ID.Value, 212, 4);    // Blank Space
            Pane.Control.FindForm().FormClosing += FeatureAudioMic_FormClosing;
        }

        private bool isOnStarting = false;
        public override void Start(NamedId who)
        {
            base.Start(who);

            if (isOnStarting)
            {
                return;
            }

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
