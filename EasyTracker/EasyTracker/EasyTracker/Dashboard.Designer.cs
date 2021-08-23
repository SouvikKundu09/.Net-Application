namespace EasyTracker
{
    partial class EasyTracker
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
            this.txtLocalBuildPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBuild = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPolicy = new System.Windows.Forms.TextBox();
            this.chkRIS = new System.Windows.Forms.CheckBox();
            this.chkTARABS = new System.Windows.Forms.CheckBox();
            this.chkTARABAN = new System.Windows.Forms.CheckBox();
            this.chkINS = new System.Windows.Forms.CheckBox();
            this.chkRISBAL = new System.Windows.Forms.CheckBox();
            this.chkALL = new System.Windows.Forms.CheckBox();
            this.chkBuildDone = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtGeneratePath = new System.Windows.Forms.TextBox();
            this.btnProcess = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbServer = new System.Windows.Forms.ComboBox();
            this.cmbDatabase = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dtCurrentDate = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.chkSetup = new System.Windows.Forms.CheckBox();
            this.btnSetup = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtLocalBuildPath
            // 
            this.txtLocalBuildPath.Location = new System.Drawing.Point(281, 14);
            this.txtLocalBuildPath.Name = "txtLocalBuildPath";
            this.txtLocalBuildPath.Size = new System.Drawing.Size(422, 22);
            this.txtLocalBuildPath.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(80, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Local Build Path";
            // 
            // btnBuild
            // 
            this.btnBuild.Location = new System.Drawing.Point(603, 44);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(99, 27);
            this.btnBuild.TabIndex = 2;
            this.btnBuild.Text = "Build";
            this.btnBuild.UseVisualStyleBackColor = true;
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(79, 246);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(158, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Policy-Edition-Transnum";
            // 
            // txtPolicy
            // 
            this.txtPolicy.Location = new System.Drawing.Point(280, 238);
            this.txtPolicy.Name = "txtPolicy";
            this.txtPolicy.Size = new System.Drawing.Size(422, 22);
            this.txtPolicy.TabIndex = 4;
            // 
            // chkRIS
            // 
            this.chkRIS.AutoSize = true;
            this.chkRIS.Location = new System.Drawing.Point(46, 359);
            this.chkRIS.Name = "chkRIS";
            this.chkRIS.Size = new System.Drawing.Size(45, 19);
            this.chkRIS.TabIndex = 5;
            this.chkRIS.Text = "RIS";
            this.chkRIS.UseVisualStyleBackColor = true;
            // 
            // chkTARABS
            // 
            this.chkTARABS.AutoSize = true;
            this.chkTARABS.Location = new System.Drawing.Point(106, 359);
            this.chkTARABS.Name = "chkTARABS";
            this.chkTARABS.Size = new System.Drawing.Size(74, 19);
            this.chkTARABS.TabIndex = 6;
            this.chkTARABS.Text = "TARABS";
            this.chkTARABS.UseVisualStyleBackColor = true;
            // 
            // chkTARABAN
            // 
            this.chkTARABAN.AutoSize = true;
            this.chkTARABAN.Location = new System.Drawing.Point(210, 359);
            this.chkTARABAN.Name = "chkTARABAN";
            this.chkTARABAN.Size = new System.Drawing.Size(85, 19);
            this.chkTARABAN.TabIndex = 7;
            this.chkTARABAN.Text = "TARABAN";
            this.chkTARABAN.UseVisualStyleBackColor = true;
            // 
            // chkINS
            // 
            this.chkINS.AutoSize = true;
            this.chkINS.Location = new System.Drawing.Point(319, 359);
            this.chkINS.Name = "chkINS";
            this.chkINS.Size = new System.Drawing.Size(46, 19);
            this.chkINS.TabIndex = 8;
            this.chkINS.Text = "INS";
            this.chkINS.UseVisualStyleBackColor = true;
            // 
            // chkRISBAL
            // 
            this.chkRISBAL.AutoSize = true;
            this.chkRISBAL.Location = new System.Drawing.Point(390, 359);
            this.chkRISBAL.Name = "chkRISBAL";
            this.chkRISBAL.Size = new System.Drawing.Size(68, 19);
            this.chkRISBAL.TabIndex = 9;
            this.chkRISBAL.Text = "RISBAL";
            this.chkRISBAL.UseVisualStyleBackColor = true;
            this.chkRISBAL.CheckedChanged += new System.EventHandler(this.chkRISBAL_CheckedChanged);
            // 
            // chkALL
            // 
            this.chkALL.AutoSize = true;
            this.chkALL.Location = new System.Drawing.Point(488, 359);
            this.chkALL.Name = "chkALL";
            this.chkALL.Size = new System.Drawing.Size(49, 19);
            this.chkALL.TabIndex = 10;
            this.chkALL.Text = "ALL";
            this.chkALL.UseVisualStyleBackColor = true;
            this.chkALL.CheckedChanged += new System.EventHandler(this.chkALL_CheckedChanged);
            // 
            // chkBuildDone
            // 
            this.chkBuildDone.AutoSize = true;
            this.chkBuildDone.Location = new System.Drawing.Point(46, 52);
            this.chkBuildDone.Name = "chkBuildDone";
            this.chkBuildDone.Size = new System.Drawing.Size(93, 19);
            this.chkBuildDone.TabIndex = 11;
            this.chkBuildDone.Text = "Build Done";
            this.chkBuildDone.UseVisualStyleBackColor = true;
            this.chkBuildDone.CheckedChanged += new System.EventHandler(this.chkBuildDone_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(80, 397);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 15);
            this.label3.TabIndex = 12;
            this.label3.Text = "File Generation Path";
            // 
            // txtGeneratePath
            // 
            this.txtGeneratePath.Location = new System.Drawing.Point(280, 395);
            this.txtGeneratePath.Name = "txtGeneratePath";
            this.txtGeneratePath.Size = new System.Drawing.Size(422, 22);
            this.txtGeneratePath.TabIndex = 13;
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(543, 438);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(160, 27);
            this.btnProcess.TabIndex = 14;
            this.btnProcess.Text = "Start Processing";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(80, 287);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 15);
            this.label4.TabIndex = 15;
            this.label4.Text = "Server";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(79, 326);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 15);
            this.label5.TabIndex = 16;
            this.label5.Text = "Database";
            // 
            // cmbServer
            // 
            this.cmbServer.FormattingEnabled = true;
            this.cmbServer.Location = new System.Drawing.Point(280, 279);
            this.cmbServer.Name = "cmbServer";
            this.cmbServer.Size = new System.Drawing.Size(422, 23);
            this.cmbServer.TabIndex = 17;
            this.cmbServer.SelectedIndexChanged += new System.EventHandler(this.cmbServer_SelectedIndexChanged);
            // 
            // cmbDatabase
            // 
            this.cmbDatabase.FormattingEnabled = true;
            this.cmbDatabase.Location = new System.Drawing.Point(280, 318);
            this.cmbDatabase.Name = "cmbDatabase";
            this.cmbDatabase.Size = new System.Drawing.Size(422, 23);
            this.cmbDatabase.TabIndex = 18;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(80, 204);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 15);
            this.label6.TabIndex = 19;
            this.label6.Text = "Cycle Date";
            // 
            // dtCurrentDate
            // 
            this.dtCurrentDate.Location = new System.Drawing.Point(280, 196);
            this.dtCurrentDate.Name = "dtCurrentDate";
            this.dtCurrentDate.Size = new System.Drawing.Size(422, 22);
            this.dtCurrentDate.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(79, 87);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(347, 15);
            this.label7.TabIndex = 21;
            this.label7.Text = "Have you done the primitive INS and RISBAL setup yet?";
            // 
            // chkSetup
            // 
            this.chkSetup.AutoSize = true;
            this.chkSetup.Location = new System.Drawing.Point(455, 86);
            this.chkSetup.Name = "chkSetup";
            this.chkSetup.Size = new System.Drawing.Size(48, 19);
            this.chkSetup.TabIndex = 22;
            this.chkSetup.Text = "Yes";
            this.chkSetup.UseVisualStyleBackColor = true;
            this.chkSetup.CheckedChanged += new System.EventHandler(this.chkSetup_CheckedChanged);
            // 
            // btnSetup
            // 
            this.btnSetup.Location = new System.Drawing.Point(436, 121);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(99, 27);
            this.btnSetup.TabIndex = 23;
            this.btnSetup.Text = "Start Setup";
            this.btnSetup.UseVisualStyleBackColor = true;
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // EasyTracker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.ClientSize = new System.Drawing.Size(784, 492);
            this.Controls.Add(this.btnSetup);
            this.Controls.Add(this.chkSetup);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.dtCurrentDate);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbDatabase);
            this.Controls.Add(this.cmbServer);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.txtGeneratePath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkBuildDone);
            this.Controls.Add(this.chkALL);
            this.Controls.Add(this.chkRISBAL);
            this.Controls.Add(this.chkINS);
            this.Controls.Add(this.chkTARABAN);
            this.Controls.Add(this.chkTARABS);
            this.Controls.Add(this.chkRIS);
            this.Controls.Add(this.txtPolicy);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnBuild);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtLocalBuildPath);
            this.Font = new System.Drawing.Font("Lucida Fax", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "EasyTracker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLocalBuildPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPolicy;
        private System.Windows.Forms.CheckBox chkRIS;
        private System.Windows.Forms.CheckBox chkTARABS;
        private System.Windows.Forms.CheckBox chkTARABAN;
        private System.Windows.Forms.CheckBox chkINS;
        private System.Windows.Forms.CheckBox chkRISBAL;
        private System.Windows.Forms.CheckBox chkALL;
        private System.Windows.Forms.CheckBox chkBuildDone;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtGeneratePath;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbServer;
        private System.Windows.Forms.ComboBox cmbDatabase;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtCurrentDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkSetup;
        private System.Windows.Forms.Button btnSetup;
    }
}

