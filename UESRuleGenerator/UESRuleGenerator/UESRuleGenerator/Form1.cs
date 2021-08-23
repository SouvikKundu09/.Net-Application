using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;

namespace UESRuleGenerator
{
    public partial class Form1 : Form
    {
        DataSet dsReferralRecords;
        DataSet dsBeReferralRecords;
        DataSet dsRuleRefRecords;
        DataSet dsRuleBeRefRecords;
        public Form1()
        {
            InitializeComponent();
            cmbServer.Items.Add("PRD - MFRKNTACESQP04");
            cmbServer.Items.Add("STG - MFRKNTACESQT12");
            cmbServer.Items.Add("QAT - MFRKNTACESQT04");
            cmbServer.Items.Add("DEV - MFRKNTACESQT11");
            cmbServer.SelectedIndex = 0;
        }

        private void btn_GetDB_Click(object sender, EventArgs e)
        {
            DataTable databases = null;
            string dbServer = cmbServer.Text.Substring(6, 14);
            if (dbServer != string.Empty && dbServer.Length == 14)
            {
                try
                {
                    using (var con = new SqlConnection("Data Source=" + dbServer + ";Integrated Security=True;"))
                    {
                        con.Open();
                        databases = con.GetSchema("Databases");
                        con.Close();
                    }
                    var strExpr = "database_name like 'ues$%'";
                    DataRow[] drDatabases = databases.Select(strExpr);
                    System.Data.DataTable dtDatabases = new System.Data.DataTable();
                    dtDatabases.Columns.Add("database_name");
                    foreach (DataRow dr in drDatabases)
                    {
                        dtDatabases.Rows.Add(dr["database_name"]);
                    }

                    DataRow drDefault = dtDatabases.NewRow();
                    drDefault["database_name"] = "--Select Database--";
                    dtDatabases.Rows.InsertAt(drDefault, 0);
                    cmbDatabase.DataSource = dtDatabases;
                    cmbDatabase.DisplayMember = "database_name";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Either server name is not valid or there might some error while connecting server. Error Message : " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please provide a valid server name.");
            }
            if (cmbServer.Text.Substring(6, 14) != string.Empty && cmbDatabase.Items.Count > 0)
            {
                grpPolicyDetails.Enabled = true;
            }
        }

