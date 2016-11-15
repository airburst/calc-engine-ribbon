using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Office.Tools.Excel;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;

namespace CalculationEngine
{
    public partial class ThisWorkbook
    {        
        private void ThisWorkbook_Startup(object sender, System.EventArgs e)
        {
            // Create list of config parameters
            List<string[]> ConfigList = new List<string[]>();

            // Set default parameters
            // String array [Name, PropertyName, Default Value]
            ConfigList.Add(new string[] { "Version Number", "OLM_ModelVersion", "0.0" });
            ConfigList.Add(new string[] { "Calc Engine Url [TEST]", "OLM_UrlTest", "" });
            ConfigList.Add(new string[] { "Calc Engine Url [LIVE]", "OLM_UrlLive", "" });
            ConfigList.Add(new string[] { "Upload Method", "OLM_UploadMethod", "calc/add.cfm" });
            ConfigList.Add(new string[] { "Calculate Method", "OLM_CalculateMethod", "calc/ws/calcService.cfc" });
            ConfigList.Add(new string[] { "Version History Network Path", "OLM_NetworkLocation", "" });
            ConfigList.Add(new string[] { "Show Link Button", "OLM_ShowLinkButton", "Y" });
            ConfigList.Add(new string[] { "Integration Properties Location [TEST]", "OLM_IntegrationPropertiesTest", "" });
            ConfigList.Add(new string[] { "Integration Properties Location [LIVE]", "OLM_IntegrationPropertiesLive", "" });
            ConfigList.Add(new string[] { "Version History Folder", "OLM_VersionFolderName", "Versions" });
            ConfigList.Add(new string[] { "Matched Test", "OLM_MatchText", "OK" });
            ConfigList.Add(new string[] { "Unmatched Text", "OLM_NotMatchText", "ERROR" });
            ConfigList.Add(new string[] { "Default comment markup", "OLM_DefaultCommentTag", "Default" });
            ConfigList.Add(new string[] { "Tolerance", "OLM_Tolerance", "0.005" });
            ConfigList.Add(new string[] { "Use SDS Mode?", "OLM_UseSDSMode", "Y" });
            ConfigList.Add(new string[] { "Publish Timeout Setting", "OLM_Timeout", "20" });

            CalculationModel model = new CalculationModel();

            string propertyVal;
            bool showConfig = false;

            foreach (string[] config in ConfigList)
            {
                propertyVal = model.Initialise(config[1], config[2]);
                if ((propertyVal == "") || (propertyVal == null)) showConfig = true;
            }

            // Show config dialog if any values are not set
            if (showConfig)
            {
                FormConfig config = new FormConfig();
                config.ShowDialog();
            }
        }

        private void ThisWorkbook_Shutdown(object sender, System.EventArgs e)
        {
        }

        //==============================================================================================
        // Update Custom Document Properties (used for persitence of config settings)
        //==============================================================================================
        public void UpdateDocumentProperty(string propertyName, string value)
        {
            Microsoft.Office.Core.DocumentProperties properties;
            properties = (Office.DocumentProperties)Globals.ThisWorkbook.CustomDocumentProperties;

            if (ReadDocumentProperty(propertyName) != null)
            {
                properties[propertyName].Delete();
            }

            properties.Add(propertyName, false,
                Microsoft.Office.Core.MsoDocProperties.msoPropertyTypeString, value);
        }

        public string ReadDocumentProperty(string propertyName)
        {
            Office.DocumentProperties properties;
            properties = (Office.DocumentProperties)Globals.ThisWorkbook.CustomDocumentProperties;

            foreach (Office.DocumentProperty prop in properties)
            {
                if (prop.Name == propertyName)
                {
                    return prop.Value.ToString();
                }
            }
            return null;
        }

        public void DeleteDocumentProperty(string propertyName)
        {
            Office.DocumentProperties properties;
            properties = (Office.DocumentProperties)Globals.ThisWorkbook.CustomDocumentProperties;

            foreach (Office.DocumentProperty prop in properties)
            {
                if (prop.Name == propertyName)
                {
                    prop.Delete();
                }
            }
        }

        //==============================================================================================
        // Returns a list of all custom document properties as key-value string arrays
        //==============================================================================================
        public List<string[]> GetAllConfigProperties()
        {
            List<string[]> configList = new List<string[]>();

            Office.DocumentProperties properties;
            properties = (Office.DocumentProperties)Globals.ThisWorkbook.CustomDocumentProperties;

            foreach (Office.DocumentProperty prop in properties)
            {
                if (prop.Name.Substring(0, 3) == "OLM")
                {
                    configList.Add(new string[] { prop.Name, prop.Value.ToString() });
                }
            }
            return configList;
        }

        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisWorkbook_Startup);
            this.Shutdown += new System.EventHandler(ThisWorkbook_Shutdown);
        }

        #endregion

    }
}
