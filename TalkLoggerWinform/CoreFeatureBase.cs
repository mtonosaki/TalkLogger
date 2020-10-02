// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using Tono;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class CoreFeatureBase : FeatureBase
    {
        public static readonly NamedId TokenSettingsLoaded = NamedId.FromName("SettingsLoaded");
        public static readonly NamedId TokenSpeechEventQueued = NamedId.FromName("TokenSpeechEventQueued");
        public const int LayerTalkBar = 100;
        protected IRichPane TarPane { get; set; }
        public DataHot Hot
        {
            get
            {
                return (DataHot)base.Data;
            }
        }

        public new DataParts Parts
        {
            get
            {
                return (DataParts)base.Parts;
            }
        }

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
                cd.LT.X * DataHot.LayoutPixelPerSecond,
                MakeTopByRowID(cd.LT.Y),
                cd.RB.X * DataHot.LayoutPixelPerSecond,
                MakeBottomByRowID(cd.RB.Y));
        }

        public CodeRect TalkPosCoder(LayoutRect rect, PartsBase target)
        {
            return CodeRect.FromLTRB(
                rect.LT.X / DataHot.LayoutPixelPerSecond,
                FindRowIDByLayoutY(rect.LT.Y),
                rect.RB.X / DataHot.LayoutPixelPerSecond,
                FindRowIDByLayoutY(rect.RB.Y));
        }
    }
}
