// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using System;
using Tono;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class FeatureJumpToNow : CoreFeatureBase
    {
        public override void OnInitInstance()
        {
            base.OnInitInstance();
            TarPane = Pane.GetPane("GuiViewMain");

            Timer.AddTrigger(1100, () => {
                Start(null);
            });
        }

        public override void Start(NamedId who)
        {
            var td = DateTime.Now - Hot.FirstSpeech;
            var x = Hot.TimelineParts.GetScRect(TarPane, CodeRect.FromLTRB(0, -1, (int)(td.TotalSeconds * 12), -1)).Width / 12;
            var p = FeatureLogPanelCustom.IsLogDrawing ? 70 : 90;
            Pane.Scroll = ScreenPos.FromInt(-x + Pane.GetPaneRect().Width * p / 100, Pane.Scroll.Y);
            Pane.Invalidate(null);
        }
    }
}
