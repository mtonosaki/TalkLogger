namespace TalkLoggerWinform
{
    partial class FormPreferences
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPreferences));
            this.GroupBoxCognitive = new System.Windows.Forms.GroupBox();
            this.textBoxServiceRegion = new System.Windows.Forms.TextBox();
            this.LabelServiceRegion = new System.Windows.Forms.Label();
            this.textBoxSubscriptionKey = new System.Windows.Forms.TextBox();
            this.LabelSubscriptionKey = new System.Windows.Forms.Label();
            this.GroupBoxAudioChannel1 = new System.Windows.Forms.GroupBox();
            this.comboBoxLoopbackDevice = new System.Windows.Forms.ComboBox();
            this.LabelListeningLoopbackDevice = new System.Windows.Forms.Label();
            this.GroupBoxAudioChannel2 = new System.Windows.Forms.GroupBox();
            this.LabelListeningMicDevice = new System.Windows.Forms.Label();
            this.comboBoxMicDevice = new System.Windows.Forms.ComboBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.LabelWarning = new System.Windows.Forms.Label();
            this.comboBoxLoopbackLanguage = new System.Windows.Forms.ComboBox();
            this.LabelLoopbackLanguage = new System.Windows.Forms.Label();
            this.LabelMicLanguage = new System.Windows.Forms.Label();
            this.comboBoxMicLanguage = new System.Windows.Forms.ComboBox();
            this.GroupBoxCognitive.SuspendLayout();
            this.GroupBoxAudioChannel1.SuspendLayout();
            this.GroupBoxAudioChannel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBoxCognitive
            // 
            this.GroupBoxCognitive.Controls.Add(this.textBoxServiceRegion);
            this.GroupBoxCognitive.Controls.Add(this.LabelServiceRegion);
            this.GroupBoxCognitive.Controls.Add(this.textBoxSubscriptionKey);
            this.GroupBoxCognitive.Controls.Add(this.LabelSubscriptionKey);
            this.GroupBoxCognitive.Location = new System.Drawing.Point(12, 41);
            this.GroupBoxCognitive.Name = "GroupBoxCognitive";
            this.GroupBoxCognitive.Size = new System.Drawing.Size(421, 97);
            this.GroupBoxCognitive.TabIndex = 0;
            this.GroupBoxCognitive.TabStop = false;
            this.GroupBoxCognitive.Text = "AzureCognitiveService";
            // 
            // textBoxServiceRegion
            // 
            this.textBoxServiceRegion.Location = new System.Drawing.Point(146, 61);
            this.textBoxServiceRegion.Name = "textBoxServiceRegion";
            this.textBoxServiceRegion.Size = new System.Drawing.Size(268, 23);
            this.textBoxServiceRegion.TabIndex = 3;
            this.textBoxServiceRegion.WordWrap = false;
            // 
            // LabelServiceRegion
            // 
            this.LabelServiceRegion.AutoSize = true;
            this.LabelServiceRegion.Location = new System.Drawing.Point(7, 65);
            this.LabelServiceRegion.Name = "LabelServiceRegion";
            this.LabelServiceRegion.Size = new System.Drawing.Size(81, 15);
            this.LabelServiceRegion.TabIndex = 2;
            this.LabelServiceRegion.Text = "ServiceRegion";
            // 
            // textBoxSubscriptionKey
            // 
            this.textBoxSubscriptionKey.Location = new System.Drawing.Point(146, 31);
            this.textBoxSubscriptionKey.Name = "textBoxSubscriptionKey";
            this.textBoxSubscriptionKey.Size = new System.Drawing.Size(268, 23);
            this.textBoxSubscriptionKey.TabIndex = 1;
            this.textBoxSubscriptionKey.WordWrap = false;
            // 
            // LabelSubscriptionKey
            // 
            this.LabelSubscriptionKey.AutoSize = true;
            this.LabelSubscriptionKey.Location = new System.Drawing.Point(7, 35);
            this.LabelSubscriptionKey.Name = "LabelSubscriptionKey";
            this.LabelSubscriptionKey.Size = new System.Drawing.Size(92, 15);
            this.LabelSubscriptionKey.TabIndex = 0;
            this.LabelSubscriptionKey.Text = "SubscriptionKey";
            // 
            // GroupBoxAudioChannel1
            // 
            this.GroupBoxAudioChannel1.Controls.Add(this.comboBoxLoopbackLanguage);
            this.GroupBoxAudioChannel1.Controls.Add(this.LabelLoopbackLanguage);
            this.GroupBoxAudioChannel1.Controls.Add(this.comboBoxLoopbackDevice);
            this.GroupBoxAudioChannel1.Controls.Add(this.LabelListeningLoopbackDevice);
            this.GroupBoxAudioChannel1.Location = new System.Drawing.Point(12, 166);
            this.GroupBoxAudioChannel1.Name = "GroupBoxAudioChannel1";
            this.GroupBoxAudioChannel1.Size = new System.Drawing.Size(421, 115);
            this.GroupBoxAudioChannel1.TabIndex = 1;
            this.GroupBoxAudioChannel1.TabStop = false;
            this.GroupBoxAudioChannel1.Text = "AudioChannel1";
            // 
            // comboBoxLoopbackDevice
            // 
            this.comboBoxLoopbackDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLoopbackDevice.FormattingEnabled = true;
            this.comboBoxLoopbackDevice.Location = new System.Drawing.Point(146, 23);
            this.comboBoxLoopbackDevice.Name = "comboBoxLoopbackDevice";
            this.comboBoxLoopbackDevice.Size = new System.Drawing.Size(268, 23);
            this.comboBoxLoopbackDevice.TabIndex = 0;
            // 
            // LabelListeningLoopbackDevice
            // 
            this.LabelListeningLoopbackDevice.AutoSize = true;
            this.LabelListeningLoopbackDevice.Location = new System.Drawing.Point(7, 27);
            this.LabelListeningLoopbackDevice.Name = "LabelListeningLoopbackDevice";
            this.LabelListeningLoopbackDevice.Size = new System.Drawing.Size(90, 15);
            this.LabelListeningLoopbackDevice.TabIndex = 4;
            this.LabelListeningLoopbackDevice.Text = "ListeningDevice";
            // 
            // GroupBoxAudioChannel2
            // 
            this.GroupBoxAudioChannel2.Controls.Add(this.LabelMicLanguage);
            this.GroupBoxAudioChannel2.Controls.Add(this.comboBoxMicLanguage);
            this.GroupBoxAudioChannel2.Controls.Add(this.LabelListeningMicDevice);
            this.GroupBoxAudioChannel2.Controls.Add(this.comboBoxMicDevice);
            this.GroupBoxAudioChannel2.Location = new System.Drawing.Point(12, 289);
            this.GroupBoxAudioChannel2.Name = "GroupBoxAudioChannel2";
            this.GroupBoxAudioChannel2.Size = new System.Drawing.Size(421, 115);
            this.GroupBoxAudioChannel2.TabIndex = 5;
            this.GroupBoxAudioChannel2.TabStop = false;
            this.GroupBoxAudioChannel2.Text = "AudioChannel2";
            // 
            // LabelListeningMicDevice
            // 
            this.LabelListeningMicDevice.AutoSize = true;
            this.LabelListeningMicDevice.Location = new System.Drawing.Point(7, 27);
            this.LabelListeningMicDevice.Name = "LabelListeningMicDevice";
            this.LabelListeningMicDevice.Size = new System.Drawing.Size(90, 15);
            this.LabelListeningMicDevice.TabIndex = 4;
            this.LabelListeningMicDevice.Text = "ListeningDevice";
            // 
            // comboBoxMicDevice
            // 
            this.comboBoxMicDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMicDevice.FormattingEnabled = true;
            this.comboBoxMicDevice.Location = new System.Drawing.Point(146, 23);
            this.comboBoxMicDevice.Name = "comboBoxMicDevice";
            this.comboBoxMicDevice.Size = new System.Drawing.Size(268, 23);
            this.comboBoxMicDevice.TabIndex = 0;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(270, 441);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(351, 441);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // LabelWarning
            // 
            this.LabelWarning.BackColor = System.Drawing.Color.Maroon;
            this.LabelWarning.ForeColor = System.Drawing.Color.Yellow;
            this.LabelWarning.Location = new System.Drawing.Point(12, 9);
            this.LabelWarning.Name = "LabelWarning";
            this.LabelWarning.Size = new System.Drawing.Size(421, 19);
            this.LabelWarning.TabIndex = 8;
            this.LabelWarning.Text = "You can edit when Pause";
            this.LabelWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBoxLoopbackLanguage
            // 
            this.comboBoxLoopbackLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLoopbackLanguage.FormattingEnabled = true;
            this.comboBoxLoopbackLanguage.Location = new System.Drawing.Point(146, 52);
            this.comboBoxLoopbackLanguage.Name = "comboBoxLoopbackLanguage";
            this.comboBoxLoopbackLanguage.Size = new System.Drawing.Size(268, 23);
            this.comboBoxLoopbackLanguage.TabIndex = 5;
            // 
            // LabelLoopbackLanguage
            // 
            this.LabelLoopbackLanguage.AutoSize = true;
            this.LabelLoopbackLanguage.Location = new System.Drawing.Point(7, 56);
            this.LabelLoopbackLanguage.Name = "LabelLoopbackLanguage";
            this.LabelLoopbackLanguage.Size = new System.Drawing.Size(59, 15);
            this.LabelLoopbackLanguage.TabIndex = 6;
            this.LabelLoopbackLanguage.Text = "Language";
            // 
            // LabelMicLanguage
            // 
            this.LabelMicLanguage.AutoSize = true;
            this.LabelMicLanguage.Location = new System.Drawing.Point(7, 56);
            this.LabelMicLanguage.Name = "LabelMicLanguage";
            this.LabelMicLanguage.Size = new System.Drawing.Size(59, 15);
            this.LabelMicLanguage.TabIndex = 6;
            this.LabelMicLanguage.Text = "Language";
            // 
            // comboBoxMicLanguage
            // 
            this.comboBoxMicLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMicLanguage.FormattingEnabled = true;
            this.comboBoxMicLanguage.Location = new System.Drawing.Point(146, 52);
            this.comboBoxMicLanguage.Name = "comboBoxMicLanguage";
            this.comboBoxMicLanguage.Size = new System.Drawing.Size(268, 23);
            this.comboBoxMicLanguage.TabIndex = 5;
            // 
            // FormPreferences
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(453, 476);
            this.Controls.Add(this.LabelWarning);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.GroupBoxAudioChannel2);
            this.Controls.Add(this.GroupBoxAudioChannel1);
            this.Controls.Add(this.GroupBoxCognitive);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormPreferences";
            this.Text = "Preferences";
            this.GroupBoxCognitive.ResumeLayout(false);
            this.GroupBoxCognitive.PerformLayout();
            this.GroupBoxAudioChannel1.ResumeLayout(false);
            this.GroupBoxAudioChannel1.PerformLayout();
            this.GroupBoxAudioChannel2.ResumeLayout(false);
            this.GroupBoxAudioChannel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GroupBoxCognitive;
        private System.Windows.Forms.TextBox textBoxServiceRegion;
        private System.Windows.Forms.Label LabelServiceRegion;
        private System.Windows.Forms.TextBox textBoxSubscriptionKey;
        private System.Windows.Forms.Label LabelSubscriptionKey;
        private System.Windows.Forms.GroupBox GroupBoxAudioChannel1;
        private System.Windows.Forms.Label LabelListeningLoopbackDevice;
        private System.Windows.Forms.ComboBox comboBoxLoopbackDevice;
        private System.Windows.Forms.GroupBox GroupBoxAudioChannel2;
        private System.Windows.Forms.Label LabelListeningMicDevice;
        private System.Windows.Forms.ComboBox comboBoxMicDevice;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label LabelWarning;
        private System.Windows.Forms.ComboBox comboBoxLoopbackLanguage;
        private System.Windows.Forms.Label LabelLoopbackLanguage;
        private System.Windows.Forms.Label LabelMicLanguage;
        private System.Windows.Forms.ComboBox comboBoxMicLanguage;
    }
}