using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkLoggerUwp.Models;
using Tono.Gui;
using Tono.Gui.Uwp;

namespace TalkLoggerUwp
{
    /// <summary>
    /// Top row as timeline
    /// </summary>
    public class FeatureTimeline : FeatureCommonBase
    {
        public override void OnInitialInstance()
        {
            base.OnInitialInstance();
            Hot.Now = DateTime.Now;
            Pane.Target = Pane["ChatPanel"];

            var pt = new PartsTimeline
            {
                Hot = Hot,
                Location = CodePos<DateTime, Person>.From(CodeX<DateTime>.From(DateTime.Now), CodeY<Person>.From(null)), // not used Location because timeline is located fix position.
                PositionerX = PositionerPanelMode,
                PositionerY = PositionerPerson,
                CoderX = CoderDateTime,
                CoderY = CoderPanelPerson,
            };
            Parts.Add(Pane.Target, pt, LAYERS.Timeline);
        }

        [EventCatch]
        public void OnScrolled(EventTokenPaneChanged token)
        {
            Pane.Target.ScrollX = Pane.Main.ScrollX;
            Pane.Target.ZoomX = Pane.Main.ZoomX;
        }
    }
}
