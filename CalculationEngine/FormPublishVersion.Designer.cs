namespace CalculationEngine
{
    partial class FormPublishVersion
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
            this.RadioMajor = new System.Windows.Forms.RadioButton();
            this.LabelMajor = new System.Windows.Forms.Label();
            this.LabelMinor = new System.Windows.Forms.Label();
            this.RadioMinor = new System.Windows.Forms.RadioButton();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.groupBoxVersion = new System.Windows.Forms.GroupBox();
            this.labelEnvironment = new System.Windows.Forms.Label();
            this.comboBoxEnvironment = new System.Windows.Forms.ComboBox();
            this.groupBoxVersion.SuspendLayout();
            this.SuspendLayout();
            // 
            // RadioMajor
            // 
            this.RadioMajor.AutoSize = true;
            this.RadioMajor.Location = new System.Drawing.Point(110, 62);
            this.RadioMajor.Name = "RadioMajor";
            this.RadioMajor.Size = new System.Drawing.Size(14, 13);
            this.RadioMajor.TabIndex = 0;
            this.RadioMajor.UseVisualStyleBackColor = true;
            // 
            // LabelMajor
            // 
            this.LabelMajor.AutoSize = true;
            this.LabelMajor.Location = new System.Drawing.Point(23, 62);
            this.LabelMajor.Name = "LabelMajor";
            this.LabelMajor.Size = new System.Drawing.Size(71, 13);
            this.LabelMajor.TabIndex = 4;
            this.LabelMajor.Text = "Major Version";
            // 
            // LabelMinor
            // 
            this.LabelMinor.AutoSize = true;
            this.LabelMinor.Location = new System.Drawing.Point(9, 59);
            this.LabelMinor.Name = "LabelMinor";
            this.LabelMinor.Size = new System.Drawing.Size(71, 13);
            this.LabelMinor.TabIndex = 5;
            this.LabelMinor.Text = "Minor Version";
            // 
            // RadioMinor
            // 
            this.RadioMinor.AutoSize = true;
            this.RadioMinor.Checked = true;
            this.RadioMinor.Location = new System.Drawing.Point(110, 95);
            this.RadioMinor.Name = "RadioMinor";
            this.RadioMinor.Size = new System.Drawing.Size(14, 13);
            this.RadioMinor.TabIndex = 1;
            this.RadioMinor.TabStop = true;
            this.RadioMinor.UseVisualStyleBackColor = true;
            // 
            // ButtonOK
            // 
            this.ButtonOK.Location = new System.Drawing.Point(176, 125);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 23);
            this.ButtonOK.TabIndex = 2;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Location = new System.Drawing.Point(95, 125);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancel.TabIndex = 3;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // groupBoxVersion
            // 
            this.groupBoxVersion.Controls.Add(this.LabelMinor);
            this.groupBoxVersion.Location = new System.Drawing.Point(14, 36);
            this.groupBoxVersion.Name = "groupBoxVersion";
            this.groupBoxVersion.Size = new System.Drawing.Size(237, 82);
            this.groupBoxVersion.TabIndex = 7;
            this.groupBoxVersion.TabStop = false;
            this.groupBoxVersion.Text = "Version";
            // 
            // labelEnvironment
            // 
            this.labelEnvironment.AutoSize = true;
            this.labelEnvironment.Location = new System.Drawing.Point(11, 12);
            this.labelEnvironment.Name = "labelEnvironment";
            this.labelEnvironment.Size = new System.Drawing.Size(66, 13);
            this.labelEnvironment.TabIndex = 8;
            this.labelEnvironment.Text = "Environment";
            // 
            // comboBoxEnvironment
            // 
            this.comboBoxEnvironment.FormattingEnabled = true;
            this.comboBoxEnvironment.Location = new System.Drawing.Point(94, 9);
            this.comboBoxEnvironment.Name = "comboBoxEnvironment";
            this.comboBoxEnvironment.Size = new System.Drawing.Size(156, 21);
            this.comboBoxEnvironment.TabIndex = 9;
            // 
            // FormPublishVersion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(263, 157);
            this.Controls.Add(this.comboBoxEnvironment);
            this.Controls.Add(this.labelEnvironment);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.RadioMinor);
            this.Controls.Add(this.LabelMajor);
            this.Controls.Add(this.RadioMajor);
            this.Controls.Add(this.groupBoxVersion);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPublishVersion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Publish Model";
            this.Load += new System.EventHandler(this.FormPublishVersion_Load);
            this.groupBoxVersion.ResumeLayout(false);
            this.groupBoxVersion.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton RadioMajor;
        private System.Windows.Forms.Label LabelMajor;
        private System.Windows.Forms.Label LabelMinor;
        private System.Windows.Forms.RadioButton RadioMinor;
        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.GroupBox groupBoxVersion;
        private System.Windows.Forms.Label labelEnvironment;
        private System.Windows.Forms.ComboBox comboBoxEnvironment;
    }
}