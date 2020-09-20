using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class DataLink : DataLinkBase
    {
        public override void Clear()
        {
        }

        public override ICollection GetPartsset(RecordBase key)
        {
            return new RecordBase[] { };
        }

        public override ICollection GetRecordset(PartsBase key)
        {
            return new PartsBase[] { };
        }

        public override void RemoveEquivalent(PartsBase parts)
        {
        }

        public override void SetEquivalent(RecordBase record, PartsBase parts)
        {
        }
    }
}
