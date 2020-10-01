using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Drawing;
using System.Windows.Forms;
using Tono;

namespace TalkLoggerWinform
{
    public partial class FormTextLogList : Form
    {
        public FormTextLogList()
        {
            InitializeComponent();
        }

        public class Talk
        {
            public Id ID { get; set; }
            public DateTime TimeGenerated { get; set; }
            public string Text { get; set; }
            public Color Color { get; set; }
            public override string ToString()
            {
                return $"ID={ID},{TimeGenerated.ToString(TimeUtil.FormatYMDHMSms)} : {Text}";
            }
        }

        public Dictionary<Id, string> DisplayNames { get; } = new Dictionary<Id, string>();

        public void SetTextData(IEnumerable<Talk> talks)
        {
        }
    }
}
