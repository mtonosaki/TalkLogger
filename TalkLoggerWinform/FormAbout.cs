// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using System;
using System.Windows.Forms;

namespace TalkLoggerWinform
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            textBox1.Select(0, 0);
        }
    }
}
