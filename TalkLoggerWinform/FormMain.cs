using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            new FormShapePersister(this);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // INIT Tono.Gui.WinForm
            GuiViewMain.IsDrawEmptyBackground = false;
            PaneChat.IdColor = Color.FromArgb(48, 48, 48);
            PaneTimeline.IdColor = Color.FromArgb(48, 48, 48);
            Mes.SetDefault();
            FeatureLoader2.SetResources(Properties.Resources.ResourceManager);
            FeatureLoader2.SetUsingClass(GetType());
            GuiViewMain.Initialize(typeof(FeatureLoader2));
            Mes.Current.ResetText(this);
            DateTimeEx.SetDayStrings(Mes.Current);
            GuiViewMain.GetFeatureRoot().ParseCommandLineParameter(Environment.GetCommandLineArgs());
            GuiViewMain.GetFeatureRoot().FlushFeatureTriggers();
        }
    }
}
