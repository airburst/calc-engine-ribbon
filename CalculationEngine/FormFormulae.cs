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
    public partial class FormFormulae : Form
    {
        public FormFormulae()
        {
            InitializeComponent();

            //Initialise grid headers
            this.DataGridViewFormulae.ColumnCount = 4;
            this.DataGridViewFormulae.ColumnHeadersVisible = true;
            this.DataGridViewFormulae.Columns[0].HeaderText = "WorkSheet";
            this.DataGridViewFormulae.Columns[1].HeaderText = "Cell";
            this.DataGridViewFormulae.Columns[2].HeaderText = "Formula";
            this.DataGridViewFormulae.Columns[3].HeaderText = "Errors";

            //And col widths
            this.DataGridViewFormulae.Columns[0].Width = 100;
            this.DataGridViewFormulae.Columns[1].Width = 50;
            this.DataGridViewFormulae.Columns[2].Width = 420;
            this.DataGridViewFormulae.Columns[3].Width = 200;

            // Update document properties with editted values
            CalculationModel model = new CalculationModel();
            List<string[]> formulae = new List<string[]>();

            // Get list of properties
            formulae = model.ListFormulae();

            // Bind list to DataGridView
            foreach (string[] row in formulae)
            {
                this.DataGridViewFormulae.Rows.Add(row);
            }

            // Highlight all errors
            foreach (DataGridViewRow row in DataGridViewFormulae.Rows)
            {
                if ((string)row.Cells[3].Value != "")
                {
                    // Don't include last (Summary) row
                    if (row.Index < DataGridViewFormulae.Rows.Count - 2)
                    {
                        // Highlight the row
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                    }
                }
            }

            // Write summary to label
            this.LabelErrors.Text = (string)formulae[formulae.Count - 1][2];

            // Disallow adding new rows
            this.DataGridViewFormulae.AllowUserToAddRows = false;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}
