using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkLoggerWinform
{
    /// <summary>
    /// Setting POCO
    /// </summary>
    public class SettingModel
    {
        public string SubscriptionKey { get; set; }
        public string ServiceRegion { get; set; }
        public string Device1ID { get; set; }
        public string Device2ID { get; set; }
    }
}
