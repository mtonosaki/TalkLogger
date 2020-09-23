// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

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
