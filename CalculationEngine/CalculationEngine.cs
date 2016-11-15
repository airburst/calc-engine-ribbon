using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Office.Tools.Excel;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;

namespace CalculationEngine
{
    // Class definition for an assessment
    public class Assessment
    {
        public string Name {get; set;}
        public Boolean Linked {get; set;}
        public string LinkedModel { get; set; }
    }
    
    // Main Class
    public class CalculationModel
    {
        //==============================================================================================
        // Fields
        //==============================================================================================
        public string Name;
        public string Version;
        public string CalculateUrlTest;
        public string CalculateUrlLive;
        public string UploadUrlTest;
        public string UploadUrlLive;
        public string IntegrationPropertiesTest;
        public string IntegrationPropertiesLive;
        public string MatchText;
        public string NotMatchText;
        public double Tolerance;
        public string ProductVersion;
        public string DefaultCommentTag;

        private string urlTest;
        private string urlLive;
        private string VersionFolderName;
        private string NetworkLocation;
        private string SDSMode;

        //==============================================================================================
        // Constructor
        //==============================================================================================
        public CalculationModel()
        {
            // Initialise variables
            Name = GetWorkbookStub();
            Version = Globals.ThisWorkbook.ReadDocumentProperty("OLM_ModelVersion");
            // TEST Endpoints
            urlTest = AddSlash(Globals.ThisWorkbook.ReadDocumentProperty("OLM_UrlTest"));  
            CalculateUrlTest = urlTest + Globals.ThisWorkbook.ReadDocumentProperty("OLM_CalculateMethod");  
            UploadUrlTest = urlTest + Globals.ThisWorkbook.ReadDocumentProperty("OLM_UploadMethod");
            IntegrationPropertiesTest = Globals.ThisWorkbook.ReadDocumentProperty("OLM_IntegrationPropertiesTest");
            // LIVE Endpoints
            urlLive = AddSlash(Globals.ThisWorkbook.ReadDocumentProperty("OLM_UrlLive"));  
            CalculateUrlLive = urlLive + Globals.ThisWorkbook.ReadDocumentProperty("OLM_CalculateMethod");
            IntegrationPropertiesLive = Globals.ThisWorkbook.ReadDocumentProperty("OLM_IntegrationPropertiesLive");
            UploadUrlLive = urlLive + Globals.ThisWorkbook.ReadDocumentProperty("OLM_UploadMethod");
            // General
            MatchText = Globals.ThisWorkbook.ReadDocumentProperty("OLM_MatchText");
            NotMatchText = Globals.ThisWorkbook.ReadDocumentProperty("OLM_NotMatchText");
            VersionFolderName = Globals.ThisWorkbook.ReadDocumentProperty("OLM_VersionFolderName");
            DefaultCommentTag = Globals.ThisWorkbook.ReadDocumentProperty("OLM_DefaultCommentTag");   
            NetworkLocation = Globals.ThisWorkbook.ReadDocumentProperty("OLM_NetworkLocation");
            SDSMode = Globals.ThisWorkbook.ReadDocumentProperty("OLM_UseSDSMode");

            // Cast Tolerance as double
            if (!double.TryParse(Globals.ThisWorkbook.ReadDocumentProperty("OLM_Tolerance"), out Tolerance))
            {
                // Default
                Tolerance = 0.005;
            };

            // Set Product version from AssemblyInfo
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            ProductVersion = assembly.GetName().Version.ToString();
        }


        //==============================================================================================
        // Methods
        //==============================================================================================


        //==============================================================================================
        // Set version number and document property for persistence
        //==============================================================================================
        public string Initialise(string propertyName, string defaultPropertyVal)
        {
            //Set values from Document Properties
            string propertyVal;

            propertyVal = Globals.ThisWorkbook.ReadDocumentProperty(propertyName);
            if ((propertyVal == null) || (propertyVal == ""))
            {
                // Set default value
                propertyVal = defaultPropertyVal;
                Globals.ThisWorkbook.UpdateDocumentProperty(propertyName, propertyVal);
            }
            return propertyVal;
        }


        //==============================================================================================
        // Set version number and document property for persistence
        //==============================================================================================
        public void SetVersion(string newVersion)
        {
            Globals.ThisWorkbook.UpdateDocumentProperty("OLM_ModelVersion", newVersion);
            Version = newVersion;
        }


        //==============================================================================================
        // Return a list of inputs (cells flagged with Comment Type 'I')
        //==============================================================================================
        public List<string[]> InputsList()
        {
            List<string[]> inputs = new List<string[]>();
            inputs = GetTaggedCells("I"); 
            return inputs;
        }

        //==============================================================================================
        // Overloaded Method
        //==============================================================================================
        // Return a list of inputs (cells flagged with Comment Type 'I')
        // Forces update mode iun GetTaggedCells
        //==============================================================================================
        public List<string[]> InputsList(bool updateComments)
        {
            List<string[]> inputs = new List<string[]>();
            inputs = GetTaggedCells("I", updateComments);
            return inputs;
        }

        //==============================================================================================
        // Return a list of outputs (cells flagged with Comment Type 'O')
        //==============================================================================================
        public List<string[]> OutputsList()
        {
            List<string[]> outputs = new List<string[]>();
            outputs = GetTaggedCells("O");
            return outputs;
        }

        //==============================================================================================
        // Overloaded Method
        //==============================================================================================
        // Return a list of outputs (cells flagged with Comment Type 'O')
        // Forces update mode iun GetTaggedCells
        //==============================================================================================
        public List<string[]> OutputsList(bool updateComments)
        {
            List<string[]> outputs = new List<string[]>();
            outputs = GetTaggedCells("O", updateComments);
            return outputs;
        }

