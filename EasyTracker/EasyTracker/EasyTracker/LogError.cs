using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EasyTracker
{
    public class LogError
    {
        //Logging error in Notepad
        public static void ErrorLog(string sErrMsg)
        {
            string sPathName = Environment.CurrentDirectory.Replace("~", string.Empty) + @"\Log";
            Helper.CreateFolder(sPathName);
            File.Create(sPathName + "\\Log.txt").Close();
            StreamWriter sw = new StreamWriter(sPathName + "\\Log.txt", true);
            sw.WriteLine(String.Format("{0}==>{1}", DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss"), sErrMsg));
            sw.Flush();
            sw.Close();
        }
    }
}