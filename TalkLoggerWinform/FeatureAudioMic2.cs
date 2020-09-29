// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using System.CodeDom;
using System.Drawing;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace TalkLoggerWinform
{
    public class FeatureAudioMic2 : FeatureAudioCaptureBase
    {
        public override void OnInitInstance()
        {
            base.OnInitInstance();

            Hot.AddRowID(ID.Value, 211, 42);            // Device 2 : Mic
            Hot.AddRowID(0x8000 | ID.Value, 212, 4);    // Blank Space
        }
        protected override Color GetBarColor() => Color.FromArgb(64, Color.DarkCyan);

        protected override MMDeviceCollection GetDeviceEndpoints()
        {
            return new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
        }

        protected override string GetTargetDeviceID()
        {
            return Hot.Setting.Device2ID;
        }

        protected override string GetTargetRecognizeLanguage()
        {
            return Hot.Setting.Device2LanguageCode;
        }

        protected override IWaveIn CreateCaptureInstance(MMDevice device)
        {
            return new WasapiCapture(device)
            {
                ShareMode = AudioClientShareMode.Shared
            };
        }
    }
}
