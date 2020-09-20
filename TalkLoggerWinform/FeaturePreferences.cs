using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tono;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class FeaturePreferences : CoreFeatureBase
    {
        public override void OnInitInstance()
        {
            base.OnInitInstance();
            Pane.Control.FindForm().FormClosing += Application_FormClosing;
            Hot.Setting = LoadSetting();
            Token.Add(TokenSettingsLoaded, this);
        }

        private void Application_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSetting(Hot.Setting);
        }

        public static void SaveSetting(SettingModel setting)
        {
            var json = JsonSerializer.Serialize(setting);
            var enc = new Encrypt();
            var sec = enc.Encode(json);
            ConfigRegister.Current["Settings"] = sec;
        }

        public static SettingModel LoadSetting()
        {
            var sec = (string)ConfigRegister.Current["Settings", ""];
            if (sec != "")
            {
                var enc = new Encrypt();
                var json = enc.Decode(sec);
                var ret = JsonSerializer.Deserialize<SettingModel>(json);
                return ret;
            }
            else
            {
                return new SettingModel();
            }
        }

        public override void Start(NamedId who)
        {
            base.Start(who);

            var fo = new FormPreferences
            {
                Setting = Hot.Setting,
            };
            if (fo.ShowDialog(Pane.Control) == DialogResult.OK)
            {
                Hot.Setting = fo.Setting;
                SaveSetting(Hot.Setting);
            }
        }
    }
}
