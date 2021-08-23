using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Reflection;

namespace ListFiles
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtPath.ReadOnly = true;
            txtExportPath.ReadOnly = true;
            btnStart.Visible = true;
            dtPicker.Format = DateTimePickerFormat.Custom;
            dtPicker.CustomFormat = "MM-dd-yyyy";
            dtPicker.Value = DateTime.Now;
        }

        //Browse button
        private void BrowseBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDlg = new FolderBrowserDialog();
            folderBrowserDlg.ShowNewFolderButton = true;
            DialogResult result = folderBrowserDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtPath.Text = folderBrowserDlg.SelectedPath;
                Environment.SpecialFolder root = folderBrowserDlg.RootFolder;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDlg = new FolderBrowserDialog();
            folderBrowserDlg.ShowNewFolderButton = true;
            DialogResult result = folderBrowserDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtExportPath.Text = folderBrowserDlg.SelectedPath;
                Environment.SpecialFolder root = folderBrowserDlg.RootFolder;
            }
        }

        //Start button
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPath.Text.Trim()))
            {
                MessageBox.Show("Please browse a folder");
            }
            else if (string.IsNullOrWhiteSpace(txtExportPath.Text.Trim()))
            {
                MessageBox.Show("Please browse an export path");
            }
            else if (string.IsNullOrWhiteSpace(extDetails.Text.Trim()))
            {
                MessageBox.Show("Please provide file extensions in this format - .mkv or .mp4,.txt,.avi");
            }
            else
            {
                btnStart.Visible = false;
                extDetails.Enabled = false;
                dtPicker.Enabled = false;
                StoreToText(txtPath.Text.Trim(), txtExportPath.Text.Trim(), extDetails.Text.Trim());
                btnStart.Visible = true;
                extDetails.Enabled = true;
                dtPicker.Enabled = true;
            }
        }

        //Store Records to Text
        private void StoreToText(string path,string exportPath, string extensions)
        {
            int fileCounter = 1;
            bool flag = default(bool);
            string[] allExts = extensions.Split(',');
            string lastFolderName = Path.GetFileName(Path.GetDirectoryName(path + @"\"));
            string rawFilePath = exportPath + @"\" + lastFolderName + "_List.txt";
            string dateAppendedName = Path.GetFileNameWithoutExtension(rawFilePath) + "_" + DateTime.Now.ToString("MM/dd/yyyy").Replace("/", string.Empty) + Path.GetExtension(rawFilePath);
            string finalFilePath = exportPath + @"\" + dateAppendedName;
            try
            {
                var allDirectories = Directory.GetDirectories(path).Select(
                    p =>
                    {
                        DirectoryInfo dInfo = new DirectoryInfo(p);
                        bool inaccessible = default(bool);
                        if (dInfo.Attributes.HasFlag(FileAttributes.System))
                        {
                            inaccessible = true;
                        }
                        if (inaccessible)
                            return null;
                        else
                            return p;
                    }).Distinct().Where(p => !string.IsNullOrWhiteSpace(p)).ToList();

                List<string> allItems = new List<string>();
                foreach(var directory in allDirectories)
                {
                    var tempItems = Directory.GetFileSystemEntries(directory, "*", SearchOption.AllDirectories);
                    allItems.AddRange(tempItems);
                }

                List<string> allFiles = new List<string>();
                foreach (var ext in allExts)
                {
                    var tempList = allItems.Where(p => Path.GetExtension(p) == ext).ToList();
                    allFiles.AddRange(tempList);
                }

                var allFilesWIthoutExtensions = allFiles.Select(p => Path.GetFileNameWithoutExtension(p));

                var simplifiedFileNames = allFilesWIthoutExtensions.Select(
                    p =>
                    {
                        Regex rgx = new Regex(@"^(#){1}[0-9]{1}(-){1}(.*)$");
                        bool isSeries = rgx.IsMatch(p);
                        if (isSeries)
                            return p.Substring(3, p.Length - 3);
                        else
                            return p;
                    }).ToList();

                var fileList = simplifiedFileNames.Distinct().Where(p => !string.IsNullOrWhiteSpace(p));

                var finalFileList = fileList.OrderBy(p => p).ToList();

                FileCreation(exportPath, finalFilePath);

                foreach (var file in finalFileList)
                {
                    File.AppendAllText(finalFilePath, fileCounter + ") " + file + Environment.NewLine);
                    fileCounter++;
                }

                File.SetAttributes(finalFilePath, FileAttributes.Normal);

                flag = true;

                MessageBox.Show("File list generated successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                if (!flag)
                {
                    if (File.Exists(finalFilePath))
                    {
                        File.Delete(finalFilePath);
                    }
                    MessageBox.Show("Error in generating file");
                }
            }
        }

        //Create the text file
        private void FileCreation(string exportPath, string finalFilePath)
        {
            if (File.Exists(finalFilePath))
            {
                File.Delete(finalFilePath);
                File.Create(finalFilePath).Close();
                File.SetAttributes(finalFilePath, FileAttributes.Hidden);
            }
            else
            {
                File.Create(finalFilePath).Close();
                File.SetAttributes(finalFilePath, FileAttributes.Hidden);
            }
        }
    }
}

