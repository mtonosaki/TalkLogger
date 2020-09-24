// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using System;
using System.Drawing;
using System.Windows.Forms;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private bool IsInInit = true;

        private void FormMain_Load(object sender, EventArgs e)
        {
            // INIT Tono.Gui.WinForm
            GuiViewMain.IsDrawEmptyBackground = false;
            Mes.SetDefault();
            Mes.SetCurrentLanguage((string)ConfigRegister.Current["LastLanguage", "en"]);
            FeatureLoader2.SetResources(Properties.Resources.ResourceManager);
            FeatureLoader2.SetUsingClass(GetType());
            GuiViewMain.Initialize(typeof(FeatureLoader2));
            Mes.Current.ResetText(this);
            Mes.Current.CodeChanged += OnMesCodeChanged;
            DateTimeEx.SetDayStrings(Mes.Current);
            GuiViewMain.GetFeatureRoot().ParseCommandLineParameter(Environment.GetCommandLineArgs());
            GuiViewMain.GetFeatureRoot().FlushFeatureTriggers();

            GuiViewMain.Zoom = new XyBase { X = 1090, Y = 1000, };
            GuiViewMain.Scroll = ScreenPos.FromInt(GuiViewMain.Scroll.X, 12);


            // Restore Form dimension
            Location = new Point {
                X = (int)ConfigRegister.Current["X", 12],
                Y = (int)ConfigRegister.Current["Y", 24],
            };
            Size = new Size {
                Width = (int)ConfigRegister.Current["Width", 885],
                Height = (int)ConfigRegister.Current["Height", 236],
            };
            splitContainerMain.SplitterDistance = (int)ConfigRegister.Current["PaneSplitterY", 25];
            textBoxTalk.Text = "";
            labelTalkBarTime.Text = "";

            IsInInit = false;
        }

        private void OnMesCodeChanged(object sender, Mes.CodeChangedEventArgs e)
        {
            if (IsInInit)
            {
                return;
            }

            ConfigRegister.Current["LastLanguage"] = Mes.Current.GetCode();
        }

        private void splitContainerMain_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (IsInInit)
            {
                return;
            }

            ConfigRegister.Current["PaneSplitterY"] = splitContainerMain.SplitterDistance;
        }

        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            labelClosing.Location = new Point {
                X = Width / 2 - labelClosing.Width / 2,
                Y = Height / 2 - labelClosing.Height / 2,
            };

            if (IsInInit)
            {
                return;
            }

            ConfigRegister.Current["Width"] = Width;
            ConfigRegister.Current["Height"] = Height;
        }

        private void FormMain_LocationChanged(object sender, EventArgs e)
        {
            if (IsInInit)
            {
                return;
            }

            ConfigRegister.Current["X"] = Location.X;
            ConfigRegister.Current["Y"] = Location.Y;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            labelClosing.Visible = true;
            Application.DoEvents();
        }
    }
}
