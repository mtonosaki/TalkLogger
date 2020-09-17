using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Tono.Gui.Uwp;
using Windows.Devices.Enumeration;
using Windows.Media.Audio;
using Windows.Media.Devices;
using Windows.Media.Render;
using Windows.UI.Xaml.Controls;

namespace TalkLoggerUwp
{
    /// <summary>
    /// Capture Speeker
    /// </summary>
    public class FeatureSpeechCaptureFromSpeaker : FeatureCommonBase
    {
        private ComboBox cbDevices;
        private AudioGraph AudioGraph;

        public override void OnInitialInstance()
        {
            cbDevices = ControlUtil.FindControl(View, "DeviceSelector") as ComboBox;
            cbDevices.SelectionChanged += CbDevices_SelectionChanged;
            Debug.Assert(cbDevices != null, "Put a ComboBox named 'DeviceSelector' on your GuiView");
            Pane.Target = Pane.Main;
            _ = MakeDeviceList();   // uncontrollable task management : only a few thread 
        }

        public class SpeakerDevice
        {
            public DeviceInformation Device { get; set; }
            public override string ToString()
            {
                return $"{Device?.Name ?? "(null)" ?? "(n/a)"}";
            }
        }

        private async Task MakeDeviceList()
        {
            try
            {
                // SELECT A AUDIO DEVICE
                var devices = await DeviceInformation.FindAllAsync(MediaDevice.GetAudioRenderSelector());

                var deviceList = devices.Where(a => a.IsEnabled).Select(a => new SpeakerDevice
                {
                    Device = a,
                }).ToList();
                foreach (var sd in deviceList)
                {
                    cbDevices.Items.Add(sd);
                }
                cbDevices.SelectedItem = deviceList.Where(a => a.Device.IsDefault).FirstOrDefault() ?? deviceList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                LOG.AddException(ex);
            }
        }

        private async void CbDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await SelectDevice((SpeakerDevice)cbDevices.SelectedItem);
        }

        private async Task SelectDevice(SpeakerDevice sd)
        {
            if(AudioGraph != null)
            {
                AudioGraph.Dispose();
                AudioGraph = null;
            }
            var settings = new AudioGraphSettings(AudioRenderCategory.Media);
            settings.QuantumSizeSelectionMode = QuantumSizeSelectionMode.LowestLatency;
            settings.PrimaryRenderDevice = sd.Device;

            var result = await AudioGraph.CreateAsync(settings);
            if (result.Status == AudioGraphCreationStatus.Success)
            {
                AudioGraph = result.Graph;
                LOG.AddMes(LLV.INF, "Mes-001"); // Graph successfully created
            }
            else
            {
                LOG.AddMes(LLV.INF, "Error-001");   // AudioGraph Creation Error below.
                LOG.WriteLine(LLV.ERR, result.Status.ToString());
            }
        }
    }
}
