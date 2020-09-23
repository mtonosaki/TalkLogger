using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tono.GuiWinForm;

namespace TalkLoggerWinform
{
    public class FeatureLogPanelCustom : FeatureLogGroupPanel
    {
        public override void OnInitInstance()
        {
            base.OnInitInstance();
            var h = (int)ConfigRegister.Current["LogPanelGroupHeight", -1];
            if (h < 0)
            {
                ConfigRegister.Current["LogPanelGroupHeight"] = 96;
            }
            GetParentForm().SizeChanged += Pane_SizeChanged;
            Timer.AddTrigger(200, () =>
            {
                Pane_SizeChanged(this, EventArgs.Empty);
            });
        }
        private Form GetParentForm()
        {
            Control root;
            for (root = Pane.Control; root.Parent != null; root = root.Parent)
            {
                ;
            }

            return root.FindForm();
        }

        private dpLogPanelCustom LogParts = null;
        protected override dpLogPanel createLogPartInstance(Dictionary<LLV, ScreenPos> clickArea)
        {
            return LogParts = new dpLogPanelCustom(clickArea);
        }

        private void Pane_SizeChanged(object sender, EventArgs e)
        {
            if (LogParts != null)
            {
                var r = Pane.GetPaneRect();
                LogParts.SetMargin(ScreenRect.FromLTRB(r.RB.X - 480, 0, 0, 0));
            }
            else
            {
                Timer.AddTrigger(200, () =>
                {
                    Pane_SizeChanged(this, EventArgs.Empty);
                });
            }
        }

        protected class dpLogPanelCustom : FeatureLogGroupPanel.dpLogPanel
        {
            static dpLogPanelCustom()
            {
                _fontTitle = new Font("Segoe UI", 8.0f, FontStyle.Bold);
                _font = new Font("Segoe UI", 8.0f, FontStyle.Regular);
                _bDev = new SolidBrush(Color.FromArgb(255, 160, 160, 160));
                _bInf = new SolidBrush(Color.FromArgb(255, 192, 255, 64));
                _bTodo = new SolidBrush(Color.FromArgb(255, 126, 255, 58));
                _bWar = new SolidBrush(Color.FromArgb(255, 227, 227, 227));
                _bErr = new SolidBrush(Color.FromArgb(255, 255, 255, 0));
                _pLine = new Pen(Color.FromArgb(16, 0, 0, 0), 1);
                _pRegionShadow = new Pen(Color.FromArgb(64, 64, 0, 0), 1);
            }

            public dpLogPanelCustom(Dictionary<LLV, ScreenPos> clickArea) : base(clickArea)
            {
            }

            public override bool Draw(IRichPane rp)
            {
#if DEBUG
                base.Draw(rp);
#endif
                return true;
            }

            protected override Brush createLogPanelBG(ScreenRect sr)
            {
                return new System.Drawing.Drawing2D.LinearGradientBrush(
                    sr.LT, sr.RT,
                    Color.FromArgb(128, 0, 0, 0),
                    Color.FromArgb(32, 0, 0, 0)
                );
            }
        }
    }
}

