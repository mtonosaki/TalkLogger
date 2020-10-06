// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Tono;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class DataParts : PartsCollection
    {
        private object SyncRoot = new object();

        public override void Add(IRichPane target, PartsBase value, int layerLevel)
        {
            lock (SyncRoot)
            {
                base.Add(target, value, layerLevel);
            }
        }

        public override void ProvideDrawFunction()
        {
            lock (SyncRoot)
            {
                base.ProvideDrawFunction();
            }
        }
        public override IList<PartsBase> GetPartsByLocationID(Id pos)
        {
            lock (SyncRoot)
            {
                return base.GetPartsByLocationID(pos);
            }
        }

        public override IEnumerable<PartsBase> GetLayerParts(int layer)
        {
            lock(SyncRoot)
            {
                return base.GetLayerParts(layer).ToArray();
            }
        }
    }
}
