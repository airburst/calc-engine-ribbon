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
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
            getVersion();
        }

        private void getVersion()
        {
            CalculationModel model = new CalculationModel();
            LabelProductVersion.Text = model.ProductVersion;
            LabelServerVersion.Text = "Contacting Server...";
            this.Refresh();
        }

        private void getServerVersion()
        {
            // Get server version using web service
            CalculationModel model = new CalculationModel();
            List<string[]> resultsList = new List<string[]>();
            resultsList = model.CheckServerVersion();
            if (resultsList != null)
            {
                string[] record = resultsList[0];

                // Write server version (index 3)
                // Note: order is { compatible, latest, latestversion, message, serverversion }
                LabelServerVersion.Text = record[4];

                // If not compatible, display the server warning message
                if (record[0] != "true")
                {
                    MessageBox.Show(record[3], "Add-In Version", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    var currentVersion = new Version(model.ProductVersion);
                    var availableVersion = new Version(record[2]);

                    if (currentVersion.CompareTo(availableVersion) < 0)
                    {
                        MessageBox.Show("A newer version of this Excel Add-In (" + record[2] + ") is available for download", "Add-In Version", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("There was an error trying to establish the RAS server version", "Communication Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Get server version using web service calls
        // Note: this is only called after the form has been displayed
        private void FormAbout_Shown(object sender, EventArgs e)
        {
            getServerVersion();
        }
    }
}
