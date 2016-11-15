using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CalculationEngine;

namespace CalculationEngine
{
    public partial class FormTest : Form
    {
        public FormTest()
        {
            InitializeComponent();

            //Initialise grid headers
            this.DataGridViewResults.ColumnCount = 4;
            this.DataGridViewResults.ColumnHeadersVisible = true;
            // Change header for non-CareFirst customers
            if (Globals.ThisWorkbook.ReadDocumentProperty("OLM_ShowLinkButton") == "Y")
            {
                this.DataGridViewResults.Columns[0].HeaderText = "Question Reference";
            }
            else
            {
                this.DataGridViewResults.Columns[0].HeaderText = "Control";
            }
            this.DataGridViewResults.Columns[1].HeaderText = "Spreadsheet Result";
            this.DataGridViewResults.Columns[2].HeaderText = "RAS Engine Result";
            this.DataGridViewResults.Columns[3].HeaderText = "Match";

            //And col widths
            this.DataGridViewResults.Columns[0].Width = 250;
            this.DataGridViewResults.Columns[1].Width = 150;
            this.DataGridViewResults.Columns[2].Width = 150;
            this.DataGridViewResults.Columns[3].Width = 100;

            // Disallow adding new rows
            this.DataGridViewResults.AllowUserToAddRows = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CalculationModel model = new CalculationModel();
            List<string[]> resultsList = new List<string[]>();

            // Test model and return list of results
            resultsList = model.Test();

            if (resultsList == null)
            {
                this.Close();
                this.Dispose();
            }
            else
            {
                // Bind list to datagridview
                foreach (string[] rowArray in resultsList)
                {
                    this.DataGridViewResults.Rows.Add(rowArray);
                }

                // Highlight errors
                foreach (DataGridViewRow dgvrow in DataGridViewResults.Rows)
                {
                    if ((string)dgvrow.Cells[3].Value == model.NotMatchText)
                    {
                        dgvrow.DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                    }
                }
            }
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
        
    }
}
