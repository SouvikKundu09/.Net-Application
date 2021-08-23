using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using BlackBookHitChoice.IccVinService;

namespace BlackBookInteractor
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            cmbServer.Items.Add("QAT - MFRKNTACESQT04");
            cmbServer.Items.Add("STG - MFRKNTACESQT12");
            cmbServer.Items.Add("DEV - MFRKNTACESQT11");
            cmbServer.SelectedIndex = 0;
            input_control(false);
            output_control(false);
            checkBox1.Enabled = button2.Enabled = button3.Enabled = textBox8.Enabled = textBox9.Enabled = textBox10.Enabled = false;
            textBox8.Font = new Font(textBox8.Font, FontStyle.Bold);
            textBox9.Font = new Font(textBox9.Font, FontStyle.Bold);
            textBox10.Font = new Font(textBox10.Font, FontStyle.Bold);
            Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                input_control(true);
                output_control(false);
                textBox4.Text = textBox5.Text= textBox6.Text= string.Empty;
                checkBox1.Checked = checkBox1.Enabled = button3.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable databases = null;
            string dbServer = cmbServer.Text.Substring(6, 14);
            if (dbServer != string.Empty && dbServer.Length == 14)
            {
                try
                {
                    using (var con = new SqlConnection("Data Source=" + dbServer + ";Integrated Security=SSPI;"))
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
                    input_control(true);
                    output_control(false);
                    button2.Enabled = true;
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

        private void button2_Click(object sender, EventArgs e)
        {
            string dbServer = cmbServer.Text.Substring(6, 14);
            DataSet nd_Data = new DataSet();
            if ((textBox1.Text.Trim().StartsWith("267") || textBox1.Text.Trim().StartsWith("268")) && Regex.IsMatch(textBox1.Text.Trim(), @"^[0-9]{9}$") && Regex.IsMatch(textBox2.Text.Trim(), @"^[0-9]{1,4}$") && Regex.IsMatch(textBox3.Text.Trim(), @"^[0-9]{1,4}$"))
            {
                if (dbServer != string.Empty && dbServer.Length == 14)
                {
                    try
                    {
                        using (var con = new SqlConnection("Data Source=" + dbServer + ";Initial Catalog=" + cmbDatabase.Text + ";Integrated Security=false;User ID=wpc1;Pwd=wpctest;"))
                        {
                            con.Open();
                            string queryUnit = "select top 1 nd.hit_ind, nd.market_value, nd.msrp_amount from nada_market_value nd(nolock) inner join punit pu(nolock) on nd.a00_pnum = pu.a00_pnum and nd.a06_edition = pu.a06_edition and nd.b79_unit = pu.b79_unit and pu.a00_pnum = '" + textBox1.Text.Trim() + "' and pu.a06_edition = (" + textBox2.Text.Trim() + " + 1) and pu.b94_location = " + textBox3.Text.Trim() + " and pu.d78_status = 'I'";
                            SqlCommand cmnd = new SqlCommand(queryUnit, con);
                            cmnd.CommandType = CommandType.Text;
                            SqlDataAdapter dtAdptr = new SqlDataAdapter(cmnd);
                            dtAdptr.Fill(nd_Data);
                            if (nd_Data != null && nd_Data.Tables.Count > 0 && nd_Data.Tables[0].Rows.Count > 0)
                            {
                                input_control(false);
                                output_control(true);
                                checkBox1.Enabled = button3.Enabled = true;
                                textBox4.Text = string.IsNullOrWhiteSpace(nd_Data.Tables[0].Rows[0]["market_value"].ToString()) ? string.Empty : nd_Data.Tables[0].Rows[0]["market_value"].ToString();
                                textBox5.Text = string.IsNullOrWhiteSpace(nd_Data.Tables[0].Rows[0]["msrp_amount"].ToString()) ? string.Empty : nd_Data.Tables[0].Rows[0]["msrp_amount"].ToString();
                                textBox6.Text = string.IsNullOrWhiteSpace(nd_Data.Tables[0].Rows[0]["hit_ind"].ToString()) ? string.Empty : nd_Data.Tables[0].Rows[0]["hit_ind"].ToString();
                            }
                            else
                            {
                                input_control(true);
                                output_control(false);
                                MessageBox.Show("Unable to find records for this Policy-Edition-Vehicle combination in nada_market_value table. Please ensure that you've executed Save To UES before this update.");
                            }
                            con.Close();
                            dtAdptr.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Either server name is not valid or there might be some error while connecting server. Error Message : " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please provide correct input for Policy, Edition and Vehicle Number");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string dbServer = cmbServer.Text.Substring(6, 14);
            if (Regex.IsMatch(textBox4.Text.Trim(), @"^[0-9]*$") && Regex.IsMatch(textBox5.Text.Trim(), @"^[0-9]*$") && Regex.IsMatch(textBox6.Text.Trim(), @"^[3-7]{1}$"))
            {
                try
                {
                    using (var con = new SqlConnection("Data Source=" + dbServer + ";Initial Catalog=" + cmbDatabase.Text + ";Integrated Security=false;User ID=wpc1;Pwd=wpctest;"))
                    {
                        con.Open();
                        string updtNADA = "update nd set nd.hit_ind = " + textBox6.Text.Trim() + ", " + (string.IsNullOrWhiteSpace(textBox4.Text.Trim()) ? "nd.market_value = NULL" : "nd.market_value = " + textBox4.Text.Trim()) + ", " + (string.IsNullOrWhiteSpace(textBox5.Text.Trim()) ? "nd.msrp_amount = NULL" : "nd.msrp_amount = " + textBox5.Text.Trim()) + " from nada_market_value nd inner join punit pu on nd.a00_pnum = pu.a00_pnum and nd.a06_edition = pu.a06_edition and nd.b79_unit = pu.b79_unit and pu.a00_pnum = '" + textBox1.Text.Trim() + "' and pu.a06_edition = (" + textBox2.Text.Trim() + " + 1) and pu.b94_location = " + textBox3.Text.Trim() + " and pu.d78_status = 'I'";
                        SqlCommand command = new SqlCommand(updtNADA, con);
                        int result = command.ExecuteNonQuery();
                        con.Close();
                        if (result < 0)
                            MessageBox.Show("Error while performing back-end update");
                        else
                            MessageBox.Show("Back-end update complete");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Either server name is not valid or there might be some error while connecting server. Error Message : " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please provide correct input for ARC, OCN and Hit Indicator");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string year = string.Empty;
            string make = string.Empty;
            string model = string.Empty;
            if (!string.IsNullOrWhiteSpace(textBox7.Text) && !string.IsNullOrWhiteSpace(textBox11.Text))
            {
                if (SearchICCVinKBBMessage(ref year, ref make, ref model))
                {
                    GetICCVinKBBMessage(year, make, model);
                }
            }
            else
            {
                MessageBox.Show("Please provide the VIN Number and the State you're testing for to get the results");
            }
        }

        private void input_control(bool control)
        {
            textBox1.Enabled = control;
            textBox2.Enabled = control;
            textBox3.Enabled = control;
        }

        private void output_control(bool control)
        {
            textBox4.Enabled = control;
            textBox5.Enabled = control;
            textBox6.Enabled = control;
        }

        private static Double? ConvertToDouble(string strConvert)
        {
            try
            {
                if (string.IsNullOrEmpty(strConvert))
                {
                    return null;
                }
                else
                {
                    return Convert.ToDouble(strConvert);
                }
            }
            catch
            {
                return null;
            }
        }

        private int GetHitIndicator(double? OCN, double? ARC, double RateDateYear, double VehicleYear)
        {
            if (OCN > default(double) && ARC > default(double))
            {
                return 7;
            }
            else if (OCN > default(double) && ARC.Equals(null))
            {
                if ((Math.Abs(RateDateYear) - Math.Abs(VehicleYear)) <= 1)
                {
                    return 5;
                }
                else
                {
                    return 6;
                }

            }
            else if (OCN.Equals(null) && ARC > default(double))
            {
                return 4;
            }
            else
            {
                return 3;
            }
        }

        private Boolean SearchICCVinKBBMessage(ref string year, ref string make, ref string model)
        {
            try
            {
                VINInformationRetrievalEPL2X1Client VRC = new VINInformationRetrievalEPL2X1Client();
                SearchVINInformationByCriteria SVBC = new SearchVINInformationByCriteria()
                {
                    SearchVINInformationByCriteriaRequest = new SearchVINInformationByCriteriaRequest()
                    {
                        PartialVehicleIdentificationNumber = textBox7.Text.Trim().ToUpper(),
                        ManufacturersName = string.Empty,
                        ModelName = string.Empty,
                        ModelYear = string.Empty
                    }
                };
                SearchVINInformationByCriteriaResponse VRS = VRC.SearchVINInformationByCriteria(SVBC);
                if (VRS.SearchVINInformationByCriteriaReply.HighLevelStatusInformation.StatusCode.ToString() == "S")
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ModelYear", typeof(string));
                    dt.Columns.Add("ManufacturersName", typeof(string));
                    dt.Columns.Add("ModelName", typeof(string));
                    DataRow dr;
                    foreach (var item in VRS.SearchVINInformationByCriteriaReply.SearchVINInformationByCriteriaOutput.VINInformation)
                    {
                        dr = dt.NewRow();
                        dr["ModelYear"] = item.ModelYear;
                        dr["ManufacturersName"] = item.ManufacturersName;
                        dr["ModelName"] = item.ModelName;
                        dt.Rows.Add(dr);
                    }
                    year = dt.Rows[0]["ModelYear"].ToString();
                    make = dt.Rows[0]["ManufacturersName"].ToString();
                    model = dt.Rows[0]["ModelName"].ToString();
                    return true;
                }
                else
                {
                    MessageBox.Show(VRS.SearchVINInformationByCriteriaReply.SearchVINInformationByCriteriaOutput.ErrorInformation.ErrorDescription.ToString());
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Facing this following issue with the BlackBook Service : " + ex.Message);
                return false;
            }
        }

        private void GetICCVinKBBMessage(string year, string make, string model)
        {
            try
            {
                VINInformationRetrievalEPL2X1Client VRC = new VINInformationRetrievalEPL2X1Client();
                GetExpandedVINInformationInput GEVI = new GetExpandedVINInformationInput();
                GEVI.VehicleIdentificationNumber = textBox7.Text.Trim().ToUpper();
                GEVI.ManufacturersName = make.ToString().Trim();
                GEVI.ModelName = model.ToString().Trim();
                GEVI.ModelYear = year.ToString().Trim();
                GEVI.PostalStateAbbreviation = textBox11.Text.ToString().Trim();
                GEVI.RateDate = DateTime.Today;
                GetExpandedVINInformationListRequest GEVR = new GetExpandedVINInformationListRequest()
                {
                    GetExpandedVINInformationList = new GetExpandedVINInformationList()
                    {
                        GetExpandedVINInformationListRequest = new GetExpandedVINInformationInput[1],
                    }
                };

                GEVR.GetExpandedVINInformationList.GetExpandedVINInformationListRequest[0] = GEVI;
                GetExpandedVINInformationListResponse GETR = VRC.GetExpandedVINInformationList(GEVR.GetExpandedVINInformationList);
                if (GETR.GetExpandedVINInformationListReply.HighLevelStatusInformation.StatusCode.ToString() == "S")
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ModelYear", typeof(string));
                    dt.Columns.Add("ManufacturersName", typeof(string));
                    dt.Columns.Add("ModelName", typeof(string));
                    dt.Columns.Add("averageRetailPriceAmount", typeof(string));
                    dt.Columns.Add("vehicleOriginalCostNewAmount", typeof(string));

                    DataRow dr;
                    foreach (var item in GETR.GetExpandedVINInformationListReply.GetExpandedVINInformationOutput)
                    {
                        dr = dt.NewRow();
                        dr["ModelYear"] = item.VINInformation.ModelYear;
                        dr["ManufacturersName"] = item.VINInformation.ManufacturersName;
                        dr["ModelName"] = item.VINInformation.ModelName;
                        dr["averageRetailPriceAmount"] = item.VINInformation.AverageRetailPriceAmount;
                        dr["vehicleOriginalCostNewAmount"] = item.VINInformation.VehicleOriginalCostNewAmount;
                        dt.Rows.Add(dr);
                    }
                    textBox8.Text = dt.Rows[0]["averageRetailPriceAmount"].ToString();
                    textBox9.Text = dt.Rows[0]["vehicleOriginalCostNewAmount"].ToString();
                    double? ARC = ConvertToDouble(textBox8.Text).Equals(default(double)) ? null : ConvertToDouble(textBox8.Text);
                    double? OCN = ConvertToDouble(textBox9.Text).Equals(default(double)) ? null : ConvertToDouble(textBox9.Text);
                    textBox10.Text = Convert.ToString(GetHitIndicator(OCN, ARC, Convert.ToDouble(DateTime.Now.Year), Convert.ToDouble(dt.Rows[0]["ModelYear"].ToString())));
                }
                else
                {
                    MessageBox.Show("BlackBook Service is not returning any data form this VIN Number");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Facing this following issue with the BlackBook Service : " + ex.Message);
            }
        }
    }
}
