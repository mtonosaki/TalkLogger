﻿namespace TalkLoggerWinform
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
            this.LabelListeningDevice1 = new System.Windows.Forms.Label();
            this.comboBoxDevice1 = new System.Windows.Forms.ComboBox();
            this.GroupBoxAudioChannel2 = new System.Windows.Forms.GroupBox();
            this.LabelListeningDevice2 = new System.Windows.Forms.Label();
            this.comboBoxDevice2 = new System.Windows.Forms.ComboBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
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
            this.GroupBoxCognitive.Location = new System.Drawing.Point(14, 14);
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
            this.GroupBoxAudioChannel1.Controls.Add(this.LabelListeningDevice1);
            this.GroupBoxAudioChannel1.Controls.Add(this.comboBoxDevice1);
            this.GroupBoxAudioChannel1.Location = new System.Drawing.Point(14, 153);
            this.GroupBoxAudioChannel1.Name = "GroupBoxAudioChannel1";
            this.GroupBoxAudioChannel1.Size = new System.Drawing.Size(421, 115);
            this.GroupBoxAudioChannel1.TabIndex = 1;
            this.GroupBoxAudioChannel1.TabStop = false;
            this.GroupBoxAudioChannel1.Text = "AudioChannel1";
            // 
            // LabelListeningDevice1
            // 
            this.LabelListeningDevice1.AutoSize = true;
            this.LabelListeningDevice1.Location = new System.Drawing.Point(7, 27);
            this.LabelListeningDevice1.Name = "LabelListeningDevice1";
            this.LabelListeningDevice1.Size = new System.Drawing.Size(90, 15);
            this.LabelListeningDevice1.TabIndex = 4;
            this.LabelListeningDevice1.Text = "ListeningDevice";
            // 
            // comboBoxDevice1
            // 
            this.comboBoxDevice1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDevice1.FormattingEnabled = true;
            this.comboBoxDevice1.Location = new System.Drawing.Point(146, 23);
            this.comboBoxDevice1.Name = "comboBoxDevice1";
            this.comboBoxDevice1.Size = new System.Drawing.Size(268, 23);
            this.comboBoxDevice1.TabIndex = 0;
            // 
            // GroupBoxAudioChannel2
            // 
            this.GroupBoxAudioChannel2.Controls.Add(this.LabelListeningDevice2);
            this.GroupBoxAudioChannel2.Controls.Add(this.comboBoxDevice2);
            this.GroupBoxAudioChannel2.Location = new System.Drawing.Point(14, 276);
            this.GroupBoxAudioChannel2.Name = "GroupBoxAudioChannel2";
            this.GroupBoxAudioChannel2.Size = new System.Drawing.Size(421, 115);
            this.GroupBoxAudioChannel2.TabIndex = 5;
            this.GroupBoxAudioChannel2.TabStop = false;
            this.GroupBoxAudioChannel2.Text = "AudioChannel2";
            // 
            // LabelListeningDevice2
            // 
            this.LabelListeningDevice2.AutoSize = true;
            this.LabelListeningDevice2.Location = new System.Drawing.Point(7, 27);
            this.LabelListeningDevice2.Name = "LabelListeningDevice2";
            this.LabelListeningDevice2.Size = new System.Drawing.Size(90, 15);
            this.LabelListeningDevice2.TabIndex = 4;
            this.LabelListeningDevice2.Text = "ListeningDevice";
            // 
            // comboBoxDevice2
            // 
            this.comboBoxDevice2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDevice2.FormattingEnabled = true;
            this.comboBoxDevice2.Location = new System.Drawing.Point(146, 23);
            this.comboBoxDevice2.Name = "comboBoxDevice2";
            this.comboBoxDevice2.Size = new System.Drawing.Size(268, 23);
            this.comboBoxDevice2.TabIndex = 0;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(272, 416);
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
            this.buttonCancel.Location = new System.Drawing.Point(353, 416);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // FormPreferences
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(453, 451);
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
        private System.Windows.Forms.Label LabelListeningDevice1;
        private System.Windows.Forms.ComboBox comboBoxDevice1;
        private System.Windows.Forms.GroupBox GroupBoxAudioChannel2;
        private System.Windows.Forms.Label LabelListeningDevice2;
        private System.Windows.Forms.ComboBox comboBoxDevice2;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
    }
}