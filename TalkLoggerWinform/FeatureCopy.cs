using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tono;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class FeatureCopy : CoreFeatureBase
    {
        public override void Start(NamedId who)
        {
            base.Start(who);
            if (!string.IsNullOrEmpty(Hot.SelectedText))
            {
                Clipboard.SetText(Hot.SelectedText);
                LOG.WriteLine(LLV.DEV, $"Copy {Hot.SelectedText}");
            } else
            {
                LOG.WriteLine(LLV.DEV, $"Cannot Copy because of NULL");
            }
        }
    }
}
