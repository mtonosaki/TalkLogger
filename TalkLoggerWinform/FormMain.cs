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

namespace TalkLoggerWinform
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void toolStripMenuItem1Quit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private bool IsWindowMoving = false;
        private Point DragStartLocationMouse;
        private Point DragStartLocationForm;
        private void FormMain_MouseDown(object sender, MouseEventArgs e)
        {
            DragStartLocationMouse = PointToScreen(e.Location);
            DragStartLocationForm = Location;
            IsWindowMoving = true;
        }

        private void FormMain_MouseMove(object sender, MouseEventArgs e)
        {
            var mpos = PointToScreen(e.Location);
            
            if (IsWindowMoving)
            {
                this.Location = new Point
                {
                    X = DragStartLocationForm.X + mpos.X - DragStartLocationMouse.X,
                    Y = DragStartLocationForm.Y + mpos.Y - DragStartLocationMouse.Y,
                };
            }
        }

        private void FormMain_MouseUp(object sender, MouseEventArgs e)
        {
            IsWindowMoving = false;
        }

        private void toolStripMenuItemFitToBottom_Click(object sender, EventArgs e)
        {
            Location = new Point
            {
                X = Screen.PrimaryScreen.WorkingArea.Left,
                Y = Screen.PrimaryScreen.WorkingArea.Bottom - Height,
            };
            Size = new Size
            {
                Width = Screen.PrimaryScreen.WorkingArea.Width,
                Height = Height,
            };
        }
    }
}
