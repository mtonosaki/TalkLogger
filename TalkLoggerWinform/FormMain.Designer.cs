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
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GuiViewMain = new Tono.GuiWinForm.TGuiView(this.components);
            this.KeyEnablerMain = new Tono.GuiWinForm.TKeyEnabler();
            this.labelTalkBarTime = new System.Windows.Forms.Label();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.textBoxTalk = new System.Windows.Forms.RichTextBox();
            this.labelClosing = new System.Windows.Forms.Label();
            this.textBoxTime = new System.Windows.Forms.TextBox();
            this.menuStripMain.SuspendLayout();
            this.GuiViewMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.SuspendLayout();
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
            this.menuStripMain.Size = new System.Drawing.Size(869, 24);
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
            this.GuiViewMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.GuiViewMain.Controls.Add(this.KeyEnablerMain);
            this.GuiViewMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GuiViewMain.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.GuiViewMain.IdText = "GuiViewMain";
            this.GuiViewMain.IsDrawEmptyBackground = false;
            this.GuiViewMain.Location = new System.Drawing.Point(0, 0);
            this.GuiViewMain.Name = "GuiViewMain";
            this.GuiViewMain.Scroll = ((Tono.GuiWinForm.ScreenPos)(resources.GetObject("GuiViewMain.Scroll")));
            this.GuiViewMain.ScrollMute = ((Tono.GuiWinForm.ScreenPos)(resources.GetObject("GuiViewMain.ScrollMute")));
            this.GuiViewMain.Size = new System.Drawing.Size(845, 129);
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
            // labelTalkBarTime
            // 
            this.labelTalkBarTime.AutoSize = true;
            this.labelTalkBarTime.BackColor = System.Drawing.Color.Transparent;
            this.labelTalkBarTime.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTalkBarTime.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.labelTalkBarTime.Location = new System.Drawing.Point(3, 8);
            this.labelTalkBarTime.Name = "labelTalkBarTime";
            this.labelTalkBarTime.Size = new System.Drawing.Size(35, 13);
            this.labelTalkBarTime.TabIndex = 5;
            this.labelTalkBarTime.Text = "12:37";
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainerMain.Location = new System.Drawing.Point(12, 31);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.textBoxTalk);
            this.splitContainerMain.Panel1.Controls.Add(this.labelTalkBarTime);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.GuiViewMain);
            this.splitContainerMain.Size = new System.Drawing.Size(845, 158);
            this.splitContainerMain.SplitterDistance = 25;
            this.splitContainerMain.TabIndex = 7;
            this.splitContainerMain.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainerMain_SplitterMoved);
            // 
            // textBoxTalk
            // 
            this.textBoxTalk.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTalk.AutoWordSelection = true;
            this.textBoxTalk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(64)))), ((int)(((byte)(32)))));
            this.textBoxTalk.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxTalk.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTalk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.textBoxTalk.Location = new System.Drawing.Point(44, 0);
            this.textBoxTalk.Name = "textBoxTalk";
            this.textBoxTalk.Size = new System.Drawing.Size(801, 23);
            this.textBoxTalk.TabIndex = 6;
            this.textBoxTalk.Text = "Hello, What a cool software this is!";
            // 
            // labelClosing
            // 
            this.labelClosing.AutoSize = true;
            this.labelClosing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelClosing.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.labelClosing.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelClosing.ForeColor = System.Drawing.Color.Yellow;
            this.labelClosing.Location = new System.Drawing.Point(130, 100);
            this.labelClosing.Name = "labelClosing";
            this.labelClosing.Size = new System.Drawing.Size(589, 25);
            this.labelClosing.TabIndex = 8;
            this.labelClosing.Text = "Post-processing. Please wait a moment to exit safely...";
            this.labelClosing.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelClosing.UseWaitCursor = true;
            this.labelClosing.Visible = false;
            // 
            // textBoxTime
            // 
            this.textBoxTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxTime.BackColor = System.Drawing.Color.Black;
            this.textBoxTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxTime.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTime.ForeColor = System.Drawing.Color.SkyBlue;
            this.textBoxTime.Location = new System.Drawing.Point(12, 169);
            this.textBoxTime.Name = "textBoxTime";
            this.textBoxTime.Size = new System.Drawing.Size(81, 21);
            this.textBoxTime.TabIndex = 9;
            this.textBoxTime.Text = "00:00:00";
            this.textBoxTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BackgroundImage = global::TalkLoggerWinform.Properties.Resources.bg1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(869, 197);
            this.Controls.Add(this.textBoxTime);
            this.Controls.Add(this.labelClosing);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.menuStripMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripMain;
            this.MinimumSize = new System.Drawing.Size(640, 160);
            this.Name = "FormMain";
            this.Text = "Talk Logger";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.LocationChanged += new System.EventHandler(this.FormMain_LocationChanged);
            this.SizeChanged += new System.EventHandler(this.FormMain_SizeChanged);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.GuiViewMain.ResumeLayout(false);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel1.PerformLayout();
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private Tono.GuiWinForm.TGuiView GuiViewMain;
        private Tono.GuiWinForm.TKeyEnabler KeyEnablerMain;
        private System.Windows.Forms.ToolStripMenuItem toolToolStripMenuItem;
        private System.Windows.Forms.Label labelTalkBarTime;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.RichTextBox textBoxTalk;
        private System.Windows.Forms.Label labelClosing;
        private System.Windows.Forms.TextBox textBoxTime;
    }
}

