// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class FeaturePlayVisializer : CoreFeatureBase
    {
        private PartsMask _parts;

        public override void OnInitInstance()
        {
            base.OnInitInstance();
            TarPane = Pane.GetPane("GuiViewMain");

            Parts.Add(TarPane, _parts = new PartsMask {
                Hot = Hot,
                IsMasking = true,
                PartsPositioner = TalkPositioner,
                PartsPositionCorder = TalkPosCoder,
            }, 65535);
            Timer.AddTrigger(500, PlayingMonitor);
        }
        private void PlayingMonitor()
        {
            try
            {
                _parts.IsMasking = !Hot.IsPlaying;
            }
            finally
            {
                Timer.AddTrigger(500, PlayingMonitor);
            }
        }
        private class PartsMask : PartsBase
        {
            public DataHot Hot { get; set; }
            public bool IsMasking { get; set; }

            private Font _font = new Font("Tahoma", 8.0f, FontStyle.Bold);
            private int _cnt = 0;

            public override bool Draw(IRichPane rp)
            {
                if (IsMasking)
                {
                    var sr = rp.GetPaneRect();
                    rp.Graphics.DrawString("PAUSE", _font, Brushes.DarkGray, sr.LT.X + 48, sr.RB.Y - 42);
                }
                else
                {
                    // Left side RED curtain
                    var sr = rp.GetPaneRect();
                    var W = 240;
                    var rc1 = ScreenRect.FromLTWH(sr.LT.X, sr.LT.Y, W, sr.Height);
                    rp.Graphics.FillRectangle(new LinearGradientBrush(new Point(0, 0), new PointF(W, 0), Color.FromArgb(128, 255, 0, 0), Color.FromArgb(0, 255, 0, 0)), rc1);

                    // Now pointer RED curtain
                    var now = DateTime.Now;
                    now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
                    var rc2 = GetScRect(rp, CodeRect.FromLTRB((int)(now - Hot.FirstSpeech).TotalSeconds, 0, 0, 0));
                    rc2 = ScreenRect.FromLTRB(rc2.LT.X, sr.LT.Y, sr.RB.X, sr.RB.Y);
                    rp.Graphics.FillRectangle(new LinearGradientBrush(new Point(rc2.LT.X, rc2.LT.Y), new PointF(rc2.RB.X, rc2.RB.Y), Color.FromArgb(8, 255, 0, 0), Color.FromArgb(96, 255, 0, 0)), rc2);

                    // REC LABEL
                    if (++_cnt % 2 == 0)
                    {
                        rp.Graphics.DrawString("REC", _font, Brushes.Yellow, sr.LT.X + 48, sr.RB.Y - 42);
                    }
                }
                return true;
            }
        }
    }
}
