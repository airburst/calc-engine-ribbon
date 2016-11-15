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
    public partial class FormEnvironments : Form
    {
        // Make the environment list a public class, so that we can preserve change details
        // when writing back to the calling Settings form
        public List<CalculonEnvironment> EnvironmentList = new List<CalculonEnvironment>();
        
        public FormEnvironments()
        {
            InitializeComponent();
        }

        private void FormEnvironments_Load(object sender, EventArgs e)
        {
            // Populate the listbox with environment names
            CalculationModel model = new CalculationModel();
            EnvironmentList = model.EnvironmentList;

            for (int i = 0; i < EnvironmentList.Count; i++)
            {
                ListBoxEnvironments.Items.Add(EnvironmentList[i].Name);
            }
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            AddEnvironment fa = new AddEnvironment();   // Note: this form class should have been named FormAddEnvironment
            DialogResult dr = fa.ShowDialog();

            // Test for OK pressed
            if (dr == DialogResult.OK)
            {
                // Get the environment name
                string environment = fa.EnvironmentName;

                // Update the list of environments in Document Properties and this settings dialog
                ListBoxEnvironments.Items.Add(environment);

                // Update the public class
                CalculonEnvironment env = new CalculonEnvironment();
                env.Name = environment;
                EnvironmentList.Add(env);

                // Dispose of the child form object
                fa.Dispose();
            }
        }

        private void Rename_Click(object sender, EventArgs e)
        {
            // Test that we have a selected item
            int index = ListBoxEnvironments.SelectedIndex;
            if (index == -1)
            {
                MessageBox.Show("Please select an environment to rename");
            }
            else
            {
                AddEnvironment fa = new AddEnvironment();   // Note: this form class should have been named FormAddEnvironment

                // Set the public environment variable to selected item
                fa.EnvironmentName = ListBoxEnvironments.SelectedItem.ToString();

                DialogResult dr = fa.ShowDialog();

                // Test for OK pressed
                if (dr == DialogResult.OK)
                {
                    // Get the environment name
                    string environment = fa.EnvironmentName;

                    // Update the list of environments in Document Properties and this settings dialog
                    // Replace the selected item in list at same index position
                    ListBoxEnvironments.Items.RemoveAt(index);
                    ListBoxEnvironments.Items.Insert(index, environment);

                    // Update public class
                    // Only want to preserve the original name for matching
                    if ((EnvironmentList[index].Oldname == null) || (EnvironmentList[index].Oldname == "")) 
                    { 
                        EnvironmentList[index].Oldname = EnvironmentList[index].Name; 
                    }  
                    EnvironmentList[index].Name = environment;

                    // Dispose of the child form object
                    fa.Dispose();
                }
            }
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            // Remove selected item from list
            int index = ListBoxEnvironments.SelectedIndex;
            if (index == -1)
            {
                MessageBox.Show("Please select an environment to remove");
            }
            else
            {
                ListBoxEnvironments.Items.Remove(ListBoxEnvironments.SelectedItem);

                // Remove from public class
                EnvironmentList.RemoveAt(index);
            }
            
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            // Close
            this.Close();
            this.Dispose();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            // Check for zero items
            if (ListBoxEnvironments.Items.Count == 0)
            {
                MessageBox.Show("Please add at least one environment");
            }
            else
            {
                // Copy name into any non-blank Oldname fields for the public class
                for (int i = 0; i < EnvironmentList.Count; i++)
                {
                    if (EnvironmentList[i].Oldname == "") 
                    {
                        EnvironmentList[i].Oldname = EnvironmentList[i].Name;
                    }
                }
                
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
