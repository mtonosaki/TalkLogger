using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class CoreFeatureBase : FeatureBase
    {
        protected IRichPane TarPane { get; set; }
        public DataHot Hot => (DataHot)base.Data;
        private const int LayoutPixelPerSecond = 40;

        /// <summary>
        /// Top position (Layout coodinate)
        /// </summary>
        /// <param name="rowid"></param>
        /// <returns></returns>
        public int MakeTopByRowID(int rowid)
        {
            var ly = 0;
            for (var i = 0; i < Hot.RowIDs.Count; i++)
            {
                if (Hot.RowIDs[i].RowID == rowid)
                {
                    break;
                }
                ly += Hot.RowIDs[i].IsVisible ? Hot.RowIDs[i].LayoutHeight : 0;
            }
            return ly;
        }

        /// <summary>
        /// Bottom position (Layout coodinate)
        /// </summary>
        /// <param name="rowid"></param>
        /// <returns></returns>
        public int MakeBottomByRowID(int rowid)
        {
            var ly = 0;
            var h = 0;
            for (var i = 0; i < Hot.RowIDs.Count; i++)
            {
                h = Hot.RowIDs[i].IsVisible ? Hot.RowIDs[i].LayoutHeight : 0;
                if (Hot.RowIDs[i].RowID == rowid)
                {
                    break;
                }
                ly += h;
            }
            return ly + h - 1;
        }
        public int FindRowIDByLayoutY(int ly)
        {
            var y = 0;
            for (var i = 0; i < Hot.RowIDs.Count; i++)
            {
                if (ly >= y && ly < y + Hot.RowIDs[i].LayoutHeight)
                {
                    return Hot.RowIDs[i].RowID;
                }
                y += Hot.RowIDs[i].LayoutHeight;
            }
            return -1;
        }


        /// <summary>
        /// Make layout position
        /// </summary>
        /// <param name="cd">X=Second from First Speech / Y=Speech Device ID</param>
        /// <param name="target"></param>
        /// <returns></returns>
        public LayoutRect TalkPositioner(CodeRect cd, PartsBase target)
        {
            return LayoutRect.FromLTRB(
                cd.LT.X * LayoutPixelPerSecond, 
                MakeTopByRowID(cd.LT.Y), 
                cd.RB.X * LayoutPixelPerSecond,
                MakeBottomByRowID(cd.RB.Y));
        }

        public CodeRect TalkPosCoder(LayoutRect rect, PartsBase target)
        {
            return CodeRect.FromLTRB(
                rect.LT.X / LayoutPixelPerSecond, 
                FindRowIDByLayoutY(rect.LT.Y), 
                rect.RB.X / LayoutPixelPerSecond, 
                FindRowIDByLayoutY(rect.RB.Y));
        }
    }
}
