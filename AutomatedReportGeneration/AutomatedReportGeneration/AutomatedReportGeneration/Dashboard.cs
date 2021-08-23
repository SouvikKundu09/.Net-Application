using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using Reflection = System.Reflection;
using System.Text;

namespace AutomatedReportGeneration
{
    public partial class Dashboard : Form
    {
        static string headerDate;
        static string tailerCount;
        static int progressCount;

        private string fileTempTarget = default(string), templateFileLocation = default(string), logFilePath = default(string);
        Excel.Application excelApp;
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            progressBar1.Visible = false;
            lblProgress.Visible = false;

            //txtSecondFeedFile.Visible = false;
            //lblSecondFeedFile.Visible = false;
            //secondFeedFileBtn.Visible = false;
            //label2.Visible = false;
            //dtPickerCurrentDateSecondFeed.Visible = false;

            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;


            dtPickerCurrentDateFirstFeed.Format = DateTimePickerFormat.Custom;
            dtPickerCurrentDateFirstFeed.CustomFormat = "MM-dd-yyyy";
            dtPickerCurrentDateFirstFeed.Value = DateTime.Now;

            dtPickerCurrentDateSecondFeed.Format = DateTimePickerFormat.Custom;
            dtPickerCurrentDateSecondFeed.CustomFormat = "MM-dd-yyyy";
            dtPickerCurrentDateSecondFeed.Value = DateTime.Now;

            //Intialize File Temporary Target and Template location

            fileTempTarget = Environment.CurrentDirectory.Replace("~", string.Empty) + @"\TempLocation\";
            if (!Directory.Exists(fileTempTarget))
            {
                Directory.CreateDirectory(fileTempTarget);
            }

            templateFileLocation = Environment.CurrentDirectory.Replace("~", string.Empty) + @"\TemplateReports\";
            if (!Directory.Exists(templateFileLocation))
            {
                MessageBox.Show("Template Reports are not present .Please add those in this path :" + templateFileLocation);
                Directory.CreateDirectory(templateFileLocation);
            }


        }

        //1st feed file browse button click
        private void BrowseBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Title = "Browse Text Feed File";

