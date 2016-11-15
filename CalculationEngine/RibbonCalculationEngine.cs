using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using System.Windows.Forms;
using CalculationEngine;

namespace CalculationEngine
{
    public partial class Ribbon1
    {
        public Ribbon1()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
            // Get version of model from document
            CalculationModel model = new CalculationModel();
            LabelVersion.Label = model.Version;

            // Disable publish and test buttons if appropriate settings are not configured
            ButtonPublish.Enabled = !(Globals.ThisWorkbook.ReadDocumentProperty("OLM_UrlTest") == "") && !(Globals.ThisWorkbook.ReadDocumentProperty("OLM_NetworkLocation") == "");
            ButtonTest.Enabled = !(Globals.ThisWorkbook.ReadDocumentProperty("OLM_UrlTest") == "");

            // Hide Link-to-Assessment Button for third parties
            if (Globals.ThisWorkbook.ReadDocumentProperty("OLM_ShowLinkButton") != "Y")
            {
                ButtonLinkToAssessment.Visible = false;
            }

            // TODO: Check server version compatibility
            // Check compatibility with server version#
            if (!model.CompatibleWithServer())
            {
                // The method will display dialogs
                // If we failed, then close application
                Globals.ThisWorkbook.Close();
            }
        }

        private void ButtonTest_Click(object sender, RibbonControlEventArgs e)
        {
            FormTest resultsForm = new FormTest();
            resultsForm.ShowDialog();
        }

        private void ButtonPublish_Click(object sender, RibbonControlEventArgs e)
        {
            FormPublishVersion publishForm = new FormPublishVersion();
            DialogResult dr = publishForm.ShowDialog();

            // Test for OK pressed
            if (dr == DialogResult.OK)
            {
                // Get version number and environment, then close Publish dialog
                string version = publishForm.Version;
                string environment = publishForm.Environment;
                publishForm.Dispose();

                // Set model version (temporarily - this will get rolled back if publishing fails)
                CalculationModel model = new CalculationModel();
                string rollbackVersion = model.Version;
                model.SetVersion(version);

                // Set environment to document property for persistence
                // Test() will use the last published environment
                Globals.ThisWorkbook.UpdateDocumentProperty("OLM_TargetEnvironment", environment);

                // Save a copy and upload to service
                if (model.IsValid())
                {
                    if (model.Publish(environment)) 
                    {
                        //Update label with version number
                        LabelVersion.Label = model.Version;
                    }
                    else
                    {
                        // Roll back version number
                        model.SetVersion(rollbackVersion);
                    }
                }
                else
                {
                    // Roll back version number
                    model.SetVersion(rollbackVersion);
                }
            }
            
        }

        private void ButtonConfig_Click(object sender, RibbonControlEventArgs e)
        {
            FormConfig config = new FormConfig();
            DialogResult dr = config.ShowDialog();

            // Test for OK pressed
            if (dr == DialogResult.OK)
            {
                LabelVersion.Label = Globals.ThisWorkbook.ReadDocumentProperty("OLM_ModelVersion");

                // Hide Link-to-Assessment Button for third parties
                if (Globals.ThisWorkbook.ReadDocumentProperty("OLM_ShowLinkButton") != "Y")
                {
                    ButtonLinkToAssessment.Visible = false;
                }
                else
                {
                    ButtonLinkToAssessment.Visible = true;
                }

                // Enable / Disable publish and test buttons if appropriate settings are not configured
                ButtonPublish.Enabled = !(Globals.ThisWorkbook.ReadDocumentProperty("OLM_UrlTest") == "") && !(Globals.ThisWorkbook.ReadDocumentProperty("OLM_NetworkLocation") == "");
                ButtonTest.Enabled = !(Globals.ThisWorkbook.ReadDocumentProperty("OLM_UrlTest") == "");
            }
        }

        private void ButtonLinkToAssessment_Click(object sender, RibbonControlEventArgs e)
        {
            FormLink link = new FormLink();
            link.ShowDialog();
        }

        private void ResetButton_Click(object sender, RibbonControlEventArgs e)
        {
            // Reset all input form values to defaults
            CalculationModel model = new CalculationModel();
            model.Reset();
        }

        private void ButtonAbout_Click(object sender, RibbonControlEventArgs e)
        {
            // Display product version information
            CalculationModel model = new CalculationModel();

            // Add-In version
            Form about = new FormAbout();
            about.ShowDialog();
            
            // TODO: get server version as well
        }

        private void ButtonFormulae_Click(object sender, RibbonControlEventArgs e)
        {
            FormFormulae f = new FormFormulae();
            f.ShowDialog();
        }
    }
}
