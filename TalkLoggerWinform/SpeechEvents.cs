using System;

namespace TalkLoggerWinform
{
    public class SpeechEvent
    {
        public enum Actions
        {
            Start,
            Recognizing,
            Recognized,
            End,
            Canceled,
            SetColor,
        };
        public Actions Action { get; set; }
        public DateTime TimeGenerated { get; set; } // DateTime at Token Fire
        public string SessionID { get; set; }
        public int RowID { get; set; }
        public string Text { get; set; }
    }
}
