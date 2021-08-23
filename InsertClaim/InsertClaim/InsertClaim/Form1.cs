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

namespace InsertClaim
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;
            textBox9.Enabled = false;
            textBox11.Enabled = false;
            textBox12.Enabled = false;
            textBox13.Enabled = false;
            textBox14.Enabled = false;
            textBox15.Enabled = false;
            textBox16.Enabled = false;
            textClaim.Enabled = false;
            txtClaim.Enabled = false;
            txtUnitName.Enabled = false;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            cmbLossType.Enabled = false;
            cmbLossTypeInstallment.Enabled = false;
            textBox6.Text = "000";
            textBox12.Text = "-100";
            cmbServer.Items.Add("QAT - MFRKNTACESQT04");
            cmbServer.Items.Add("STG - MFRKNTACESQT12");
            cmbServer.Items.Add("DEV - MFRKNTACESQT11");
            cmbServer.SelectedIndex = 0;
            cmbLossType.Items.Add("COMPFGLASS");
            cmbLossType.Items.Add("COMPREHENSIVE");
            cmbLossType.Items.Add("COLLISION");
            cmbLossType.Items.Add("PIP LOSS");
            cmbLossType.SelectedIndex = 0;
            cmbLossTypeInstallment.Items.AddRange(cmbLossType.Items.Cast<Object>().ToArray());
            cmbLossTypeInstallment.SelectedIndex = 0;
            comboBox2.Items.Add("Closed");
            comboBox2.Items.Add("Open");
            comboBox2.SelectedIndex = 0;
            chkChoice.Enabled = false;
        }

        private void chkChoice_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Text = "000";
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox11.Clear();
            textBox12.Text = "-100";
            textBox13.Clear();
            textBox14.Clear();
            comboBox2.SelectedIndex = 0;
            gBox1.Enabled = (chkChoice.Checked) ? true : false;
            gBox2.Enabled = (chkChoice.Checked) ? false : true;
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
                    textBox6.Enabled = true;
                    textBox8.Enabled = true;
                    textBox9.Enabled = true;
                    textBox11.Enabled = true;
                    textBox12.Enabled = true;
                    textBox15.Enabled = true;
                    textBox16.Enabled = true;
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                    txtClaim.Enabled = true;
                    chkChoice.Enabled = true;
                    chkChoice.Checked = true;
                    chkChoice_CheckedChanged(sender, e);
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox15.Text) || string.IsNullOrWhiteSpace(textBox16.Text))
            {
               MessageBox.Show("Please provide both policy number and edition to proceed.");
            }
            else
            {
                DataTable datastates = null;
                DataSet ds = new DataSet();
                DataSet cmn = new DataSet();
                string dbServer = cmbServer.Text.Substring(6, 14);
                if (dbServer != string.Empty && dbServer.Length == 14)
                {
                    try
                    {
                        using (var connec = new SqlConnection("Data Source=" + dbServer + ";Initial Catalog=" + cmbDatabase.Text + ";Integrated Security=SSPI;"))
                        {
                            connec.Open();
                            string squeryunit = "select (b80_userline + ' - ' +  c86_unitdesc) as unit_details, b79_unit as unit_number from punit(nolock) where a00_pnum = '" + textBox15.Text + "' and a06_edition = " + textBox16.Text + " and UPPER(c86_unitdesc) <> 'POLICY LEVEL ENDORSEMENTS' and d78_status='I'";
                            SqlCommand scmd = new SqlCommand(squeryunit, connec);
                            scmd.CommandType = CommandType.Text;
                            SqlDataAdapter da = new SqlDataAdapter(scmd);
                            da.Fill(ds);
                            datastates = ds.Tables[0];
                            if (datastates.Rows.Count > 0)
                            {
                                DataRow dummy = datastates.NewRow();
                                dummy["unit_details"] = "--select unit--";
                                datastates.Rows.InsertAt(dummy, 0);
                                comboBox1.DataSource = datastates;
                                comboBox1.DisplayMember = "unit_details";
                                comboBox1.ValueMember = "unit_number";
                                comboBox1.SelectedIndex = 0;
                                string squerycommon = "select a01_company, a08_fdate from pcommon(nolock) where a00_pnum = '" + textBox15.Text + "' and a06_edition = " + textBox16.Text + " and d14_status='I'";
                                SqlCommand cmd = new SqlCommand(squerycommon, connec);
                                cmd.CommandType = CommandType.Text;
                                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                                adap.Fill(cmn);
                                textBox2.Text = textBox15.Text;
                                textBox3.Text = textBox16.Text;
                                string[] effdate = cmn.Tables[0].Rows[0]["a08_fdate"].ToString().Split(' ');
                                textBox4.Text = effdate[0];
                                textBox14.Text = cmn.Tables[0].Rows[0]["a01_company"].ToString();
                                textBox7.Enabled = true;
                                connec.Close();
                                adap.Dispose();
                                da.Dispose();
                            }
                            else
                            {
                                MessageBox.Show("This edition of the policy doesn't exist.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Either server name is not valid or there might some error while connecting server. Error Message : " + ex.Message);
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string unit_desc = comboBox1.Text.ToString();
            if (comboBox1.SelectedIndex > 0)
            {
                textBox1.Text = comboBox1.SelectedValue.ToString();
                textBox5.Text = unit_desc.Substring(5, unit_desc.Length - 5);
                DataSet dataState = new DataSet();
                string dbServer = cmbServer.Text.Substring(6, 14);
                if (dbServer != string.Empty && dbServer.Length == 14)
                {
                    try
                    {
                        using (var connection = new SqlConnection("Data Source=" + dbServer + ";Initial Catalog=" + cmbDatabase.Text + ";Integrated Security=SSPI;"))
                        {
                            connection.Open();
                            string squerystate = "select b80_userline, a02_state from punit(nolock) where a00_pnum = '" + textBox15.Text + "' and a06_edition = " + textBox16.Text + " and c86_unitdesc = '" + textBox5.Text + "' and d78_status='I'";
                            SqlCommand scmd = new SqlCommand(squerystate, connection);
                            scmd.CommandType = CommandType.Text;
                            SqlDataAdapter da = new SqlDataAdapter(scmd);
                            da.Fill(dataState);
                            textBox13.Text = dataState.Tables[0].Rows[0]["a02_state"].ToString();
                            if (dataState.Tables[0].Rows[0]["b80_userline"].ToString().Equals("01"))
                            {
                                cmbLossType.Enabled = true;
                            }
                            else
                            {
                                cmbLossType.Enabled = false;
                            }
                            connection.Close();
                            da.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Either server name is not valid or there might some error while connecting server. Error Message : " + ex.Message);
                    }
                }
            }
            else
            {
                textBox1.Text = string.Empty;
                textBox5.Text = string.Empty;
                textBox13.Text = string.Empty;
            }
        }

        private void btnMapUnit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtClaim.Text))
            {
                MessageBox.Show("Please provide the existing claim number Claim Number to proceed.");
            }
            else
            {
                DataTable datastates = null;
                DataSet ds = new DataSet();
                DataSet cmn = new DataSet();
                string dbServer = cmbServer.Text.Substring(6, 14);
                if (dbServer != string.Empty && dbServer.Length == 14)
                {
                    try
                    {
                        using (var connec = new SqlConnection("Data Source=" + dbServer + ";Initial Catalog=" + cmbDatabase.Text + ";Integrated Security=SSPI;"))
                        {
                            connec.Open();
                            string squeryunit = "select (b80_userline + ' - ' +  c86_unitdesc) as unit_details, b79_unit as unit_number, b80_userline from creserve(nolock) where b69_claim_occur = '" + txtClaim.Text + "'";
                            SqlCommand scmd = new SqlCommand(squeryunit, connec);
                            scmd.CommandType = CommandType.Text;
                            SqlDataAdapter da = new SqlDataAdapter(scmd);
                            da.Fill(ds);
                            datastates = ds.Tables[0];
                            if (datastates.Rows.Count > 0)
                            {
                                txtUnitName.Text = datastates.Rows[0]["unit_details"].ToString();
                                string squerycommon = "select pc.a00_pnum, pc.a06_edition, pc.a01_company, pc.a08_fdate, v.d87_acc_state, v.b70_loss_date from pcommon pc(nolock) INNER JOIN (select a00_pnum, a06_edition, d87_acc_state, b70_loss_date from ccommon(nolock) where b69_claim_occur = '" + txtClaim.Text + "') as v on pc.a00_pnum = v.a00_pnum and pc.a06_edition = v.a06_edition and pc.d14_status = 'I'";
                                SqlCommand cmd = new SqlCommand(squerycommon, connec);
                                cmd.CommandType = CommandType.Text;
                                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                                adap.Fill(cmn);
                                textBox1.Text = datastates.Rows[0]["unit_number"].ToString();
                                textBox2.Text = cmn.Tables[0].Rows[0]["a00_pnum"].ToString();
                                textBox3.Text = cmn.Tables[0].Rows[0]["a06_edition"].ToString();
                                string[] effdate = cmn.Tables[0].Rows[0]["a08_fdate"].ToString().Split(' ');
                                textBox4.Text = effdate[0];
                                textBox5.Text = txtUnitName.Text.Substring(5, txtUnitName.Text.Length - 5);
                                string[] lossdate = cmn.Tables[0].Rows[0]["b70_loss_date"].ToString().Split(' ');
                                textBox7.Text = lossdate[0];
                                textBox7.Enabled = false;
                                textBox13.Text = cmn.Tables[0].Rows[0]["d87_acc_state"].ToString();
                                textBox14.Text = cmn.Tables[0].Rows[0]["a01_company"].ToString();
                                if (datastates.Rows[0]["b80_userline"].ToString().Equals("01"))
                                {
                                    cmbLossTypeInstallment.Enabled = true;
                                }
                                else
                                {
                                    cmbLossTypeInstallment.Enabled = false;
                                }
                                connec.Close();
                                adap.Dispose();
                                da.Dispose();
                            }
                            else
                            {
                                MessageBox.Show("This claim number doesn't exist in database.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Either server name is not valid or there might some error while connecting server. Error Message : " + ex.Message);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataSet dsnewclaimnumber = new DataSet();
            string dbServer = cmbServer.Text.Substring(6, 14);
            DateTime dt1, dt2;
            int result = 0;
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) || string.IsNullOrWhiteSpace(textBox5.Text) || string.IsNullOrWhiteSpace(textBox6.Text) ||
                string.IsNullOrWhiteSpace(textBox7.Text) || string.IsNullOrWhiteSpace(textBox8.Text) || string.IsNullOrWhiteSpace(textBox9.Text) ||
                string.IsNullOrWhiteSpace(comboBox2.Text) || string.IsNullOrWhiteSpace(textBox11.Text) || string.IsNullOrWhiteSpace(textBox12.Text) ||
                string.IsNullOrWhiteSpace(textBox13.Text) || string.IsNullOrWhiteSpace(textBox14.Text))
            {
                MessageBox.Show("All fields are mandatory.");
            }
            else
            {
                if (!DateTime.TryParse(textBox7.Text, out dt1))
                {
                    textBox7.Text = string.Empty;
                    MessageBox.Show("Please provide valid loss date.");
                }
                else
                {
                    if (!DateTime.TryParse(textBox9.Text, out dt2))
                    {
                        textBox9.Text = string.Empty;
                        MessageBox.Show("Please provide valid paid date.");
                    }
                    else
                    {
                        if (dbServer != string.Empty && dbServer.Length == 14)
                        {
                            try
                            {
                                string insertquery = string.Empty;
                                using (var connection = new SqlConnection("Data Source=" + dbServer + ";Initial Catalog=" + cmbDatabase.Text + ";Integrated Security=SSPI;"))
                                {
                                    connection.Open();
                                    if (chkChoice.Checked)
                                    {
                                        string newclaimnumber = "select (MAX(b69_claim_occur)+1) as Claim_Number from ccommon(nolock)";
                                        SqlCommand scmd = new SqlCommand(newclaimnumber, connection);
                                        scmd.CommandType = CommandType.Text;
                                        SqlDataAdapter da = new SqlDataAdapter(scmd);
                                        da.Fill(dsnewclaimnumber);
                                        insertquery = string.Format("insert into ccommon(a00_pnum,a06_edition,a29_fdate,a30_xdate,b69_claim_occur,b70_loss_date,b98_ptype,d43_reported_date,d55_cat_code,d74_claim_type,d78_status,d87_acc_state,e04_insured_num,e87_status,e87_status_date,f26_ind_adjuster,f43_claims,f58_userid,h53_ndate,r28_supervisor,r30_in_house_adj,v62_occur_deduct,z13_reported,f67_notes_ind,a87_f_destroy_date,l15_attorney,a13_act_status,h68_severity,a0t_policy_limit,f58_user_lockout,a01_company,a36_groupline,d85_originating_co,t91_pol_key,a4g_insured_liable,d94_auto_rein_calc,d44_claims_made,w63_es_claim_comp,b25_add_interests,a03_claim_branch,at1_suit_date,v82_waive_ded_ind,b99_link_type,b69_link_num,f58_settle_userid,u24_settle_date,u24_settle_amount,g78_program_code,b80_userline,b72_excused,rewritten_polnum,rewritten_edition,b64_pay_date) select @policy,@edition,@a29_fdate,DATEADD(YEAR, 1, @a29_fdate),@NewclaimNumber,@b70_loss_date,b98_ptype,(DATEADD(MONTH, 1, @b70_loss_date)),@d55_cat_code,d74_claim_type,d78_status,@d87_acc_state,e04_insured_num,@e87_status,e87_status_date,f26_ind_adjuster,f43_claims,f58_userid,h53_ndate,r28_supervisor,r30_in_house_adj,v62_occur_deduct,z13_reported,f67_notes_ind,a87_f_destroy_date,l15_attorney,a13_act_status,h68_severity,a0t_policy_limit,f58_user_lockout,@a01_company,a36_groupline,d85_originating_co,t91_pol_key,a4g_insured_liable,d94_auto_rein_calc,d44_claims_made,w63_es_claim_comp,b25_add_interests,a03_claim_branch,at1_suit_date,v82_waive_ded_ind,b99_link_type,b69_link_num,f58_settle_userid,u24_settle_date,u24_settle_amount,g78_program_code,b80_userline,b72_excused,rewritten_polnum,rewritten_edition,@b64_pay_date from MFRKNTACESQT04.sqs$atw.dbo.ccommon where b69_claim_occur= @claimNumber; insert into creserve(b69_claim_occur,b79_unit,c86_unitdesc,e04_claimant_num,e87_status,e87_status_date,e93_di_loss,e98_di_loss_orig,e98_di_exp_orig,e99_di_exp,f00_di_loss_sal,f00_di_exp_sal,f02_di_loss_sub,f02_di_exp_sub,f03_di_loss_ded,f03_di_exp_ded,f04_di_loss_paid,f09_di_exp_paid,f10_di_loss_sal_rc,f10_di_exp_sal_rc,f11_di_loss_sub_rc,f11_di_exp_sub_rc,f12_di_loss_ded_rc,f12_di_exp_ded_rc,f58_userid,h53_ndate,u10_clmnum,d59_cause_of_loss,d60_type_of_loss,e93_ds_loss,e99_ds_exp,f00_ds_exp_sal,f00_ds_loss_sal,f02_ds_exp_sub,f02_ds_loss_sub,f03_ds_exp_ded,f03_ds_loss_ded,max_loss,max_expense,b83_class_code_ind,c07_limit_1_ind,c07_limit_2_ind,c07_limit_3_ind,b85_ded_amt_ind,c07_aggregate,c07_policy_limit,c07_claimid,b80_userline,b85_ded_amt,u35_rdft_lo_totamt,u35_rdft_ex_totamt,ac4_di_loss_contr,ac4_di_exp_contr,ac4_ds_loss_contr,ac4_ds_exp_contr,ac5_di_loss_rc,ac5_di_exp_rc,c87_split_ind,t29_sched_key,t29_item,e04_sched_orignum,b64_pay_date) select @NewclaimNumber,@b79_unit,@unitDesc,e04_claimant_num,@e87_status,e87_status_date,e93_di_loss,e98_di_loss_orig,e98_di_exp_orig,e99_di_exp,f00_di_loss_sal,f00_di_exp_sal,f02_di_loss_sub,f02_di_exp_sub,f03_di_loss_ded,f03_di_exp_ded,@losspaid,f09_di_exp_paid,f10_di_loss_sal_rc,f10_di_exp_sal_rc,@f11_di_loss_sub_rc,f11_di_exp_sub_rc,f12_di_loss_ded_rc,f12_di_exp_ded_rc,f58_userid,h53_ndate,1,@d59_cause_of_loss,d60_type_of_loss,e93_ds_loss,e99_ds_exp,f00_ds_exp_sal,f00_ds_loss_sal,f02_ds_exp_sub,f02_ds_loss_sub,f03_ds_exp_ded,f03_ds_loss_ded,max_loss,max_expense,b83_class_code_ind,c07_limit_1_ind,c07_limit_2_ind,c07_limit_3_ind,b85_ded_amt_ind,c07_aggregate,c07_policy_limit,c07_claimid,b80_userline,b85_ded_amt,u35_rdft_lo_totamt,u35_rdft_ex_totamt,ac4_di_loss_contr,ac4_di_exp_contr,ac4_ds_loss_contr,ac4_ds_exp_contr,ac5_di_loss_rc,ac5_di_exp_rc,c87_split_ind,t29_sched_key,t29_item,e04_sched_orignum,b64_pay_date from MFRKNTACESQT04.sqs$atw.dbo.creserve where b69_claim_occur=@claimNumber; insert into cstat(b83_class,b85_ded_amt,b69_claim_occur,c07_limit_1,c07_limit_2,c07_limit_3,t91_punit_key,t91_pcov_key,u10_clmnum,b97_sbl,c87_coverage,e62_asl,e64_injury_kind,u58_loss_coverage,e66_injury_nature,i62_exposure,aa7_loss_activity,b45_gender,b41_date_of_birth,e65_body_part,g34_type_injury,g28_rdft_lo_sdate,g28_rdft_ex_sdate,g29_rdft_lo_tpmnts,g29_rdft_ex_tpmnts,g30_rdft_lo_intrvl,g30_rdft_ex_intrvl,v34_rdft_lo_tmspd,v34_rdft_ex_tmspd,a81_end_no,b80_userline,b84_ded_type,c96_coins_pct,n66_retro_date,a8p_tail_date) select b83_class,b85_ded_amt,@NewclaimNumber,c07_limit_1,c07_limit_2,c07_limit_3,t91_punit_key,t91_pcov_key,1,b97_sbl,{0},e62_asl,e64_injury_kind,u58_loss_coverage,e66_injury_nature,i62_exposure,aa7_loss_activity,b45_gender,b41_date_of_birth,e65_body_part,g34_type_injury,g28_rdft_lo_sdate,g28_rdft_ex_sdate,g29_rdft_lo_tpmnts,g29_rdft_ex_tpmnts,g30_rdft_lo_intrvl,g30_rdft_ex_intrvl,v34_rdft_lo_tmspd,v34_rdft_ex_tmspd,a81_end_no,b80_userline,b84_ded_type,c96_coins_pct,n66_retro_date,a8p_tail_date from MFRKNTACESQT04.sqs$atw.dbo.cstat where b69_claim_occur=@claimNumber; update ccommon set e04_insured_num=(select top 1 e04_ins_orignum from pcommon(nolock) where a00_pnum=@policy) where a00_pnum=@policy;", comboBox1.Text.ToString().Substring(0, 2).Equals("01") ? "@coverage" : "c87_coverage");
                                        SqlCommand command = new SqlCommand(insertquery, connection);
                                        command.Parameters.AddWithValue("@b79_unit", textBox1.Text);
                                        command.Parameters.AddWithValue("@policy", textBox2.Text);
                                        command.Parameters.AddWithValue("@edition", textBox3.Text);
                                        command.Parameters.AddWithValue("@a29_fdate", textBox4.Text);
                                        command.Parameters.AddWithValue("@unitDesc", textBox5.Text);
                                        command.Parameters.AddWithValue("@d55_cat_code", textBox6.Text);
                                        command.Parameters.AddWithValue("@b70_loss_date", textBox7.Text);
                                        command.Parameters.AddWithValue("@losspaid", textBox8.Text);
                                        command.Parameters.AddWithValue("@b64_pay_date", textBox9.Text);
                                        command.Parameters.AddWithValue("@d59_cause_of_loss", textBox11.Text);
                                        command.Parameters.AddWithValue("@f11_di_loss_sub_rc", textBox12.Text);
                                        command.Parameters.AddWithValue("@d87_acc_state", textBox13.Text);
                                        command.Parameters.AddWithValue("@a01_company", textBox14.Text);
                                        command.Parameters.AddWithValue("@NewclaimNumber", dsnewclaimnumber.Tables[0].Rows[0]["Claim_Number"].ToString());
                                        if (comboBox2.Text.ToString().Equals("Closed"))
                                            command.Parameters.AddWithValue("@e87_status", "2");
                                        else if (comboBox2.Text.ToString().Equals("Open"))
                                            command.Parameters.AddWithValue("@e87_status", "0");
                                        if (comboBox1.Text.ToString().Substring(0, 2).Equals("01"))
                                        {
                                            command.Parameters.AddWithValue("@claimNumber", "98801483");
                                            if (cmbLossType.Text.ToString().Equals("COMPFGLASS"))
                                                command.Parameters.AddWithValue("@coverage", "211");
                                            else if (cmbLossType.Text.ToString().Equals("COMPREHENSIVE"))
                                                command.Parameters.AddWithValue("@coverage", "210");
                                            else if (cmbLossType.Text.ToString().Equals("COLLISION"))
                                                command.Parameters.AddWithValue("@coverage", "200");
                                            else if (cmbLossType.Text.ToString().Equals("PIP LOSS"))
                                                command.Parameters.AddWithValue("@coverage", "160");
                                        }
                                        else
                                        {
                                            command.Parameters.AddWithValue("@claimNumber", "77107031");
                                        }
                                        result = command.ExecuteNonQuery();
                                        textClaim.Text = dsnewclaimnumber.Tables[0].Rows[0]["Claim_Number"].ToString();
                                    }
                                    else
                                    {
                                        string newClmInstallment = "select (MAX(u10_clmnum)+1) as Claim_Ins from creserve(nolock) where b69_claim_occur = '" + txtClaim.Text + "'";
                                        SqlCommand scmd = new SqlCommand(newClmInstallment, connection);
                                        scmd.CommandType = CommandType.Text;
                                        SqlDataAdapter da = new SqlDataAdapter(scmd);
                                        da.Fill(dsnewclaimnumber);
                                        insertquery = string.Format("update ccommon set b64_pay_date = @b64_pay_date where b69_claim_occur = @b69_claim_occur; insert into creserve(b69_claim_occur,b79_unit,c86_unitdesc,e04_claimant_num,e87_status,e87_status_date,e93_di_loss,e98_di_loss_orig,e98_di_exp_orig,e99_di_exp,f00_di_loss_sal,f00_di_exp_sal,f02_di_loss_sub,f02_di_exp_sub,f03_di_loss_ded,f03_di_exp_ded,f04_di_loss_paid,f09_di_exp_paid,f10_di_loss_sal_rc,f10_di_exp_sal_rc,f11_di_loss_sub_rc,f11_di_exp_sub_rc,f12_di_loss_ded_rc,f12_di_exp_ded_rc,f58_userid,h53_ndate,u10_clmnum,d59_cause_of_loss,d60_type_of_loss,e93_ds_loss,e99_ds_exp,f00_ds_exp_sal,f00_ds_loss_sal,f02_ds_exp_sub,f02_ds_loss_sub,f03_ds_exp_ded,f03_ds_loss_ded,max_loss,max_expense,b83_class_code_ind,c07_limit_1_ind,c07_limit_2_ind,c07_limit_3_ind,b85_ded_amt_ind,c07_aggregate,c07_policy_limit,c07_claimid,b80_userline,b85_ded_amt,u35_rdft_lo_totamt,u35_rdft_ex_totamt,ac4_di_loss_contr,ac4_di_exp_contr,ac4_ds_loss_contr,ac4_ds_exp_contr,ac5_di_loss_rc,ac5_di_exp_rc,c87_split_ind,t29_sched_key,t29_item,e04_sched_orignum,b64_pay_date) select top 1 @b69_claim_occur,b79_unit,c86_unitdesc,e04_claimant_num,@e87_status,e87_status_date,e93_di_loss,e98_di_loss_orig,e98_di_exp_orig,e99_di_exp,f00_di_loss_sal,f00_di_exp_sal,f02_di_loss_sub,f02_di_exp_sub,f03_di_loss_ded,f03_di_exp_ded,@losspaid,f09_di_exp_paid,f10_di_loss_sal_rc,f10_di_exp_sal_rc,@f11_di_loss_sub_rc,f11_di_exp_sub_rc,f12_di_loss_ded_rc,f12_di_exp_ded_rc,f58_userid,h53_ndate,@NewClmNum,@d59_cause_of_loss,d60_type_of_loss,e93_ds_loss,e99_ds_exp,f00_ds_exp_sal,f00_ds_loss_sal,f02_ds_exp_sub,f02_ds_loss_sub,f03_ds_exp_ded,f03_ds_loss_ded,max_loss,max_expense,b83_class_code_ind,c07_limit_1_ind,c07_limit_2_ind,c07_limit_3_ind,b85_ded_amt_ind,c07_aggregate,c07_policy_limit,c07_claimid,b80_userline,b85_ded_amt,u35_rdft_lo_totamt,u35_rdft_ex_totamt,ac4_di_loss_contr,ac4_di_exp_contr,ac4_ds_loss_contr,ac4_ds_exp_contr,ac5_di_loss_rc,ac5_di_exp_rc,c87_split_ind,t29_sched_key,t29_item,e04_sched_orignum,b64_pay_date from creserve where b69_claim_occur = @b69_claim_occur; insert into cstat(b83_class,b85_ded_amt,b69_claim_occur,c07_limit_1,c07_limit_2,c07_limit_3,t91_punit_key,t91_pcov_key,u10_clmnum,b97_sbl,c87_coverage,e62_asl,e64_injury_kind,u58_loss_coverage,e66_injury_nature,i62_exposure,aa7_loss_activity,b45_gender,b41_date_of_birth,e65_body_part,g34_type_injury,g28_rdft_lo_sdate,g28_rdft_ex_sdate,g29_rdft_lo_tpmnts,g29_rdft_ex_tpmnts,g30_rdft_lo_intrvl,g30_rdft_ex_intrvl,v34_rdft_lo_tmspd,v34_rdft_ex_tmspd,a81_end_no,b80_userline,b84_ded_type,c96_coins_pct,n66_retro_date,a8p_tail_date) select top 1 b83_class,b85_ded_amt,@b69_claim_occur,c07_limit_1,c07_limit_2,c07_limit_3,t91_punit_key,t91_pcov_key,@NewClmNum,b97_sbl,{0},e62_asl,e64_injury_kind,u58_loss_coverage,e66_injury_nature,i62_exposure,aa7_loss_activity,b45_gender,b41_date_of_birth,e65_body_part,g34_type_injury,g28_rdft_lo_sdate,g28_rdft_ex_sdate,g29_rdft_lo_tpmnts,g29_rdft_ex_tpmnts,g30_rdft_lo_intrvl,g30_rdft_ex_intrvl,v34_rdft_lo_tmspd,v34_rdft_ex_tmspd,a81_end_no,b80_userline,b84_ded_type,c96_coins_pct,n66_retro_date,a8p_tail_date from cstat where b69_claim_occur = @b69_claim_occur;", txtUnitName.Text.ToString().Substring(0, 2).Equals("01") ? "@coverage" : "c87_coverage");
                                        SqlCommand command = new SqlCommand(insertquery, connection);
                                        command.Parameters.AddWithValue("@b69_claim_occur", txtClaim.Text);
                                        command.Parameters.AddWithValue("@losspaid", textBox8.Text);
                                        command.Parameters.AddWithValue("@b64_pay_date", textBox9.Text);
                                        command.Parameters.AddWithValue("@d59_cause_of_loss", textBox11.Text);
                                        command.Parameters.AddWithValue("@f11_di_loss_sub_rc", textBox12.Text);
                                        command.Parameters.AddWithValue("@NewClmNum", dsnewclaimnumber.Tables[0].Rows[0]["Claim_Ins"].ToString());
                                        if (comboBox2.Text.ToString().Equals("Closed"))
                                            command.Parameters.AddWithValue("@e87_status", "2");
                                        else if (comboBox2.Text.ToString().Equals("Open"))
                                            command.Parameters.AddWithValue("@e87_status", "0");
                                        if (txtUnitName.Text.ToString().Substring(0, 2).Equals("01"))
                                        {
                                            command.Parameters.AddWithValue("@claimNumber", "98801483");
                                            if (cmbLossType.Text.ToString().Equals("COMPFGLASS"))
                                                command.Parameters.AddWithValue("@coverage", "211");
                                            else if (cmbLossType.Text.ToString().Equals("COMPREHENSIVE"))
                                                command.Parameters.AddWithValue("@coverage", "210");
                                            else if (cmbLossType.Text.ToString().Equals("COLLISION"))
                                                command.Parameters.AddWithValue("@coverage", "200");
                                            else if (cmbLossType.Text.ToString().Equals("PIP LOSS"))
                                                command.Parameters.AddWithValue("@coverage", "160");
                                        }
                                        else
                                        {
                                            command.Parameters.AddWithValue("@claimNumber", "77107031");
                                        }
                                        result = command.ExecuteNonQuery();
                                        textClaim.Text = txtClaim.Text + "-" + dsnewclaimnumber.Tables[0].Rows[0]["Claim_Ins"].ToString();
                                    }
                                    connection.Close();
                                    if (result < 0)
                                        MessageBox.Show("Error inserting data into Database!");
                                    else
                                        MessageBox.Show("Claim successfully inserted.");
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

        private void button3_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, textClaim.Text);
        }
    }
}
