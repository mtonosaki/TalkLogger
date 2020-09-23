using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class FeatureTimeline : CoreFeatureBase
    {
        public const int ROWID_TIMELINE = -1;

        public override void OnInitInstance()
        {
            base.OnInitInstance();
            Hot.FirstSpeech = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute / 10 * 10, 0);

            TarPane = Pane.GetPane("GuiViewMain");

            Hot.AddRowID( ROWID_TIMELINE, orderNo:100, layoutHeight:24);  // Set Timeline Height
            Hot.AddRowID(-999, 101, 4);            // Dummy Space
            Hot.TimelineParts = new PartsTimeline {
                Hot = Hot,
                Rect = CodeRect.FromLTRB(0, ROWID_TIMELINE, 0, ROWID_TIMELINE),
                PartsPositioner = TalkPositioner,
                PartsPositionCorder = TalkPosCoder,
            };
            Parts.Add(TarPane, Hot.TimelineParts);

        }
    }
}
