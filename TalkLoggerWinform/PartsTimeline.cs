using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Tono;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class PartsTimeline : PartsBase
    {
        public DataHot Hot { get; set; }
        static readonly Font FontTime = new Font("Tahoma", 8.0f);
        static readonly Font FontTimeNow = new Font("Tahoma", 9.0f, FontStyle.Bold);
        static readonly Font FontSec = new Font("Tahoma", 7.0f);
        static readonly Font FontSecNow = new Font("Tahoma", 8.0f, FontStyle.Bold);
        public override bool Draw(IRichPane rp)
        {
            var paneRect = rp.GetPaneRect();
            var sr0 = GetScRect(rp);

            var sr = ScreenRect.FromLTRB(paneRect.LT.X, paneRect.LT.Y, paneRect.RB.X, paneRect.LT.Y + sr0.Height);
            rp.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            rp.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(64, 0, 32, 64)), sr);   // Background
            var nowr = GetScRect(rp, CodeRect.FromLTRB((int)(DateTime.Now - Hot.FirstSpeech).TotalSeconds, 0, 0, 0));
            
            var span = GetSpan(rp);
            var sec1 = GetCdPos(rp, sr.LT).X;   // Code.X --> Seconds from Hot.FirstSpeech
            var sec0 = sec1 / span * span;
            int sec;
            for (sec = sec0; ; sec += span)
            {
                var time = Hot.FirstSpeech + TimeSpan.FromSeconds(sec);
                if (time + TimeSpan.FromSeconds(1) > DateTime.Now) break;
                var r = GetScRect(rp, CodeRect.FromLTRB(sec, 0, 0, 0));
                if (r.LT.X > sr.RB.X) break;

                // LINE
                if (Math.Abs(r.LT.X - nowr.LT.X) > 8)
                {
                    rp.Graphics.DrawLine(new Pen(Color.FromArgb((time.Second % 10 == 0 ? 128 : 72), Color.SteelBlue), 0.5f), r.LT.X, sr.RB.Y - (time.Second == 0 ? 8 : 2), r.LT.X, paneRect.RB.Y);
                }

                // SECOND  LABEL
                if (time.Second != 0 && Math.Abs(r.LT.X - nowr.LT.X) > 16)
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
                if (time >= DateTime.Now) continue;
                var r = GetScRect(rp, CodeRect.FromLTRB(sec, 0, 0, 0));

                // TIME LABEL
                var a = (int)Math.Max(0.0, Math.Min((double)(x - 32) / 110 * 255, 255));
                var br = a >= 255 ? Brushes.White : new SolidBrush(Color.FromArgb(a, Color.White));
                x = Math.Max(r.LT.X, 16);
                if (Math.Abs(x - nowr.LT.X) > 36)
                {
                    rp.Graphics.DrawString(time.ToString(TimeUtil.FormatHM), FontTime, br, x, sr.LT.Y + 2, new StringFormat {
                        Alignment = StringAlignment.Center,
                        Trimming = StringTrimming.None,
                    });
                }
                if (r.LT.X < 0)
                {
                    break;
                }
            }
            // DRAW NOW
            {
                // LINE
                rp.Graphics.DrawLine(Pens.DarkGreen, nowr.LT.X, sr.RB.Y, nowr.LT.X, paneRect.RB.Y);

                // SECOND  LABEL
                rp.Graphics.DrawString(DateTime.Now.ToString("ss"), FontSecNow, Brushes.LimeGreen, nowr.LT.X, sr.RB.Y - 8, new StringFormat {
                    Alignment = StringAlignment.Center,
                    Trimming = StringTrimming.None,
                });

                // TIME LABEL
                rp.Graphics.DrawString(DateTime.Now.ToString(TimeUtil.FormatHM), FontTimeNow, Brushes.LimeGreen, nowr.LT.X, sr.LT.Y + 1, new StringFormat {
                    Alignment = StringAlignment.Center,
                    Trimming = StringTrimming.None,
                });
            }

            // rp.Graphics.DrawLine(Pens.White, paneRect.RT, paneRect.LB); // for TEST

            return true;
        }

        private int GetSpan(IRichPane rp)
        {
            var z = rp.Zoom.X * DataHot.LayoutPixelPerSecond / 40;
            if (z < 9) return 300;
            if (z < 22) return 120;
            if (z < 50) return 30;
            if (z < 70) return 20;
            if (z < 90) return 10;
            if (z < 300) return 5;
            if (z < 400) return 2;
            return 1;
        }
    }
}
