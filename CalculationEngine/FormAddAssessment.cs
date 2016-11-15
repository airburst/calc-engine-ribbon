using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CalculationEngine
{
    public partial class FormAddAssessment : Form
    {
        public string AssessmentName;
        
        public FormAddAssessment()
        {
            InitializeComponent();
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            AssessmentName = TextBoxAssessment.Text;
            string errMessage = "";

            // Validate entered text
            if (AssessmentName.IndexOf(" ") > -1)
            {
                errMessage = "The assessment name cannot contain a space";
            }

            // Flag any validation errors
            if (errMessage != "")
            {
                MessageBox.Show(errMessage, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                // Set DialogResult to OK
                this.DialogResult = DialogResult.OK;
            }
            // NOTE: the calling form (FormLink) will dispose of this form
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            // Close
            this.Close();
            this.Dispose();
        }
    }
}
