namespace CalculationEngine
{
    partial class FormConfig
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
            this.DataGridViewConfig = new System.Windows.Forms.DataGridView();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.labelText = new System.Windows.Forms.Label();
            this.ButtonEnvironments = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewConfig)).BeginInit();
            this.SuspendLayout();
            // 
            // DataGridViewConfig
            // 
            this.DataGridViewConfig.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridViewConfig.Location = new System.Drawing.Point(12, 38);
            this.DataGridViewConfig.Name = "DataGridViewConfig";
            this.DataGridViewConfig.Size = new System.Drawing.Size(468, 276);
            this.DataGridViewConfig.TabIndex = 0;
            this.DataGridViewConfig.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewConfig_CellEndEdit);
            // 
            // ButtonOK
            // 
            this.ButtonOK.Location = new System.Drawing.Point(405, 320);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 23);
            this.ButtonOK.TabIndex = 1;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Location = new System.Drawing.Point(324, 320);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancel.TabIndex = 2;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // labelText
            // 
            this.labelText.AutoSize = true;
            this.labelText.Location = new System.Drawing.Point(12, 13);
            this.labelText.Name = "labelText";
            this.labelText.Size = new System.Drawing.Size(209, 13);
            this.labelText.TabIndex = 3;
            this.labelText.Text = "Edit configuration values in the table below";
            // 
            // ButtonEnvironments
            // 
            this.ButtonEnvironments.Location = new System.Drawing.Point(12, 320);
            this.ButtonEnvironments.Name = "ButtonEnvironments";
            this.ButtonEnvironments.Size = new System.Drawing.Size(81, 23);
            this.ButtonEnvironments.TabIndex = 4;
            this.ButtonEnvironments.Text = "Environments";
            this.ButtonEnvironments.UseVisualStyleBackColor = true;
            this.ButtonEnvironments.Click += new System.EventHandler(this.ButtonEnvironments_Click);
            // 
            // FormConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(492, 352);
            this.Controls.Add(this.ButtonEnvironments);
            this.Controls.Add(this.labelText);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.DataGridViewConfig);
            this.Name = "FormConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewConfig)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DataGridViewConfig;
        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Label labelText;
        private System.Windows.Forms.Button ButtonEnvironments;
    }
}