        public DataSet GetReferralReport(string policyNumber)
        {
            SqlDataAdapter da;
            SqlConnection objConn;
            string dbServer = string.Empty;
            string database = string.Empty;
            SqlCommand cmd;
            if (cmbServer.Text != string.Empty)
            {
                dbServer = cmbServer.Text.Substring(6, 14);
            }
            if (cmbDatabase.SelectedIndex > 0)
            {
                database = cmbDatabase.GetItemText(cmbDatabase.SelectedItem);
            }
            string connString = "Data Source=" + dbServer + ";Initial Catalog=" + database + "; Integrated Security=True;";
            objConn = new SqlConnection(connString);

            if (policyNumber != string.Empty && policyNumber.Length == 9)
            {
                try
                {
                    string sqlReferralQuery = "select policy_number,runid,referral_code,referral_subcode,identifier,outcome,routing,qualifier,recency,description from referrals(nolock) where policy_number='" + policyNumber + "'";
                    cmd = new SqlCommand(sqlReferralQuery, objConn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@a00_pnum", SqlDbType.NVarChar);
                    cmd.Parameters["@a00_pnum"].Value = policyNumber;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dsReferralRecords);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dsReferralRecords;
        }

        public DataSet GetBeReferralReport(string policyNumber)
        {
            SqlDataAdapter da;
            SqlConnection objConn;
            string dbServer = string.Empty;
            string database = string.Empty;
            SqlCommand cmd;
            if (cmbServer.Text != string.Empty && txtPolicyNumber.Text.Length == 9)
            {
                dbServer = cmbServer.Text.Substring(6, 14);
            }
            if (cmbDatabase.SelectedIndex > 0)
            {
                database = cmbDatabase.GetItemText(cmbDatabase.SelectedItem);
            }
            string connString = "Data Source=" + dbServer + ";Initial Catalog=" + database + "; Integrated Security=True;";
            objConn = new SqlConnection(connString);

            if (policyNumber != string.Empty && policyNumber.Length == 9)
            {
                try
                {
                    string sqlBeReferralQuery = "select policy_number,runid,routing,outcome,a03_division,a03_branch,a07_proc_center,state_code,agt_code,a08_eff_date,q70_uw_supr_id,q69_ua_id,q70_uw_id,z35_csr_id,f58_userid,h53_date,h54_time,company,referral_code,referral_subcode,identifier,description,date_added,a06_edition,azn_chg_eff_date from be_referrals(nolock) where policy_number='" + policyNumber + "'";
                    cmd = new SqlCommand(sqlBeReferralQuery, objConn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@a00_pnum", SqlDbType.NVarChar);
                    cmd.Parameters["@a00_pnum"].Value = policyNumber;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dsBeReferralRecords);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dsBeReferralRecords;
        }

        private void btnGetRecords_Click(object sender, EventArgs e)
        {
            string policyNo = txtPolicyNumber.Text;
            if (policyNo != string.Empty && policyNo.Length == 9)
            {
                if (rbGetAllRules.Checked == true)
                {
                    dsReferralRecords = new DataSet();
                    dsBeReferralRecords = new DataSet();
                    dsReferralRecords = GetReferralReport(policyNo);
                    dsBeReferralRecords = GetBeReferralReport(policyNo);
                    if ((dsReferralRecords == null && dsBeReferralRecords == null) || (dsReferralRecords.Tables[0].Rows.Count == 0 && dsBeReferralRecords.Tables[0].Rows.Count == 0))
                    {
                        MessageBox.Show("No rule fired for this policy. Please check whether UES job is Up and Running.");
                        lblReferralCount.Text = "";
                        lblBeReferralCount.Text = "";
                        btnDownloadReferral.Enabled = false;
                        btnDownloadBeReferral.Enabled = false;
                    }
                    else
                    {
                        if (dsReferralRecords != null && dsReferralRecords.Tables[0].Rows.Count > 0)
                        {
                            lblReferralCount.Text = Convert.ToString(dsReferralRecords.Tables[0].Rows.Count);
                            btnDownloadReferral.Enabled = true;
                        }
                        else
                        {
                            lblReferralCount.Text = "0";
                            btnDownloadReferral.Enabled = false;
                        }

                        if (dsBeReferralRecords != null && dsBeReferralRecords.Tables[0].Rows.Count > 0)
                        {
                            lblBeReferralCount.Text = Convert.ToString(dsBeReferralRecords.Tables[0].Rows.Count);
                            btnDownloadBeReferral.Enabled = true;
                        }
                        else
                        {
                            lblBeReferralCount.Text = "0";
                            btnDownloadBeReferral.Enabled = false;
                        }
                    }
                }
                else if (rbCheckRule.Checked == true)
                {
                    SqlDataAdapter da;
                    SqlConnection objConn;
                    string dbServer = string.Empty;
                    string database = string.Empty;
                    SqlCommand cmd;
                    dsRuleRefRecords = new DataSet();
                    dsRuleBeRefRecords = new DataSet();
                    if (cmbServer.Text != string.Empty)
                    {
                        dbServer = cmbServer.Text.Substring(6, 14);
                    }
                    if (cmbDatabase.SelectedIndex > 0)
                    {
                        database = cmbDatabase.GetItemText(cmbDatabase.SelectedItem);
                    }
                    string connString = "Data Source=" + dbServer + ";Initial Catalog=" + database + "; Integrated Security=True;";
                    objConn = new SqlConnection(connString);
                    if (policyNo != string.Empty && policyNo.Length == 9)
                    {
                        string ruleNo = txtRuleNo.Text;
                        string sqlReferralQuery = "select referral_code as RuleCode,runid as RunId,description as Description from referrals(nolock) where policy_number='" + policyNo + "'";
                        if (ruleNo != string.Empty)
                        {
                            sqlReferralQuery = sqlReferralQuery + " and referral_code like '%" + ruleNo.Substring(2) + "%'";
                        }
                        try
                        {
                            cmd = new SqlCommand(sqlReferralQuery, objConn);
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Add("@a00_pnum", SqlDbType.NVarChar);
                            cmd.Parameters["@a00_pnum"].Value = policyNo;
                            if (ruleNo != string.Empty)
                            {
                                cmd.Parameters.Add("@referral_code", SqlDbType.NVarChar);
                                cmd.Parameters["@referral_code"].Value = ruleNo;
                            }
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dsRuleRefRecords);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                        string sqlBeReferralQuery = "select referral_code as RuleCode,runid as RunId,description as Description from be_referrals(nolock) where policy_number='" + policyNo + "'";
                        if (ruleNo != string.Empty)
                        {
                            sqlBeReferralQuery = sqlBeReferralQuery + " and referral_code like '%" + ruleNo.Substring(2) + "%'";
                        }
                        try
                        {
                            cmd = new SqlCommand(sqlBeReferralQuery, objConn);
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Add("@a00_pnum", SqlDbType.NVarChar);
                            cmd.Parameters["@a00_pnum"].Value = policyNo;
                            if (ruleNo != string.Empty)
                            {
                                cmd.Parameters.Add("@referral_code", SqlDbType.NVarChar);
                                cmd.Parameters["@referral_code"].Value = ruleNo;
                            }
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dsRuleBeRefRecords);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    if ((dsRuleRefRecords != null && dsRuleRefRecords.Tables[0].Rows.Count > 0) || (dsRuleBeRefRecords != null && dsRuleBeRefRecords.Tables[0].Rows.Count > 0))
                    {
                        //dgvReferrals.AutoGenerateColumns = false;
                        dgvReferrals.DataSource = dsRuleRefRecords.Tables[0];
                        //dgvBeReferrals.AutoGenerateColumns = false;
                        dgvBeReferrals.DataSource = dsRuleBeRefRecords.Tables[0];

                        foreach (DataGridViewColumn col in dgvReferrals.Columns)
                        {
                            if (col.Name == "Description")
                                col.Width = 520;
                        }
                        foreach (DataGridViewColumn col in dgvBeReferrals.Columns)
                        {
                            if (col.Name == "Description")
                                col.Width = 520;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please provide a valid policy number.");
                }
            }
        }

        private void btnDownloadReferral_Click(object sender, EventArgs e)
        {
            string FileName = "ReferralRecords_" + txtPolicyNumber.Text;
            GenerateExcelReport(dsReferralRecords, FileName);
        }

        private void btnDownloadBeReferral_Click(object sender, EventArgs e)
        {
            string FileName = "BeReferralRecords_" + txtPolicyNumber.Text;
            GenerateExcelReport(dsBeReferralRecords, FileName);
        }

        public void GenerateExcelReport(DataSet dsRecords, string strFileName)
        {
            try
            {
                if (dsRecords != null && dsRecords.Tables[0].Rows.Count > 0)
                {
                    Excel.Application xlApp;
                    Excel.Workbook xlWorkBook;
                    Excel.Worksheet xlWorkSheet;
                    object misValue = System.Reflection.Missing.Value;
                    xlApp = new Excel.Application();
                    xlWorkBook = xlApp.Workbooks.Add(misValue);
                    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                    for (int k = 0; k <= dsRecords.Tables[0].Columns.Count - 1; k++)
                    {
                        xlWorkSheet.Cells[1, k + 1] =
                            dsRecords.Tables[0].Columns[k].ColumnName.ToString();
                    }

                    for (int i = 0; i <= dsRecords.Tables[0].Rows.Count - 1; i++)
                    {
                        for (int j = 0; j <= dsRecords.Tables[0].Columns.Count - 1; j++)
                        {
                            xlWorkSheet.Cells[i + 2, j + 1] =
                            dsRecords.Tables[0].Rows[i].ItemArray[j].ToString();
                        }
                    }

                    System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();
                    saveDlg.InitialDirectory = @"C:\";
                    saveDlg.FileName = strFileName;
                    saveDlg.Filter = "Excel files (*.xlsx)|*.xlsx";
                    saveDlg.FilterIndex = 0;
                    saveDlg.RestoreDirectory = true;
                    saveDlg.Title = "Export Excel File To";
                    if (saveDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string path = saveDlg.FileName;
                        xlWorkBook.SaveCopyAs(path);
                        xlWorkBook.Saved = true;
                        xlWorkBook.Close(true, misValue, misValue);
                        xlApp.Quit();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void rbGetAllRules_CheckedChanged(object sender, EventArgs e)
        {
            grpPolicyDetails.Enabled = true;
            grpReferralDetails.Enabled = false;
            grpBeReferrals.Enabled = false;
            lblRuleNo.Enabled = false;
            txtRuleNo.Enabled = false;
        }

        private void rbCheckRule_CheckedChanged(object sender, EventArgs e)
        {
            grpPolicyDetails.Enabled = false;
            grpReferralDetails.Enabled = true;
            grpBeReferrals.Enabled = true;
            lblRuleNo.Enabled = true;
            txtRuleNo.Enabled = true;
        }
    }
}
