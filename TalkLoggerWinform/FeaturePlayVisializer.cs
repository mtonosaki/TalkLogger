// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class FeaturePlayVisializer : CoreFeatureBase
    {
        private PartsMask _parts;
        private bool _prevMaskFlag;
        public override void OnInitInstance()
        {
            base.OnInitInstance();
            TarPane = Pane.GetPane("GuiViewMain");
            _prevMaskFlag = false;

            Parts.Add(TarPane, _parts = new PartsMask
            {
                IsMasking = true,
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
            public bool IsMasking { get; set; }

            private Brush _bg = new SolidBrush(Color.FromArgb(192, 64, 64, 64));
            private Font _font = new Font("Tahoma", 8.0f, FontStyle.Bold);
            private int _cnt = 0;
            public override bool Draw(IRichPane rp)
            {
                if (IsMasking)
                {
                    var sr = rp.GetPaneRect();
                    rp.Graphics.FillRectangle(_bg, sr);
                    rp.Graphics.DrawString("PAUSE", _font, Brushes.DarkGray, sr.LT.X + 48, sr.RB.Y - 42);
                }
                else
                {
                    if (++_cnt % 2 == 0)
                    {
                        var sr = rp.GetPaneRect();
                        rp.Graphics.DrawString("REC", _font, Brushes.Yellow, sr.LT.X + 48, sr.RB.Y - 42);
                    }
                }
                return true;
            }
        }
    }
}
