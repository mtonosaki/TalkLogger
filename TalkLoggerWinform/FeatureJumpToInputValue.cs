// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tono;
using Tono.GuiWinForm;
using static TalkLoggerWinform.CoreFeatureBase;

namespace TalkLoggerWinform
{
    public class FeatureJumpToInputValue : FeatureControlBridgeBase, ICloseCallback
    {
        private TextBox Box;
        private IRichPane TarPane;
        private Label ClickJump;

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
            Box.KeyDown += Box_KeyDown;
            Box.Validated += Box_Validated;

            ClickJump = (Label)GetControl("LabelTalkBarTime");
            if( ClickJump != null)
            {
                ClickJump.Click += ClickJump_Click;
            }

            Timer.AddTrigger(500, UpdateBoxValue);
        }
        public void OnClosing()
        {
            IsFormClosing = true;
        }

        private void ClickJump_Click(object sender, EventArgs e)
        {
            if (sender is Control co)
            {
                Box.Text = co.Tag.ToString();
                Jump(120);
            }
        }

        private void UpdateBoxValue()
        {
            if (!IsInput && !IsFormClosing)
            {
                try
                {
                    var cpos = Hot.TimelineParts.GetCdPos(TarPane, TarPane.GetPaneRect().LT);
                    var tartime = Hot.FirstSpeech + TimeSpan.FromSeconds(cpos.X);
                    ThreadSafe.SetTextControl(Box, lastTimeText = tartime.ToString(TimeUtil.FormatHMS));
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
            Timer.AddTrigger(20, () =>
            {
                var txt = ThreadSafe.GetTextControl(Box);
                ThreadSafe.SetSelectTextBox(Box, 0, txt.Length);
            });
        }
        private void Box_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Jump();
            }
        }
        private void Box_Validated(object sender, EventArgs e)
        {
            IsInput = false;
        }


        private string lastTimeText;

        private DateTime GetTime(string str, DateTime tartime)
        {
            var val = Math.Abs(int.Parse(str));
            var settime = tartime;
            var sgn = 0;
            if (str.StartsWith("+"))
            {
                sgn = 1;
            }
            if (str.StartsWith("-"))
            {
                sgn = -1;
            }

            if (sgn == 0)
            {
                if (str.Length <= 3)  // Change second only
                {
                    settime = new DateTime(tartime.Year, tartime.Month, tartime.Day, tartime.Hour, tartime.Minute, 0) + TimeSpan.FromSeconds(val);
                }
                else
                if (str.Length == 4)
                {
                    var M = val / 100;
                    var S = val % 100;
                    settime = new DateTime(tartime.Year, tartime.Month, tartime.Day, tartime.Hour, M, 0) + TimeSpan.FromSeconds(S);
                }
                else
                {
                    var H = val / 10000;
                    var M = val / 100 % 100;
                    var S = val % 100;
                    settime = new DateTime(tartime.Year, tartime.Month, tartime.Day, H, M, 0) + TimeSpan.FromSeconds(S);
                }
            }
            else
            {
                try
                {
                    tartime = DateTime.Parse(lastTimeText);
                    if (str.Length <= 4)  // Change second only
                    {
                        settime = new DateTime(tartime.Year, tartime.Month, tartime.Day, tartime.Hour, tartime.Minute, tartime.Second) + TimeSpan.FromSeconds(val * sgn);
                    }
                    else
                    if (str.Length == 5)
                    {
                        var M = val / 100 * sgn;
                        var S = val % 100 * sgn;
                        settime = new DateTime(tartime.Year, tartime.Month, tartime.Day, tartime.Hour, tartime.Minute, tartime.Second) + TimeSpan.FromSeconds(M * 60 + S);
                    }
                    else
                    {
                        var H = val / 10000 * sgn;
                        var M = val / 100 % 100 * sgn;
                        var S = val % 100 * sgn;
                        settime = new DateTime(tartime.Year, tartime.Month, tartime.Day, tartime.Hour, tartime.Minute, tartime.Second) + TimeSpan.FromSeconds(H * 3600 + M * 60 + S);
                    }
                }
                catch
                {
                    settime = tartime + TimeSpan.FromSeconds(val);
                }
            }
            return settime;
        }
        private DateTime GetTime(string str1, string str2, DateTime tartime)
        {
            if (str1.StartsWith("+") || str1.StartsWith("-"))
            {
                return GetTime($"{str1[0]}{Math.Abs(int.Parse(str1)):00}{int.Parse(str2):00}", tartime);
            }
            else
            {
                return GetTime($"{int.Parse(str1):00}{int.Parse(str2):00}", tartime);
            }
        }
        private DateTime GetTime(string str1, string str2, string str3, DateTime tartime)
        {
            if (str1.StartsWith("+") || str1.StartsWith("-"))
            {
                return GetTime($"{str1[0]}{Math.Abs(int.Parse(str1)):00}{int.Parse(str2):00}{int.Parse(str3):00}", tartime);
            }
            else
            {
                return GetTime($"{int.Parse(str1):00}{int.Parse(str2):00}{int.Parse(str3):00}", tartime);
            }
        }

        private void Jump(int offsetSx = 0)
        {
            Box.BackColor = BgColor;
            var line = Box.Text;

            try
            {
                var cpos = Hot.TimelineParts.GetCdPos(TarPane, TarPane.GetPaneRect().LT);
                var tartime = Hot.FirstSpeech + TimeSpan.FromSeconds(cpos.X);
                var tardate = new DateTime(tartime.Year, tartime.Month, tartime.Day);
                var cs = line.Split(':').Select(a => a.Trim()).Where(a => !string.IsNullOrEmpty(a)).ToArray();

                // Set new time text
                DateTime settime;
                switch (cs.Length)
                {
                    case 1:
                        settime = GetTime(cs[0], tartime);
                        break;
                    case 2:
                        settime = GetTime(cs[0], cs[1], tartime);
                        break;
                    case 3:
                        settime = GetTime(cs[0], cs[1], cs[2], tartime);
                        break;
                    default:
                        settime = default;
                        break;
                }

                // Scroll
                if (settime != default)
                {
                    Box.Text = lastTimeText = settime.ToString(TimeUtil.FormatHMS);
                    Box.SelectAll();

                    var spos = Hot.TimelineParts.GetScRect(TarPane, CodeRect.FromLTWH((int)(settime - Hot.FirstSpeech).TotalSeconds, 0, 0, 0));
                    Pane.Scroll = ScreenPos.FromInt(Pane.Scroll.X - spos.LT.X + offsetSx, Pane.Scroll.Y);
                    Token.Add(FeatureAutoScroll.TokenAutoScrollOFF, this);
                    Pane.Invalidate(null);
                    GetRoot().FlushFeatureTriggers();
                }
            }
            catch
            {
                Box.BackColor = Color.DarkRed;
            }
        }
    }
}
