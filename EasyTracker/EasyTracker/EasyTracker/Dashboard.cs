using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO.Compression;

namespace EasyTracker
{
    public partial class EasyTracker : Form
    {
        List<BatchAppDtls> lstBatchJobs = new List<BatchAppDtls>();
        BatchPath batchpath = new BatchPath();
        string currentDirectory = Environment.CurrentDirectory.Replace("~", string.Empty);
        string BuildPath = @"C:\DownstreamBuild";
        string DownstreamPath = @"C:\BatchApps\Downstream\";

        public EasyTracker()
        {
            InitializeComponent();
            Initialize();
        }

        //Inititalize values for the Windows Application
        private void Initialize()
        {
            dtCurrentDate.Enabled = false;

            chkSetup.Checked = true;
            chkSetup.Text = "Yes";
            chkBuildDone.Checked = true;
            chkRIS.Checked = false;
            chkTARABS.Checked = false;
            chkTARABAN.Checked = false;
            chkINS.Checked = false;
            chkRISBAL.Checked = false;
            chkALL.Checked = false;

            txtLocalBuildPath.Enabled = false;
            btnBuild.Enabled = false;
            btnSetup.Enabled = false;
            lstBatchJobs = PopulateBatchJobList();

            cmbServer.Items.Add("QAT - MFRKNTACESQT04");
            cmbServer.Items.Add("DEV - MFRKNTACESQT11");

            if (cmbServer.Text.Equals("QAT - MFRKNTACESQT04") || (cmbServer.Text.Equals("DEV - MFRKNTACESQT11")))
            {
                DataTable dtDatabases = Helper.LoadDatabases(cmbServer.SelectedText);
                cmbDatabase.DataSource = dtDatabases;
                cmbDatabase.DisplayMember = "database_name";
            }

            cmbServer.SelectedIndex = 0;

            batchpath.pathPBACC = BuildPath + @"\PBACC";
            batchpath.pathRIS = BuildPath + @"\RIS";
            batchpath.pathTARABS = BuildPath + @"\TARABS";
            batchpath.pathTARABAN = BuildPath + @"\TARABAN";
            batchpath.pathINS = BuildPath + @"\INS";
            batchpath.pathRISBAL = BuildPath + @"\RISBAL";

            Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
        }

