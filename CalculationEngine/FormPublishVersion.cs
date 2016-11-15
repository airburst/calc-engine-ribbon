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
    public partial class FormPublishVersion : Form
    {

        public string Version;
        public string Environment;
        
        public FormPublishVersion()
        {
            InitializeComponent();
        }

        private void FormPublishVersion_Load(object sender, EventArgs e)
        {
            // Set values of versions
            CalculationModel model = new CalculationModel();
            this.RadioMajor.Text = model.GetNextVersion("Major");
            this.RadioMinor.Text = model.GetNextVersion("Minor");
            Version = "";
            
            // And Environments
            for (int i = 0; i < model.EnvironmentList.Count; i++)
            {
                this.comboBoxEnvironment.Items.Add(model.EnvironmentList[i].Name);
            }

            this.comboBoxEnvironment.SelectedItem = model.EnvironmentList[0].Name;
            Environment = model.EnvironmentList[0].Name;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            // Set DialogResult to OK
            this.DialogResult = DialogResult.OK;
            
            // Set Version
            if (this.RadioMajor.Checked)
            {
                Version = this.RadioMajor.Text;
            }
            else if (this.RadioMinor.Checked)
            {
                Version = this.RadioMinor.Text;
            }

            // Set Environment
            Environment = this.comboBoxEnvironment.SelectedItem.ToString();

            // NOTE: the calling Ribbon button event will dispose of this form
        }
        
        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
