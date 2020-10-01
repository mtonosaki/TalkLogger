namespace TalkLoggerWinform
{
    partial class FormTextLogList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTextLogList));
            this.richTextBoxMain = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // richTextBoxMain
            // 
            this.richTextBoxMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxMain.ForeColor = System.Drawing.SystemColors.WindowText;
            this.richTextBoxMain.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxMain.Name = "richTextBoxMain";
            this.richTextBoxMain.ReadOnly = true;
            this.richTextBoxMain.Size = new System.Drawing.Size(800, 450);
            this.richTextBoxMain.TabIndex = 0;
            this.richTextBoxMain.Text = "";
            this.richTextBoxMain.WordWrap = false;
            this.richTextBoxMain.KeyUp += new System.Windows.Forms.KeyEventHandler(this.richTextBoxMain_KeyUp);
            // 
            // FormTextLogList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.richTextBoxMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormTextLogList";
            this.Text = "Text Log List";
            this.Load += new System.EventHandler(this.FormTextLogList_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxMain;
    }
}