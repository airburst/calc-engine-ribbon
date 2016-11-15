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
    public partial class AddEnvironment : Form
    {
        public string EnvironmentName;
        
        public AddEnvironment()
        {
            InitializeComponent();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            // Close
            this.Close();
            this.Dispose();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            EnvironmentName = TextBoxEnvironment.Text;
            string errMessage = "";

            // Validate entered text
            if (EnvironmentName.IndexOf(" ") > -1)
            {
                errMessage = "The environment name cannot contain a space";
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

        private void AddEnvironment_Load(object sender, EventArgs e)
        {
            // Check whether an environment was set (rename)
            // If so, initialise the textbox
            if (EnvironmentName != "")
            {
                TextBoxEnvironment.Text = EnvironmentName;
            }
        }
    }
}
