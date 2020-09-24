// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using System.Drawing;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace TalkLoggerWinform
{
    public class FeatureAudioLoopback2 : FeatureAudioCaptureBase
    {
        public override void OnInitInstance()
        {
            base.OnInitInstance();

            Hot.AddRowID(ID.Value, 201, 42);            // Device 1 : Loopback
            Hot.AddRowID(0x8000 | ID.Value, 202, 4);    // Blank Space
        }

        protected override Color GetBarColor() => Color.FromArgb(64, Color.DarkGreen);

        protected override MMDeviceCollection GetDeviceEndpoints()
        {
            return new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
        }

        protected override string GetTargetDeviceID()
        {
            return Hot.Setting.Device1ID;
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
