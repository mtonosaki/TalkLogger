// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class FeatureScrollZoomLock : CoreFeatureBase, IMouseListener
    {
        public override void OnInitInstance()
        {
            base.OnInitInstance();
            TarPane = Pane.GetPane("GuiViewMain");
        }

        public void OnMouseDown(MouseState e)
        {
        }

        public void OnMouseMove(MouseState e)
        {
            Pane.Scroll = ScreenPos.FromInt(Pane.Scroll.X, 12);
            Pane.Zoom = new XyBase { X = Pane.Zoom.X, Y = 1000, };
        }

        public void OnMouseUp(MouseState e)
        {
        }

        public void OnMouseWheel(MouseState e)
        {
        }
    }
}
