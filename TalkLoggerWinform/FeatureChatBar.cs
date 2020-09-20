using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class FeatureChatBar : CoreFeatureBase
    {
        public override void OnInitInstance()
        {
            base.OnInitInstance();
            TarPane = Pane.GetPane("Resource");
            Hot.RowIDs.Clear();
        }
    }
}
