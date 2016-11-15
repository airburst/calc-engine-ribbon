namespace CalculationEngine
{
    partial class FormFormulae
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
            this.label1 = new System.Windows.Forms.Label();
            this.DataGridViewFormulae = new System.Windows.Forms.DataGridView();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.LabelErrors = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewFormulae)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(228, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "The following cells are calculated in this model:";
            // 
            // DataGridViewFormulae
            // 
            this.DataGridViewFormulae.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridViewFormulae.Location = new System.Drawing.Point(13, 30);
            this.DataGridViewFormulae.Name = "DataGridViewFormulae";
            this.DataGridViewFormulae.Size = new System.Drawing.Size(819, 451);
            this.DataGridViewFormulae.TabIndex = 1;
            // 
            // ButtonOK
            // 
            this.ButtonOK.Location = new System.Drawing.Point(757, 487);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 23);
            this.ButtonOK.TabIndex = 2;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // LabelErrors
            // 
            this.LabelErrors.AutoSize = true;
            this.LabelErrors.Location = new System.Drawing.Point(16, 488);
            this.LabelErrors.Name = "LabelErrors";
            this.LabelErrors.Size = new System.Drawing.Size(56, 13);
            this.LabelErrors.TabIndex = 3;
            this.LabelErrors.Text = "Summary: ";
            // 
            // FormFormulae
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 522);
            this.Controls.Add(this.LabelErrors);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.DataGridViewFormulae);
            this.Controls.Add(this.label1);
            this.Name = "FormFormulae";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Calculated Cells";
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewFormulae)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView DataGridViewFormulae;
        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.Label LabelErrors;
    }
}