        //Enable/Disable the INS/RISBAL setup button
        private void chkSetup_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSetup.Checked)
            {
                chkSetup.Text = "Yes";
                btnSetup.Enabled = false;
                if (chkBuildDone.Checked)
                {
                    txtPolicy.Enabled = true;
                    txtGeneratePath.Enabled = true;
                    cmbServer.Enabled = true;
                    cmbDatabase.Enabled = true;
                    chkRIS.Enabled = true;
                    chkTARABS.Enabled = true;
                    chkTARABAN.Enabled = true;
                    chkINS.Enabled = true;
                    chkRISBAL.Enabled = true;
                    chkALL.Enabled = true;
                    btnProcess.Enabled = true;
                }
            }
            else
            {
                chkSetup.Text = "No";
                btnSetup.Enabled = true;
                txtPolicy.Enabled = false;
                txtGeneratePath.Enabled = false;
                cmbServer.Enabled = false;
                cmbDatabase.Enabled = false;
                chkRIS.Enabled = false;
                chkTARABS.Enabled = false;
                chkTARABAN.Enabled = false;
                chkINS.Enabled = false;
                chkRISBAL.Enabled = false;
                chkALL.Enabled = false;
                btnProcess.Enabled = false;
            }
        }

        //Perform Setup for INS/RISBAL
        private void btnSetup_Click(object sender, EventArgs e)
        {
            if (RunBatch.EditRegistry() && RunBatch.ExecuteCMD() && RunBatch.ConfigureODBC())
            {
                chkSetup.Checked = true;
                chkSetup_CheckedChanged(sender, e);
                MessageBox.Show("INS and RISBAL Setup Completed.");
            }
        }

        //Load Real-Time Databases instantly
        private void cmbServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cmbServer.Text.Equals(string.Empty))
            {
                DataTable dtDatabases = Helper.LoadDatabases(cmbServer.Text);
                cmbDatabase.DataSource = dtDatabases;
                cmbDatabase.DisplayMember = "database_name";
            }
        }

        //Select the Downstream Batches to Build
        private List<BatchAppDtls> PopulateBatchJobList()
        {
            try
            {
                List<BatchAppDtls> oLstDetails = new List<BatchAppDtls>();
                oLstDetails.Add(new BatchAppDtls { ApplicationName = "PBACC" });
                oLstDetails.Add(new BatchAppDtls { ApplicationName = "RIS" });
                oLstDetails.Add(new BatchAppDtls { ApplicationName = "TARABS" });
                oLstDetails.Add(new BatchAppDtls { ApplicationName = "TARABAN" });
                return oLstDetails;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Error in populating Batch Job List.");
                LogError.ErrorLog(exp.StackTrace);
                return null;
            }
        }

        //If Build is not done then, first Build the Batches
        private void chkBuildDone_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBuildDone.Checked)
            {
                txtLocalBuildPath.Enabled = false;
                btnBuild.Enabled = false;
                if (chkSetup.Checked)
                {
                    txtPolicy.Enabled = true;
                    txtGeneratePath.Enabled = true;
                    cmbServer.Enabled = true;
                    cmbDatabase.Enabled = true;
                    chkRIS.Enabled = true;
                    chkTARABS.Enabled = true;
                    chkTARABAN.Enabled = true;
                    chkINS.Enabled = true;
                    chkRISBAL.Enabled = true;
                    chkALL.Enabled = true;
                    btnProcess.Enabled = true;
                }
            }
            else
            {
                txtLocalBuildPath.Enabled = true;
                btnBuild.Enabled = true;
                txtPolicy.Enabled = false;
                txtGeneratePath.Enabled = false;
                cmbServer.Enabled = false;
                cmbDatabase.Enabled = false;
                chkRIS.Enabled = false;
                chkTARABS.Enabled = false;
                chkTARABAN.Enabled = false;
                chkINS.Enabled = false;
                chkRISBAL.Enabled = false;
                chkALL.Enabled = false;
                btnProcess.Enabled = false;
            }
        }

        //If ALL checkbox is selected, all batches will run
        private void chkALL_CheckedChanged(object sender, EventArgs e)
        {
            if (chkALL.Checked)
            {
                chkRIS.Enabled = false;
                chkTARABS.Enabled = false;
                chkTARABAN.Enabled = false;
                chkINS.Enabled = false;
                chkRISBAL.Enabled = false;
                chkRIS.Checked = true;
                chkTARABS.Checked = true;
                chkTARABAN.Checked = true;
                chkINS.Checked = true;
                chkRISBAL.Checked = true;
            }
            else
            {
                chkRIS.Enabled = true;
                chkTARABS.Enabled = true;
                chkTARABAN.Enabled = true;
                chkINS.Enabled = true;
                chkRISBAL.Enabled = true;
                chkRIS.Checked = false;
                chkTARABS.Checked = false;
                chkTARABAN.Checked = false;
                chkINS.Checked = false;
                chkRISBAL.Checked = false;
            }
        }

        //Processing RIS and INS is Mandatory to Process RISBAL
        private void chkRISBAL_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRISBAL.Checked)
            {
                chkRIS.Enabled = false;
                chkINS.Enabled = false;
                chkRIS.Checked = true;
                chkINS.Checked = true;
            }
            else
            {
                chkRIS.Enabled = true;
                chkINS.Enabled = true;
            }
        }

        //Build PBACC, RIS, TARABS, TARABAN, INS, RISBAL and make the setup ready
        private void btnBuild_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtLocalBuildPath.Text))
                {
                    MessageBox.Show("Please provide the BatchApp Package Path of PLS Solution.");
                }
                else
                {
                    Stopwatch stpwtch = new Stopwatch();
                    stpwtch.Start();
                    string actualAppPath = txtLocalBuildPath.Text + "\\BatchApp";

                    if (INISetup() && DownstreamBuildFolderHierarchy() && PreserveStaticStructureForINSAndRISBAL() && BuildAndExecuteBATFile(actualAppPath) && SetupBatchesInCurrentEnvironment(BuildPath))
                    {
                        txtLocalBuildPath.Clear();
                        chkBuildDone.Checked = true;
                        chkBuildDone_CheckedChanged(sender, e);
                        stpwtch.Stop();
                        MessageBox.Show("Downstream Build Successful !!! \n\n Total time taken to build: " + stpwtch.Elapsed.Minutes.ToString() + " mins " + stpwtch.Elapsed.Seconds.ToString() + " seconds");
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Downstream Build failure");
                LogError.ErrorLog(exp.StackTrace);
            }
        }

        //Setup INI file for Downstream
        private Boolean INISetup()
        {
            try
            {
                Helper.CreateFolder(@"C:\BatchApps");
                Helper.CreateFolder(@"C:\BatchApps\Downstream");
                Helper.CreateFolder(@"C:\Services");
                Helper.CreateFolder(@"C:\Services\Apps");
                Helper.CreateFolder(@"C:\Services\Apps\pls");
                Helper.Clear(@"C:\Services\Apps\pls");
                File.Copy(currentDirectory + @"\Helper\PLS.INI", @"C:\Services\Apps\pls\PLS.INI", true);
                return true;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Error in setting up INI file.");
                LogError.ErrorLog(exp.StackTrace);
                return false;
            }
        }

        //Build Folders for INS and RISBAL and keep the structure static
        private Boolean PreserveStaticStructureForINSAndRISBAL()
        {
            try
            {
                if (!Directory.Exists(batchpath.pathINS))
                {
                    Helper.CreateFolder(batchpath.pathINS);
                    File.Copy(currentDirectory + @"\Helper\RisInsd.exe", batchpath.pathINS + "\\RisInsd.exe", true);
                }
                if (!Directory.Exists(batchpath.pathRISBAL))
                {
                    Helper.CreateFolder(batchpath.pathRISBAL);
                    File.Copy(currentDirectory + @"\Helper\RisBal.exe", batchpath.pathRISBAL + "\\RisBal.exe", true);
                }
                return true;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Error in setting up INS and RISBAL batches.");
                LogError.ErrorLog(exp.StackTrace);
                return false;
            }
        }

        //Build Downstream Folder Hierarchy
        private Boolean DownstreamBuildFolderHierarchy()
        {
            try
            {
                if (Directory.Exists(BuildPath))
                {
                    Helper.Clear(batchpath.pathPBACC);
                    Helper.Clear(batchpath.pathRIS);
                    Helper.Clear(batchpath.pathTARABS);
                    Helper.Clear(batchpath.pathTARABAN);
                }
                else
                {
                    Helper.CreateFolder(BuildPath);
                    Helper.CreateFolder(batchpath.pathPBACC);
                    Helper.CreateFolder(batchpath.pathRIS);
                    Helper.CreateFolder(batchpath.pathTARABS);
                    Helper.CreateFolder(batchpath.pathTARABAN);
                }
                return true;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Error in setting up batch directory structure.");
                LogError.ErrorLog(exp.StackTrace);
                return false;
            }
        }

        //Build and Execute BAT file for Building Downstream Batches
        private Boolean BuildAndExecuteBATFile(string actualAppPath)
        {
            try
            {
                if (Directory.Exists(actualAppPath))
                {
                    string batFileLocation = currentDirectory + @"\BuildLocation";
                    Helper.CreateFolder(batFileLocation);
                    
                    File.Create(batFileLocation + "\\PLSBatchJobs.bat").Close();
                    StringBuilder sbBatch = new StringBuilder();

                    sbBatch.Append("cls" + Environment.NewLine);
                    foreach (var item in lstBatchJobs)
                    {
                        if (File.Exists(actualAppPath + "\\" + item.ApplicationName + "\\" + item.ApplicationName + ".csproj"))
                        {
                            sbBatch.Append("@ECHO ON" + Environment.NewLine);
                            sbBatch.Append("@ECHO Building " + item.ApplicationName + Environment.NewLine);
                            sbBatch.Append("@ECHO OFF" + Environment.NewLine);
                            sbBatch.Append("cd \"C:\\Windows\\Microsoft.NET\\Framework64\\v4.0.30319\" > NUL" + Environment.NewLine);
                            sbBatch.Append("msbuild.exe \"" + actualAppPath + "\\" + item.ApplicationName + "\\" + item.ApplicationName + ".csproj" + "\" /t:Clean,Build /p:configuration=debug > NUL" + Environment.NewLine);
                            sbBatch.Append("@ECHO ON" + Environment.NewLine);
                            sbBatch.Append("@ECHO Building " + item.ApplicationName + " Succeded !!!!" + Environment.NewLine);
                            sbBatch.Append(Environment.NewLine);
                        }
                    }
                    if (File.Exists(batFileLocation + "\\PLSBatchJobs.bat"))
                    {
                        StreamWriter sw = new StreamWriter(batFileLocation + "\\PLSBatchJobs.bat", true);
                        sw.Write(sbBatch.ToString());
                        sw.Close();
                        RunBatch.RunBatchFile(batFileLocation, "PLSBatchJobs.bat");
                        Directory.Delete(batFileLocation,true);
                    }
                }
                return true;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Error in building downstream batches.");
                LogError.ErrorLog(exp.StackTrace);
                return false;
            }
        }

        //Complete the Batch Setup
        private Boolean SetupBatchesInCurrentEnvironment(string directoryPath)
        {
            try
            {
                foreach (var item in lstBatchJobs)
                {
                    item.AppPath = txtLocalBuildPath.Text + "\\BatchApp\\" + item.ApplicationName + "\\bin\\Debug";
                    foreach (var file in Directory.GetFiles(item.AppPath))
                    {
                        File.Copy(file, Path.Combine(directoryPath + "\\" + item.ApplicationName, Path.GetFileName(file)));
                    }
                }
                return true;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Error in setting up batches in your current environment.");
                LogError.ErrorLog(exp.StackTrace);
                return false;
            }
        }

        //Process the Downstream
        private void btnProcess_Click(object sender, EventArgs e)
        {
            Regex rgx = new Regex(@"^([0-9]{9}[-][0-9]{1,2}[-][0-9]{1,3}[,]*)+$");
            bool polControl = rgx.IsMatch(txtPolicy.Text) ? true : false;
            bool checkControl = (!chkALL.Checked && !chkRIS.Checked && !chkINS.Checked && !chkTARABS.Checked && !chkTARABAN.Checked && !chkRISBAL.Checked) ? false : true;
            bool dbControl = cmbDatabase.Text.Substring(0, 3).Equals("sqs") ? true : false;
            bool pathControl = Directory.Exists(txtGeneratePath.Text) ? true : false;

            if (Directory.Exists(BuildPath) && GetBuildDirectorySize() > 10)
            {
                if (polControl)
                {
                    if (checkControl)
                    {
                        if (dbControl)
                        {
                            if (pathControl)
                            {
                                Stopwatch stpwtch = new Stopwatch();
                                stpwtch.Start();
                                List<PolDetails> lstPol = ExtractPoliciesFromInput();
                                StartProcess(lstPol, batchpath);
                                stpwtch.Stop();
                                MessageBox.Show("Downstream Tracker Successfully Processed !!! \n\n Total time taken to Process: " + stpwtch.Elapsed.Minutes.ToString() + " mins " + stpwtch.Elapsed.Seconds.ToString() + " seconds");
                            }
                            else
                            {
                                MessageBox.Show("Please provide a valid path to generate the downstream files.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please select a valid DB to execute the downstream tracker.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please check at least one downstream batch to execute.");
                    }
                }
                else
                {
                    MessageBox.Show("Please provide the Policy-Edition-Transnum in comma separated list.");
                }
            }
            else
            {
                MessageBox.Show("Please perform a successful build before processing downstream batches.");
            }
        }

        //Get Build Directory size to check if a successful build is done before processing downstream
        private long GetBuildDirectorySize()
        {
            long size = 0;
            string[] allFiles = Directory.GetFiles(BuildPath + "\\", "*.*",SearchOption.AllDirectories);
            foreach (string file in allFiles)
            {
                FileInfo fileInfo = new FileInfo(file);
                size += fileInfo.Length;
            }
            return (size/1024)/1024;
        }

        //Extract Policies from Input
        private List<PolDetails> ExtractPoliciesFromInput()
        {
            string[] policies = txtPolicy.Text.Trim().Split(',');
            List<PolDetails> lstPol = new List<PolDetails>();
            foreach (var j in policies)
            {
                string[] d = j.Trim().Split('-');
                PolDetails pdtls = new PolDetails()
                {
                    Policy = d[0],
                    Edition = d.Count() > 1 ? Convert.ToInt32(d[1]) : 0,
                    Transnum = d.Count() > 2 ? Convert.ToInt32(d[2]) : -1,
                };
                lstPol.Add(pdtls);
            }
            return lstPol;
        }

        //Process one Batch at a time
        private void StartProcess(List<PolDetails> lstPol, BatchPath batchpath)
        {
            string server = cmbServer.Text.Substring(6, 14);
            bool PBACCComplete = false;
            bool RISComplete = false;
            bool INSComplete = false;
            string polX = String.Join(",", lstPol.Select(j => "'" + j.Policy + j.Edition + j.Transnum.ToString() + "'"));
            string polstring = String.Join(",", lstPol.Select(j => "'" + j.Policy + "'"));
            string xPol2 = String.Format("pr.a00_pnum + CONVERT(VARCHAR,pr.a06_edition) + CONVERT(VARCHAR,pr.e01_transnum) IN ({0})", polX);
            
            try
            {
                if (Helper.UpdateFlag(polstring, xPol2, cmbServer.Text, cmbDatabase.Text, chkRIS.Checked, chkINS.Checked, chkTARABS.Checked, chkTARABAN.Checked))
                {
                    Helper.Clear(DownstreamPath);
                    xPol2 = String.Format(" AND {0} ", xPol2);
                    if (chkALL.Checked || chkRIS.Checked || chkINS.Checked || chkTARABS.Checked || chkTARABAN.Checked || chkRISBAL.Checked)
                    {
                        string xBatch = "wpc1 wpctest 0 /N " + DateTime.Now.ToString("MM/dd/yyyy");
                        if (RunBatch.LaunchApp(batchpath.pathPBACC, "PBACC", "PBACC.exe.config", xBatch, "", server, cmbDatabase.Text))
                        {
                            PBACCComplete = true;
                        }
                        else
                        {
                            MessageBox.Show("PBACC Execution Failed.");
                        }
                    }
                    if (chkRIS.Checked && PBACCComplete)
                    {
                        string xBatch = "wpc1 wpctest 0 /N " + DateTime.Now.ToString("MM/dd/yyyy") + " 0 9";
                        if (RunBatch.LaunchApp(batchpath.pathRIS, "RIS", "RIS.exe.config", xBatch, xPol2, server, cmbDatabase.Text))
                        {
                            RISComplete = true;
                            Helper.ClearAfterRIS(DownstreamPath);
                        }
                        else
                        {
                            MessageBox.Show("RIS Execution Failed.");
                        }
                    }
                    if (chkTARABS.Checked && PBACCComplete)
                    {
                        string xBatch = "wpc1 wpctest 0 /N " + DateTime.Now.ToString("MM/dd/yyyy");
                        if (RunBatch.LaunchApp(batchpath.pathTARABS, "TARABS", "TARABS.exe.config", xBatch, xPol2, server, cmbDatabase.Text))
                        {
                            Helper.ClearAfterTAR(DownstreamPath);
                        }
                        else
                        {
                            MessageBox.Show("TARABS Execution Failed.");
                        }
                    }
                    if (chkTARABAN.Checked && PBACCComplete)
                    {
                        string xBatch = "wpc1 wpctest 0 /N " + DateTime.Now.ToString("MM/dd/yyyy");
                        if (RunBatch.LaunchApp(batchpath.pathTARABAN, "TARABAN", "TARABAN.exe.config", xBatch, xPol2, server, cmbDatabase.Text))
                        {
                            Helper.ClearAfterTAR(DownstreamPath);
                        }
                        else
                        {
                            MessageBox.Show("TARABAN Execution Failed.");
                        }
                    }
                    xPol2 = String.Format(" AND p.a00_pnum + CONVERT(VARCHAR,p.a06_edition) + CONVERT(VARCHAR,p.e01_transnum) IN ({0})", polX);
                    if (chkINS.Checked && PBACCComplete)
                    {
                        string xBatch = String.Format("{0}|{1}|{2}|{3}|{4}|{5}", "wpc1", "wpctest", DateTime.Now.ToString("MM/dd/yyyy"), xPol2, server, cmbDatabase.Text);
                        if (RunBatch.LaunchApp(batchpath.pathINS, "RisInsd", "", xBatch, xPol2, server, cmbDatabase.Text))
                        {
                            INSComplete = true;
                            Helper.ClearAfterINS(DownstreamPath);
                        }
                        else
                        {
                            MessageBox.Show("INS Execution Failed.");
                        }
                    }
                    if (chkRISBAL.Checked && PBACCComplete && RISComplete && INSComplete)
                    {
                        Helper.RenameFilesRISBAL(DownstreamPath);
                        string xBatch = String.Format("{0},{1},{2},{3},{4}", "wpc1", "wpctest", DateTime.Now.ToString("MM/dd/yyyy"), server, cmbDatabase.Text);
                        if (RunBatch.LaunchApp(batchpath.pathRISBAL, "RisBal", "", xBatch, "", server, cmbDatabase.Text))
                        {
                            Helper.ClearAfterRISBAL(DownstreamPath);
                        }
                        else
                        {
                            MessageBox.Show("RISBAL Execution Failed.");
                        }
                    }
                    ZipFile.CreateFromDirectory(DownstreamPath, txtGeneratePath.Text + "\\" + cmbDatabase.Text + ".zip");
                }
            }
            catch (Exception exp)
            {
                LogError.ErrorLog(exp.StackTrace);
            }
        }
    }
}