namespace UESRuleGenerator
{
    partial class Form1
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
            this.lblServer = new System.Windows.Forms.Label();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.btn_GetDB = new System.Windows.Forms.Button();
            this.cmbDatabase = new System.Windows.Forms.ComboBox();
            this.lblReferrals = new System.Windows.Forms.Label();
            this.lblBeReferral = new System.Windows.Forms.Label();
            this.txtPolicyNumber = new System.Windows.Forms.TextBox();
            this.lblPolicyNumber = new System.Windows.Forms.Label();
            this.btnGetRecords = new System.Windows.Forms.Button();
            this.btnDownloadReferral = new System.Windows.Forms.Button();
            this.btnDownloadBeReferral = new System.Windows.Forms.Button();
            this.grpPolicyDetails = new System.Windows.Forms.GroupBox();
            this.lblBeReferralCount = new System.Windows.Forms.Label();
            this.lblReferralCount = new System.Windows.Forms.Label();
            this.rbGetAllRules = new System.Windows.Forms.RadioButton();
            this.rbCheckRule = new System.Windows.Forms.RadioButton();
            this.txtRuleNo = new System.Windows.Forms.TextBox();
            this.lblRuleNo = new System.Windows.Forms.Label();
            this.grpReferralDetails = new System.Windows.Forms.GroupBox();
            this.dgvReferrals = new System.Windows.Forms.DataGridView();
            this.grpBeReferrals = new System.Windows.Forms.GroupBox();
            this.dgvBeReferrals = new System.Windows.Forms.DataGridView();
            this.cmbServer = new System.Windows.Forms.ComboBox();
            this.grpPolicyDetails.SuspendLayout();
            this.grpReferralDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReferrals)).BeginInit();
            this.grpBeReferrals.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBeReferrals)).BeginInit();
            this.SuspendLayout();
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServer.Location = new System.Drawing.Point(23, 21);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(38, 13);
            this.lblServer.TabIndex = 0;
            this.lblServer.Text = "Server";
            // 
            // lblDatabase
            // 
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDatabase.Location = new System.Drawing.Point(23, 86);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(53, 13);
            this.lblDatabase.TabIndex = 1;
            this.lblDatabase.Text = "Database";
            // 
            // btn_GetDB
            // 
            this.btn_GetDB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_GetDB.Location = new System.Drawing.Point(341, 40);
            this.btn_GetDB.Name = "btn_GetDB";
            this.btn_GetDB.Size = new System.Drawing.Size(75, 23);
            this.btn_GetDB.TabIndex = 4;
            this.btn_GetDB.Text = "Get DB";
            this.btn_GetDB.UseVisualStyleBackColor = true;
            this.btn_GetDB.Click += new System.EventHandler(this.btn_GetDB_Click);
            // 
            // cmbDatabase
            // 
            this.cmbDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDatabase.FormattingEnabled = true;
            this.cmbDatabase.Location = new System.Drawing.Point(237, 83);
            this.cmbDatabase.Name = "cmbDatabase";
            this.cmbDatabase.Size = new System.Drawing.Size(179, 21);
            this.cmbDatabase.TabIndex = 5;
            // 
            // lblReferrals
            // 
            this.lblReferrals.AutoSize = true;
            this.lblReferrals.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReferrals.Location = new System.Drawing.Point(11, 26);
            this.lblReferrals.Name = "lblReferrals";
            this.lblReferrals.Size = new System.Drawing.Size(90, 13);
            this.lblReferrals.TabIndex = 6;
            this.lblReferrals.Text = "Referral Records:";
            // 
            // lblBeReferral
            // 
            this.lblBeReferral.AutoSize = true;
            this.lblBeReferral.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBeReferral.Location = new System.Drawing.Point(11, 54);
            this.lblBeReferral.Name = "lblBeReferral";
            this.lblBeReferral.Size = new System.Drawing.Size(109, 13);
            this.lblBeReferral.TabIndex = 7;
            this.lblBeReferral.Text = "Be_Referral Records:";
            // 
            // txtPolicyNumber
            // 
            this.txtPolicyNumber.Location = new System.Drawing.Point(626, 23);
            this.txtPolicyNumber.Name = "txtPolicyNumber";
            this.txtPolicyNumber.Size = new System.Drawing.Size(79, 20);
            this.txtPolicyNumber.TabIndex = 12;
            // 
            // lblPolicyNumber
            // 
            this.lblPolicyNumber.AutoSize = true;
            this.lblPolicyNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPolicyNumber.Location = new System.Drawing.Point(435, 26);
            this.lblPolicyNumber.Name = "lblPolicyNumber";
            this.lblPolicyNumber.Size = new System.Drawing.Size(75, 13);
            this.lblPolicyNumber.TabIndex = 13;
            this.lblPolicyNumber.Text = "Policy Number";
            // 
            // btnGetRecords
            // 
            this.btnGetRecords.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetRecords.Location = new System.Drawing.Point(712, 20);
            this.btnGetRecords.Name = "btnGetRecords";
            this.btnGetRecords.Size = new System.Drawing.Size(92, 23);
            this.btnGetRecords.TabIndex = 14;
            this.btnGetRecords.Text = "Get Records";
            this.btnGetRecords.UseVisualStyleBackColor = true;
            this.btnGetRecords.Click += new System.EventHandler(this.btnGetRecords_Click);
            // 
            // btnDownloadReferral
            // 
            this.btnDownloadReferral.Enabled = false;
            this.btnDownloadReferral.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownloadReferral.Location = new System.Drawing.Point(291, 16);
            this.btnDownloadReferral.Name = "btnDownloadReferral";
            this.btnDownloadReferral.Size = new System.Drawing.Size(75, 23);
            this.btnDownloadReferral.TabIndex = 15;
            this.btnDownloadReferral.Text = "Download";
            this.btnDownloadReferral.UseVisualStyleBackColor = true;
            this.btnDownloadReferral.Click += new System.EventHandler(this.btnDownloadReferral_Click);
            // 
            // btnDownloadBeReferral
            // 
            this.btnDownloadBeReferral.Enabled = false;
            this.btnDownloadBeReferral.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownloadBeReferral.Location = new System.Drawing.Point(290, 56);
            this.btnDownloadBeReferral.Name = "btnDownloadBeReferral";
            this.btnDownloadBeReferral.Size = new System.Drawing.Size(75, 23);
            this.btnDownloadBeReferral.TabIndex = 16;
            this.btnDownloadBeReferral.Text = "Download";
            this.btnDownloadBeReferral.UseVisualStyleBackColor = true;
            this.btnDownloadBeReferral.Click += new System.EventHandler(this.btnDownloadBeReferral_Click);
            // 
            // grpPolicyDetails
            // 
            this.grpPolicyDetails.Controls.Add(this.lblBeReferralCount);
            this.grpPolicyDetails.Controls.Add(this.lblReferralCount);
            this.grpPolicyDetails.Controls.Add(this.btnDownloadReferral);
            this.grpPolicyDetails.Controls.Add(this.btnDownloadBeReferral);
            this.grpPolicyDetails.Controls.Add(this.lblReferrals);
            this.grpPolicyDetails.Controls.Add(this.lblBeReferral);
            this.grpPolicyDetails.Enabled = false;
            this.grpPolicyDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpPolicyDetails.Location = new System.Drawing.Point(438, 107);
            this.grpPolicyDetails.Name = "grpPolicyDetails";
            this.grpPolicyDetails.Size = new System.Drawing.Size(374, 85);
            this.grpPolicyDetails.TabIndex = 17;
            this.grpPolicyDetails.TabStop = false;
            this.grpPolicyDetails.Text = "Policy UES Details";
            // 
            // lblBeReferralCount
            // 
            this.lblBeReferralCount.AutoSize = true;
            this.lblBeReferralCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBeReferralCount.ForeColor = System.Drawing.Color.Green;
            this.lblBeReferralCount.Location = new System.Drawing.Point(145, 54);
            this.lblBeReferralCount.Name = "lblBeReferralCount";
            this.lblBeReferralCount.Size = new System.Drawing.Size(0, 13);
            this.lblBeReferralCount.TabIndex = 18;
            // 
            // lblReferralCount
            // 
            this.lblReferralCount.AutoSize = true;
            this.lblReferralCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReferralCount.ForeColor = System.Drawing.Color.Green;
            this.lblReferralCount.Location = new System.Drawing.Point(145, 26);
            this.lblReferralCount.Name = "lblReferralCount";
            this.lblReferralCount.Size = new System.Drawing.Size(0, 13);
            this.lblReferralCount.TabIndex = 17;
            // 
            // rbGetAllRules
            // 
            this.rbGetAllRules.AutoSize = true;
            this.rbGetAllRules.Checked = true;
            this.rbGetAllRules.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbGetAllRules.Location = new System.Drawing.Point(438, 69);
            this.rbGetAllRules.Name = "rbGetAllRules";
            this.rbGetAllRules.Size = new System.Drawing.Size(86, 17);
            this.rbGetAllRules.TabIndex = 18;
            this.rbGetAllRules.TabStop = true;
            this.rbGetAllRules.Text = "Get All Rules";
            this.rbGetAllRules.UseVisualStyleBackColor = true;
            this.rbGetAllRules.CheckedChanged += new System.EventHandler(this.rbGetAllRules_CheckedChanged);
            // 
            // rbCheckRule
            // 
            this.rbCheckRule.AutoSize = true;
            this.rbCheckRule.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbCheckRule.Location = new System.Drawing.Point(555, 69);
            this.rbCheckRule.Name = "rbCheckRule";
            this.rbCheckRule.Size = new System.Drawing.Size(81, 17);
            this.rbCheckRule.TabIndex = 19;
            this.rbCheckRule.Text = "Check Rule";
            this.rbCheckRule.UseVisualStyleBackColor = true;
            this.rbCheckRule.CheckedChanged += new System.EventHandler(this.rbCheckRule_CheckedChanged);
            // 
            // txtRuleNo
            // 
            this.txtRuleNo.Enabled = false;
            this.txtRuleNo.Location = new System.Drawing.Point(729, 66);
            this.txtRuleNo.Name = "txtRuleNo";
            this.txtRuleNo.Size = new System.Drawing.Size(75, 20);
            this.txtRuleNo.TabIndex = 20;
            // 
            // lblRuleNo
            // 
            this.lblRuleNo.AutoSize = true;
            this.lblRuleNo.Enabled = false;
            this.lblRuleNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRuleNo.Location = new System.Drawing.Point(670, 69);
            this.lblRuleNo.Name = "lblRuleNo";
            this.lblRuleNo.Size = new System.Drawing.Size(46, 13);
            this.lblRuleNo.TabIndex = 21;
            this.lblRuleNo.Text = "Rule No";
            // 
            // grpReferralDetails
            // 
            this.grpReferralDetails.Controls.Add(this.dgvReferrals);
            this.grpReferralDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpReferralDetails.Location = new System.Drawing.Point(18, 193);
            this.grpReferralDetails.Name = "grpReferralDetails";
            this.grpReferralDetails.Size = new System.Drawing.Size(794, 183);
            this.grpReferralDetails.TabIndex = 22;
            this.grpReferralDetails.TabStop = false;
            this.grpReferralDetails.Text = "Referral Records";
            // 
            // dgvReferrals
            // 
            this.dgvReferrals.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReferrals.Location = new System.Drawing.Point(7, 21);
            this.dgvReferrals.Name = "dgvReferrals";
            this.dgvReferrals.Size = new System.Drawing.Size(778, 156);
            this.dgvReferrals.TabIndex = 0;
            // 
            // grpBeReferrals
            // 
            this.grpBeReferrals.Controls.Add(this.dgvBeReferrals);
            this.grpBeReferrals.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBeReferrals.Location = new System.Drawing.Point(18, 382);
            this.grpBeReferrals.Name = "grpBeReferrals";
            this.grpBeReferrals.Size = new System.Drawing.Size(794, 163);
            this.grpBeReferrals.TabIndex = 23;
            this.grpBeReferrals.TabStop = false;
            this.grpBeReferrals.Text = "Be_Referral Records";
            // 
            // dgvBeReferrals
            // 
            this.dgvBeReferrals.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBeReferrals.Location = new System.Drawing.Point(6, 19);
            this.dgvBeReferrals.Name = "dgvBeReferrals";
            this.dgvBeReferrals.Size = new System.Drawing.Size(779, 138);
            this.dgvBeReferrals.TabIndex = 0;
            // 
            // cmbServer
            // 
            this.cmbServer.FormattingEnabled = true;
            this.cmbServer.Location = new System.Drawing.Point(237, 13);
            this.cmbServer.Name = "cmbServer";
            this.cmbServer.Size = new System.Drawing.Size(177, 21);
            this.cmbServer.TabIndex = 24;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 557);
            this.Controls.Add(this.cmbServer);
            this.Controls.Add(this.grpBeReferrals);
            this.Controls.Add(this.grpReferralDetails);
            this.Controls.Add(this.lblRuleNo);
            this.Controls.Add(this.txtRuleNo);
            this.Controls.Add(this.rbCheckRule);
            this.Controls.Add(this.rbGetAllRules);
            this.Controls.Add(this.grpPolicyDetails);
            this.Controls.Add(this.cmbDatabase);
            this.Controls.Add(this.btnGetRecords);
            this.Controls.Add(this.txtPolicyNumber);
            this.Controls.Add(this.lblPolicyNumber);
            this.Controls.Add(this.btn_GetDB);
            this.Controls.Add(this.lblDatabase);
            this.Controls.Add(this.lblServer);
            this.Name = "Form1";
            this.Text = "UES Rule Generator";
            this.grpPolicyDetails.ResumeLayout(false);
            this.grpPolicyDetails.PerformLayout();
            this.grpReferralDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReferrals)).EndInit();
            this.grpBeReferrals.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBeReferrals)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.Button btn_GetDB;
        private System.Windows.Forms.ComboBox cmbDatabase;
        private System.Windows.Forms.Label lblReferrals;
        private System.Windows.Forms.Label lblBeReferral;
        private System.Windows.Forms.TextBox txtPolicyNumber;
        private System.Windows.Forms.Label lblPolicyNumber;
        private System.Windows.Forms.Button btnGetRecords;
        private System.Windows.Forms.Button btnDownloadReferral;
        private System.Windows.Forms.Button btnDownloadBeReferral;
        private System.Windows.Forms.GroupBox grpPolicyDetails;
        private System.Windows.Forms.Label lblBeReferralCount;
        private System.Windows.Forms.Label lblReferralCount;
        private System.Windows.Forms.RadioButton rbGetAllRules;
        private System.Windows.Forms.RadioButton rbCheckRule;
        private System.Windows.Forms.TextBox txtRuleNo;
        private System.Windows.Forms.Label lblRuleNo;
        private System.Windows.Forms.GroupBox grpReferralDetails;
        private System.Windows.Forms.DataGridView dgvReferrals;
        private System.Windows.Forms.GroupBox grpBeReferrals;
        private System.Windows.Forms.DataGridView dgvBeReferrals;
        private System.Windows.Forms.ComboBox cmbServer;
    }
}

