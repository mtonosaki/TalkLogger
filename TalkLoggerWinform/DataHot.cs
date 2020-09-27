// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class DataHot : DataHotBase
    {
        public bool IsPlaying = false;
        public const int LayoutPixelPerSecond = 20;
        public DateTime FirstSpeech { get; set; }
        public PartsTimeline TimelineParts { get; set; }
        public SettingModel Setting { get; set; } = new SettingModel();
        public Queue<SpeechEvent> SpeechEventQueue { get; } = new Queue<SpeechEvent>();
        public string SelectedText { get; set; }

        /// <summary>
        /// RowID (You always have to manually sort this list by OrderNo)
        /// </summary>
        public List<(int RowID, int OrderNo, int LayoutHeight, bool IsVisible)> RowIDs = new List<(int RowID, int OrderNo, int LayoutHeight, bool IsVisible)>();

        public void AddRowID(int rowID, int orderNo, int layoutHeight, bool isVisible = true)
        {
            int i;
            for (i = RowIDs.Count - 1; i >= 0; i--)
            {
                if (RowIDs[i].RowID == rowID)
                {
                    RowIDs[i] = (rowID, orderNo, layoutHeight, isVisible);
                    break;
                }
            }
            if (i < 0)
            {
                RowIDs.Add((rowID, orderNo, layoutHeight, isVisible));
            }
            RowIDs.Sort((a, b) => a.OrderNo - b.OrderNo);
        }
    }
}
