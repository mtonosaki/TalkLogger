using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class DataHot : DataHotBase
    {
        public DateTime FirstSpeech { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

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
