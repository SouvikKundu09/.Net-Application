using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BLL.Shared
{
    public class Export
    {
        public string ExportToSQL(string content)
        {
            SaveFileDialog dlgSave = new SaveFileDialog();
            string fileName = string.Empty;
            try
            {
                dlgSave.Filter = "SQL Script|*.sql";
                DialogResult res = dlgSave.ShowDialog();
                if (res == DialogResult.OK && !dlgSave.FileName.Equals(string.Empty))
                {
                    fileName = dlgSave.FileName;
                    File.WriteAllText(fileName, content);
                }
                else if (res == DialogResult.Cancel)
                {
                    fileName = Global.MsgExportCanceled;
                }
            }
            catch (Exception ex)
            {
                fileName = string.Empty;
            }
            return fileName;
        }
    }
}
