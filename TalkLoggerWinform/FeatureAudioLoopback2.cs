// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using NAudio.CoreAudioApi;
using NAudio.Wave;
using System.Drawing;

namespace TalkLoggerWinform
{
    public class FeatureAudioLoopback2 : FeatureNAudioBase
    {
        public override void OnInitInstance()
        {
            base.OnInitInstance();

            Hot.AddRowID(ID.Value, 201, 42);            // Device 1 : Loopback
            Hot.AddRowID(0x8000 | ID.Value, 202, 4);    // Blank Space
        }

        public override string DisplayName
        {
            get
            {
                return $"SPK";
            }
        }

        protected override Color GetBarColor()
        {
            return Color.FromArgb(64, Color.DarkGreen);
        }

        protected override MMDeviceCollection GetDeviceEndpoints()
        {
            return new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
        }

        protected override string GetTargetDeviceID()
        {
            return Hot.Setting.Device1ID;
        }

        protected override string GetTargetRecognizeLanguage()
        {
            return Hot.Setting.Device1LanguageCode;
        }

        protected override IWaveIn CreateCaptureInstance(MMDevice device)
        {
            return new WasapiLoopbackCapture(device)
            {
                ShareMode = AudioClientShareMode.Shared
            };
        }
    }
}
