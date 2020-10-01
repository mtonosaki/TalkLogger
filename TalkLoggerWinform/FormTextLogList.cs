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
            var rt = richTextBoxMain;
            foreach (var talk in talks)
            {
                var ep = richTextBoxMain.Text.Length;
                rt.Font = new Font("Yu Gothic UI", 11.0f, FontStyle.Regular);
                rt.AppendText(talk.TimeGenerated.ToString(TimeUtil.FormatHMS));
                rt.AppendText("\t");
                rt.AppendText(GetDisplayName(talk.ID));
                rt.AppendText("\t");
                rt.AppendText(talk.Text);
                rt.Select(ep, rt.Text.Length - ep);
                rt.SelectionBackColor = Color.FromArgb(255, talk.Color.R, talk.Color.G, talk.Color.B);
                rt.SelectionColor = Color.White;
                rt.AppendText(Environment.NewLine);
            }
            richTextBoxMain.Select(rt.Text.Length, 0);
        }

        private string GetDisplayName(Id id)
        {
            if (DisplayNames.TryGetValue(id, out var name))
            {
                return name;
            } else
            {
                return $"(noname {id.Value})";
            }
        }

        DateTime loadTime = DateTime.Now;
        private void FormTextLogList_Load(object sender, EventArgs e)
        {
            loadTime = DateTime.Now;
        }
        private void richTextBoxMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F8)
            {
                if( (DateTime.Now - loadTime).TotalMilliseconds > 500)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }
    }
}
