// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using System;
using System.Windows.Forms;
using Tono;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class FeaturePlayButton : FeatureControlBridgeBase
    {
        public static readonly NamedId TokenStart = NamedId.FromName("TokenStart");
        public static readonly NamedId TokenStop = NamedId.FromName("TokenStop");

        private CheckBox Btn;
        private DataHot Hot
        {
            get
            {
                return (DataHot)base.Data;
            }
        }

        public override void OnInitInstance()
        {
            base.OnInitInstance();

            Btn = GetControl("checkBoxStart") as CheckBox;
            Btn.Click += Btn_Click;
        }

        public override void Start(NamedId who)
        {
            base.Start(who);

            Btn_Click(this, EventArgs.Empty);
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Hot.IsPlaying = !Hot.IsPlaying;
            Btn.Checked = Hot.IsPlaying;

            if (Hot.IsPlaying)
            {
                LOG.WriteLine(LLV.INF, $"START at {DateTime.Now.ToString(TimeUtil.FormatYMDHMSms)}");
                Token.Add(TokenStart, this);
            }
            else
            {
                LOG.WriteLine(LLV.INF, $"Requested to STOP at {DateTime.Now.ToString(TimeUtil.FormatYMDHMSms)}");
                GetRoot().SetUrgentToken(TokenStop, TokenStop, null);
            }
        }
    }
}
