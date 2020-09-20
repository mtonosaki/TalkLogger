using System;
using System.Diagnostics;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class FeatureAutoScroll : CoreFeatureBase
    {
        private DataSharingManager.Boolean _isAutoScroll;
        private DateTime AnchorDate;
        private int AnchorZoomX = -1;
        private int PrevScrollX;

        public override void OnInitInstance()
        {
            base.OnInitInstance();
            TarPane = Pane.GetPane("Resource");
            _isAutoScroll = (DataSharingManager.Boolean)Share.Get("_isAutoScroll", typeof(DataSharingManager.Boolean));
            Timer.AddTrigger(1000, OnPorling);
        }
        public override bool Enabled {
            get => _isAutoScroll.value;
            set {
                _isAutoScroll.value = value;
                AnchorZoomX = -1;   // Request to reset integer distributing
            }
        }

        private void OnPorling()
        {
            if (Enabled)
            {
                if (AnchorZoomX != Pane.Zoom.X)
                {
                    AnchorZoomX = Pane.Zoom.X;
                    AnchorDate = DateTime.Now;
                    PrevScrollX = 0;
                }
                var td = DateTime.Now - AnchorDate;
                var x = Hot.TimelineParts.GetScRect(TarPane, CodeRect.FromLTRB(0, -1, (int)(td.TotalSeconds * 120), -1)).Width / 120;
                var dx = x - PrevScrollX;
                PrevScrollX = x;
                if (dx >= 0)
                {
                    Pane.Scroll = ScreenPos.FromInt(Pane.Scroll.X - dx, Pane.Scroll.Y);
                }
                else
                {
                    AnchorZoomX = -1;
                }
            }
            Timer.AddTrigger(20, OnPorling);
            Pane.Invalidate(null);
        }
    }
}
