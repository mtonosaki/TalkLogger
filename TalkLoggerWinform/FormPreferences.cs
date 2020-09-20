using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tono;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public partial class FormPreferences : Form
    {
        public SettingModel Setting
        {
            get
            {
                var deviceID1 = "";
                if(comboBoxDevice1.Items.Count > 0)
                {
                    deviceID1 = ((MMDevice)comboBoxDevice1.Items[comboBoxDevice1.SelectedIndex]).ID;
                }
                return new SettingModel
                {
                    SubscriptionKey = textBoxSubscriptionKey.Text,
                    ServiceRegion = textBoxServiceRegion.Text,
                    Device1ID =  deviceID1,
                    Device2ID = "TODO",
                };
            }
            set
            {
                textBoxSubscriptionKey.Text = value.SubscriptionKey;
                textBoxServiceRegion.Text = value.ServiceRegion;

                int i;
                for (i = comboBoxDevice1.Items.Count - 1; i >= 0; i--)
                {
                    var d = (MMDevice)comboBoxDevice1.Items[i];
                    if (d.ID == value.Device1ID)
                    {
                        comboBoxDevice1.SelectedIndex = i;
                        break;
                    }
                }
                if (i < 0)
                {
                    if (comboBoxDevice1.Items.Count > 0)
                    {
                        comboBoxDevice1.SelectedIndex = 0;
                    }
                }
            }
        }
        public FormPreferences()
        {
            InitializeComponent();

            Mes.Current.ResetText(this);

            // Loopback Device
            var devices = new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            comboBoxDevice1.Items.AddRange(devices.ToArray());
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
