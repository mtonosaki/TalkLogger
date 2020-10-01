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

        private bool isStarting = false;
        public override void Start(NamedId who)
        {
            base.Start(who);

            if (isStarting == false)
            {
                isStarting = true;
                Btn_Click(this, EventArgs.Empty);
                isStarting = false;
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Hot.IsPlaying = !Hot.IsPlaying;
            Btn.Checked = Hot.IsPlaying;

            if (Hot.IsPlaying)
            {
                LOG.WriteLine(LLV.INF, $"STARTED at {DateTime.Now.ToString(TimeUtil.FormatYMDHMSms)}");
                Token.Add(TokenStart, this);
            }
            else
            {
                LOG.WriteLine(LLV.INF, $"STOPPED at {DateTime.Now.ToString(TimeUtil.FormatYMDHMSms)}");
                Token.Add(TokenStop, this);
            }
            if (isStarting == false)
            {
                GetRoot().FlushFeatureTriggers();
            }
        }
    }
}
