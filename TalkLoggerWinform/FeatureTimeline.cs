using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tono;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class FeatureTimeline : CoreFeatureBase
    {
        const int ROWID_TIMELINE = -1;

        public override void OnInitInstance()
        {
            base.OnInitInstance();

            TarPane = Pane.GetPane("Resource");

            Hot.AddRowID( ROWID_TIMELINE, orderNo:100, layoutHeight:24);  // Set Timeline Height
            var pts = new PartsTimeline {
                Hot = Hot,
                Rect = CodeRect.FromLTRB(0, ROWID_TIMELINE, 0, ROWID_TIMELINE),
                PartsPositioner = TalkPositioner,
                PartsPositionCorder = TalkPosCoder,
            };
            Parts.Add(TarPane, pts);

        }
    }

    public class PartsTimeline : PartsBase
    {
        public DataHot Hot { get; set; }
        static readonly Font FontTime = new Font("Tahoma", 8.0f);
        static readonly Font FontSec = new Font("Tahoma", 7.0f);
        public override bool Draw(IRichPane rp)
        {
            var paneRect = rp.GetPaneRect();
            var sr = GetScRect(rp);
            sr = ScreenRect.FromLTRB(paneRect.LT.X, paneRect.LT.Y, paneRect.RB.X, paneRect.LT.Y + sr.Height);
            rp.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            rp.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(64, 0, 32, 64)), sr);

            var span = GetSpan(rp);
            var sec1 = GetCdPos(rp, sr.LT).X;   // Code.X --> Seconds from Hot.FirstSpeech
            var sec0 = sec1 / span * span;
            int sec;
            for (sec = sec0; ; sec += span)
            {
                var time = Hot.FirstSpeech + TimeSpan.FromSeconds(sec);
                var r = GetScRect(rp, CodeRect.FromLTRB(sec, 0, 0, 0));
                if (r.LT.X > sr.RB.X) break;

                // LINE
                rp.Graphics.DrawLine(new Pen(Color.FromArgb((time.Second % 10 == 0 ? 128 : 72), Color.SteelBlue), 0.5f), r.LT.X, sr.RB.Y - (time.Second == 0 ? 8 : 2), r.LT.X, paneRect.RB.Y);

                // SECOND  LABEL
                if (time.Second != 0)
                {
                    rp.Graphics.DrawString(time.ToString("ss"), FontSec, Brushes.DarkGray, r.LT.X, sr.RB.Y - 6, new StringFormat {
                        Alignment = StringAlignment.Center,
                        Trimming = StringTrimming.None,
                    });
                }
            }
            span = Math.Max(span, 60);
            sec = sec / span * span;
            var x = paneRect.RB.X + 999;
            for (; ; sec -= span)
            {
                var time = Hot.FirstSpeech + TimeSpan.FromSeconds(sec);
                var r = GetScRect(rp, CodeRect.FromLTRB(sec, 0, 0, 0));

                // TIME LABEL
                var a = (int)Math.Max(0.0, Math.Min((double)(x - 32) / 110 * 255, 255));
                var br = a >= 255 ? Brushes.White : new SolidBrush(Color.FromArgb(a, Color.White));
                x = Math.Max(r.LT.X, 16);
                rp.Graphics.DrawString(time.ToString(TimeUtil.FormatHM), FontTime, br, x, sr.LT.Y + 2, new StringFormat {
                    Alignment = StringAlignment.Center,
                    Trimming = StringTrimming.None,
                });

                if (r.LT.X < 0)
                {
                    break;
                }
            }

            return true;
        }

        private int GetSpan(IRichPane rp)
        {
            if (rp.Zoom.X < 9) return 300;
            if (rp.Zoom.X < 22) return 120;
            if (rp.Zoom.X < 50) return 30;
            if (rp.Zoom.X < 70) return 20;
            if (rp.Zoom.X < 90) return 10;
            if (rp.Zoom.X < 300) return 5;
            if (rp.Zoom.X < 400) return 2;
            return 1;
        }
    }
}
