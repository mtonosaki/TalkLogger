// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using System;
using System.Linq;
using System.Windows.Forms;
using NAudio.CoreAudioApi;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public partial class FormPreferences : Form
    {
        public FormPreferences()
        {
            InitializeComponent();

            Mes.Current.ResetText(this);

            // Loopback Device
            var lds = new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            comboBoxLoopbackDevice.Items.AddRange(lds.ToArray());

            // Mic Device
            var cds = new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
            comboBoxMicDevice.Items.AddRange(cds.ToArray());
        }

        public bool IsPlaying
        {
            set
            {
                var sw = !value;
                comboBoxLoopbackDevice.Enabled = sw;
                comboBoxMicDevice.Enabled = sw;
                textBoxServiceRegion.Enabled = sw;
                textBoxSubscriptionKey.Enabled = sw;
                buttonOK.Enabled = sw;
                LabelWarning.Visible = !sw;
            }
        }

        public SettingModel Setting
        {
            get
            {
                var loopbackDeviceID = "";
                if (comboBoxLoopbackDevice.Items.Count > 0)
                {
                    loopbackDeviceID = ((MMDevice)comboBoxLoopbackDevice.Items[comboBoxLoopbackDevice.SelectedIndex]).ID;
                }
                var micDeviceID = "";
                if (comboBoxMicDevice.Items.Count > 0)
                {
                    micDeviceID = ((MMDevice)comboBoxMicDevice.Items[comboBoxMicDevice.SelectedIndex]).ID;
                }
                return new SettingModel
                {
                    SubscriptionKey = textBoxSubscriptionKey.Text,
                    ServiceRegion = textBoxServiceRegion.Text,
                    Device1ID = loopbackDeviceID,
                    Device2ID = micDeviceID,
                };
            }
            set
            {
                textBoxSubscriptionKey.Text = value.SubscriptionKey;
                textBoxServiceRegion.Text = value.ServiceRegion;

                int i;
                // Device Loopback
                for (i = comboBoxLoopbackDevice.Items.Count - 1; i >= 0; i--)
                {
                    var d = (MMDevice)comboBoxLoopbackDevice.Items[i];
                    if (d.ID == value.Device1ID)
                    {
                        comboBoxLoopbackDevice.SelectedIndex = i;
                        break;
                    }
                }
                if (i < 0)
                {
                    if (comboBoxLoopbackDevice.Items.Count > 0)
                    {
                        comboBoxLoopbackDevice.SelectedIndex = 0;
                    }
                }

                // Device Mic
                for (i = comboBoxMicDevice.Items.Count - 1; i >= 0; i--)
                {
                    var d = (MMDevice)comboBoxMicDevice.Items[i];
                    if (d.ID == value.Device2ID)
                    {
                        comboBoxMicDevice.SelectedIndex = i;
                        break;
                    }
                }
                if (i < 0)
                {
                    if (comboBoxMicDevice.Items.Count > 0)
                    {
                        comboBoxMicDevice.SelectedIndex = 0;
                    }
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
