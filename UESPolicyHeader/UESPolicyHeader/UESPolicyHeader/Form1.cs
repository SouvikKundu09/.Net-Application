using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UESPolicyHeader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Enabled = false;
            cmbServer.Items.Add("QAT - mfrkntacesqt04");
            cmbServer.Items.Add("STG - mfrkntacesqt12");
            cmbServer.Items.Add("DEV - mfrkntacesqt11");
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
                    var strExpr = "database_name like 'sqs$%'";
                    DataRow[] drDatabases = databases.Select(strExpr);
                    DataTable dtDatabases = new DataTable();
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
                    textBox1.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Either server name is not valid or there might some error while connecting server. Error Message : " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select a server name from the dropdown.");
            }
        }

        private void btnClick_Click(object sender, EventArgs e)
        {
            string input = string.Format("'{0}'", string.Join("','", textBox1.Text.Split(',').ToList().Select(i => i.Replace("'", "''"))));
            string dbServer = cmbServer.Text.Substring(6, 14);
            string uesDB = cmbDatabase.Text.Replace("sqs$","ues$");
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please provide the policy numbers in comma separated list (Eg. 267001867,268905674).");
            }
            else
            {
                if (dbServer != string.Empty && dbServer.Length == 14)
                {
                    try
                    {
                        using (var connection = new SqlConnection("Data Source=" + dbServer + ";Initial Catalog=" + cmbDatabase.Text + ";Integrated Security=SSPI;"))
                        {
                            connection.Open();
                            string UesRerunQuery = "update pe00_schedule set awg_process_status='M' where a00_pol_num not in (" + input + ") and awg_process_status='E';" +
                                                "update pe00_schedule set awg_process_status='N' where a00_pol_num not in (" + input + ") and awg_process_status='A';" +
                                                "update pe00_schedule set awg_process_status='E' where a00_pol_num in (" + input + ");" +
                                                "delete from " + uesDB + "..hist_policy where a00_pol_num in (" + input + ");" +
                                                "delete from " + uesDB + "..hist_units where a00_pol_num in (" + input + ");" +
                                                "delete from " + uesDB + "..hist_otheritems where a00_pol_num in (" + input + ");" +
                                                "delete from " + uesDB + "..hist_endorsements where a00_pol_num in (" + input + ");" +
                                                "delete from " + uesDB + "..hist_vehicles where a00_pol_num in (" + input + ");" +
                                                "delete from " + uesDB + "..hist_coverages where a00_pol_num in (" + input + ");" +
                                                "delete from " + uesDB + "..hist_accidents where a00_pol_num in (" + input + ");" +
                                                "delete from " + uesDB + "..hist_addinterests where a00_pol_num in (" + input + ");" +
                                                "delete from " + uesDB + "..hist_claims where a00_pol_num in (" + input + ");" +
                                                "delete from " + uesDB + "..hist_dwellings where a00_pol_num in (" + input + ");" +
                                                "delete from " + uesDB + "..hist_drivers where a00_pol_num in (" + input + ");" +
                                                "delete from " + uesDB + "..hist_vehdrv where a00_pol_num in (" + input + ");" +
                                                "delete from " + uesDB + "..hist_transactions where a00_pol_num in (" + input + ");" +
                                                "delete from " + uesDB + "..hist_violations where a00_pol_num in (" + input + ");" +
                                                "delete from " + uesDB + "..referrals where policy_number in (" + input + ");" +
                                                "delete from " + uesDB + "..be_referrals where policy_number in (" + input + ");" +
                                                "delete from " + uesDB + "..es_run_record where policy_number in (" + input + ");" +
                                                "delete from " + uesDB + "..be_es_run_record where policy_number in (" + input + ");";
                            SqlCommand command = new SqlCommand(UesRerunQuery, connection);
                            int result = command.ExecuteNonQuery();
                            connection.Close();
                            if (result < 0)
                                MessageBox.Show("Error inserting data into Database!");
                            else
                                MessageBox.Show("The mentioned policy/policies are bypassed for UES job processing.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Either server name is not valid or there might some error while connecting server. Error Message : " + ex.Message);
                    }
                }
            }
        }
    }
}
