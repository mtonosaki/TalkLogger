using System;
using Tono.Gui.Uwp;

namespace TalkLoggerUwp
{
    public class EventClockUpdatedTokenTrigger : EventTokenTrigger
    {
        public DateTime Pre { get; set; }
        public DateTime Now { get; set; }
    }
}
