// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using System.Drawing;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class PartsTalkBar : PartsBase, IPartsSelectable
    {
        private static readonly Font FontTalk = new Font("Yu Gothic UI", 8.0f, FontStyle.Regular);
        public string SessionID { get; set; }
        public bool IsSelected { get; set; }
        public bool IsCancelled { get; set; }
        public Color BarColor { get; set; } = Color.FromArgb(64, 192, 192, 192);


        public override bool Draw(IRichPane rp)
        {
            var sr = GetScRect(rp);
            if (sr.RB.X < 0) return true;

            var srf = new RectangleF(sr.LT.X, sr.LT.Y, sr.Width, sr.Height);
            var tf = new StringFormat {
                Alignment = StringAlignment.Far,
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter,
            };
            var brush = new SolidBrush(BarColor);

            if (IsCancelled)
            {
                if (sr.Width > 2)
                {
                    rp.Graphics.FillRectangle(brush, sr);
                    rp.Graphics.DrawString(Text, FontTalk, brush, srf, tf);
                }
            }
            else
            {
                rp.Graphics.FillRectangle(brush, sr);
                rp.Graphics.DrawString(Text, FontTalk, Brushes.White, srf, tf);
                if (IsSelected)
                {
                    foreach (var col in new[] {
                    Color.Yellow,
                    Color.FromArgb(160, Color.LightGreen),
                    Color.FromArgb(120, Color.Blue),
                })
                    {
                        rp.Graphics.DrawRectangle(new Pen(col), sr);
                        sr.Inflate(1);
                    }
                }
            }
            return true;
        }
    }
}
