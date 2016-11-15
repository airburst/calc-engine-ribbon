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
    public partial class FormLink : Form
    {
        public FormLink()
        {
            InitializeComponent();
        }

        private void FormLink_Load(object sender, EventArgs e)
        {
            // Initialise Listboxes by filling from properties file
            CalculationModel model = new CalculationModel();
            List<Assessment> assessments = new List<Assessment>();

            // Load list of all assessments into left listbox
            assessments = model.GetAllAssessmentsFromPropertiesFile();

            if (assessments == null)
            {
                // Issue reading properties file - close dialog
                this.Close();
                this.Dispose();
            }
            else
            {
                foreach (Assessment assessment in assessments)
                {
                    string item = assessment.Name;
                    if (assessment.Linked == true) { item += " --- Linked to " + assessment.LinkedModel; }
                    ListBoxAssessments.Items.Add(item);
                }

                // And all linked assessments into the right
                assessments = model.GetLinkedAssessmentsFromPropertiesFile();
                foreach (Assessment assessment in assessments)
                {
                    ListBoxLinks.Items.Add(assessment.Name);
                }

                // Add mouse handler for drag and drop: Assessments to Links
                ListBoxAssessments.MouseDown += new MouseEventHandler(ListBoxAssessments_MouseDown);
                ListBoxLinks.DragOver += new DragEventHandler(ListBoxLinks_DragOver);
                ListBoxLinks.DragDrop += new DragEventHandler(ListBoxLinks_DragDrop);

                // Add mouse handler for drag and drop: Out of Links
                ListBoxLinks.MouseDown += new MouseEventHandler(ListBoxLinks_MouseDown);
                ListBoxAssessments.DragOver += new DragEventHandler(ListBoxAssessments_DragOver);
                ListBoxAssessments.DragDrop += new DragEventHandler(ListBoxAssessments_DragDrop);
            }
        }


        // Drag event handler for Assessments Listbox
        private void ListBoxAssessments_MouseDown(object sender, MouseEventArgs e)
        {
            if (ListBoxAssessments.Items.Count == 0) return;

            int index = ListBoxAssessments.IndexFromPoint(e.X, e.Y);
            string s = ListBoxAssessments.Items[index].ToString();

            // Only allow drag for unlinked items -- Added in 1.3.1
            if (!s.Contains("Linked") && (ListBoxLinks.Items.Count == 0))
            {
                DragDropEffects dde1 = DoDragDrop(s, DragDropEffects.All);

                if (dde1 == DragDropEffects.All)
                {
                    // Change item to show [Linked]  -- Added at 1.3.1
                    ListBoxAssessments.Items.RemoveAt(index);
                    ListBoxAssessments.Items.Insert(index, s + " --- Linked");
                }
            }
        }


        // Event handlers to drag items into Links Listbox ============================================
        private void ListBoxLinks_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void ListBoxLinks_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                // Get string from object (the assessment name)
                string assessment = (string)e.Data.GetData(DataFormats.StringFormat);

                // If the assesment is not already present then add it to Links list
                int index = ListBoxLinks.FindString(assessment, -1);
                if (index == -1)
                {
                    // Add item
                    ListBoxLinks.Items.Add(assessment);
                }
            }
        }


        // Event handlers to drag items out of Links Listbox ============================================
        private void ListBoxLinks_MouseDown(object sender, MouseEventArgs e)
        {
            if (ListBoxLinks.Items.Count == 0) return;

            int index = ListBoxLinks.IndexFromPoint(e.X, e.Y);
            string s = ListBoxLinks.Items[index].ToString();
            DragDropEffects dde1 = DoDragDrop(s, DragDropEffects.All);

            if (dde1 == DragDropEffects.All)
            {
                ListBoxLinks.Items.RemoveAt(ListBoxAssessments.IndexFromPoint(e.X, e.Y));
            }
        }

        private void ListBoxAssessments_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void ListBoxAssessments_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                // Get string from object (the assessment name)
                string assessment = (string)e.Data.GetData(DataFormats.StringFormat);

                // Change text to remove [Linked]
                int index = ListBoxAssessments.FindString(assessment, -1);
                if (index > -1)
                {
                    // Add item
                    ListBoxAssessments.Items.RemoveAt(index);
                    ListBoxAssessments.Items.Insert(index, assessment);
                }
            }
        }
        // End event handlers ===========================================================================

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            // Close
            this.Close();
            this.Dispose();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            CalculationModel model = new CalculationModel();
            List<string> allAssessments = new List<string>();
            List<string> linkedAssessments = new List<string>();

            // Populate lists from listbox contents
            foreach (string asmt in ListBoxAssessments.Items)
            {
                // Remove [Linked] text -- Added in 1.3.1
                string[] tokens = asmt.Split(' ');
                allAssessments.Add(tokens[0]);
            }
            foreach (string link in ListBoxLinks.Items)
            {
                linkedAssessments.Add(link);
            }
            // Write the assessment lists to properties file
            model.UpdatePropertiesFile(allAssessments, linkedAssessments);

            // Close
            this.Close();
            this.Dispose();
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            FormAddAssessment add = new FormAddAssessment();
            DialogResult dr = add.ShowDialog();

            // Test for Add pressed
            if (dr == DialogResult.OK)
            {
                // Get the assessment name
                string assessment = add.AssessmentName.ToUpper();

                // Add to all assessments listbox only
                ListBoxAssessments.Items.Add(assessment);
                //ListBoxLinks.Items.Add(assessment);

                // Dispose of the child form object
                add.Dispose();
            }
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to remove all unlinked assessments?", "Confirm Removal", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // Remove all items in ListBoxAssessments without text [Linked]
                List<string> newList = new List<string>();

                foreach (string item in ListBoxAssessments.Items)
                {
                    if (item.Contains("Linked"))
                    {
                        // Add to new list
                        newList.Add(item);
                    }
                }
                // Clear old list and replace
                ListBoxAssessments.Items.Clear();
                foreach (string item in newList)
                {
                    ListBoxAssessments.Items.Add(item);
                }
            }
        }
    }
}
