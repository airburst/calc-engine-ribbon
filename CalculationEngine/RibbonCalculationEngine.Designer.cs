namespace CalculationEngine
{
    partial class Ribbon1 : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ribbon1));
            this.tab1 = this.Factory.CreateRibbonTab();
            this.GroupCalculon = this.Factory.CreateRibbonGroup();
            this.box1 = this.Factory.CreateRibbonBox();
            this.ButtonPublish = this.Factory.CreateRibbonButton();
            this.label1 = this.Factory.CreateRibbonLabel();
            this.LabelVersion = this.Factory.CreateRibbonLabel();
            this.ButtonTest = this.Factory.CreateRibbonButton();
            this.ButtonLinkToAssessment = this.Factory.CreateRibbonButton();
            this.GroupConfig = this.Factory.CreateRibbonGroup();
            this.ResetButton = this.Factory.CreateRibbonButton();
            this.ButtonFormulae = this.Factory.CreateRibbonButton();
            this.ButtonConfig = this.Factory.CreateRibbonButton();
            this.ButtonAbout = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.GroupCalculon.SuspendLayout();
            this.box1.SuspendLayout();
            this.GroupConfig.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.Groups.Add(this.GroupCalculon);
            this.tab1.Groups.Add(this.GroupConfig);
            this.tab1.Label = "RAS";
            this.tab1.Name = "tab1";
            this.tab1.Position = this.Factory.RibbonPosition.BeforeOfficeId("TabHome");
            // 
            // GroupCalculon
            // 
            this.GroupCalculon.Items.Add(this.box1);
            this.GroupCalculon.Items.Add(this.ButtonTest);
            this.GroupCalculon.Items.Add(this.ButtonLinkToAssessment);
            this.GroupCalculon.Label = "RAS Models";
            this.GroupCalculon.Name = "GroupCalculon";
            // 
            // box1
            // 
            this.box1.Items.Add(this.ButtonPublish);
            this.box1.Items.Add(this.label1);
            this.box1.Items.Add(this.LabelVersion);
            this.box1.Name = "box1";
            // 
            // ButtonPublish
            // 
            this.ButtonPublish.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.ButtonPublish.Image = ((System.Drawing.Image)(resources.GetObject("ButtonPublish.Image")));
            this.ButtonPublish.Label = "Publish";
            this.ButtonPublish.Name = "ButtonPublish";
            this.ButtonPublish.ShowImage = true;
            this.ButtonPublish.SuperTip = "Publish the current calculation model to the RAS Engine service.  Once you have d" +
    "one this, you will be able to test the model.";
            this.ButtonPublish.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ButtonPublish_Click);
            // 
            // label1
            // 
            this.label1.Label = "Version";
            this.label1.Name = "label1";
            // 
            // LabelVersion
            // 
            this.LabelVersion.Label = "0.1";
            this.LabelVersion.Name = "LabelVersion";
            // 
            // ButtonTest
            // 
            this.ButtonTest.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.ButtonTest.Image = ((System.Drawing.Image)(resources.GetObject("ButtonTest.Image")));
            this.ButtonTest.Label = "Test";
            this.ButtonTest.Name = "ButtonTest";
            this.ButtonTest.ShowImage = true;
            this.ButtonTest.SuperTip = "Test the model against the RAS Engine. The results table will compare answers in " +
    "this spreadsheet with the answer returned by the Engine.";
            this.ButtonTest.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ButtonTest_Click);
            // 
            // ButtonLinkToAssessment
            // 
            this.ButtonLinkToAssessment.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.ButtonLinkToAssessment.Image = global::CalculationEngine.Properties.Resources.link;
            this.ButtonLinkToAssessment.Label = "Link To Assessments";
            this.ButtonLinkToAssessment.Name = "ButtonLinkToAssessment";
            this.ButtonLinkToAssessment.ShowImage = true;
            this.ButtonLinkToAssessment.SuperTip = "Link this model to one or more Question Set Applications in CareAssess.";
            this.ButtonLinkToAssessment.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ButtonLinkToAssessment_Click);
            // 
            // GroupConfig
            // 
            this.GroupConfig.Items.Add(this.ResetButton);
            this.GroupConfig.Items.Add(this.ButtonFormulae);
            this.GroupConfig.Items.Add(this.ButtonConfig);
            this.GroupConfig.Items.Add(this.ButtonAbout);
            this.GroupConfig.Label = "Configuration";
            this.GroupConfig.Name = "GroupConfig";
            // 
            // ResetButton
            // 
            this.ResetButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.ResetButton.Image = global::CalculationEngine.Properties.Resources.reset;
            this.ResetButton.Label = "Reset";
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.ShowImage = true;
            this.ResetButton.SuperTip = "Reset all cells to their default value.  The default value can be specified for i" +
    "nput cells by adding a default line to the cell comment.";
            this.ResetButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ResetButton_Click);
            // 
            // ButtonFormulae
            // 
            this.ButtonFormulae.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.ButtonFormulae.Image = global::CalculationEngine.Properties.Resources.formula;
            this.ButtonFormulae.Label = "Calculations";
            this.ButtonFormulae.Name = "ButtonFormulae";
            this.ButtonFormulae.ShowImage = true;
            this.ButtonFormulae.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ButtonFormulae_Click);
            // 
            // ButtonConfig
            // 
            this.ButtonConfig.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.ButtonConfig.Image = global::CalculationEngine.Properties.Resources.settings1;
            this.ButtonConfig.Label = "Settings";
            this.ButtonConfig.Name = "ButtonConfig";
            this.ButtonConfig.ShowImage = true;
            this.ButtonConfig.SuperTip = "Change settings for this RAS model, including server details and default values.";
            this.ButtonConfig.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ButtonConfig_Click);
            // 
            // ButtonAbout
            // 
            this.ButtonAbout.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.ButtonAbout.Image = global::CalculationEngine.Properties.Resources.about;
            this.ButtonAbout.Label = "About";
            this.ButtonAbout.Name = "ButtonAbout";
            this.ButtonAbout.ShowImage = true;
            this.ButtonAbout.SuperTip = "Information about RAS Engine version.";
            this.ButtonAbout.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ButtonAbout_Click);
            // 
            // Ribbon1
            // 
            this.Name = "Ribbon1";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon1_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.GroupCalculon.ResumeLayout(false);
            this.GroupCalculon.PerformLayout();
            this.box1.ResumeLayout(false);
            this.box1.PerformLayout();
            this.GroupConfig.ResumeLayout(false);
            this.GroupConfig.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup GroupCalculon;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ButtonTest;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ButtonPublish;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel label1;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel LabelVersion;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup GroupConfig;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ButtonConfig;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ButtonLinkToAssessment;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ResetButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ButtonAbout;
        internal Microsoft.Office.Tools.Ribbon.RibbonBox box1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ButtonFormulae;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon1 Ribbon1
        {
            get { return this.GetRibbon<Ribbon1>(); }
        }
    }
}
