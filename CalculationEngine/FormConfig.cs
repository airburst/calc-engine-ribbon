using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace CalculationEngine
{
    public partial class FormConfig : Form
    {
        public bool ShowIntegration = true;
        
        public FormConfig()
        {
            InitializeComponent();
            this.Init();
        }

        private void Init()
        {
            // Set flag to hide integration properties settings
            //bool showIntegration = true;
            
            //Initialise grid headers
            this.DataGridViewConfig.ColumnCount = 2;
            this.DataGridViewConfig.ColumnHeadersVisible = true;
            this.DataGridViewConfig.Columns[0].HeaderText = "Property";
            this.DataGridViewConfig.Columns[1].HeaderText = "Value";

            //And col widths
            this.DataGridViewConfig.Columns[0].Width = 200;
            this.DataGridViewConfig.Columns[1].Width = 200;

            // Update document properties with editted values
            CalculationModel model = new CalculationModel();
            List<string[]> configTable = new List<string[]>();

            // Get list of properties
            configTable = Globals.ThisWorkbook.GetAllConfigProperties();

            // Bind list to DataGridView
            foreach (string[] row in configTable)
            {
                // If ShowLinkButton is not 'Y' then don't show Integration Properties
                if ((row[0] == "OLM_ShowLinkButton") && (row[1] != "Y"))
                {
                    ShowIntegration = false;
                }
                this.DataGridViewConfig.Rows.Add(row);
            }

            // Remove integration properties
            if (ShowIntegration == false) 
            {
                removeIntegrationSettings();
            }

            // Highlight unset values
            foreach (DataGridViewRow row in DataGridViewConfig.Rows)
            {
                if ((string)row.Cells[1].Value == "")
                {
                    // Highlight the row
                    row.DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                }
            }

            // Make column [1] in grid editable
            this.DataGridViewConfig.EditMode = DataGridViewEditMode.EditOnEnter;
            this.DataGridViewConfig.Columns[0].ReadOnly = true;

            // Disallow adding new rows
            this.DataGridViewConfig.AllowUserToAddRows = false;

            // Disable OK button if any cells are still empty
            int emptyRowCount = EmptyRowCount();
        }

        // Set Tooltips for each row
        //private void DataGridViewConfig_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{
        //    if ((e.ColumnIndex == this.DataGridViewConfig.Columns["Value"].Index) && (e.Value != null))
        //    {
        //        var cell = this.DataGridViewConfig.Rows[e.RowIndex].Cells["Value"];
        //        cell.ToolTipText = "TEST";//this.DataGridViewConfig.Rows[e.RowIndex].Cells["Value"].Value.ToString();
        //    }
        //}

        private int EmptyRowCount()
        {
            int emptyRowCount = 0;

            // Return count of empty rows in datagrid
            foreach (DataGridViewRow row in DataGridViewConfig.Rows)
            {
                // If OLM_ShowLinkButton is not 'Y' then ignore integration properties items
                if (ShowIntegration == false)
                {
                    if (((row.Cells[1].Value == null) || ((string)row.Cells[1].Value == "")) && ((string)row.Cells[0].Value != "OLM_IntegrationPropertiesTest") && ((string)row.Cells[0].Value != "OLM_IntegrationPropertiesLive"))
                    {
                        // Highlight the row
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.Red;

                        // Increment counter and disable the OK button
                        emptyRowCount += 1;
                    }
                }
                else
                // if it's empty, highlight it
                {
                    if ((row.Cells[1].Value == null) || ((string)row.Cells[1].Value == ""))
                    {
                        // Highlight the row
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.Red;

                        // Increment counter and disable the OK button
                        emptyRowCount += 1;
                    }
                }
            }

            // Set enabled state of OK button
            if (emptyRowCount > 0)
            {
                ButtonOK.Enabled = false;
            }
            else
            {
                ButtonOK.Enabled = true;
            }
            return emptyRowCount;
        }


        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            string propertyName;
            string propertyVal;

            // Set DialogResult to OK
            this.DialogResult = DialogResult.OK;

            // Fetch all values from DataGrid and write back to Document Properties
            foreach (DataGridViewRow row in this.DataGridViewConfig.Rows)
            {
                // Test for empty value
                if ((row.Cells[0].Value != null) && (row.Cells[1].Value != null))
                {
                    propertyName = row.Cells[0].Value.ToString();
                    propertyVal = row.Cells[1].Value.ToString();
                    Globals.ThisWorkbook.UpdateDocumentProperty(propertyName, propertyVal);
                }
            }

            // Close
            this.Close();
            this.Dispose();
        }

        // Evaluated when a cell loses focus
        private void DataGridViewConfig_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Test for cell not empty
            if ((this.DataGridViewConfig.Rows[e.RowIndex].Cells[1].Value == null) || ((string)this.DataGridViewConfig.Rows[e.RowIndex].Cells[1].Value == ""))
            {
                    // Add red highlight
                    this.DataGridViewConfig.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                    ButtonOK.Enabled = false;
            }
            else
            {
                // Validate url and folder entries
                if (isValid(this.DataGridViewConfig.Rows[e.RowIndex].Cells[0].Value.ToString(), this.DataGridViewConfig.Rows[e.RowIndex].Cells[1].Value.ToString()))
                {
                    // Remove red highlight
                    this.DataGridViewConfig.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.White;
                }
                else
                {
                    // Throw a warning and empty the cell
                    MessageBox.Show("This property must be a valid URI.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    // Empty cell
                    this.DataGridViewConfig.Rows[e.RowIndex].Cells[1].Value = "";
                    //this.DataGridViewConfig.CurrentCell = this.DataGridViewConfig[e.RowIndex,1];
                }
            }

            // If OLM_ShowLinkButton is not 'Y' then remove integration properties settings and refresh
            if (((string)DataGridViewConfig.Rows[e.RowIndex].Cells[0].Value == "OLM_ShowLinkButton") && ((string)DataGridViewConfig.Rows[e.RowIndex].Cells[1].Value != "Y"))
            {
                ShowIntegration = false;
                removeIntegrationSettings();
            }
            // Otherwise, show them
            if (((string)DataGridViewConfig.Rows[e.RowIndex].Cells[0].Value == "OLM_ShowLinkButton") && ((string)DataGridViewConfig.Rows[e.RowIndex].Cells[1].Value == "Y"))
            {
                ShowIntegration = true;
                showIntegrationSettings();
            }

            // Disable OK button if any cells are still empty
            int emptyRowCount = EmptyRowCount();
        }

        private void removeIntegrationSettings()
        {
            // Remove integration settings
            foreach (DataGridViewRow row in DataGridViewConfig.Rows)
            {
                if (((string)row.Cells[0].Value == "OLM_IntegrationPropertiesTest") || ((string)row.Cells[0].Value == "OLM_IntegrationPropertiesLive"))
                {
                    // Hide the row
                    row.Visible = false;
                }
            }
        }

        private void showIntegrationSettings()
        {
            // Remove integration settings
            foreach (DataGridViewRow row in DataGridViewConfig.Rows)
            {
                if (((string)row.Cells[0].Value == "OLM_IntegrationPropertiesTest") || ((string)row.Cells[0].Value == "OLM_IntegrationPropertiesLive"))
                {
                    // Hide the row
                    row.Visible = true;
                }
            }
        }

        private bool isValid(string key, string value)
        {
            // Test for urls
            if (key.Contains("Url"))
            {
                try
                {
                    HttpWebRequest h = (HttpWebRequest)WebRequest.Create(value);
                    // Now try to cast as a URI
                    try
                    {
                        Uri uriResult;
                        bool result = Uri.TryCreate(value, UriKind.Absolute, out uriResult);
                        return result;
                    }
                    catch
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is InvalidCastException)
                    {
                        return false;
                    }
                }
                return true;
                
            }
            else
            {
                return true;
            }
        }
    }
}