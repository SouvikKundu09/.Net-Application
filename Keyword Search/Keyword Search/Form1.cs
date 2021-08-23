using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeywordSearch
{
  public class Form1 : Form
  {
    private static string[] filterExclusions = (string[]) new string[8]
    {
      ".pdb",
      ".suo",
      ".dll",
      ".refresh",
      ".publishproj",
      ".sln",
      ".exe",
      ".cache"
    }.Clone();
    private string filename = string.Empty;
    private string rangeStart = string.Empty;
    private string rangeEnd = string.Empty;
    private string rootFolderPath = string.Empty;
    private string filters = string.Empty;
    private List<string> keywords = (List<string>) null;
    private List<MatchDetails> totalMatches = (List<MatchDetails>) null;
    private object thisLock = new object();
    private IContainer components = (IContainer) null;
    private GroupBox groupBox1;
    private TextBox txtSearchPattern;
    private Label label3;
    private Button btnOpenFileDialog;
    private TextBox txtSheetName;
    private TextBox txtExcelPath;
    private Label label2;
    private Label label1;
    private Button btnFetchKeywords;
    private Label label5;
    private TextBox txtRangeEnd;
    private TextBox txtRangeStart;
    private Label label4;
    private GroupBox groupBox2;
    private Button btnOpenFolderDialog;
    private TextBox txtCodeBasePath;
    private Label label6;
    private Button btnSearchFiles;
    private ProgressBar pbSearchStatus;
    private Label lblScanStatus;
    private Func<string, bool> closure_0;

        public Form1()
    {
      this.InitializeComponent();
      this.SetDefaultValues();
      this.Init();
    }

    private void Init()
    {
      this.txtExcelPath.Enabled = false;
      this.txtSheetName.Enabled = false;
      this.txtRangeStart.Enabled = false;
      this.txtRangeEnd.Enabled = false;
      this.btnFetchKeywords.Enabled = false;
      this.filename = string.Empty;
      this.rangeStart = string.Empty;
      this.rangeEnd = string.Empty;
      this.rootFolderPath = string.Empty;
      this.filters = string.Empty;
      this.keywords = (List<string>) null;
      this.totalMatches = (List<MatchDetails>) null;
      this.txtCodeBasePath.Enabled = false;
      this.txtSearchPattern.Enabled = false;
      this.btnOpenFolderDialog.Enabled = false;
      this.btnSearchFiles.Enabled = false;
      this.pbSearchStatus.Visible = false;
      this.lblScanStatus.Visible = false;
    }

    private void SetDefaultValues()
    {
      if (Debugger.IsAttached)
      {
        this.txtSheetName.Text = "Keywords";
        this.txtRangeStart.Text = "A1";
        this.txtRangeEnd.Text = "A5";
        this.txtCodeBasePath.Text = "D:\\Sushovan\\TFS_Sushovan\\MLIApps\\2016R06\\ibws";
      }
      this.txtSearchPattern.Text = "*.htm;*.html;*.aspx; *.js; *.cs; *.vb; *.cshtml; *.vbhtml";
    }

    private void btnOpenFileDialog_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "Microsoft Excel 1997-2003 Workbook (.xls)|*.xls|Microsoft Excel Workbook (.xlsx)|*.xlsx";
      openFileDialog.FilterIndex = 2;
      openFileDialog.Multiselect = false;
      openFileDialog.Title = "Choose the spreadsheet containing the keywords";
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      this.txtExcelPath.Text = openFileDialog.FileName.Trim();
      this.filename = this.txtExcelPath.Text;
      this.txtSheetName.Enabled = true;
      this.txtRangeStart.Enabled = true;
      this.txtRangeEnd.Enabled = true;
      this.btnFetchKeywords.Enabled = true;
      AutoCompleteStringCollection sheetNames = this.GetSheetNames();
      if (sheetNames != null)
      {
        this.txtSheetName.AutoCompleteMode = AutoCompleteMode.Suggest;
        this.txtSheetName.AutoCompleteSource = AutoCompleteSource.CustomSource;
        this.txtSheetName.AutoCompleteCustomSource = sheetNames;
      }
    }

    private void btnFetchKeywords_Click(object sender, EventArgs e)
    {
      bool flag = false;
      this.rangeStart = this.txtRangeStart.Text.Trim();
      this.rangeEnd = this.txtRangeEnd.Text.Trim();
      if (char.IsLetter(this.rangeStart, 0) && char.IsLetter(this.rangeEnd, 0))
      {
        try
        {
          if (int.Parse(this.rangeEnd.Substring(1)) >= int.Parse(this.rangeStart.Substring(1)))
            flag = true;
        }
        catch (Exception)
        {
        }
      }
      if (flag)
      {
        try
        {
          using (OleDbConnection selectConnection = new OleDbConnection())
          {
            selectConnection.ConnectionString = this.GetConnectionString();
            selectConnection.Open();
            string selectCommandText = string.Format("SELECT * FROM [{0}${1}:{2}]", (object) this.txtSheetName.Text, (object) this.rangeStart, (object) this.rangeEnd);
            DataTable dataTable = new DataTable("Keywords");
            new OleDbDataAdapter(selectCommandText, selectConnection).Fill(dataTable);
            this.keywords = dataTable.AsEnumerable().Where<DataRow>((Func<DataRow, bool>) (y => y[0].ToString().Trim() != string.Empty)).Select<DataRow, string>((Func<DataRow, string>) (x => x[0].ToString())).ToList<string>();
            if (this.keywords.Count > 0)
            {
              int num = (int) MessageBox.Show("Total Keywords Count: " + (object) this.keywords.Count, "Keywords Hit");
              this.btnOpenFolderDialog.Enabled = true;
            }
            else
            {
              int num1 = (int) MessageBox.Show("No keywords found in the selected spreadsheet!", "No Data Present", MessageBoxButtons.OK);
            }
          }
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Error Description: " + ex.Message, "Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Hand);
        }
      }
      else
      {
        int num2 = (int) MessageBox.Show("Please provide a valid Range to proceed!", "Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Hand);
      }
    }

    private string GetConnectionString()
    {
      OleDbConnectionStringBuilder connectionStringBuilder = new OleDbConnectionStringBuilder();
      try
      {
        if (Path.GetExtension(this.filename).Equals(".XLS", StringComparison.InvariantCultureIgnoreCase))
        {
          connectionStringBuilder.Provider = "Microsoft.Jet.OLEDB.4.0";
          connectionStringBuilder.Add("Extended Properties", (object) "Excel 8.0;IMEX=1;HDR=YES;");
        }
        else
        {
          connectionStringBuilder.Provider = "Microsoft.ACE.OLEDB.12.0";
          connectionStringBuilder.Add("Extended Properties", (object) "Excel 12.0;IMEX=1;HDR=YES;");
        }
        connectionStringBuilder.DataSource = this.filename;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error Description: " + ex.Message, "Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Hand);
      }
      return connectionStringBuilder.ConnectionString;
    }

    private void btnOpenFolderDialog_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
      {
        this.txtCodeBasePath.Text = folderBrowserDialog.SelectedPath.ToString();
        this.rootFolderPath = this.txtCodeBasePath.Text.Trim();
        this.txtSearchPattern.Enabled = true;
        this.btnSearchFiles.Enabled = true;
      }
      if (!Debugger.IsAttached)
        return;
      this.txtSearchPattern.Enabled = true;
      this.btnSearchFiles.Enabled = true;
    }

    private void btnSearchFiles_Click(object sender, EventArgs e)
    {
      this.filters = this.txtSearchPattern.Text;
      if (Debugger.IsAttached)
        this.rootFolderPath = this.rootFolderPath = this.txtCodeBasePath.Text.Trim();
      List<string> stringList = this.filters.IndexOf("*.*") != -1 ? ((IEnumerable<string>) Directory.GetFiles(this.rootFolderPath, "*.*", SearchOption.AllDirectories)).ToList<string>() : ((IEnumerable<string>) Directory.GetFiles(this.rootFolderPath, "*.*", SearchOption.AllDirectories)).Where<string>((Func<string, bool>) (ext => this.filters.Contains(Path.GetExtension(ext)))).ToList<string>();
      stringList.RemoveAll((Predicate<string>) (file => ((IEnumerable<string>) Form1.filterExclusions).Contains<string>(file.Substring(file.LastIndexOf('.')))));
      string[] lines = (string[]) null;
      MatchDetails.TotalFilesScanned = stringList.Count;
      this.InitProgressBar();
      this.lblScanStatus.Text = "Scanning Started";
      this.lblScanStatus.Update();
      this.lblScanStatus.Refresh();
      foreach (string keyword in this.keywords)
      {
        string key = keyword;
        MatchDetails matchDetails2 = new MatchDetails();
        matchDetails2.MatchingFilesCount = 0;
        matchDetails2.SearchedText = key.Trim();
        matchDetails2.MatchingLinesCount = 0;
        matchDetails2.MatchedLines = (List<MatchedLinesDetails>) null;
        foreach (string str in stringList)
        {
          string file = str;
          if (new FileInfo(file).Length > 20971520L)
          {
            File.AppendAllText(Application.ExecutablePath.Substring(0, Application.ExecutablePath.IndexOf("Keyword Search")) + "MissedFileLogs.txt", DateTime.Now.ToString() + " and FileName : " + file + Environment.NewLine);
          }
          else
          {
            lines = File.ReadAllLines(file);
            List<MatchedLinesDetails> list = ((IEnumerable<string>) lines).AsParallel<string>().Where<string>(closure_0 ?? (closure_0 = (Func<string, bool>) (line =>
            {
              if (line.Trim() != string.Empty)
                return line.Trim().ToLower().Contains(key.Trim().ToLower());
              return false;
            }))).Select<string, MatchedLinesDetails>((Func<string, MatchedLinesDetails>) (matchedLine => new MatchedLinesDetails()
            {
              FilePath = file.Trim(),
              LineNumber = Array.IndexOf<string>(lines, matchedLine) + 1,
              MatchedLine = matchedLine.Trim()
            })).ToList<MatchedLinesDetails>();
            if (matchDetails2.MatchedLines == null)
              matchDetails2.MatchedLines = new List<MatchedLinesDetails>();
            matchDetails2.MatchedLines.AddRange((IEnumerable<MatchedLinesDetails>) list);
            matchDetails2.MatchingLinesCount += list.Count;
            if (list.Count > 0)
              ++matchDetails2.MatchingFilesCount;
          }
        }
        if (this.totalMatches == null)
          this.totalMatches = new List<MatchDetails>();
        this.totalMatches.Add(matchDetails2);
        lock (this)
          this.UpdateProgressBar();
      }
      if (this.pbSearchStatus.Value == this.pbSearchStatus.Maximum)
        this.lblScanStatus.Text = "Scanning Completed";
      if (MessageBox.Show("Do you want to export the result to a text file?", "Please choose an option", MessageBoxButtons.YesNo) == DialogResult.Yes)
      {
        string sourceFilePath = string.Empty;
        Task.WaitAll(Task.Factory.StartNew((Action) (() => sourceFilePath = this.ExportToTextFile(this.totalMatches))));
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Title = "Please choose a location to save this file";
        saveFileDialog.Filter = "Text File (.txt)|*.txt|All Files (*.*)|*.*";
        saveFileDialog.FilterIndex = 1;
        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
          string fileName = saveFileDialog.FileName;
          if (File.Exists(fileName))
            File.Delete(fileName);
          File.Move(sourceFilePath, fileName);
          int num = (int) MessageBox.Show("Detailed Scanned Report has been generated successfully!", "Export Success");
        }
        if (MessageBox.Show("Would you like to perform another search?", "Please choose an option", MessageBoxButtons.YesNo) == DialogResult.Yes)
          this.Init();
        else
          this.Dispose();
      }
      else if (MessageBox.Show("Would you like to perform another search?", "Please choose an option", MessageBoxButtons.YesNo) == DialogResult.Yes)
        this.Init();
      else
        this.Dispose();
    }

    private void UpdateProgressBar()
    {
      ++this.pbSearchStatus.Value;
      this.pbSearchStatus.Refresh();
      this.pbSearchStatus.CreateGraphics().DrawString(((int) ((double) (this.pbSearchStatus.Value - 1) / (double) this.pbSearchStatus.Maximum * 100.0)).ToString() + "%", new Font("Arial", 8.25f, FontStyle.Bold), Brushes.Black, new PointF((float) (this.pbSearchStatus.Width / 2 - 10), (float) (this.pbSearchStatus.Height / 2 - 7)));
      this.lblScanStatus.Invalidate();
      this.lblScanStatus.Update();
      this.lblScanStatus.Refresh();
      Thread.Sleep(200);
    }

    private void InitProgressBar()
    {
      this.pbSearchStatus.Maximum = this.keywords.Count;
      this.pbSearchStatus.Minimum = 0;
      this.pbSearchStatus.Value = this.pbSearchStatus.Minimum;
      this.pbSearchStatus.Step = 1;
      this.pbSearchStatus.Visible = true;
      this.lblScanStatus.Visible = true;
      this.Update();
      this.Refresh();
    }

    private string ExportToTextFile(List<MatchDetails> totalMatches)
    {
      string name;
      using (StreamWriter streamWriter = new StreamWriter(string.Format("{0}\\{1}{2}", (object) Path.GetTempPath(), (object) this.rootFolderPath.Substring(this.rootFolderPath.LastIndexOf("\\")), (object) "_AnalysisReport.txt"), false))
      {
        bool flag = true;
        foreach (MatchDetails totalMatch in totalMatches)
        {
          if (flag)
          {
            streamWriter.WriteLine("Total Files Scanned: {0}", (object) MatchDetails.TotalFilesScanned);
            streamWriter.WriteLine();
            flag = false;
          }
          streamWriter.WriteLine("Searched Text: {0}", (object) totalMatch.SearchedText);
          foreach (MatchedLinesDetails matchedLine in totalMatch.MatchedLines)
          {
            matchedLine.MatchedLine = matchedLine.MatchedLine.Replace(",", "_$_$_");
            streamWriter.WriteLine("{0} ({1}): \t, {2}", (object) matchedLine.FilePath, (object) matchedLine.LineNumber, (object) matchedLine.MatchedLine);
          }
          streamWriter.WriteLine("Matching Lines: {0} \t Matching Files: {1}", (object) totalMatch.MatchingLinesCount, (object) totalMatch.MatchingFilesCount);
          streamWriter.WriteLine();
        }
        name = ((FileStream) streamWriter.BaseStream).Name;
      }
      return name;
    }

    private void ExportToExcel()
    {
    }

    private AutoCompleteStringCollection GetSheetNames()
    {
      AutoCompleteStringCollection stringCollection = (AutoCompleteStringCollection) null;
      try
      {
        using (OleDbConnection oleDbConnection = new OleDbConnection())
        {
          oleDbConnection.ConnectionString = this.GetConnectionString();
          oleDbConnection.Open();
          foreach (DataRow row in (InternalDataCollectionBase) oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, (object[]) null).Rows)
          {
            if (stringCollection == null)
              stringCollection = new AutoCompleteStringCollection();
            if (row["TABLE_NAME"].ToString().Contains<char>(' '))
            {
              string str = row["TABLE_NAME"].ToString().Replace("'", "");
              stringCollection.Add(str.Substring(0, str.Length - 1));
            }
            else
              stringCollection.Add(row["TABLE_NAME"].ToString().Substring(0, row["TABLE_NAME"].ToString().Length - 1));
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error Description: " + ex.Message, "Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Hand);
      }
      return stringCollection;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupBox1 = new GroupBox();
      this.label5 = new Label();
      this.txtRangeEnd = new TextBox();
      this.txtRangeStart = new TextBox();
      this.label4 = new Label();
      this.btnFetchKeywords = new Button();
      this.btnOpenFileDialog = new Button();
      this.txtSheetName = new TextBox();
      this.txtExcelPath = new TextBox();
      this.label2 = new Label();
      this.label1 = new Label();
      this.label3 = new Label();
      this.txtSearchPattern = new TextBox();
      this.groupBox2 = new GroupBox();
      this.lblScanStatus = new Label();
      this.pbSearchStatus = new ProgressBar();
      this.btnSearchFiles = new Button();
      this.btnOpenFolderDialog = new Button();
      this.txtCodeBasePath = new TextBox();
      this.label6 = new Label();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      this.groupBox1.Controls.Add((Control) this.label5);
      this.groupBox1.Controls.Add((Control) this.txtRangeEnd);
      this.groupBox1.Controls.Add((Control) this.txtRangeStart);
      this.groupBox1.Controls.Add((Control) this.label4);
      this.groupBox1.Controls.Add((Control) this.btnFetchKeywords);
      this.groupBox1.Controls.Add((Control) this.btnOpenFileDialog);
      this.groupBox1.Controls.Add((Control) this.txtSheetName);
      this.groupBox1.Controls.Add((Control) this.txtExcelPath);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Location = new Point(15, 23);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(650, 160);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Input Spreadsheet containing keywords";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(188, 85);
      this.label5.Name = "label5";
      this.label5.Size = new Size(10, 13);
      this.label5.TabIndex = 11;
      this.label5.Text = ":";
      this.txtRangeEnd.Location = new Point(203, 82);
      this.txtRangeEnd.Name = "txtRangeEnd";
      this.txtRangeEnd.Size = new Size(38, 20);
      this.txtRangeEnd.TabIndex = 4;
      this.txtRangeStart.Location = new Point(145, 81);
      this.txtRangeStart.Name = "txtRangeStart";
      this.txtRangeStart.Size = new Size(38, 20);
      this.txtRangeStart.TabIndex = 3;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(19, 84);
      this.label4.Name = "label4";
      this.label4.Size = new Size(39, 13);
      this.label4.TabIndex = 8;
      this.label4.Text = "Range";
      this.btnFetchKeywords.Location = new Point(490, 108);
      this.btnFetchKeywords.Name = "btnFetchKeywords";
      this.btnFetchKeywords.Size = new Size(133, 37);
      this.btnFetchKeywords.TabIndex = 5;
      this.btnFetchKeywords.Text = "Fetch Keywords";
      this.btnFetchKeywords.UseVisualStyleBackColor = true;
      this.btnFetchKeywords.Click += new EventHandler(this.btnFetchKeywords_Click);
      this.btnOpenFileDialog.Location = new Point(528, 17);
      this.btnOpenFileDialog.Name = "btnOpenFileDialog";
      this.btnOpenFileDialog.Size = new Size(95, 23);
      this.btnOpenFileDialog.TabIndex = 1;
      this.btnOpenFileDialog.Text = "Browse";
      this.btnOpenFileDialog.UseVisualStyleBackColor = true;
      this.btnOpenFileDialog.Click += new EventHandler(this.btnOpenFileDialog_Click);
      this.txtSheetName.Location = new Point(145, 50);
      this.txtSheetName.Name = "txtSheetName";
      this.txtSheetName.Size = new Size(200, 20);
      this.txtSheetName.TabIndex = 2;
      this.txtExcelPath.AcceptsReturn = true;
      this.txtExcelPath.Location = new Point(145, 19);
      this.txtExcelPath.Name = "txtExcelPath";
      this.txtExcelPath.Size = new Size(367, 20);
      this.txtExcelPath.TabIndex = 2;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(19, 53);
      this.label2.Name = "label2";
      this.label2.Size = new Size(90, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Worksheet Name";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(19, 22);
      this.label1.Name = "label1";
      this.label1.Size = new Size(115, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Choose a Spreadsheet";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(19, 54);
      this.label3.Name = "label3";
      this.label3.Size = new Size(59, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "Find in files";
      this.txtSearchPattern.Location = new Point(145, 51);
      this.txtSearchPattern.Name = "txtSearchPattern";
      this.txtSearchPattern.Size = new Size(290, 20);
      this.txtSearchPattern.TabIndex = 7;
      this.groupBox2.Controls.Add((Control) this.lblScanStatus);
      this.groupBox2.Controls.Add((Control) this.pbSearchStatus);
      this.groupBox2.Controls.Add((Control) this.btnSearchFiles);
      this.groupBox2.Controls.Add((Control) this.btnOpenFolderDialog);
      this.groupBox2.Controls.Add((Control) this.txtCodeBasePath);
      this.groupBox2.Controls.Add((Control) this.label6);
      this.groupBox2.Controls.Add((Control) this.txtSearchPattern);
      this.groupBox2.Controls.Add((Control) this.label3);
      this.groupBox2.Location = new Point(15, 189);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(650, 172);
      this.groupBox2.TabIndex = 7;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Input folder containing files";
      this.lblScanStatus.AutoSize = true;
      this.lblScanStatus.Location = new Point(19, 112);
      this.lblScanStatus.Name = "lblScanStatus";
      this.lblScanStatus.Size = new Size(0, 13);
      this.lblScanStatus.TabIndex = 10;
      this.pbSearchStatus.Location = new Point(22, 128);
      this.pbSearchStatus.Name = "pbSearchStatus";
      this.pbSearchStatus.Size = new Size(601, 23);
      this.pbSearchStatus.TabIndex = 9;
      this.pbSearchStatus.UseWaitCursor = true;
      this.btnSearchFiles.Location = new Point(528, 85);
      this.btnSearchFiles.Name = "btnSearchFiles";
      this.btnSearchFiles.Size = new Size(95, 37);
      this.btnSearchFiles.TabIndex = 8;
      this.btnSearchFiles.Text = "Search";
      this.btnSearchFiles.UseVisualStyleBackColor = true;
      this.btnSearchFiles.Click += new EventHandler(this.btnSearchFiles_Click);
      this.btnOpenFolderDialog.Location = new Point(528, 20);
      this.btnOpenFolderDialog.Name = "btnOpenFolderDialog";
      this.btnOpenFolderDialog.Size = new Size(95, 23);
      this.btnOpenFolderDialog.TabIndex = 6;
      this.btnOpenFolderDialog.Text = "Browse";
      this.btnOpenFolderDialog.UseVisualStyleBackColor = true;
      this.btnOpenFolderDialog.Click += new EventHandler(this.btnOpenFolderDialog_Click);
      this.txtCodeBasePath.Location = new Point(145, 22);
      this.txtCodeBasePath.Name = "txtCodeBasePath";
      this.txtCodeBasePath.Size = new Size(367, 20);
      this.txtCodeBasePath.TabIndex = 8;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(19, 25);
      this.label6.Name = "label6";
      this.label6.Size = new Size(84, 13);
      this.label6.TabIndex = 7;
      this.label6.Text = "Choose a Folder";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(682, 373);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.groupBox1);
      this.Name = "Form1";
      this.Text = "Advanced Text Search";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
