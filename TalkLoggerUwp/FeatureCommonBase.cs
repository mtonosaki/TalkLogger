using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkLoggerUwp.Models;
using Tono.Gui;
using Tono.Gui.Uwp;

namespace TalkLoggerUwp
{
    public abstract class FeatureCommonBase : FeatureBase
    {
        public DataCold Cold { get => (DataCold)base.DataCold; }
        public DataHot Hot { get => (DataHot)base.DataHot; }
        protected double lxsec => 4;

        private readonly Dictionary<Person, LayoutY> personlys = new Dictionary<Person, LayoutY>();
        private readonly Dictionary<int, Person> lypersons = new Dictionary<int, Person>();

        public LayoutX PositionerPanelMode(CodeX<DateTime> dt, CodeY<Person> _)
        {
            var ts = dt.Cx - Hot.Now;
            return LayoutX.From(ts.TotalSeconds / lxsec);
        }

        public CodeX<DateTime> CoderDateTime(LayoutX lx, LayoutY _)
        {
            return CodeX<DateTime>.From(Hot.Now + TimeSpan.FromSeconds(lx.Lx * lxsec));
        }

        public LayoutY PositionerPerson(CodeX<DateTime> _, CodeY<Person> cperson)
        {
            return personlys[cperson.Cy];
        }

        public CodeY<Person> CoderPanelPerson(LayoutX _, LayoutY ly)
        {
            if (lypersons.TryGetValue((int)ly.Ly, out var person))
            {
                return CodeY<Person>.From(person);
            }
            else
            {
                return null;
            }
        }
    }
}
