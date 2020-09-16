using Microsoft.Graphics.Canvas.Brushes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tono.Gui;
using Tono.Gui.Uwp;
using Windows.UI;
using static Tono.Gui.Uwp.CastUtil;

namespace TalkLoggerUwp
{
    public class PartsLogCustom : PartsLog
    {
        public override void Draw(DrawProperty dp)
        {
            var sr = dp.PaneRect;

            drawBackground(dp, sr);

            if (Parent.IsVisible)
            {
                var theight = drawTitleBar(dp, sr);
                drawMessage(dp, sr, theight);
            }
        }

        protected override void drawBackground(DrawProperty dp, ScreenRect sr)
        {
            var bg = new CanvasLinearGradientBrush(dp.Canvas, Parent.BackgroundColor1, Parent.BackgroundColor2)
            {
                StartPoint = ScreenPos.From(sr.LT.X, sr.LT.Y),
                EndPoint = ScreenPos.From(sr.LT.X, sr.RB.Y),
            };
            dp.Graphics.FillRectangle(_(sr), bg);
        }

        protected override ScreenY drawTitleBar(DrawProperty dp, ScreenRect sr)
        {
            return ScreenY.From(2);
        }

        protected override Color getMessageColor(LLV lv, int lastIndex, int maxIndex)
        {
            switch (lv)
            {
                case LLV.ERR: return Color.FromArgb(0xff, 0xff, 0xee, 0xee);
                case LLV.WAR: return Color.FromArgb(0xff, 0xff, 0xff, 0xff);
                case LLV.INF: return Color.FromArgb(0x80, 0xff, 0xff, 0xff);
                default: return Color.FromArgb(0x40, 0x80, 0x80, 0x80);
            }
        }
        protected override bool isAlignBottom => true;
    }
}
