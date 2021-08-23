namespace AutomatedReportGeneration
{
    partial class Dashboard
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtPickerCurrentDateSecondFeed = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCurrentFile = new System.Windows.Forms.Label();
            this.dtPickerCurrentDateFirstFeed = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.lblProgress = new System.Windows.Forms.Label();
            this.lblFirstFeedFile = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblSecondFeedFile = new System.Windows.Forms.Label();
            this.secondFeedFileBtn = new System.Windows.Forms.Button();
            this.txtSecondFeedFile = new System.Windows.Forms.TextBox();
            this.txtfirstFeedFile = new System.Windows.Forms.TextBox();
            this.firstFeedFileBtn = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btnGenerateReport = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtPickerCurrentDateSecondFeed);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lblCurrentFile);
            this.groupBox1.Controls.Add(this.dtPickerCurrentDateFirstFeed);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblProgress);
            this.groupBox1.Controls.Add(this.lblFirstFeedFile);
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Controls.Add(this.lblSecondFeedFile);
            this.groupBox1.Controls.Add(this.secondFeedFileBtn);
            this.groupBox1.Controls.Add(this.txtSecondFeedFile);
            this.groupBox1.Controls.Add(this.txtfirstFeedFile);
            this.groupBox1.Controls.Add(this.firstFeedFileBtn);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(596, 199);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Inputs";
            // 
            // dtPickerCurrentDateSecondFeed
            // 
            this.dtPickerCurrentDateSecondFeed.Location = new System.Drawing.Point(157, 51);
            this.dtPickerCurrentDateSecondFeed.Name = "dtPickerCurrentDateSecondFeed";
            this.dtPickerCurrentDateSecondFeed.Size = new System.Drawing.Size(335, 20);
            this.dtPickerCurrentDateSecondFeed.TabIndex = 34;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 13);
            this.label2.TabIndex = 33;
            this.label2.Text = "Second Feed File Run Date :";
            // 
            // lblCurrentFile
            // 
            this.lblCurrentFile.AutoSize = true;
            this.lblCurrentFile.Location = new System.Drawing.Point(62, 145);
            this.lblCurrentFile.Name = "lblCurrentFile";
            this.lblCurrentFile.Size = new System.Drawing.Size(0, 13);
            this.lblCurrentFile.TabIndex = 32;
            // 
            // dtPickerCurrentDateFirstFeed
            // 
            this.dtPickerCurrentDateFirstFeed.Location = new System.Drawing.Point(157, 25);
            this.dtPickerCurrentDateFirstFeed.Name = "dtPickerCurrentDateFirstFeed";
            this.dtPickerCurrentDateFirstFeed.Size = new System.Drawing.Size(335, 20);
            this.dtPickerCurrentDateFirstFeed.TabIndex = 31;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "First Feed File Run Date :";
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.Location = new System.Drawing.Point(59, 170);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(0, 20);
            this.lblProgress.TabIndex = 28;
            // 
            // lblFirstFeedFile
            // 
            this.lblFirstFeedFile.AutoSize = true;
            this.lblFirstFeedFile.Location = new System.Drawing.Point(67, 85);
            this.lblFirstFeedFile.Name = "lblFirstFeedFile";
            this.lblFirstFeedFile.Size = new System.Drawing.Size(78, 13);
            this.lblFirstFeedFile.TabIndex = 27;
            this.lblFirstFeedFile.Text = "First Feed File :";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(57, 164);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(403, 23);
            this.progressBar1.TabIndex = 25;
            // 
            // lblSecondFeedFile
            // 
            this.lblSecondFeedFile.AutoSize = true;
            this.lblSecondFeedFile.Location = new System.Drawing.Point(49, 110);
            this.lblSecondFeedFile.Name = "lblSecondFeedFile";
            this.lblSecondFeedFile.Size = new System.Drawing.Size(96, 13);
            this.lblSecondFeedFile.TabIndex = 24;
            this.lblSecondFeedFile.Text = "Second Feed File :";
            // 
            // secondFeedFileBtn
            // 
            this.secondFeedFileBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.secondFeedFileBtn.Location = new System.Drawing.Point(504, 108);
            this.secondFeedFileBtn.Name = "secondFeedFileBtn";
            this.secondFeedFileBtn.Size = new System.Drawing.Size(75, 23);
            this.secondFeedFileBtn.TabIndex = 23;
            this.secondFeedFileBtn.Text = "Browse";
            this.secondFeedFileBtn.UseVisualStyleBackColor = true;
            this.secondFeedFileBtn.Click += new System.EventHandler(this.BrowseBtn_Click);
            // 
            // txtSecondFeedFile
            // 
            this.txtSecondFeedFile.Location = new System.Drawing.Point(156, 108);
            this.txtSecondFeedFile.Name = "txtSecondFeedFile";
            this.txtSecondFeedFile.Size = new System.Drawing.Size(336, 20);
            this.txtSecondFeedFile.TabIndex = 22;
            // 
            // txtfirstFeedFile
            // 
            this.txtfirstFeedFile.Location = new System.Drawing.Point(157, 82);
            this.txtfirstFeedFile.Name = "txtfirstFeedFile";
            this.txtfirstFeedFile.Size = new System.Drawing.Size(335, 20);
            this.txtfirstFeedFile.TabIndex = 20;
            // 
            // firstFeedFileBtn
            // 
            this.firstFeedFileBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.firstFeedFileBtn.Location = new System.Drawing.Point(504, 80);
            this.firstFeedFileBtn.Name = "firstFeedFileBtn";
            this.firstFeedFileBtn.Size = new System.Drawing.Size(75, 23);
            this.firstFeedFileBtn.TabIndex = 19;
            this.firstFeedFileBtn.Text = "Browse";
            this.firstFeedFileBtn.UseVisualStyleBackColor = true;
            this.firstFeedFileBtn.Click += new System.EventHandler(this.BrowseBtn_Click);
            // 
            // btnGenerateReport
            // 
            this.btnGenerateReport.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnGenerateReport.Location = new System.Drawing.Point(205, 230);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(133, 23);
            this.btnGenerateReport.TabIndex = 22;
            this.btnGenerateReport.Text = "Generate Report";
            this.btnGenerateReport.UseVisualStyleBackColor = true;
            this.btnGenerateReport.Click += new System.EventHandler(this.btnGenerateReport_Click);
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 287);
            this.Controls.Add(this.btnGenerateReport);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Dashboard";
            this.Text = "Report Automation";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Label lblFirstFeedFile;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblSecondFeedFile;
        private System.Windows.Forms.Button secondFeedFileBtn;
        private System.Windows.Forms.TextBox txtSecondFeedFile;
        private System.Windows.Forms.TextBox txtfirstFeedFile;
        private System.Windows.Forms.Button firstFeedFileBtn;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnGenerateReport;
        private System.Windows.Forms.DateTimePicker dtPickerCurrentDateFirstFeed;
        private System.Windows.Forms.Label lblCurrentFile;
        private System.Windows.Forms.DateTimePicker dtPickerCurrentDateSecondFeed;
        private System.Windows.Forms.Label label2;
    }
}