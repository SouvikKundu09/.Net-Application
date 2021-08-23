using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyTracker
{
    internal class Helper
    {
        //Creating a Folder if it isn't there
        public static void CreateFolder(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }

        //Delete all contents(Files and Folders) of a Folder
        public static void Clear(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            if (Directory.Exists(path))
            {
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
        }

        //Delete redundant Files after RIS
        public static void ClearAfterRIS(string path)
        {
            Directory.GetFiles(path).ToList().ForEach(file =>
            {
                FileInfo fi = new FileInfo(file);
                if (!fi.Extension.Equals(".RIS", StringComparison.OrdinalIgnoreCase))
                {
                    File.Delete(file);
                }
            });
        }

        //Delete redundant Files after INS
        public static void ClearAfterINS(string path)
        {
            Directory.GetFiles(path).ToList().ForEach(file =>
            {
                FileInfo fi = new FileInfo(file);
                if (!fi.Extension.Equals(".INS", StringComparison.OrdinalIgnoreCase) && !fi.Extension.Equals(".RIS", StringComparison.OrdinalIgnoreCase) && !fi.Extension.Equals(".DAT", StringComparison.OrdinalIgnoreCase))
                {
                    File.Delete(file);
                }
            });
        }

        //Delete redundant Files after TARABS & TARABAN
        public static void ClearAfterTAR(string path)
        {
            Directory.GetFiles(path).ToList().ForEach(file =>
            {
                FileInfo fi = new FileInfo(file);
                if (fi.Extension.Equals(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    File.Delete(file);
                }
            });
        }

        //Organize files after RISBAL
        public static void ClearAfterRISBAL(string path)
        {
            Helper.CreateFolder(path + "\\RISBAL");
            Directory.GetFiles(path + "\\").ToList().ForEach(file =>
            {
                FileInfo fi = new FileInfo(file);
                if (fi.Name.Contains("B"))
                {
                    File.Move(file, path + "\\RISBAL\\" + fi.Name);
                }
            });
        }

        //Rename .RIS and .INS files for RISBAL processing
        public static void RenameFilesRISBAL(string path)
        {
            Directory.GetFiles(path).ToList().ForEach(file =>
            {
                FileInfo fi = new FileInfo(file);
                if (fi.Extension.Equals(".INS", StringComparison.OrdinalIgnoreCase) || fi.Extension.Equals(".RIS", StringComparison.OrdinalIgnoreCase))
                {
                    File.Move(file, Path.GetDirectoryName(file) + "\\" + DateTime.Now.ToString("MMddyy") + Path.GetExtension(file));
                }
            });
        }

        //Update tracker flag in precap_history
        public static bool UpdateFlag(string pol, string args, string server, string database, bool chkRIS, bool chkINS, bool chkTARABS, bool chkTARABAN)
        {
            try
            {
                string dbServer = server.Substring(6, 14);
                string ConnectionString = String.Format("Data Source={0};Initial Catalog={1};{2}", dbServer, database, "Integrated Security=false;User ID=wpc1;Pwd=wpctest;");
                if (!String.IsNullOrWhiteSpace(ConnectionString) && !String.IsNullOrWhiteSpace(args))
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        connection.Open();
                        string resetquerystr = String.Format("UPDATE pr SET {0} FROM precap_history pr WHERE a00_pnum in ({1})", string.Join(",", (chkRIS ? "pr.z15_ris_extracted = 'Y'" : string.Empty), (chkINS ? "pr.z15_insd_extracted = 'Y'" : string.Empty), (chkTARABS ? "z15_tar_extracted = '2'" : string.Empty), (chkTARABAN ? "tar_name_extracted = '2'" : string.Empty)).Trim(',') , pol);
                        SqlCommand resetquery = new SqlCommand(resetquerystr, connection);
                        resetquery.ExecuteNonQuery();
                        string querystr = String.Format("UPDATE pr SET {0},{1} FROM precap_history pr WHERE {2}", string.Join(",", (chkRIS ? "pr.z15_ris_extracted = NULL" : string.Empty), (chkINS ? "pr.z15_insd_extracted = NULL" : string.Empty), (chkTARABS ? "z15_tar_extracted = NULL" : string.Empty), (chkTARABAN ? "tar_name_extracted = NULL" : string.Empty)).Trim(','), "pr.b04_cycle_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "'", args);
                        SqlCommand query = new SqlCommand(querystr, connection);
                        query.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                return true;
            }
            catch (Exception exp)
            {
                MessageBox.Show("Error in updating downstream indicators in the listed policies.");
                LogError.ErrorLog(exp.StackTrace);
                return false;
            }
        }
    
        //Load databases from server dropdown
        public static DataTable LoadDatabases(string server)
        {
 	        DataTable databases = null;
            string dbServer = server.Substring(6, 14);

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
                    return dtDatabases;
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Either server name is not valid or there might some error while connecting server. Error Message : " + exp.Message);
                    LogError.ErrorLog(exp.StackTrace);
                    return null;
                }
            }
            else
            {
                MessageBox.Show("Please select a server name from the dropdown.");
                return null;
            }
        }
    }
}