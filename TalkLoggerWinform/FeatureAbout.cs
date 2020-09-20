using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tono;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class FeatureAbout : FeatureBase
    {
        public override void Start(NamedId who)
        {
            base.Start(who);
            var fa = new FormAbout();
            fa.ShowDialog(Pane.Control);
        }
    }
}
