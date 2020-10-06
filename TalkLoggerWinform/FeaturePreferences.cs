// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using System.Text.Json;
using System.Windows.Forms;
using Tono;
using Tono.GuiWinForm;
using static TalkLoggerWinform.CoreFeatureBase;

namespace TalkLoggerWinform
{
    public class FeaturePreferences : CoreFeatureBase, ICloseCallback
    {
        public override void OnInitInstance()
        {
            base.OnInitInstance();

            Hot.Setting = LoadSetting();
            Token.Add(TokenSettingsLoaded, this);
        }

        public void OnClosing()
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
                ret = Migration(ret);
                return ret;
            }
            else
            {
                return new SettingModel();
            }
        }
        public static SettingModel Migration(SettingModel value)
        {
            if (value.Device1LanguageCode == null)
            {
                value.Device1LanguageCode = "ja-JP";
            }
            if (value.Device2LanguageCode == null)
            {
                value.Device2LanguageCode = "ja-JP";
            }
            return value;
        }

        public override void Start(NamedId who)
        {
            base.Start(who);

            Enabled = false;    // Considering Re-entering by FlushToken in timer trigger.

            var fo = new FormPreferences
            {
                Setting = Hot.Setting,
                IsPlaying = Hot.IsPlaying,
            };
            if (fo.ShowDialog(Pane.Control) == DialogResult.OK)
            {
                Hot.Setting = fo.Setting;
                SaveSetting(Hot.Setting);
                Token.Add(TokenSettingsLoaded, this);
            }
            Enabled = true;
        }
    }
}