        //==============================================================================================
        // Publish will upload a copy to server and return new version number
        // Pre-validates for Excel Errors, no I/O comments, etc.
        //==============================================================================================
        public bool Publish(string Environment)
        {
            // Set UploadUrl
            string uploadUrl;
            if (Environment == "LIVE")
            {
                uploadUrl = this.UploadUrlLive;
            }
            else
            {
                uploadUrl = this.UploadUrlTest;
            }

            // Reset all input cells to default values
            this.Reset();
            
            // Save a versioned copy of the file
            string filepath = this.SaveModel();
            if (filepath == null)
            {
                return false;
            }
            else {
                // Read file data
                FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                fs.Close();

                // Generate post objects to match form controls
                Dictionary<string, object> postParameters = new Dictionary<string, object>();
                postParameters.Add("overwrite", "true");
                postParameters.Add("validate", "true");
                // Note: this expects xlsx file format
                postParameters.Add("workbook", new FormUpload.FileParameter(data, filepath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));

                // Create request and receive response
                string userAgent = "";
                HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(uploadUrl, userAgent, postParameters, ProductVersion);
                if (webResponse != null)
                {
                    // Process response
                    StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                    string fullResponse = responseReader.ReadToEnd();

                    // Success Message; return Header Message from Calculon
                    MessageBox.Show(webResponse.Headers["Message"]);
                    webResponse.Close();

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        //==============================================================================================
        // Sends inputs from workbook to web service and matches results; returns list of results
        //==============================================================================================
        public List<string[]> Test()
        {
            // Send Inputs to Calculation Engine Web Service 
            // Get current Target Environment (TEST or LIVE, as set)  Default is TEST
            string environment = Globals.ThisWorkbook.ReadDocumentProperty("OLM_TargetEnvironment");
            if ((environment == null) || (environment == "")) environment = "TEST";

            //Create request message
            string strRequest = CreateTestRequestXml();

            // Send request
            string resultsXml = this.SendXml(environment, strRequest);
            if (resultsXml != null)
            {

                List<string[]> resultsList = new List<string[]>();
                List<string[]> mergeList = new List<string[]>();

                try
                {
                    //Load response xml
                    XDocument xDoc = XDocument.Parse(resultsXml);

                    //Parse xml ito collection                               
                    IEnumerable<XElement> items = xDoc.Descendants("item");
                    IEnumerable<XElement> item_list = from item in items
                                                        select item;

                    foreach (XElement element in item_list)
                    {
                        string key = element.Element("key").Value.ToString();
                        string val = element.Element("value").Value.ToString();
                        resultsList.Add(new string[] { key, val });
                    }
                    // Sort list by element name [0]
                    resultsList.Sort(delegate(string[] r1, string[] r2) { return r1[0].CompareTo(r2[0]); });

                    //Merge results from web service with spreadsheet values
                    mergeList = this.MergeResults(resultsList);

                    return mergeList;
                }
                catch (NotSupportedException nsEx)
                //Handles bad response from web service
                {
                    //Display error message
                    MessageBox.Show("Error response from Calculation Engine service: \n\n" + resultsXml + "\n\n" + nsEx.ToString());
                    return null;
                }
                catch (XmlException xmlEx)
                {
                    //Display error message
                    MessageBox.Show("Error parsing XML response: \n\n" + resultsXml + "\n\n" + xmlEx.ToString());
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        //==============================================================================================
        // Sends inputs from workbook to web service and matches results; returns list of results
        //==============================================================================================
        public List<string[]> CheckServerVersion()
        {
            // Send Inputs to Calculation Engine Web Service 
            // Get current Target Environment (TEST or LIVE, as set)  Default is TEST
            string environment = Globals.ThisWorkbook.ReadDocumentProperty("OLM_TargetEnvironment");
            if ((environment == null) || (environment == "")) environment = "TEST";

            //Create request message
            string strRequest = CreateVersionCheckRequestXml();

            // Send request
            string responseXml = this.SendXml(environment, strRequest);
            if (responseXml != null)
            {

                List<string[]> resultsList = new List<string[]>();

                try
                {
                    //Load response xml
                    XDocument xDoc = XDocument.Parse(responseXml);

                    // Parse response values from XML
                    IEnumerable<XElement> results = xDoc.Descendants("multiRef");
                    foreach (XElement element in results)
                    {
                        string compatible = element.Element("compatible").Value.ToString();
                        string latest = element.Element("latest").Value.ToString();
                        string latestversion = element.Element("latestversion").Value.ToString();
                        string message = element.Element("message").Value.ToString();
                        string serverversion = element.Element("serverversion").Value.ToString();

                        resultsList.Add(new string[] { compatible, latest, latestversion, message, serverversion });
                    }

                    return resultsList;
                }
                catch (NotSupportedException ex)
                //Handles bad response from web service
                {
                    //Display error message
                    MessageBox.Show("Error response from Calculation Engine service (versionCheck method): \n\n" + responseXml + "\n\n" + ex.Message);
                    return null;
                }
                catch (XmlException ex) {
                    //Display error message: hidden because it normally gets thrown when server version < 1.2
                    string msg = ex.Message;
                    //MessageBox.Show("Error checking server version XML: \n\n" + responseXml + "\n\n" + ex.Message);
                    return null;
                }
                catch (Exception ex)
                {
                    //Display generic error message
                    MessageBox.Show("Error checking server version: \n\n" + responseXml + "\n\n" + ex.Message);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        //==============================================================================================
        // Test whether current version of Add-In is compatible with server version
        //==============================================================================================
        public bool CompatibleWithServer()
        {
            // Get server version using web service
            List<string[]> resultsList = new List<string[]>();
            resultsList = CheckServerVersion();

            if (resultsList != null)
            {
                string[] record = resultsList[0];
                // Note: order is { compatible, latest, latestversion, message, serverversion }
                string message = record[4];

                // If not compatible, display the server warning message
                if (record[0] != "true")
                {
                    MessageBox.Show(record[3], "Add-In Version", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                else
                {
                    var currentVersion = new Version(ProductVersion);
                    var availableVersion = new Version(record[2]);

                    if (currentVersion.CompareTo(availableVersion) < 0)
                    {
                        MessageBox.Show("A newer version of this Excel Add-In (" + record[2] + ") is available for download", "Add-In Version", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return true;
                }
            }
            else
            {
                // There is an issue, but we should not kill the model
                return true;
            }
        }

        //==============================================================================================
        // Merge results from web service response with output cells in spreadsheet
        //==============================================================================================
        public List<string[]> MergeResults(List<string[]> resultsList)
        {
            List<string[]> mergeList = new List<string[]>();
            List<string[]> tempList = new List<string[]>();
            tempList = this.OutputsList();
            string match;

            //Set tolerance level (0.005 by default)
            double r = 0.0;
            double o = 0.0;

            //Iterate over results list and look for matches a
            foreach (string[] result in resultsList)
            {
                foreach (string[] output in tempList)
                {
                    //Names in first index of arrays
                    if (result[0] == output[0])
                    {
                        //Test whether results are the same
                        if (double.TryParse(result[1], out r) && double.TryParse(output[1], out o))
                        {
                            // Double compare within tolerance
                            if (Math.Abs(r - o) < Tolerance)
                            {
                                match = this.MatchText;
                            }
                            else
                            {
                                match = this.NotMatchText;
                            }
                            //Add to new merge list
                            mergeList.Add(new string[] { result[0], output[1], result[1], match });
                            break;
                        }
                        else
                        {
                            // String compare
                            if (result[1] == output[1])
                            {
                                match = this.MatchText;
                            }
                            else
                            {
                                match = this.NotMatchText;
                            }
                            //Add to new merge list
                            mergeList.Add(new string[] { result[0], output[1], result[1], match });
                            break;
                        }
                    }
                }
            }
            return mergeList;
        }


        //==============================================================================================
        // Validate the workbook; return false if there are errors
        //==============================================================================================
        public bool IsValid()
        {
            bool isValid = true;

            // No inputs or outputs commented - model will not work as a service
            List<string[]> inputsList = new List<string[]>();
            List<string> dupes = new List<string>();

            inputsList = this.InputsList();
            if (inputsList.Count() == 0)
            {
                MessageBox.Show("You have not defined any input comments in this model.  It will not work as a calculation service without inputs.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                isValid = false;
            }

            // Test for duplicate inputs
            string dupeMsg = "";
            dupes = CheckDuplicates(inputsList);
            if (dupes.Count > 0)
            {
                for (int i = 0; i < dupes.Count; i++)
                {
                    dupeMsg += dupes[i] + "\n";
                }
                MessageBox.Show("You have duplicate inputs:\n\n" + dupeMsg, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                isValid = false;
            }

            List<string[]> outputsList = new List<string[]>();
            outputsList = this.OutputsList();
            if (outputsList.Count() == 0)
            {
                MessageBox.Show("You have not defined any output comments in this model.  It will not work as a calculation service without outputs.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                isValid = false;
            }

            // Test for duplicate outputs
            dupeMsg = "";
            dupes = CheckDuplicates(outputsList);
            if (dupes.Count > 0)
            {
                for (int i = 0; i < dupes.Count; i++)
                {
                    dupeMsg += dupes[i] + "\n";
                }
                MessageBox.Show("You have duplicate outputs:\n\n" + dupeMsg, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                isValid = false;
            }

            // Test for errors in formulae
            List<string[]> formulaeErrors = this.ListFormulae();

            // Test for error count in last record (column 3): should be 0
            // Note, subtract 1 because the array is zero-based but Count is one-based
            if (formulaeErrors[formulaeErrors.Count - 1][3] != "0")
            {
                DialogResult dr = MessageBox.Show("There is an error in one or more formulae within this model.  You can use the Calculations screen for more information.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (dr == DialogResult.OK)
                {
                    FormFormulae f = new FormFormulae();
                    f.ShowDialog();
                }
                isValid = false;
            }

            // SDS Mode tests
            if (SDSMode == "Y")
            {
                // Test for SDS sheet
                if (!CheckSDSSheetExists())
                {
                    MessageBox.Show("There is no SDS tab in this workbook; it will not score in SDS mode", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    isValid = false;
                }
            }
            
            return isValid;
        }

        //==============================================================================================
        // Increment Version
        //==============================================================================================
        public string GetNextVersion(string incrementLevel)
        {
            string v = this.Version;
            string[] vParts = v.Split('.');
            int majorVersion = Convert.ToInt32(vParts[0]);
            int minorVersion = Convert.ToInt32(vParts[1]);

            switch (incrementLevel)
            {
                case "Major":
                    majorVersion += 1;
                    minorVersion = 0;
                    break;
                case "Minor":
                    minorVersion += 1;
                    break;
            }

            return majorVersion + "." + minorVersion;
        }


        //==============================================================================================
        // Return a list of all assessments from properties file
        //==============================================================================================
        public List<Assessment> GetAllAssessmentsFromPropertiesFile()
        {
            
            // Get current Target Environment (TEST or LIVE, as set)  Default is TEST
            string environment = Globals.ThisWorkbook.ReadDocumentProperty("OLM_TargetEnvironment");
            if ((environment == null) || (environment == "")) environment = "TEST";
            string propertiesPath;
            if (environment == "LIVE")
            {
                propertiesPath = Globals.ThisWorkbook.ReadDocumentProperty("OLM_IntegrationPropertiesLive");
            }
            else
            {
                propertiesPath = Globals.ThisWorkbook.ReadDocumentProperty("OLM_IntegrationPropertiesTest");
            }

            List<Assessment> assessments = new List<Assessment>();

            // Read lines from file
            try
            {
                string[] lines = System.IO.File.ReadAllLines(propertiesPath);

                // Search for the pattern calculon.sds.question.set.types
                foreach (string line in lines)
                {
                    if (line.IndexOf("calculon.sds") != -1)
                    {
                        

                        string[] tokens = line.Split('=');
                        string[] names = tokens[1].Split(',');
                        foreach (string name in names)
                        {
                            Assessment assessment = new Assessment();
                            // Upper case the assessment name
                            assessment.Name = name.ToUpper();
                            // Assume unlinked - we will check shortly
                            assessment.Linked = false;
                            assessments.Add(assessment);
                        }
                    }

                    // Now check for links to models
                    // Occurence of link mapping
                    if (line.IndexOf(".xlsx") != -1)
                    {
                        // Split on = and read assessment name to the left
                        string[] tokens = line.Split('=');
                        foreach (Assessment a in assessments)
                        {
                            if (a.Name == tokens[0])
                            {
                                a.Linked = true;
                                a.LinkedModel = tokens[1];
                            }
                        }
                    }
                }
                return assessments;
            }
            catch (Exception ex)
            {
                MessageBox.Show("The integration properties file '" + propertiesPath + "' does not exist or is unavailable.  Please check your settings.\n\n" + ex.Message, "Integration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }

        //==============================================================================================
        // Return a list of LINKED assessments from properties file
        //==============================================================================================
        public List<Assessment> GetLinkedAssessmentsFromPropertiesFile()
        {
            // Get current Target Environment (TEST or LIVE, as set)  Default is TEST
            string environment = Globals.ThisWorkbook.ReadDocumentProperty("OLM_TargetEnvironment");
            if ((environment == null) || (environment == "")) environment = "TEST";
            string propertiesPath;
            if (environment == "LIVE")
            {
                propertiesPath = Globals.ThisWorkbook.ReadDocumentProperty("OLM_IntegrationPropertiesLive");
            }
            else
            {
                propertiesPath = Globals.ThisWorkbook.ReadDocumentProperty("OLM_IntegrationPropertiesTest");
            }

            List<Assessment> assessments = new List<Assessment>();

            // Read lines from file
            string[] lines = System.IO.File.ReadAllLines(propertiesPath);
            
            // Search for the current RAS model name
            string find = Name + ".xlsx";

            foreach (string line in lines)
            {
                if (line.Length > find.Length)  {
                    if (line.IndexOf(find) != -1)
                    {
                        Assessment assessment = new Assessment();

                        // Assumes that the name will exist on right side of = sign
                        string[] tokens = line.Split('=');
                        assessment.Name = tokens[0].ToUpper();
                        assessment.Linked = true;
                        assessment.LinkedModel = tokens[1];
                        assessments.Add(assessment);
                    }
                }
            }
            return assessments;
        }

        //==============================================================================================
        // Write assessments back to properties file
        //==============================================================================================
        public void UpdatePropertiesFile(List<string> AllAssessments, List<string> LinkedAssessments)
        {
            // Get current Target Environment (TEST or LIVE, as set)  Default is TEST
            string environment = Globals.ThisWorkbook.ReadDocumentProperty("OLM_TargetEnvironment");
            if ((environment == null) || (environment == "")) environment = "TEST";
            string propertiesPath;
            if (environment == "LIVE")
            {
                propertiesPath = Globals.ThisWorkbook.ReadDocumentProperty("OLM_IntegrationPropertiesLive");
            }
            else
            {
                propertiesPath = Globals.ThisWorkbook.ReadDocumentProperty("OLM_IntegrationPropertiesTest");
            }

            // Read lines from file
            List<string> output = new List<string>();
            string[] lines = System.IO.File.ReadAllLines(propertiesPath);

            // Search for replace patterns in the file
            foreach (string line in lines)
            {
                // All assessments mapping line
                if (line.IndexOf("calculon.sds") != -1)
                {
                    // Append all assessments to end of this line
                    string allAssessments = "calculon.sds.application.codes=";
                    int cnt = 0;
                    foreach (string asmt in AllAssessments)
                    {
                        allAssessments += asmt;
                        cnt += 1;
                        if (cnt < AllAssessments.Count) allAssessments += ",";
                    }
                    // Write line
                    output.Add(allAssessments);
                }
                else
                {
                    // Occurence of link mapping
                    if (line.IndexOf(Name + ".xlsx") != -1)
                    {
                        // Remove the line -- do nothing.  We will append all new mappings at the end
                    }
                    else
                    {
                        // Add the line to temp list
                        output.Add(line);
                    }
                }
            }
            // Append new spreadsheet mappings
            foreach (string link in LinkedAssessments)
            {
                output.Add(link + "=" + Name + ".xlsx");
            }

            // Write to file
            try
            {
                System.IO.File.WriteAllLines(propertiesPath, output.ToArray());  
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to update the properties file '" + propertiesPath + "'.  Please check your settings and permissions.\n\n" + ex.Message, "Integration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        //==============================================================================================
        // Get a list of all formulae used in the workbook
        //==============================================================================================
        public List<string[]> ListFormulae()
        {
            // Declare list to hold all discovered formulae in workbook
            List<string[]> formulaeList = new List<string[]>();
            int errorCount = 0;

            // Create list of supported Excel functions
            ExcelSupportedFunctions fn = new ExcelSupportedFunctions();
            List<string> supportedList = fn.FunctionList;

            //Walk through each worksheet and get cell formulae
            foreach (Excel.Worksheet ws in Globals.ThisWorkbook.Worksheets)
            {
                //Establish used range of worksheet
                Excel.Range rg = GetRealRange(ws);

                //Iterate over cells to find formulae
                for (int i = 1; i <= rg.Rows.Count; i++)
                {
                    for (int j = 1; j <= rg.Columns.Count; j++)
                    {
                        // Test for formula
                        if (rg.Cells[i, j].HasFormula)
                        {
                            string formula = rg.Cells[i, j].Formula;
                            string error = "";

                            // Test for Errors

                            // Test for #REF
                            if (formula.Contains("#REF"))
                            {
                                error += "#REF";
                                errorCount += 1;
                            }

                            // Test for External reference
                            // Patterns are [Workbook]!Cell or 'Workbook.xlsx'!Cell
                            if ((formula.Contains("[")) || (formula.Contains("\\")))
                            {
                                if (error != "") { error += " | "; }
                                error += "External Reference";
                                errorCount += 1;
                            }

                            // Test for unsupported functions
                            // Tokenise the formula into consituent parts
                            ExcelFormula excelFormula = new ExcelFormula(formula);
                            ExcelFormulaToken previousToken = null;
                            Excel.Range target = null;

                            foreach (ExcelFormulaToken token in excelFormula)
                            {
                                // Test each function
                                if ((token.Type == ExcelFormulaTokenType.Function) && (token.Subtype == ExcelFormulaTokenSubtype.Start))
                                {
                                    // Test for inclusion in supported list
                                    if (!supportedList.Contains(token.Value))
                                    {
                                        if (error != "") { error += " | "; }
                                        error += "Unsupported Function [" + token.Value + "]";
                                        errorCount += 1;
                                    }
                                }

                                // Test for math operations on text operands
                                // Math followed by Text || Text followed by Math
                                if (previousToken != null)
                                {                                    
                                    // Scenario 1: test to right of math operator
                                    if ((token.Type == ExcelFormulaTokenType.Operand) && (previousToken.Subtype == ExcelFormulaTokenSubtype.Math))
                                    {
                                        // Test for text in formula
                                        if (token.Subtype == ExcelFormulaTokenSubtype.Text)
                                        {
                                            if (error != "") { error += " | "; }
                                            error += "Mathematical operation on text";
                                            errorCount += 1;
                                        }

                                        // Test for range in formula
                                        if (token.Subtype == ExcelFormulaTokenSubtype.Range)
                                        {
                                            // Establish whether token value is a named range
                                            target = GetRangeFromName(token.Value);
                                            if (target == null)
                                            {
                                                // Get range from the token 
                                                // First; we need to establish whether the token range is off-sheet
                                                // It will contain a !
                                                Excel.Worksheet w;
                                                string[] worksheet = token.Value.Split('!');
                                                if (worksheet.Count() > 1)
                                                {
                                                    // Set worksheet to target
                                                    w = Globals.ThisWorkbook.Worksheets[worksheet[0]];
                                                    target = w.get_Range(worksheet[1]);
                                                }
                                                else
                                                {
                                                    // Use current worksheet
                                                    target = ws.get_Range(token.Value);
                                                }
                                            }
                                            
                                            if (target.Cells[1,1].Value != null)
                                            {
                                                // Test for data type of value == string
                                                if (target.Cells[1, 1].Value.GetType().ToString() == "System.String")
                                                {
                                                    if (error != "") { error += " | "; }
                                                    error += "Mathematical operation on text";
                                                    errorCount += 1;
                                                }
                                            }  
                                        }
                                    }

                                    // Scenario 2: test to left of math operator
                                    if ((previousToken.Type == ExcelFormulaTokenType.Operand) && (token.Subtype == ExcelFormulaTokenSubtype.Math))
                                    {
                                        // Test for text in formula
                                        if (previousToken.Subtype == ExcelFormulaTokenSubtype.Text)
                                        {
                                            if (error != "") { error += " | "; }
                                            error += "Mathematical operation on text";
                                            errorCount += 1;
                                        }

                                        // Test for range in formula
                                        if (previousToken.Subtype == ExcelFormulaTokenSubtype.Range)
                                        {
                                            // Establish whether token value is a named range
                                            target = GetRangeFromName(previousToken.Value);
                                            if (target == null)
                                            {
                                                // Get range from the token 
                                                // First; we need to establish whether the token range is off-sheet
                                                // It will contain a !
                                                Excel.Worksheet w;
                                                string[] worksheet = previousToken.Value.Split('!');
                                                if (worksheet.Count() > 1)
                                                {
                                                    // Set worksheet to target
                                                    w = Globals.ThisWorkbook.Worksheets[worksheet[0]];
                                                    target = w.get_Range(worksheet[1]);
                                                }
                                                else
                                                {
                                                    // Use current worksheet
                                                    target = ws.get_Range(previousToken.Value);
                                                }
                                            }
                                            if (target.Cells[1, 1].Value != null)
                                            {
                                                // Test for data type of value == string
                                                if (target.Cells[1, 1].Value.GetType().ToString() == "System.String")
                                                {
                                                    if (error != "") { error += " | "; }
                                                    error += "Mathematical operation on text";
                                                    errorCount += 1;
                                                }
                                            }
                                        }
                                    }

                                }
                                // Set previous token to current for next iteration
                                previousToken = token;
                            }

                            formulaeList.Add(new string[] { ws.Name, this.GetExcelColumnName(j) + i.ToString(), formula, error });
                        }
                    }
                }
            }
            // Add summary record
            formulaeList.Add(new string[] { "SUMMARY", "", "Formulae: " + formulaeList.Count() + " ; Errors: " + errorCount, errorCount.ToString() });
            return formulaeList;
        }

        //==============================================================================================
        // Reset all input cells to default values if set in cell comments
        //==============================================================================================
        public void Reset()
        {
            // GetTaggedCells will update cells when matching against this default type
            List<string[]> inputDefaults = GetTaggedCells(this.DefaultCommentTag);
        }

        //==============================================================================================
        // Returns a list of all named ranges (Name string)
        //==============================================================================================
        public List<string> ListNames()
        {
            List<string> names = new List<string>();
            //string msg = "";
            foreach (Excel.Name n in Globals.ThisWorkbook.Application.Names)
            {
                Excel.Worksheet w = Globals.ThisWorkbook.Worksheets.get_Item(n.RefersToRange.Worksheet.Index);
                Excel.Range r = w.get_Range(n.RefersToRange.Address);
                names.Add(n.Name);
                //msg += n.Name + n.RefersToRange.Worksheet.Name + "Rows: " + r.Rows.Count + "Cols:" + r.Columns.Count;
            }
            return names;
        }


        //==============================================================================================
        // Returns an Excel.Range object for supplied (string) named range
        //==============================================================================================
        public Excel.Range GetRangeFromName(string name)
        {
            Excel.Range r = null;

            // Walk through all names
            foreach (Excel.Name n in Globals.ThisWorkbook.Application.Names)
            {
                // Find match
                if (n.Name == name)
                {
                    // Get range
                    Excel.Worksheet w = Globals.ThisWorkbook.Worksheets.get_Item(n.RefersToRange.Worksheet.Index);
                    r = w.get_Range(n.RefersToRange.Address);
                }
            }
            return r;
        }


        //==============================================================================================
        // Private Helper Functions
        //==============================================================================================

        //==============================================================================================
        // Creates an xml request from workbook, sends to service and returns response xml as string
        //==============================================================================================
        private string SendXml(string Environment, string requestMessage)
        {
            //Fetch endpoint from app.config
            string calcUrl;
            if (Environment == "LIVE")
            {
                calcUrl = this.CalculateUrlLive;
            }
            else
            {
                calcUrl = this.CalculateUrlTest;
            }

            //Set values for the request
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(calcUrl);
                req.Method = "POST";
                req.Headers.Add("SOAPAction", "");
                req.ContentType = "text/xml;charset=\"utf-8\"";

                //Send to service
                string strResponse = "";
                req.ContentLength = requestMessage.Length;
                using (var writer = new StreamWriter(req.GetRequestStream()))
                {
                    writer.Write(requestMessage);
                }

                //Handle response
                try
                {
                    using (Stream s = req.GetResponse().GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(s);
                        strResponse = reader.ReadToEnd();
                    }
                }
                catch (InvalidOperationException ioEx)
                {
                    strResponse = ioEx.Message;
                }
                return strResponse;
            }
            catch (Exception ex)
            {
                if (ex is UriFormatException)
                {
                    MessageBox.Show("The supplied RAS Engine Url '" + calcUrl + "' is not working.  Please check your settings.", "Publishing Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    MessageBox.Show("The supplied RAS Engine Url '" + calcUrl + "' is not working.  Please check your settings.\n\n" + ex.Message, "Publishing Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                return null;
            }
        }

        //==============================================================================================
        // Function to get workbook filename without xlsx extension (used for model name)
        //==============================================================================================
        private string GetWorkbookStub()
        {
            string wName = Globals.ThisWorkbook.Name.ToString();
            string n = wName.Split('.').First();
            return n;
        }


        //==============================================================================================
        // Return a list of cells with comments matching cellType, e.g. 'I' or 'O', etc.
        //==============================================================================================
        private List<string[]> GetTaggedCells(string cellType)
        {
            //Walk through each worksheet and get cell comments
            List<string[]> commentList = new List<string[]>();
            string comment;
            string cellName;

            foreach (Excel.Worksheet ws in Globals.ThisWorkbook.Worksheets)
            {
                //Establish used range of worksheet
                Excel.Range rg = GetRealRange(ws);

                string newValue = "";

                //Iterate over cells to find comments
                for (int i = 1; i <= rg.Rows.Count; i++)
                {
                    for (int j = 1; j <= rg.Columns.Count; j++)
                    {
                        if (rg.Cells[i, j].Comment != null)
                        {
                            //Test for type I|O as per parameter cellType
                            comment = rg.Cells[i, j].Comment.Text;
                            if (GetCommentType(comment, cellType) != null )
                            {
                                cellName = GetCommentName(comment, cellType);

                                // Handle empty cell
                                if (rg.Cells[i, j].Value != null) 
                                {
                                    newValue = rg.Cells[i, j].Value.ToString();
                                }

                                // If we are matching on defaults, then update cell
                                if (cellType == this.DefaultCommentTag)
                                {
                                    rg.Cells[i, j].Value = cellName;
                                }

                                //Append to the list: Mapping name, value, sheet name and cell coordinates
                                commentList.Add(new string[] { cellName, newValue, ws.Name, i.ToString(), j.ToString() });
                            }
                        }
                    }
                }
            }
            // Sort and return
            commentList.Sort(delegate(string[] c1, string[] c2) { return c1[0].CompareTo(c2[0]); });
            return commentList;
        }

        //==============================================================================================
        // Overloaded Method
        //==============================================================================================
        // Return a list of cells with comments matching 'I' or 'O' type
        // This version also forces the comment to be rewritten more cleanly, removing redundant spaces 
        // and line breaks: it fixes issues in CareAssess integration, which is sensitive to this.
        // However, if used every time Test() is called, it affects the ability to undo changes in the
        // workbook.  Therefore, this overloaded method is ONLY called from Publish().
        //==============================================================================================
        private List<string[]> GetTaggedCells(string cellType, bool updateComment)
        {
            //Walk through each worksheet and get cell comments
            List<string[]> commentList = new List<string[]>();
            string comment;
            string cellName;

            foreach (Excel.Worksheet ws in Globals.ThisWorkbook.Worksheets)
            {
                //Establish used range of worksheet
                Excel.Range rg = GetRealRange(ws);
                string newValue = "";

                //Iterate over cells to find comments
                for (int i = 1; i <= rg.Rows.Count; i++)
                {
                    for (int j = 1; j <= rg.Columns.Count; j++)
                    {
                        if (rg.Cells[i, j].Comment != null)
                        {
                            //Test for type I|O as per parameter cellType
                            comment = rg.Cells[i, j].Comment.Text;
                            if (GetCommentType(comment, cellType) != null)
                            {
                                cellName = GetCommentName(comment, cellType);
                                // Handle empty cell
                                if (rg.Cells[i, j].Value != null)
                                {
                                    newValue = rg.Cells[i, j].Value.ToString();
                                }

                                //Append to the list: Mapping name, value
                                commentList.Add(new string[] { cellName, newValue, ws.Name, i.ToString(), j.ToString()});

                                // Reset comment to *only* contain clean (no line break) comments of supported types
                                if (updateComment == true)
                                {
                                    // Create new comment
                                    string newComment = cellType + ":" + cellName;

                                    // Input cells may also have a default value set in comment that we need to preserve
                                    if (cellType == "I")
                                    {
                                        string d = GetCommentName(comment, this.DefaultCommentTag);
                                        if (d != null)
                                        {
                                            newComment += "\n" + this.DefaultCommentTag + ":" + d;
                                        }
                                    }
                                    rg.Cells[i, j].Comment.Delete();
                                    rg.Cells[i, j].AddComment(newComment);
                                }
                            }
                        }
                    }
                }
            }
            // Sort and return
            commentList.Sort(delegate(string[] c1, string[] c2) { return c1[0].CompareTo(c2[0]); });
            return commentList;
        }

        //==============================================================================================
        // Returns the type of a comment, i.e. "I" for Input or "O" for Output
        //==============================================================================================
        private string GetCommentType(string comment, string type)
        {
            string[] parts = comment.Split('\n');
            for (int i = 0; i < parts.Length; i++)
            {
                string[] tokens = parts[i].Split(':');
                if (tokens[0] == type)
                {
                    return tokens[0];
                }
            }
            return null;
        }

        //==============================================================================================
        // Returns the user named part of a comment, i.e after I: or O: or Default:
        //==============================================================================================
        private string GetCommentName(string comment, string type)
        {
            string[] parts = comment.Split('\n');
            for (int i = 0; i < parts.Length; i++)
            {
                string[] tokens = parts[i].Split(':');
                if (tokens[0] == type)
                {
                    return tokens[1];
                }
            }
            return null;
        }

        //==============================================================================================
        // Save a copy of this workbook
        //==============================================================================================
        private string SaveModel()
        {
            // Remove line breaks from I/O comments by calling GetTaggedCells overloaded method
            List<string[]> inputs = InputsList(true);
            List<string[]> outputs = OutputsList(true);
            
            // Force save of current workbook; should not display an alert
            Globals.ThisWorkbook.Save();

            // Set default versions location
            if ((this.NetworkLocation == "") || (this.NetworkLocation == null))
            {
                // Set default location as <User> ApplicationData
                this.NetworkLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            }

            // Set base paths
            string basePath = this.NetworkLocation;
            if (basePath.Substring(basePath.Length - 1, 1) != @"\") basePath += @"\";
            basePath = basePath + @"Calculation Engine\" + this.Name;   
            string sourcePath = Globals.ThisWorkbook.Path + @"\" + this.Name + ".xlsx";

            // Check whether Versions folder exists for this model; if not create
            if (CreateAppDirectory(Path.Combine(basePath, this.VersionFolderName)))
            {
                // Create new versioned filename
                string targetPath = basePath + @"\" + this.VersionFolderName + @"\" + this.Name + "_" + this.Version + ".xlsx";
                try
                {
                    File.Copy(sourcePath, targetPath, true);

                    // Create another copy without the version name
                    targetPath = basePath + @"\" + Globals.ThisWorkbook.Name;
                    File.Copy(sourcePath, targetPath, true);

                    // Return the unversioned file for upload
                    return targetPath;
                }
                catch (Exception ex)
                {
                    if (ex is UnauthorizedAccessException)
                    {
                        MessageBox.Show("You do not have permission to save the model file [" + targetPath + "].  Please ask for assistance from your System Administrator.", "Unable to Save Model", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        MessageBox.Show("Unable to save the model file [" + targetPath + "].  Please ask for assistance from your System Administrator.", "Unable to Save Model\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    return null;
                }
            }
            else
            {
                MessageBox.Show("Unable to create the folder [" + basePath + @"\" + this.VersionFolderName + "].  Please ask for assistance from your System Administrator to ensure that you have appropriate permissions.", "Unable to Save Model", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
        }

        //==============================================================================================
        // Check for duplicate names in input / output list
        //==============================================================================================
        private List<string> CheckDuplicates (List<string[]> list) 
        {
            // Declarations 
            List<string> dupes = new List<string>();    // List of duplicate tag names
            List<string> result = new List<string>();   // return list; tag names augmented with cell refs
            List<string> key = new List<string>();      // temporary key list
            
            // Create a new array of just the names (item 0 in each entry)
            for (int i = 0; i < list.Count; i++)
            {
                key.Add(list[i][0]);
            }

            // Test for dupes using linq
            var duplicates = key
                .GroupBy(n => n)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key);
            foreach (var d in duplicates)
                dupes.Add(d);

            // Derive workbook cell references for each dupe
            foreach (string dupe in dupes)
            {
                foreach (string[] tag in list)
                {
                    if (tag[0] == dupe)
                    {
                        // Create the cell reference string
                        string location = "'" + dupe + "' in cell " + tag[2] + ":" + this.GetExcelColumnName(Int32.Parse(tag[4])) + tag[3];

                        // Add to results
                        result.Add(location);
                    }
                }
            }

            return result;
        }

        

        //==============================================================================================
        // Check for existence of SDS tab and zero value of SDS output
        //==============================================================================================
        private bool CheckSDSSheetExists()
        {
            bool result = false;

            // Test for existence of SDS tab
            foreach (Excel.Worksheet ws in Globals.ThisWorkbook.Worksheets)
            {
                if (ws.Name == "SDS")
                {
                    result = true;
                }
            }
            return result;
        }

        //==============================================================================================
        // Check for existence of SDS tab and zero value of SDS output
        //==============================================================================================
        private bool CheckSDSValues()
        {
            bool result = false;

            List<string[]> outputs = OutputsList();
            foreach (string[] o in outputs)
            {
                if (o[0] == "PersonalBudget")
                {
                    double val;
                    if (!double.TryParse(o[1], out val))
                    {
                        if (o[1] == "")
                        {
                            result = true;
                        }
                    }
                    else
                    {
                        if (val == 0.0)
                        {
                            result = true;
                        }
                    }
                }
            }

            return result;
        }

        //==============================================================================================
        // Test for existence of folder; if not, then create it
        //==============================================================================================
        private bool CreateAppDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                    return true;
                }
                catch (Exception ex)
                {
                    if (ex is ArgumentException)
                    {
                        MessageBox.Show("The folder name [" + path + "] is not a valid location.", "Unable to Create Folder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    if (ex is UnauthorizedAccessException)
                    {
                        MessageBox.Show("You do not have permission to create the model folder [" + path + "].  Please ask for assistance from your System Administrator.", "Unable to Create Folder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        //==============================================================================================
        // Create the xml request string for calculate method from workbook
        //==============================================================================================
        private string CreateTestRequestXml()
        {
            string strRequest = "";
            strRequest += "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:SOAP-ENC=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:m0=\"http://xml.apache.org/xml-soap\">";
            strRequest += "<SOAP-ENV:Header><ExcelVersion>" + ProductVersion + "</ExcelVersion></SOAP-ENV:Header>";
            strRequest += "<SOAP-ENV:Body><m:calculate xmlns:m=\"http://ws.calc\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">";
            strRequest += "<WorkbookName>" + this.Name + ".xlsx</WorkbookName>";
            strRequest += "<INPUTS>";

            //Iterate over input cells
            foreach (string[] input in this.InputsList())
            {
                strRequest += "<item><key>" + input[0] + "</key><value>" + input[1] + "</value></item>";
            }
            strRequest += "</INPUTS>";
            strRequest += "</m:calculate></SOAP-ENV:Body></SOAP-ENV:Envelope>";

            return strRequest;
        }

        //==============================================================================================
        // Create the xml request string for versionCheck method
        //==============================================================================================
        private string CreateVersionCheckRequestXml()
        {
            string strRequest = "";
            strRequest += "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:SOAP-ENC=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:m=\"http://ws\">";
	        strRequest += "<SOAP-ENV:Body>";
		    strRequest += "<m:versionCheck SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">";
			strRequest += "<ClientVersion xsi:type=\"xsd:string\">" + ProductVersion + "</ClientVersion>";
		    strRequest += "</m:versionCheck>";
	        strRequest += "</SOAP-ENV:Body>";
            strRequest += "</SOAP-ENV:Envelope>";

            return strRequest;
        }

        //==============================================================================================
        // Convert integer column number into Excel column Letter
        //==============================================================================================
        private string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        //==============================================================================================
        // Convert integer column number into Excel column Letter
        //==============================================================================================
        private string AddSlash(string path)
        {
            if ((path == null) || (path == ""))
            {
                return "";
            }
            else
            {
                string last = path.Substring(path.Length - 1, 1);
                if (last != "/") path += "/";
                return path;
            }
        }

        //==============================================================================================
        // Return the full used range of a worksheet, starting from cell A1
        //==============================================================================================
        private Excel.Range GetRealRange(Excel.Worksheet ws)
        {
            //Establish used range of worksheet
            Excel.Range used = ws.UsedRange;

            // Now force range to start at A1 (it won't if cells in the first row or column are not used)
            // This is necessary in order to calculate absolute cell addresses in the error message refMsg
            // Get the last cell in used range
            Excel.Range lastCell = null;
            lastCell = used.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);
            int maxCol = lastCell.Column;
            int maxRow = lastCell.Row;

            // Now cast the full range
            string usedRange = "A1:" + this.GetExcelColumnName(maxCol) + maxRow.ToString();
            Excel.Range rg = ws.get_Range(usedRange);

            return rg;
        }


        //==============================================================================================
        // DEBUG: Displays a list of all cells and their data type
        //==============================================================================================
        private void ShowDataTypes()
        {
            string msg = "";

            //Walk through each worksheet and get cell formulae
            foreach (Excel.Worksheet ws in Globals.ThisWorkbook.Worksheets)
            {
                //Establish used range of worksheet
                Excel.Range rg = GetRealRange(ws);
                string typeName;

                //Iterate over cells to find all values
                for (int i = 1; i <= rg.Rows.Count; i++)
                {
                    for (int j = 1; j <= rg.Columns.Count; j++)
                    {
                        // Test for value
                        if (rg.Cells[i, j].Value != null)
                        {
                            // Get data type
                            typeName = rg.Cells[i, j].Value.GetType().ToString();
                            msg += ws.Name + "!" + this.GetExcelColumnName(j) + i.ToString() + " TYPE = " + typeName + "\n";
                        }
                    }
                }
            }

            MessageBox.Show(msg);
        }

        


        //==============================================================================================
    }
}
