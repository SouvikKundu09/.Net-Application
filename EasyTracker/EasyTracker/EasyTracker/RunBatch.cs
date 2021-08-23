using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;
using System.Xml;
using Microsoft.Win32;

namespace EasyTracker
{
    public class RunBatch
    {
        [DllImport("ODBCCP32.dll")]
        private static extern bool SQLConfigDataSource(IntPtr parent, int request, string driver, string attributes);

        public static string ErrorMessage = String.Empty;

        //Programmatically Launch each Downstream Batch at a time
        public static bool LaunchApp(string dirPath, string appName, string appConfig, string argsB, string argsP, string server, string db)
        {
            int timeOut = 60000 * 10;
            ErrorMessage = String.Empty;
            try
            {
                if (appName == "RisBal" || appName == "RisInsd" || UpdateConStringAppConfig(dirPath + "\\" + appConfig, server, db))
                {
                    if (appName == "RisBal" || appName == "RisInsd" || appName == "PBACC" || UpdateEXE(dirPath, appName, argsP))
                    {
                        string exePath = dirPath + "\\" + appName + ".exe";
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.UseShellExecute = false;
                        startInfo.ErrorDialog = false;
                        startInfo.RedirectStandardOutput = true;
                        startInfo.RedirectStandardError = true;
                        startInfo.FileName = exePath;
                        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        startInfo.Arguments = argsB;
                        try
                        {
                            using (Process exeProcess = Process.Start(startInfo))
                            {
                                exeProcess.EnableRaisingEvents = true;
                                int pid = (int)exeProcess.Id;

                                exeProcess.ErrorDataReceived += (sender, errorLine) =>
                                {
                                    if (errorLine.Data != null)
                                    {
                                        Trace.WriteLine(errorLine.Data);
                                        ErrorMessage += ErrorMessage;
                                    }
                                };
                                exeProcess.OutputDataReceived += (sender, outputLine) =>
                                {
                                    if (outputLine.Data != null)
                                    {
                                        Trace.WriteLine(outputLine.Data);
                                        ErrorMessage += ErrorMessage;
                                    }
                                };
                                exeProcess.BeginErrorReadLine();
                                exeProcess.BeginOutputReadLine();
                                if (!exeProcess.WaitForExit(timeOut))
                                {
                                    exeProcess.CloseMainWindow();
                                    exeProcess.Kill();
                                    exeProcess.Close();
                                    for (int i = 0; i < 5; i++)
                                    {
                                        if (!exeProcess.HasExited)
                                        {
                                            exeProcess.Refresh();
                                            Thread.Sleep(1000);
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    throw new Exception("ERROR: Process took too long to finish");
                                }
                                else if (exeProcess.ExitCode != 0)
                                {
                                    throw new Exception("Process exited with non-zero exit code of: " + exeProcess.ExitCode + Environment.NewLine +
                                    "Output from process: " + ErrorMessage);
                                }
                                exeProcess.Close();
                            }

                        }
                        catch (Exception exp)
                        {
                            LogError.ErrorLog(exp.StackTrace);
                            ErrorMessage = exp.Message;
                        }

                        if (appName == "RIS" || appName == "TARABS" || appName == "TARABAN")
                        {
                            File.Delete(exePath);
                            if (File.Exists(dirPath + "\\" + appName + ".bk"))
                            {
                                File.Copy(dirPath + "\\" + appName + ".bk", exePath);
                                File.Delete(dirPath + "\\" + appName + ".bk");
                            }
                        }
                    }
                    else
                    {
                        ErrorMessage = "Unable to Update EXE";
                    }
                }
            }
            catch (Exception exp)
            {
                LogError.ErrorLog(exp.StackTrace);
                ErrorMessage = exp.Message + exp.StackTrace;
            }
            if (!String.IsNullOrEmpty(ErrorMessage))
                return false;
            return true;
        }

        //Update App.config with proper Server and Database name
        public static bool UpdateConStringAppConfig(string filePath, string server, string db)
        {
            try
            {
                string conStr = String.Empty;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);

                //xmlDoc.DocumentElement.FirstChild.SelectSingleNode("descendant::" + key).Attributes[0].Value = value;
                XmlNode n = xmlDoc.SelectSingleNode("/configuration/log4net/appender[@name='ADONetAppender']/connectionString/@value");
                if (n != null)
                {
                    conStr = "Data Source=" + server + ";User ID=lognet;Pwd=logpass;Initial Catalog=" + db;
                    n.Value = conStr;
                }
                var names = new XmlNamespaceManager(xmlDoc.NameTable);
                names.AddNamespace("db", "http://www.springframework.net/database");
                n = xmlDoc.SelectSingleNode("//db:provider/@connectionString", names);
                if (n != null)
                {
                    conStr = "Data Source=" + server + ";Initial Catalog=" + db + ";Persist Security Info=True;";
                    n.Value = conStr;
                }
                n = xmlDoc.SelectSingleNode("/configuration/appSettings/add[@key='database']/@value");
                if (n != null)
                {
                    n.Value = db;
                }
                xmlDoc.Save(filePath);
            }
            catch (Exception exp)
            {
                LogError.ErrorLog(exp.StackTrace);
                return false;
            }
            return true;
        }

        //Update the EXE files by injecting policy-edition-transum using de-compiler and re-compiler
        public static bool UpdateEXE(string inputFilePath, string InputFileName, string xArgs)
        {
            string tempExePath = inputFilePath + "\\tempEXE";
            Helper.CreateFolder(tempExePath);
            if (ProcessEXE(@"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools\ildasm.exe", inputFilePath + "\\" + InputFileName + ".exe /out=" + tempExePath + "\\" + InputFileName + ".il"))
            {
                string ilPath = tempExePath + "\\" + InputFileName + ".il";
                if (File.Exists(ilPath))
                {
                    var il = File.ReadAllLines(ilPath);
                    bool ldstrFound = false;
                    bool bldstr = false;
                    int y = 0;
                    for (int x = 0; x < il.Count(); x++, y++)
                    {
                        switch (InputFileName)
                        {
                            case "RIS":
                                if (!ldstrFound)
                                {
                                    if (!bldstr && !il[x].Contains("App_Startup() cil managed"))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        bldstr = true;
                                    }
                                    if (il[x].Contains("string BatchApp.RIS.Ris::strPolStrCond"))
                                    {
                                        il[x - 1] = il[x - 1].Insert(il[x - 1].LastIndexOf("\""), xArgs);
                                        ldstrFound = true;
                                        break;
                                    }
                                }
                                break;
                            case "TARABS":
                                if (il[x].Contains("isnull(a.registration_sys_ind,'R')"))
                                {
                                    il[x] = il[x].Replace("isnull(a.registration_sys_ind,'R')", xArgs.Replace("pr.", "a.").Replace(" AND  ", "").Replace("') ", "') and isnull(a.registration_sys_ind,'R')"));
                                }
                                break;
                            case "TARABAN":
                                if (il[x].Contains("RPT8%')"))
                                {
                                    il[x] = il[x].Replace("RPT8%')", xArgs.Replace("pr.", "p.").Replace(" AND  ", "RPT8%') and "));
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    string ilPath_x = tempExePath + "\\" + InputFileName + "_x.il";
                    File.WriteAllLines(ilPath_x, il);
                    if (File.Exists(ilPath_x))
                    {
                        if (ProcessEXE(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319\ilasm.exe", @"/RESOURCE=" + tempExePath + "\\" + InputFileName + ".res " + tempExePath + "\\" + InputFileName + "_x.il /output:" + tempExePath + "\\" + InputFileName + ".exe"))
                        {
                            if (File.Exists(tempExePath + "\\" + InputFileName + ".exe"))
                            {
                                if (File.Exists(inputFilePath + "\\" + InputFileName + ".exe"))
                                {
                                    File.Copy(inputFilePath + "\\" + InputFileName + ".exe", inputFilePath + "\\" + InputFileName + ".bk");
                                }
                                File.Copy(tempExePath + "\\" + InputFileName + ".exe", inputFilePath + "\\" + InputFileName + ".exe", true);
                            }
                            if (Directory.Exists(tempExePath))
                                Directory.Delete(tempExePath, true);
                        }
                    }
                }
            }
            return true;
        }

        //Processing the EXE files
        public static bool ProcessEXE(string exepath, string args)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = exepath;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = args;
            try
            {
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
                return true;
            }
            catch (Exception exp)
            {
                LogError.ErrorLog(exp.StackTrace);
            }
            return false;
        }

        //Running BAT file for Batch building
        public static bool RunBatchFile(string targetDirectory, string fileName)
        {
            try
            {
                Process proc = new Process();
                proc.StartInfo.FileName = fileName;
                proc.StartInfo.WorkingDirectory = targetDirectory;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                proc.WaitForExit();
                proc.Close();
                return true;
            }
            catch (Exception exp)
            {
                LogError.ErrorLog(exp.StackTrace);
                return false;
            }
        }

        //Run cmd to Register Object Library(regtlibv12 msdatsrc.tlb)
        public static bool ExecuteCMD()
        {
             try
             {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";
                startInfo.Verb = "runas";
                startInfo.WorkingDirectory = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319";
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.Arguments = "/c regtlibv12 msdatsrc.tlb";
                Process exeProcess = Process.Start(startInfo);
                exeProcess.WaitForExit();
                return true;
            }
            catch (Exception exp)
            {
                LogError.ErrorLog(exp.StackTrace);
                return false;
            }
        }

        //Edit Registry to add SDS => WPC SubKey under Software Key
        public static bool EditRegistry()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);
                if (!key.GetValueNames().Contains("SDS"))
                {
                    key.CreateSubKey("SDS");
                    key = key.OpenSubKey("SDS", true);
                    key.CreateSubKey("WPC");
                    key = key.OpenSubKey("WPC", true);
                    key.SetValue("", @"C:\Services\Apps\pls\PLS.INI");
                }
                else
                {
                    key = key.OpenSubKey("SDS", true);
                    if (!key.GetValueNames().Contains("WPC"))
                    {
                        key.CreateSubKey("WPC");
                        key = key.OpenSubKey("WPC", true);
                        key.SetValue("", @"C:\Services\Apps\pls\PLS.INI");
                    }
                    else
                    {
                        key = key.OpenSubKey("WPC", true);
                        key.SetValue("", @"C:\Services\Apps\pls\PLS.INI");
                    }
                }
                return true;
            }
            catch (Exception exp)
            {
                LogError.ErrorLog(exp.StackTrace);
                return false;
            }
        }

        //Configure ODBC connection for the following Databases: sqs$cog4, sqs$atf, sqs$atw, sqs$ata, sqs$at6, sqs$uat8
        public static bool ConfigureODBC()
        {
            try
            {
                List<string> keyVal = Registry.CurrentUser.OpenSubKey("Software\\ODBC\\ODBC.INI", true).GetValueNames().ToList();
                if (!keyVal.Contains("sqs$cog4"))
                {
                    SQLConfigDataSource((IntPtr)0, 1, "SQL Server", "SERVER=MFRKNTACESQT11\0DSN=sqs$cog4\0DESCRIPTION=sqs$cog4\0DATABASE=sqs$cog4\0TRUSTED_CONNECTION=YES");
                }
                if (!keyVal.Contains("sqs$atf"))
                {
                    SQLConfigDataSource((IntPtr)0, 1, "SQL Server", "SERVER=MFRKNTACESQT11\0DSN=sqs$atf\0DESCRIPTION=sqs$atf\0DATABASE=sqs$atf\0TRUSTED_CONNECTION=YES");
                }
                if (!keyVal.Contains("sqs$atw"))
                {
                    SQLConfigDataSource((IntPtr)0, 1, "SQL Server", "SERVER=MFRKNTACESQT04\0DSN=sqs$atw\0DESCRIPTION=sqs$atw\0DATABASE=sqs$atw\0TRUSTED_CONNECTION=YES");
                }
                if (!keyVal.Contains("sqs$ata"))
                {
                    SQLConfigDataSource((IntPtr)0, 1, "SQL Server", "SERVER=MFRKNTACESQT04\0DSN=sqs$ata\0DESCRIPTION=sqs$ata\0DATABASE=sqs$ata\0TRUSTED_CONNECTION=YES");
                }
                if (!keyVal.Contains("sqs$at6"))
                {
                    SQLConfigDataSource((IntPtr)0, 1, "SQL Server", "SERVER=MFRKNTACESQT04\0DSN=sqs$at6\0DESCRIPTION=sqs$at6\0DATABASE=sqs$at6\0TRUSTED_CONNECTION=YES");
                }
                if (!keyVal.Contains("sqs$uat8"))
                {
                    SQLConfigDataSource((IntPtr)0, 1, "SQL Server", "SERVER=MFRKNTACESQT04\0DSN=sqs$uat8\0DESCRIPTION=sqs$uat8\0DATABASE=sqs$uat8\0TRUSTED_CONNECTION=YES");
                }
                return true;
            }
            catch (Exception exp)
            {
                LogError.ErrorLog(exp.StackTrace);
                return false;
            }
        }
    }
}