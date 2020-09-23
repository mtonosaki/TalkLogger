using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            Timer.AddTrigger(1100, () =>
            {
                Start(null);
            });
        }

        public override void Start(NamedId who)
        {
            var td = DateTime.Now - Hot.FirstSpeech;
            var x = Hot.TimelineParts.GetScRect(TarPane, CodeRect.FromLTRB(0, -1, (int)(td.TotalSeconds * 12), -1)).Width / 12;
            Pane.Scroll = ScreenPos.FromInt(-x + Pane.GetPaneRect().Width * 85 / 100, Pane.Scroll.Y);
            Pane.Invalidate(null);
        }
    }
}
