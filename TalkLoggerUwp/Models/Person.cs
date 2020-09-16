using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tono;

namespace TalkLoggerUwp.Models
{
    public class Person : NamedId
    {
        public NamedId ID { get; set; }
        public string DisplayName { get; set; }
    }
}