            openFileDlg.CheckFileExists = true;
            openFileDlg.CheckPathExists = true;
            openFileDlg.FilterIndex = 1;
            openFileDlg.DefaultExt = "txt";
            openFileDlg.Filter = "Text|*.txt|All|*.*";
            openFileDlg.RestoreDirectory = true;
            openFileDlg.ReadOnlyChecked = true;
            openFileDlg.ShowReadOnly = true;

            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                if (((Button)sender).Name == "firstFeedFileBtn")
                {
                    txtfirstFeedFile.Text = openFileDlg.FileName;
                }
                else if (((Button)sender).Name == "secondFeedFileBtn")
                {
                    txtSecondFeedFile.Text = openFileDlg.FileName;
                }
            }

        }

        //Report generation button
        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            txtfirstFeedFile.Enabled = false;
            txtSecondFeedFile.Enabled = false;
            firstFeedFileBtn.Enabled = false;
            secondFeedFileBtn.Enabled = false;
            btnGenerateReport.Enabled = false;
            lblProgress.Visible = false;
            headerDate = default(string);
            tailerCount = default(string);
            if (!string.IsNullOrWhiteSpace(txtfirstFeedFile.Text.Trim()) || !string.IsNullOrWhiteSpace(txtSecondFeedFile.Text.Trim()))
            {
                Stopwatch stopWatch = new Stopwatch();

                // WriteToTextFile(Reflection.MethodBase.GetCurrentMethod().Name + " started ");
                stopWatch.Start();

                StopExistingExcelProcess();

                //Load text file to datatable
                DataTable dtFirstFeedFile = new DataTable();
                DataTable dtSecondFeedFile = new DataTable();
                if (!string.IsNullOrWhiteSpace(txtfirstFeedFile.Text.Trim()))
                {
                    ConvertToDataTable(ref dtFirstFeedFile, txtfirstFeedFile.Text.Trim(), 29);
                    DateTime headerDatedt = DateTime.ParseExact(headerDate, "yyyyMMdd", null);
                    dtPickerCurrentDateFirstFeed.Text = headerDatedt.ToString();
                }

                if (!string.IsNullOrWhiteSpace(txtSecondFeedFile.Text.Trim()))
                {
                    ConvertToDataTable(ref dtSecondFeedFile, txtSecondFeedFile.Text.Trim(), 29);
                    DateTime headerDatedt = DateTime.ParseExact(headerDate, "yyyyMMdd", null);
                    dtPickerCurrentDateSecondFeed.Text = headerDatedt.ToString();
                }

                if (dtFirstFeedFile.Rows.Count > 0)
                {
                    ProgressDividend(dtFirstFeedFile.Rows.Count);
                    CreateAutomatedExcelReportFirstFeed(dtFirstFeedFile);
                }

                if (dtSecondFeedFile.Rows.Count > 0)
                {
                    ProgressDividend(dtSecondFeedFile.Rows.Count);
                    CreateAutomatedExcelReportSecondFeed(dtSecondFeedFile);
                }
                progressCount = default(int);
                stopWatch.Stop();
                lblProgress.Visible = true;
                lblProgress.Text = stopWatch.ElapsedMilliseconds.ToString() + " Milliseconds elapsed.";
                progressBar1.Visible = false;
                lblProgress.Visible = false;
            }
            else
            {
                MessageBox.Show("Please browse the feed file");
            }
            txtfirstFeedFile.Text = string.Empty;
            txtSecondFeedFile.Text = string.Empty;
            txtfirstFeedFile.Enabled = true;
            txtSecondFeedFile.Enabled = true;
            firstFeedFileBtn.Enabled = true;
            secondFeedFileBtn.Enabled = true;
            btnGenerateReport.Enabled = true;
            headerDate = default(string);
            tailerCount = default(string);
            // WriteToTextFile(Reflection.MethodBase.GetCurrentMethod().Name + " Ended ");
        }

        //Load Feed to Datatable
        private void ConvertToDataTable(ref DataTable dtFeedFile, string feedFilePath, int numberOfColumns)
        {
            //  WriteToTextFile(Reflection.MethodBase.GetCurrentMethod().Name + " started ");
            string[] lines = File.ReadAllLines(feedFilePath);
            lines = lines.Where(w => w.Contains(",")).ToArray();
            string firstLine = lines.First();
            string lastLine = lines.Last();
            string[] ColumnNames = lines.Skip(1).First().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string item in ColumnNames)
            {
                dtFeedFile.Columns.Add(new DataColumn() { ColumnName = item });
            }

            lines = lines.Where(w => w != firstLine && w != lastLine).ToArray();

            foreach (string line in lines)
            {
                var cols = line.Split(',');
                DataRow dr = dtFeedFile.NewRow();
                for (int cIndex = 0; cIndex < 29; cIndex++)
                {
                    dr[cIndex] = cols[cIndex];
                }
                dtFeedFile.Rows.Add(dr);
            }
            headerDate = firstLine.Split(',')[2].Trim();
            tailerCount = lastLine.Split(',')[1].Trim();
            dtFeedFile.Rows.Remove(dtFeedFile.Rows[0]);
            //  WriteToTextFile(Reflection.MethodBase.GetCurrentMethod().Name + " Ended ");

        }

        //General Function for creating Automated Excel Report from First Feed
        private void CreateAutomatedExcelReportFirstFeed(DataTable dt1)
        {
            //  WriteToTextFile(Reflection.MethodBase.GetCurrentMethod().Name + " started ");
            string newFileName = default(string), tempFileFullPath = default(string), tempTargetFilefullPath = default(string);

            if (!string.IsNullOrWhiteSpace(txtfirstFeedFile.Text.Trim()))
            {
                //Create BAC_Feed File_Summary report
                tempFileFullPath = templateFileLocation + "BAC_Feed File_Summary.xlsm";
                newFileName = Path.GetFileNameWithoutExtension(tempFileFullPath) + "_D" + DateTime.Now.ToString("MM/dd/yyyy").Replace("/", string.Empty) + Path.GetExtension(tempFileFullPath);
                tempTargetFilefullPath = fileTempTarget + newFileName;
                lblCurrentFile.Text = "Working on " + newFileName + "...";
                Create_BAC_FeedFile_Summary_Report(tempFileFullPath, tempTargetFilefullPath, dt1);
                lblCurrentFile.Text = string.Empty;


                //Create BOA ConsolidatedFeedFile report
                tempFileFullPath = templateFileLocation + "BOA ConsolidatedFeedFile.xlsx";
                newFileName = Path.GetFileNameWithoutExtension(tempFileFullPath) + "_" + DateTime.Now.ToString("MMM-dd") + Path.GetExtension(tempFileFullPath);
                tempTargetFilefullPath = fileTempTarget + newFileName;
                lblCurrentFile.Text = "Working on " + newFileName + "...";
                Create_BOAConsolidatedFeedFile_Report(tempFileFullPath, tempTargetFilefullPath, dt1);
                lblCurrentFile.Text = string.Empty;


                //Create BOAConsolidatedFeedFile_LOA_Denied_Report
                tempFileFullPath = templateFileLocation + "BOA ConsolidatedFeedFile_LOADenied.xlsx";
                newFileName = Path.GetFileNameWithoutExtension(tempFileFullPath) + "_" + DateTime.Now.ToString("MMM-dd") + Path.GetExtension(tempFileFullPath);
                tempTargetFilefullPath = fileTempTarget + newFileName;
                lblCurrentFile.Text = "Working on " + newFileName + "...";
                Create_BOAConsolidatedFeedFile_LOA_Denied_Report(tempFileFullPath, tempTargetFilefullPath, dt1);
                lblCurrentFile.Text = string.Empty;
            }


            //  WriteToTextFile(Reflection.MethodBase.GetCurrentMethod().Name + " Ended ");
        }

        //General Function for creating Automated Excel Report from First Feed
        private void CreateAutomatedExcelReportSecondFeed(DataTable dt1)
        {
            //  WriteToTextFile(Reflection.MethodBase.GetCurrentMethod().Name + " started ");
            string newFileName = default(string), tempFileFullPath = default(string), tempTargetFilefullPath = default(string);



            if (!string.IsNullOrWhiteSpace(txtSecondFeedFile.Text.Trim()))
            {
                //Create_BOA_HR_Leave_Feed_XX
                tempFileFullPath = templateFileLocation + "BOA_HR_Leave_Feed_XX.xlsx";
                newFileName = Path.GetFileNameWithoutExtension(tempFileFullPath) + Path.GetExtension(tempFileFullPath);
                tempTargetFilefullPath = fileTempTarget + newFileName;
                lblCurrentFile.Text = "Working on " + newFileName + "...";
                Create_BOA_HR_Leave_Feed_XX(tempFileFullPath, tempTargetFilefullPath, dt1);
                lblCurrentFile.Text = string.Empty;
            }

            //  WriteToTextFile(Reflection.MethodBase.GetCurrentMethod().Name + " Ended ");
        }

        // Create_BAC_FeedFile_Summary_Report
        private void Create_BAC_FeedFile_Summary_Report(string fileTemplate, string fileTarget, DataTable dt)
        {
            excelApp = new Excel.Application();
            // WriteToTextFile(Reflection.MethodBase.GetCurrentMethod().Name + " started ");
            if (File.Exists(fileTemplate))
            {
                File.Delete(fileTarget);
                if (!File.Exists(fileTarget))
                {
                    File.Copy(fileTemplate, fileTarget, true);
                }

                Excel.Workbook m_wbBook = excelApp.Workbooks.Open(fileTarget, 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                Excel.Worksheet m_wsSheet;
                m_wsSheet = m_wbBook.Worksheets["Feedfile"];
                progressBar1.Visible = true;
                try
                {
                    m_wsSheet.Cells.EntireColumn.Hidden = false;

                    Excel.Range xlRng;
                    xlRng = m_wsSheet.Cells;
                    m_wsSheet.Range[m_wsSheet.Cells[2, 1], m_wsSheet.Cells[xlRng.Rows.Count, xlRng.Columns.Count]].ClearContents();


                    //*********
                    dynamic dataArray;
                    Excel.Range rng;
                    int maxRowValue = default(int);
                    for (int row = 1; row <= dt.Rows.Count; row++)
                    {
                        dataArray = dt.Rows[row - 1].ItemArray.Select(x => x).ToArray();
                        rng = m_wsSheet.get_Range("A" + (row + 1).ToString(), "AC" + (row + 1).ToString());
                        rng.Value = dataArray;

                        Excel.Range rngAG = m_wsSheet.Range["AG" + (row + 1).ToString()];
                        rngAG.Formula = string.Format("=IFERROR(VLOOKUP(I" + (row + 1).ToString() + ",LkupPayType,2,FALSE),\"{0}\")", "TBD");
                        Excel.Range rngAH = m_wsSheet.Range["AH" + (row + 1).ToString()];
                        rngAH.Formula = string.Format("=IFERROR(VLOOKUP(B" + (row + 1).ToString() + ",LkupPayrollCycle,2,FALSE),\"{0}\")", "TBD");
                        ProgressBarCalculation(row);
                        maxRowValue = row;
                    }
                    m_wsSheet.Range[m_wsSheet.Cells[maxRowValue + 2, 1], m_wsSheet.Cells[xlRng.Rows.Count, xlRng.Columns.Count]].ClearContents();
                    //*********


                    m_wsSheet.Range[m_wsSheet.Cells[1, 10], m_wsSheet.Cells[1, 19]].Select(); //xlRng.Rows.Count, xlRng.Columns.Count
                    excelApp.Selection.EntireColumn.Hidden = true;
                    m_wsSheet.Range[m_wsSheet.Cells[1, 21], m_wsSheet.Cells[1, 29]].Select();
                    excelApp.Selection.EntireColumn.Hidden = true;
                    m_wsSheet = m_wbBook.Worksheets["LookUp"];
                    m_wsSheet.Cells[2, 7] = headerDate;

                    progressBar1.Value = progressBar1.Maximum;



                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message.ToString());
                }
                finally
                {
                    closeExcel(m_wbBook, fileTarget, excelApp);
                    savefileAction(fileTarget);
                }
            }
            else
            {
                MessageBox.Show("There is no template file named " + Path.GetFileNameWithoutExtension(fileTemplate) + Path.GetExtension(fileTemplate) + " Present in " + Path.GetFullPath(fileTemplate) + " location.");
            }
            //   WriteToTextFile(Reflection.MethodBase.GetCurrentMethod().Name + " Ended ");
        }

        //Create_BOAConsolidatedFeedFile_Report
        private void Create_BOAConsolidatedFeedFile_Report(string fileTemplate, string fileTarget, DataTable table)
        {
            excelApp = new Excel.Application();
            //  WriteToTextFile(Reflection.MethodBase.GetCurrentMethod().Name + " started ");
            if (File.Exists(fileTemplate))
            {
                File.Delete(fileTarget);
                if (!File.Exists(fileTarget))
                {
                    File.Copy(fileTemplate, fileTarget, true);
                }
                Excel.Workbook excelWorkBook = excelApp.Workbooks.Open(fileTarget, 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                Excel.Worksheet excelWorksheet = excelWorkBook.Sheets["BOA Complete File Sheet"];

                int totalUsedRange = excelWorksheet.Cells.Find("*", Reflection.Missing.Value, Reflection.Missing.Value, Reflection.Missing.Value, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious, false, Reflection.Missing.Value, Reflection.Missing.Value).Row;

                StringBuilder formulaStrBuilder = new StringBuilder();

                try
                {

                    progressBar1.Visible = true;

                    //*********
                    dynamic dataArray;
                    Excel.Range rng;
                    int nowTotalUsedRange = default(int);
                    for (int row = totalUsedRange; row < table.Rows.Count + totalUsedRange; row++)
                    {
                        rng = excelWorksheet.get_Range("A" + (row + 1).ToString());
                        rng.Value = headerDate;

                        dataArray = table.Rows[row - totalUsedRange].ItemArray.Select(x => x).ToArray();
                        rng = excelWorksheet.get_Range("B" + (row + 1).ToString(), "AD" + (row + 1).ToString());
                        rng.Value = dataArray;

                        formulaStrBuilder.Clear();
                        formulaStrBuilder.Append("=CONCATENATE(D" + (row + 1).ToString() + ",\"|\"," +
                                            "H" + (row + 1).ToString() + ",\"|\"," +
                                            "A" + (row + 1).ToString() + ",\"|\"," +
                                            "B" + (row + 1).ToString() + ",\"|\"," +
                                            "J" + (row + 1).ToString() + ",\"|\"," +
                                            "E" + (row + 1).ToString() + ")");
                        rng = excelWorksheet.Range["AG" + (row + 1).ToString()];
                        rng.Formula = formulaStrBuilder.ToString();
                        ProgressBarCalculation(row);
                        nowTotalUsedRange = row;
                    }
                    //**********

                    excelWorksheet.Range[excelWorksheet.Cells[nowTotalUsedRange + 2, 1],
                    excelWorksheet.Cells[excelWorksheet.Rows.Count, excelWorksheet.Columns.Count]].ClearContents();
                    progressBar1.Value = progressBar1.Maximum;
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message.ToString());
                }
                finally
                {
                    closeExcel(excelWorkBook, fileTarget, excelApp);

                    //if (File.Exists(fileTarget))
                    //{
                    //    File.Copy(fileTarget, fileTemplate, true);
                    //}
                    savefileAction(fileTarget);
                }
            }
            else
            {
                MessageBox.Show("There is no template file named " + Path.GetFileNameWithoutExtension(fileTemplate) + Path.GetExtension(fileTemplate) + " Present in " + Path.GetFullPath(fileTemplate) + " location.");
            }

            //   WriteToTextFile(Reflection.MethodBase.GetCurrentMethod().Name + " Ended ");
        }

        //Create_BOAConsolidatedFeedFile_LOA_Denied_Report
        private void Create_BOAConsolidatedFeedFile_LOA_Denied_Report(string fileTemplate, string fileTarget, DataTable table)
        {
            excelApp = new Excel.Application();
            //  WriteToTextFile(Reflection.MethodBase.GetCurrentMethod().Name + " started ");
            if (File.Exists(fileTemplate))
            {

                File.Delete(fileTarget);
                if (!File.Exists(fileTarget))
                {
                    File.Copy(fileTemplate, fileTarget, true);
                }

                Excel.Workbook excelWorkBook = excelApp.Workbooks.Open(fileTarget, 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                Excel.Worksheet wSheet = excelWorkBook.Sheets["BOA Complete File Sheet"];
                Excel.Worksheet m_wsSheetLookUp = excelWorkBook.Worksheets["LookUp"];
                Excel.Worksheet m_wsSheetLoopingRecord = excelWorkBook.Worksheets["LOA Denied -Looping Record"];

                ShowAllData(wSheet);
                int totalUsedRangeWithOutHeader = wSheet.Cells.Find("*", Reflection.Missing.Value, Reflection.Missing.Value, Reflection.Missing.Value, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious, false, Reflection.Missing.Value, Reflection.Missing.Value).Row - 1;
                StringBuilder formulaStrBuilder = new StringBuilder();

                try
                {

                    progressBar1.Visible = true;

                    /************STEP 1************************/
                    dynamic dataArray;
                    Excel.Range rng;
                    int nowTotalUsedRange = default(int), currentRow = default(int), initialRowNumber = default(int);

                    for (int row = 0; row < table.Rows.Count; row++)
                    {

                        dataArray = table.Rows[row].ItemArray.Select(x => x).ToArray();

                        currentRow = row + 1 + (totalUsedRangeWithOutHeader + 1);
                        if (row == 0) initialRowNumber = currentRow;

                        rng = wSheet.get_Range("B" + currentRow.ToString(), "AD" + currentRow.ToString());
                        rng.Value = dataArray;

                        if (string.IsNullOrWhiteSpace((wSheet.Range["AG2"]).Value))
                        {
                            formulaStrBuilder.Clear();
                            formulaStrBuilder.Append("=CONCATENATE(C" + currentRow.ToString() + ",\",\"," +
                                                "D" + currentRow.ToString() + ",\",\"," +
                                                "E" + currentRow.ToString() + ",\",\"," +
                                                "F" + currentRow.ToString() + ",\",\"," +
                                                "G" + currentRow.ToString() + ",\",\"," +
                                                "H" + currentRow.ToString() + ",\",\"," +
                                                "I" + currentRow.ToString() + ",\",\"," +
                                                "J" + currentRow.ToString() + ",\",\"," +
                                                "K" + currentRow.ToString() + ",\",\"," +
                                                "L" + currentRow.ToString() + ",\",\"," +
                                                "M" + currentRow.ToString() + ",\",\"," +
                                                "N" + currentRow.ToString() + ",\",\"," +
                                                "O" + currentRow.ToString() + ",\",\"," +
                                                "P" + currentRow.ToString() + ",\",\"," +
                                                "Q" + currentRow.ToString() + ",\",\"," +
                                                "R" + currentRow.ToString() + ",\",\"," +
                                                "S" + currentRow.ToString() + ",\",\"," +
                                                "T" + currentRow.ToString() + ",\",\"," +
                                                "U" + currentRow.ToString() + ",\",\"," +
                                                "V" + currentRow.ToString() + ",\",\"," +
                                                "W" + currentRow.ToString() + ",\",\"," +
                                                "X" + currentRow.ToString() + ",\",\"," +
                                                "Y" + currentRow.ToString() + ",\",\"," +
                                                "Z" + currentRow.ToString() + ",\",\"," +
                                                "AA" + currentRow.ToString() + ",\",\"," +
                                                "AB" + currentRow.ToString() + ",\",\"," +
                                                "AC" + currentRow.ToString() + ",\",\"," +
                                                "TRIM(AD" + currentRow.ToString() + "))");

                            rng = wSheet.Range["AG" + currentRow.ToString()];
                            rng.Formula = formulaStrBuilder.ToString();
                        }
                        if (string.IsNullOrWhiteSpace((wSheet.Range["AH2"]).Value))
                        {
                            ////= IF(AND([@[Feed Run Date]]= CheckDate,[@[Leave Type]]=CheckLeave)=TRUE,"Y","-")
                            formulaStrBuilder.Clear();
                            formulaStrBuilder.Append("= IF(AND(A" + currentRow.ToString() + "= CheckDate,J" +
                                                 currentRow.ToString() + "= CheckLeave)=TRUE" + "," + "\"Y\"" + "," + "\"-\"" + ")");
                            rng = wSheet.Range["AH" + currentRow.ToString()];
                            rng.Formula = formulaStrBuilder.ToString();
                        }
                        if (string.IsNullOrWhiteSpace((wSheet.Range["AI2"]).Value))
                        {
                            //=IF([@[LOA Denied?]]="Y",IF(COUNTIFS([Record except Type],[@[Record except Type]],[Feed Run Date],"<"&CheckDate)>0,"Y","N"),"-")
                            formulaStrBuilder.Clear();
                            formulaStrBuilder.Append("=IF(AH" + currentRow.ToString() + "=\"Y\",IF(COUNTIFS([Record except Type]," + "AG" + currentRow.ToString() + ",[Feed Run Date], \"<\"&CheckDate)>0,\"Y\",\"N\"),\"-\")");
                            rng = wSheet.Range["AI" + currentRow.ToString()];
                            rng.Formula = formulaStrBuilder.ToString();
                        }
                        if (string.IsNullOrWhiteSpace((wSheet.Range["AJ2"]).Value))
                        {
                            //=IF([@[LOA Denied?]]="Y",IF(COUNTIFS([Feed Run Date],CheckDate,[Employee ID (Person ID) ],[@[Employee ID (Person ID) ]],[[Claim number code ]],[@[Claim number code ]])>1,"Y","N"),"-")
                            formulaStrBuilder.Clear();
                            formulaStrBuilder.Append("=IF(AH" + currentRow.ToString() + "=\"Y\",IF(COUNTIFS([Feed Run Date],CheckDate,[Employee ID (Person ID) ],D" + currentRow.ToString() + ",[[Claim number code ]],E" + currentRow.ToString() + ")>1,\"Y\",\"N\"),\" - \")");
                            rng = wSheet.Range["AJ" + currentRow.ToString()];
                            rng.Formula = formulaStrBuilder.ToString();
                        }
                        ProgressBarCalculation(row + 1);
                        nowTotalUsedRange = (row + 1) + (totalUsedRangeWithOutHeader + 1);
                    }
                    wSheet.Range["A" + initialRowNumber + ":A" + nowTotalUsedRange].Value = headerDate;

                    //**********

                    wSheet.Range[wSheet.Cells[nowTotalUsedRange + 1, 1], wSheet.Cells[wSheet.Rows.Count, wSheet.Columns.Count]].ClearContents();

                    Excel.Range sourceRange = wSheet.Range["A1", "A1"];
                    ShowAllData(wSheet);

                    /**STEP 3 -6*/
                    sourceRange.AutoFilter(Field: 1, Criteria1: headerDate, Operator: Excel.XlAutoFilterOperator.xlFilterValues, Criteria2: Type.Missing, VisibleDropDown: true); //1 :Feed Run Date
                    sourceRange.AutoFilter(Field: 10, Criteria1: "US - LOA Denied", Operator: Excel.XlAutoFilterOperator.xlFilterValues, Criteria2: Type.Missing, VisibleDropDown: true); // 10 :Leave Type
                    sourceRange.AutoFilter(Field: 34, Criteria1: "Y", Operator: Excel.XlAutoFilterOperator.xlFilterValues, Criteria2: Type.Missing, VisibleDropDown: true); // 34 :LOA denied
                    sourceRange.AutoFilter(Field: 35, Criteria1: "Y", Operator: Excel.XlAutoFilterOperator.xlFilterValues, Criteria2: Type.Missing, VisibleDropDown: true); //35 :Prior feed file match
                    sourceRange.AutoFilter(Field: 36, Criteria1: "N", Operator: Excel.XlAutoFilterOperator.xlFilterValues, Criteria2: Type.Missing, VisibleDropDown: true); //36 :Current File Match


                    /*STep 6*/
                    Excel.Range usedRange = wSheet.UsedRange;
                    int rows = usedRange.Rows.Count;
                    Excel.Range filteredRange = wSheet.Range[wSheet.Cells[2, 1], wSheet.Cells[usedRange.Rows.Count, 30]];
                    try
                    {
                        m_wsSheetLoopingRecord.Range[m_wsSheetLoopingRecord.Cells[2, 1], m_wsSheetLoopingRecord.Cells[m_wsSheetLoopingRecord.Rows.Count, m_wsSheetLoopingRecord.Columns.Count]].ClearContents();
                        filteredRange.SpecialCells(Excel.XlCellType.xlCellTypeVisible, Type.Missing);                      

                        Excel.Range destRange = m_wsSheetLoopingRecord.Range[m_wsSheetLoopingRecord.Cells[2, 1], m_wsSheetLoopingRecord.Cells[2, 30]]; //
                        filteredRange.Copy(destRange);
                        m_wsSheetLoopingRecord.Range["E2", "E" + m_wsSheetLoopingRecord.Rows.Count].NumberFormat = "############";
                        m_wsSheetLoopingRecord.Columns["AD"].WrapText = true;
                    }
                    catch (Exception)
                    {
                        //if (ex.Message.ToString().ToLower().Equals("no cells were found."))
                        //    MessageBox.Show("No Records found in BOA Complete File Sheet after filtering");
                        //else
                        //    MessageBox.Show(ex.Message);
                    }

                    string LookUpFormula = "= MAX(Table25[Feed Run Date])";
                    Excel.Range rngLookUp = m_wsSheetLookUp.Range["B1"];
                    rngLookUp.Formula = LookUpFormula;

                    /*********************************/
                    ShowAllData(wSheet);

                    sourceRange.AutoFilter(Field: 1, Criteria1: headerDate, Operator: Excel.XlAutoFilterOperator.xlFilterValues, Criteria2: Type.Missing, VisibleDropDown: true); //1 :Feed Run Date
                    sourceRange.AutoFilter(Field: 34, Criteria1: "Y", Operator: Excel.XlAutoFilterOperator.xlFilterValues, Criteria2: Type.Missing, VisibleDropDown: true); // 34 :LOA denied
                    sourceRange.AutoFilter(Field: 35, Criteria1: "Y", Operator: Excel.XlAutoFilterOperator.xlFilterValues, Criteria2: Type.Missing, VisibleDropDown: true); //35 :Prior feed file match


                    /************************************/

                    progressBar1.Value = progressBar1.Maximum;

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message.ToString());
                }
                finally
                {
                    closeExcel(excelWorkBook, fileTarget, excelApp);
                    //if (File.Exists(fileTarget))
                    //{
                    //    File.Copy(fileTarget, fileTemplate, true);
                    //}
                    savefileAction(fileTarget);
                }

                //   WriteToTextFile(Reflection.MethodBase.GetCurrentMethod().Name + " Ended ");
            }
            else
            {
                MessageBox.Show("There is no template file named " + Path.GetFileNameWithoutExtension(fileTemplate) + Path.GetExtension(fileTemplate) + " Present in " + Path.GetFullPath(fileTemplate) + " location.");
            }
        }

        //Create Create_BOA_HR_Leave_Feed_XX
        private void Create_BOA_HR_Leave_Feed_XX(string fileTemplate, string fileTarget, DataTable table)
        {
            excelApp = new Excel.Application();
            if (File.Exists(fileTemplate))
            {
                File.Delete(fileTarget);
                if (!File.Exists(fileTarget))
                {
                    File.Copy(fileTemplate, fileTarget, true);
                }

                Excel.Workbook xlWorkBook = excelApp.Workbooks.Open(fileTarget, 0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                Excel.Worksheet firstWorkSheet = xlWorkBook.Worksheets["Consolidated - Sorted"];
                Excel.Worksheet secondWorkSheet = xlWorkBook.Worksheets["Today's Review List"];
                Excel.Worksheet thirdWorkSheet = xlWorkBook.Worksheets["Today's EE List"];

                firstWorkSheet.Activate();
                if (firstWorkSheet.FilterMode)
                {
                    firstWorkSheet.AutoFilterMode = false;
                    firstWorkSheet.ShowAllData();
                }

                DateTime feedDate = Convert.ToDateTime(dtPickerCurrentDateSecondFeed.Text);

                int preRowValue = firstWorkSheet.Cells.Find("*", Reflection.Missing.Value, Reflection.Missing.Value, Reflection.Missing.Value, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious, false, Reflection.Missing.Value, Reflection.Missing.Value).Row;

                StringBuilder formulaStrBuilder = new StringBuilder();

                try
                {
                    progressBar1.Visible = true;
                    //*********
                    dynamic dataArray;
                    Excel.Range rng;
                    int maxRowValue = default(int);
                    for (int row = preRowValue; row < table.Rows.Count + preRowValue; row++)
                    {
                        dataArray = table.Rows[row - preRowValue].ItemArray.Select(x => x).ToArray();

                        rng = firstWorkSheet.get_Range("A" + (row + 1).ToString(), "AC" + (row + 1).ToString());
                        rng.Value = dataArray;

                        ////=IF(LEFT(I1,8)="US - LOA","Closed or Denied",IF(RIGHT(I41644,6)="Unpaid","Unpaid","Paid"))
                        formulaStrBuilder.Clear();
                        formulaStrBuilder.Append("= IF(LEFT(I" + (row + 1).ToString() + ",8)=" + "\"US - LOA\"" + "," + "\"Closed or Denied\"" + "," +
                                                    "IF(RIGHT(I" + (row + 1).ToString() + ",6)=" + "\"Unpaid\"" + "," + "\"Unpaid\"" + "," + "\"Paid\"" + "))");
                        rng = firstWorkSheet.Range["AF" + (row + 1).ToString()];
                        rng.Formula = formulaStrBuilder.ToString();

                        ////= VLOOKUP(B1, Lookup!A:B, 2, FALSE)
                        formulaStrBuilder.Clear();
                        formulaStrBuilder.Append("= VLOOKUP(B" + (row + 1).ToString() + ",Lookup!A:B, 2, FALSE");
                        rng = firstWorkSheet.Range["AG" + (row + 1).ToString()];
                        rng.Formula = formulaStrBuilder.ToString();

                        ProgressBarCalculation(row);
                        maxRowValue = row;
                    }
                    //**********

                    firstWorkSheet.Range["AD" + (preRowValue + 1) + ":AD" + (maxRowValue + 1)].Value = feedDate.ToString();
                    firstWorkSheet.Columns["I"].WrapText = true;

                    firstWorkSheet.Range["AD" + (preRowValue + 1).ToString(), "AD" + (maxRowValue + 1).ToString()].NumberFormat = "mmm-dd, yyyy (ddd)";
                    firstWorkSheet.Range[firstWorkSheet.Cells[maxRowValue + 2, 1],
                    firstWorkSheet.Cells[firstWorkSheet.Rows.Count, firstWorkSheet.Columns.Count]].ClearContents();

                    string dateValue = firstWorkSheet.UsedRange.Cells[maxRowValue + 1, 30].Text.ToString();


                    //***Clearing data from "Today's EE List" Sheet**//
                    Excel.Range firstcellEE = thirdWorkSheet.Range["A2"];
                    Excel.Range lastcellEE = firstcellEE.End[Excel.XlDirection.xlDown];
                    Excel.Range clearRangeEE = thirdWorkSheet.Range[firstcellEE, lastcellEE];
                    clearRangeEE.ClearContents();


                    //***Clearing filter from "Today's Review List" Sheet***//
                    secondWorkSheet.Activate();
                    if (secondWorkSheet.FilterMode)
                    {
                        secondWorkSheet.AutoFilterMode = false;
                        secondWorkSheet.ShowAllData();
                    }


                    //***Clearing data from "Today's Review List" Sheet***//
                    Excel.Range firstcellReview = secondWorkSheet.Range["A2"];
                    Excel.Range lastcellReview = firstcellReview.End[Excel.XlDirection.xlDown].End[Excel.XlDirection.xlToRight];
                    Excel.Range clearRangeReview = secondWorkSheet.Range[firstcellReview, lastcellReview];
                    clearRangeReview.ClearContents();


                    //***Copying data to "Today's Review List" Sheet***//
                    Excel.Range insertedDataRange = firstWorkSheet.Range[firstWorkSheet.Cells[preRowValue + 1, 1], firstWorkSheet.Cells[maxRowValue + 1, 29]];
                    Excel.Range destinationDataRange = secondWorkSheet.Range["B2", "AD2"];
                    insertedDataRange.Copy(destinationDataRange);
                    int ReviewListRowValueNow = maxRowValue - preRowValue + 2;


                    //= IF(COUNTIFS('Today''s EE List'!A:A, D1) > 0, "Y", "N")
                    formulaStrBuilder.Clear();
                    Excel.Range rngSecond;
                    for (int i = 2; i <= ReviewListRowValueNow; i++)
                    {
                        formulaStrBuilder.Clear();
                        formulaStrBuilder.Append("=IF(COUNTIFS('Today''s EE List'!A:A, D" + i.ToString() + ") > 0, \"Y\", \"N\")");
                        rngSecond = secondWorkSheet.Range["A" + i.ToString()];
                        rngSecond.Formula = formulaStrBuilder.ToString();
                    }
                    secondWorkSheet.Range[secondWorkSheet.Cells[ReviewListRowValueNow + 1, 1],
                    secondWorkSheet.Cells[secondWorkSheet.Rows.Count, secondWorkSheet.Columns.Count]].ClearContents();


                    //***Sorting data in "Consolidated - Sorted" Sheet***//
                    Excel.Range allDataRange = firstWorkSheet.Range["A2", "AH" + (maxRowValue + 1).ToString()];
                    allDataRange.Sort(allDataRange.Columns[3], Excel.XlSortOrder.xlAscending,
                                        allDataRange.Columns[30], Type.Missing, Excel.XlSortOrder.xlAscending,
                                        allDataRange.Columns[1], Excel.XlSortOrder.xlDescending,
                                        Excel.XlYesNoGuess.xlGuess, Type.Missing, Type.Missing,
                                        Excel.XlSortOrientation.xlSortColumns, Excel.XlSortMethod.xlPinYin,
                                        Excel.XlSortDataOption.xlSortNormal, Excel.XlSortDataOption.xlSortNormal,
                                        Excel.XlSortDataOption.xlSortNormal);


                    //***Applying filter in "Consolidated - Sorted" Sheet***//
                    firstWorkSheet.Activate();
                    if (!firstWorkSheet.FilterMode)
                    {
                        Excel.Range sourceRange = firstWorkSheet.Range["A1", "A1"];
                        sourceRange.EntireRow.Activate();
                        sourceRange.Select();
                        sourceRange.AutoFilter(Field: 30, Criteria1: dateValue, Operator: Excel.XlAutoFilterOperator.xlFilterValues, Criteria2: Type.Missing, VisibleDropDown: true); //1 :File Date
                    }


                    //***Applying filter in "Today's Review List" Sheet***//
                    secondWorkSheet.Activate();
                    string[] FilterList = new string[] { "US - Medical - Unpaid", "US - Statutory Leave - Unpaid", "US - LOA Closed" };
                    if (!secondWorkSheet.FilterMode)
                    {
                        Excel.Range sourceRange = secondWorkSheet.Range["A1", "A1"];
                        sourceRange.EntireRow.Activate();
                        sourceRange.Select();
                        sourceRange.AutoFilter(10, FilterList.ToArray(), Excel.XlAutoFilterOperator.xlFilterValues, Type.Missing, true);
                    }


                    //***Copying the filtered EEIDs to a "Today's EE List" sheet without duplicates***//
                    Excel.Range secondSheetRange = secondWorkSheet.Range[secondWorkSheet.Cells[2, 4], secondWorkSheet.Cells[secondWorkSheet.UsedRange.Rows.Count, 4]];
                    Excel.Range filteredRange = secondSheetRange.SpecialCells(Excel.XlCellType.xlCellTypeVisible, Type.Missing);
                    Excel.Range destinationRange = thirdWorkSheet.Range["A2"];
                    filteredRange.Copy(destinationRange);
                    int totalrows = thirdWorkSheet.Cells.Find("*", Reflection.Missing.Value, Reflection.Missing.Value, Reflection.Missing.Value, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious, false, Reflection.Missing.Value, Reflection.Missing.Value).Row;
                    Excel.Range thirdSheetRange = thirdWorkSheet.Range["A2", "A" + totalrows.ToString()];
                    thirdSheetRange.RemoveDuplicates(1, Excel.XlYesNoGuess.xlNo);
                    int nowRowThirdSheet = thirdWorkSheet.Cells.Find("*", Reflection.Missing.Value, Reflection.Missing.Value, Reflection.Missing.Value, Excel.XlSearchOrder.xlByRows, Excel.XlSearchDirection.xlPrevious, false, Reflection.Missing.Value, Reflection.Missing.Value).Row;
                    Excel.Range lastCellThirdSheet = thirdWorkSheet.Cells[nowRowThirdSheet + 1, 1];
                    lastCellThirdSheet.Delete();


                    //***Undoing filter in "Today's Review List" Sheet***//
                    secondWorkSheet.ShowAllData();


                    //***Applying filter on Column A of "Today's Review List" Sheet***//
                    secondWorkSheet.Activate();
                    if (!secondWorkSheet.FilterMode)
                    {
                        Excel.Range sourceRange = secondWorkSheet.Range["A1", "A1"];
                        sourceRange.EntireRow.Activate();
                        sourceRange.Select();
                        sourceRange.AutoFilter(1, "Y", Excel.XlAutoFilterOperator.xlFilterValues, Type.Missing, true);
                    }

                    progressBar1.Value = progressBar1.Maximum;

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message.ToString());
                }
                finally
                {
                    closeExcel(xlWorkBook, fileTarget, excelApp);
                    //if (File.Exists(fileTarget))
                    //{
                    //    File.Copy(fileTarget, fileTemplate, true);
                    //}
                    savefileAction(fileTarget);
                }
            }
            else
            {
                MessageBox.Show("There is no template file named " + Path.GetFileNameWithoutExtension(fileTemplate) + Path.GetExtension(fileTemplate) + " Present in " + Path.GetFullPath(fileTemplate) + " location.");
            }
        }

        //ProgressBar currentrow dividend Calculation
        private void ProgressDividend(int totalrow)
        {
            if (totalrow % 1000 == 0)
            {
                progressCount = (totalrow / 1000) * 10;
            }
            else
            {
                progressCount = ((totalrow / 1000) + 1) * 10;
            }
        }

        //ProgressBar Calculation
        private void ProgressBarCalculation(int currentRow)
        {
            if (currentRow % progressCount == 0)
            {
                if (progressBar1.Value != progressBar1.Maximum)
                {
                    progressBar1.Value += 1;
                }
                int percent = (int)((progressBar1.Value - progressBar1.Minimum) /
                        (double)(progressBar1.Maximum - progressBar1.Minimum) * 100);
                using (Graphics gr = progressBar1.CreateGraphics())
                {
                    gr.DrawString(percent.ToString() + "%",
                            SystemFonts.DefaultFont,
                            Brushes.Black,
                            new PointF(progressBar1.Width / 2 - (gr.MeasureString(percent.ToString() + "%",
                                SystemFonts.DefaultFont).Width / 2.0F),
                            progressBar1.Height / 2 - (gr.MeasureString(percent.ToString() + "%",
                                SystemFonts.DefaultFont).Height / 2.0F)));
                }
            }
        }

        //Closing Excel Application
        private void closeExcel(Excel.Workbook myExcelWorkbook, string excelFilePath, Excel.Application excelApp)
        {
            try
            {
                excelApp.DisplayAlerts = false;
                myExcelWorkbook.SaveAs(excelFilePath, Reflection.Missing.Value, Reflection.Missing.Value, Reflection.Missing.Value, Reflection.Missing.Value, Reflection.Missing.Value,
               Excel.XlSaveAsAccessMode.xlNoChange, Reflection.Missing.Value, Reflection.Missing.Value, Reflection.Missing.Value, Reflection.Missing.Value, Reflection.Missing.Value);


                myExcelWorkbook.Close(true, excelFilePath, Reflection.Missing.Value);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error Occured while Saving the Temporary reports" + ex.Message.ToString());
            }
            finally
            {
                if (excelApp != null)
                {
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelApp);
                    StopExistingExcelProcess();
                }

                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

        }

        //Exporting Generated Reports
        private void savefileAction(string fileTarget)
        {
            lblCurrentFile.Text = "Report has been Processed";
            if (MessageBox.Show("Do you want to export the Report to the location of your choice?",
                "Please Choose an Option", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SaveFileDialog svFlDialog = new SaveFileDialog();
                svFlDialog.Title = "Save File";
                svFlDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
                svFlDialog.FileName = Path.GetFileName(fileTarget);
                svFlDialog.DefaultExt = Path.GetExtension(fileTarget);
                if (svFlDialog.ShowDialog() == DialogResult.OK)
                {
                    string newFileTarget = svFlDialog.FileName;

                    File.Delete(newFileTarget);
                    if (!File.Exists(newFileTarget))
                    {
                        File.Move(fileTarget, newFileTarget);
                        progressBar1.Value = progressBar1.Minimum;
                        progressBar1.Visible = false;
                    }
                    lblCurrentFile.Text = "Press OK to Continue";
                    MessageBox.Show("Report has been generated successfully");
                }
            }
            progressBar1.Value = progressBar1.Minimum;
            progressBar1.Visible = false;
        }

        private void WriteToTextFile(string message)
        {
            //Create Log File
            logFilePath = Environment.CurrentDirectory.Replace("~", string.Empty) + @"\logFile.txt";
            if (!File.Exists(logFilePath))
            {
                File.Create(logFilePath);

            }

            //FileStream file = new FileStream(Environment.CurrentDirectory.Replace("~", string.Empty) + @"\logFile.txt", FileMode.Append, FileAccess.ReadWrite);
            using (FileStream fs = new FileStream(logFilePath, FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(message + DateTime.Now.ToString());
            }

        }

        //Disabling Excel Filter to show all data
        private void ShowAllData(Excel.Worksheet worksheet)
        {
            worksheet.Activate();
            Excel.Range sourceRange = worksheet.Range["A1", "A1"];
            sourceRange.EntireRow.Activate();
            sourceRange.Select();
            if (worksheet.FilterMode)
                worksheet.ShowAllData();
        }

        //Stop Existing Process for process related issue
        private void StopExistingExcelProcess()
        {
            var processes = from p in Process.GetProcessesByName("EXCEL")
                            select p;

            foreach (var process in processes)
            {
                process.Kill();
            }
        }


    }
}

