namespace TalkLoggerWinform
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemFitToBottom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GuiViewMain = new Tono.GuiWinForm.TGuiView(this.components);
            this.KeyEnablerMain = new Tono.GuiWinForm.TKeyEnabler();
            this.PaneChat = new Tono.GuiWinForm.TPane();
            this.PaneTimeline = new Tono.GuiWinForm.TPane();
            this.contextMenuStripMain.SuspendLayout();
            this.menuStripMain.SuspendLayout();
            this.GuiViewMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStripMain
            // 
            this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemFitToBottom,
            this.toolStripSeparator1,
            this.toolStripMenuItemExit});
            this.contextMenuStripMain.Name = "contextMenuStripMain";
            this.contextMenuStripMain.Size = new System.Drawing.Size(140, 54);
            // 
            // toolStripMenuItemFitToBottom
            // 
            this.toolStripMenuItemFitToBottom.Name = "toolStripMenuItemFitToBottom";
            this.toolStripMenuItemFitToBottom.Size = new System.Drawing.Size(139, 22);
            this.toolStripMenuItemFitToBottom.Text = "FitToBottom";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(136, 6);
            // 
            // toolStripMenuItemExit
            // 
            this.toolStripMenuItemExit.Name = "toolStripMenuItemExit";
            this.toolStripMenuItemExit.Size = new System.Drawing.Size(139, 22);
            this.toolStripMenuItemExit.Text = "Exit";
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.toolToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(624, 24);
            this.menuStripMain.TabIndex = 1;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // toolToolStripMenuItem
            // 
            this.toolToolStripMenuItem.Name = "toolToolStripMenuItem";
            this.toolToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.toolToolStripMenuItem.Text = "Tool";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // GuiViewMain
            // 
            this.GuiViewMain.AllowDrop = true;
            this.GuiViewMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GuiViewMain.Controls.Add(this.KeyEnablerMain);
            this.GuiViewMain.Controls.Add(this.PaneChat);
            this.GuiViewMain.Controls.Add(this.PaneTimeline);
            this.GuiViewMain.IdText = "GuiViewMain";
            this.GuiViewMain.IsDrawEmptyBackground = true;
            this.GuiViewMain.Location = new System.Drawing.Point(12, 32);
            this.GuiViewMain.Name = "GuiViewMain";
            this.GuiViewMain.Scroll = ((Tono.GuiWinForm.ScreenPos)(resources.GetObject("GuiViewMain.Scroll")));
            this.GuiViewMain.ScrollMute = ((Tono.GuiWinForm.ScreenPos)(resources.GetObject("GuiViewMain.ScrollMute")));
            this.GuiViewMain.Size = new System.Drawing.Size(600, 128);
            this.GuiViewMain.TabIndex = 2;
            this.GuiViewMain.TabStop = false;
            this.GuiViewMain.Text = "The Gui Main Pane";
            this.GuiViewMain.Zoom = ((Tono.GuiWinForm.XyBase)(resources.GetObject("GuiViewMain.Zoom")));
            this.GuiViewMain.ZoomMute = ((Tono.GuiWinForm.XyBase)(resources.GetObject("GuiViewMain.ZoomMute")));
            // 
            // KeyEnablerMain
            // 
            this.KeyEnablerMain.BackColor = System.Drawing.Color.Maroon;
            this.KeyEnablerMain.Location = new System.Drawing.Point(0, 0);
            this.KeyEnablerMain.Name = "KeyEnablerMain";
            this.KeyEnablerMain.Size = new System.Drawing.Size(1, 1);
            this.KeyEnablerMain.TabIndex = 2;
            // 
            // PaneChat
            // 
            this.PaneChat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PaneChat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.PaneChat.IdColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.PaneChat.IdText = "Resource";
            this.PaneChat.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.PaneChat.IsScrollLockX = false;
            this.PaneChat.IsScrollLockY = false;
            this.PaneChat.IsZoomLockX = false;
            this.PaneChat.IsZoomLockY = false;
            this.PaneChat.Location = new System.Drawing.Point(0, 26);
            this.PaneChat.Name = "PaneChat";
            this.PaneChat.Scroll = ((Tono.GuiWinForm.ScreenPos)(resources.GetObject("PaneChat.Scroll")));
            this.PaneChat.Size = new System.Drawing.Size(600, 102);
            this.PaneChat.TabIndex = 1;
            this.PaneChat.Visible = false;
            this.PaneChat.Zoom = ((Tono.GuiWinForm.XyBase)(resources.GetObject("PaneChat.Zoom")));
            // 
            // PaneTimeline
            // 
            this.PaneTimeline.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PaneTimeline.BackColor = System.Drawing.Color.Blue;
            this.PaneTimeline.IdColor = System.Drawing.Color.Blue;
            this.PaneTimeline.IdText = "Timeline";
            this.PaneTimeline.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.PaneTimeline.IsScrollLockX = false;
            this.PaneTimeline.IsScrollLockY = true;
            this.PaneTimeline.IsZoomLockX = false;
            this.PaneTimeline.IsZoomLockY = true;
            this.PaneTimeline.Location = new System.Drawing.Point(0, 0);
            this.PaneTimeline.Name = "PaneTimeline";
            this.PaneTimeline.Scroll = ((Tono.GuiWinForm.ScreenPos)(resources.GetObject("PaneTimeline.Scroll")));
            this.PaneTimeline.Size = new System.Drawing.Size(600, 24);
            this.PaneTimeline.TabIndex = 0;
            this.PaneTimeline.Visible = false;
            this.PaneTimeline.Zoom = ((Tono.GuiWinForm.XyBase)(resources.GetObject("PaneTimeline.Zoom")));
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BackgroundImage = global::TalkLoggerWinform.Properties.Resources.bg1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(624, 168);
            this.ContextMenuStrip = this.contextMenuStripMain;
            this.Controls.Add(this.GuiViewMain);
            this.Controls.Add(this.menuStripMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "FormMain";
            this.Text = "Talk Logger";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.contextMenuStripMain.ResumeLayout(false);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.GuiViewMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStripMain;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFitToBottom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private Tono.GuiWinForm.TGuiView GuiViewMain;
        private Tono.GuiWinForm.TPane PaneChat;
        private Tono.GuiWinForm.TPane PaneTimeline;
        private Tono.GuiWinForm.TKeyEnabler KeyEnablerMain;
        private System.Windows.Forms.ToolStripMenuItem toolToolStripMenuItem;
    }
}

