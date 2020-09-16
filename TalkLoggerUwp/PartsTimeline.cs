using Microsoft.Graphics.Canvas.Text;
using System;
using TalkLoggerUwp.Models;
using Tono;
using Tono.Gui;
using Tono.Gui.Uwp;
using Tono.Logic;
using Windows.UI;
using Windows.UI.Text;
using static Tono.Gui.Uwp.CastUtil;

namespace TalkLoggerUwp
{
    public class PartsTimeline : PartsBase<DateTime, Person>
    {
        public DataHot Hot { get; set; }

        private CanvasTextFormat fTime = new CanvasTextFormat
        {
            FontFamily = "Tahoma",
            FontSize = 9.0f,
            FontStyle = FontStyle.Normal,
            FontWeight = FontWeights.Normal,
            HorizontalAlignment = CanvasHorizontalAlignment.Center,
            VerticalAlignment = CanvasVerticalAlignment.Top,
        };
        public override void Draw(DrawProperty dp)
        {
            var t0 = Hot.Now;
            var sx0 = GetSx(dp.Pane, t0);
            var span = drawSpan(dp.Pane);
            var sr = dp.PaneRect;
            var sy = sr.T;
            var n = 0;
            for (var t = new DateTime(t0.Year, t0.Month, t0.Day, t0.Hour, t0.Minute / 10 * 10, 0) + TimeSpan.FromMinutes(10); ; t -= span, n++)   // for past time
            {
                if (t > t0) continue;
                var sx = GetSx(dp.Pane, t);
                if (sx < sr.L) break;

                dp.Graphics.DrawLine(sx.Sx, sr.T + 8, sx.Sx, sr.B, GetLineColor(t));

                if (t.Second == 0 && Math.Abs(sx0 - sx) > 32)
                {
                    var txt = t.ToString($"{t.Hour}:{t.Minute:00}");
                    dp.Graphics.DrawText(txt, sx.Sx, sy.Sy, Colors.White, fTime);
                }
            }
            {
                var sx = GetSx(dp.Pane, t0);
                dp.Graphics.DrawLine(sx.Sx, sr.T + 8, sx.Sx, sr.B, Color.FromArgb(64, 0, 255, 0));
                var txt = $"{t0.Hour}:{t0.Minute:00}:{t0.Second:00}";
                dp.Graphics.DrawText(txt, sx.Sx, sy.Sy, Colors.Green, fTime);
            }
        }

        private Color GetLineColor(DateTime t)
        {
            if (t.Second == 0) return Color.FromArgb(64, 255, 255, 255);
            if (t.Second == 30) return Color.FromArgb(32, 255, 255, 255);
            return Color.FromArgb(16, 255, 255, 255);
        }

        private TimeSpan drawSpan(IDrawArea pane)
        {
            var s0 = GetSx(pane, new DateTime(2020, 12, 25, 13, 0, 0));
            var s1 = GetSx(pane, new DateTime(2020, 12, 25, 13, 1, 0));
            var xpm = s1 - s0; // pixel per minute

            if (xpm < 15.25) return TimeSpan.FromMinutes(5);
            if (xpm < 32) return TimeSpan.FromMinutes(2);
            if (xpm < 46) return TimeSpan.FromMinutes(1);
            if (xpm < 52) return TimeSpan.FromSeconds(30);
            if (xpm < 59) return TimeSpan.FromSeconds(20);
            return TimeSpan.FromSeconds(10);
        }

        private ScreenX GetSx(IDrawArea pane, DateTime ti)
        {
            var lx = PositionerX(CodeX<DateTime>.From(ti), null);
            var sx = ScreenX.From(pane, lx);
            return sx;
        }
    }
}
