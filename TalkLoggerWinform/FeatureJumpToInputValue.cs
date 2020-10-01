// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tono;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class FeatureJumpToInputValue : FeatureControlBridgeBase
    {
        private TextBox Box;
        private IRichPane TarPane;
        private DataHot Hot
        {
            get
            {
                return (DataHot)Data;
            }
        }

        private bool IsInput = false;
        private Color BgColor;
        private bool IsFormClosing = false;

        public override void OnInitInstance()
        {
            base.OnInitInstance();
            TarPane = Pane.GetPane("GuiViewMain");

            Box = (TextBox)GetControl("textBoxTime");
            BgColor = Box.BackColor;
            Box.GotFocus += Box_GotFocus;
            Box.Validated += Box_Validated;

            Box.FindForm().FormClosing += (s, e) =>
            {
                IsFormClosing = true;
            };

            Timer.AddTrigger(500, UpdateBoxValue);
        }
        private void UpdateBoxValue()
        {
            if (!IsInput && !IsFormClosing)
            {
                try
                {
                    var cpos = Hot.TimelineParts.GetCdPos(TarPane, TarPane.GetPaneRect().LT);
                    var tartime = Hot.FirstSpeech + TimeSpan.FromSeconds(cpos.X);
                    ThreadSafe.SetTextControl(Box, $"{tartime.ToString(TimeUtil.FormatHMS)}");
                }
                catch
                {
                }
            }
            if (!IsFormClosing)
            {
                Timer.AddTrigger(500, UpdateBoxValue);
            }
        }

        private void Box_GotFocus(object sender, EventArgs e)
        {
            IsInput = true;
            Box.BackColor = ColorUtil.Blend(BgColor, Color.SteelBlue);
        }
        private void Box_Validated(object sender, EventArgs e)
        {
            IsInput = false;
            Box.BackColor = BgColor;
            var line = Box.Text;
            try
            {
                var cpos = Hot.TimelineParts.GetCdPos(TarPane, TarPane.GetPaneRect().LT);
                var tartime = Hot.FirstSpeech + TimeSpan.FromSeconds(cpos.X);
                var tardate = new DateTime(tartime.Year, tartime.Month, tartime.Day);
                var cs = line.Split(':').Select(a => a.Trim()).Where(a => !string.IsNullOrEmpty(a)).ToArray();
                double sgn = 0.0;
                if (cs[0].StartsWith("+"))
                    sgn = 1.0;
                if (cs[0].StartsWith("-"))
                    sgn = -1.0;

                if (cs.Length == 1)
                {
                    var val = int.Parse(StrUtil.Mid(cs[0], 1));
                    var H = val / 10000;
                    var M = val / 100 % 100;
                    var S = val % 100;
                    var settime = (sgn == 0.0 ? tardate : tartime) + TimeSpan.FromSeconds(sgn * H * 3600 + M * 60 + S);
                    Box.Text = settime.ToString(TimeUtil.FormatHMS);
                }
                if (cs.Length == 2)
                {
                    var H = 0;
                    var M = int.Parse(cs[0]);
                    var S = int.Parse(cs[1]);
                    var settime = (sgn == 0.0 ? tardate : tartime) + TimeSpan.FromSeconds(sgn * H * 3600 + M * 60 + S);
                    Box.Text = settime.ToString(TimeUtil.FormatHMS);
                }
                if (cs.Length == 3)
                {
                    var H = int.Parse(cs[0]);
                    var M = int.Parse(cs[1]);
                    var S = int.Parse(cs[2]);
                    var settime = (sgn == 0.0 ? tardate : tartime) + TimeSpan.FromSeconds(sgn * H * 3600 + M * 60 + S);
                    Box.Text = settime.ToString(TimeUtil.FormatHMS);
                }

                Token.Add(FeatureAutoScroll.TokenAutoScrollOFF, this);
                GetRoot().FlushFeatureTriggers();
            }
            catch
            {
                Box.BackColor = Color.DarkRed;
            }
        }
    }
}
