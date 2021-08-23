using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BLL.Shared
{
    public static class Decorator
    {
        # region Decorate Rich Text Box

        public static void DecorateSQL(ref RichTextBox rtxtScript)
        {
            int startIndex = 0;
            int endIndex = 0;
            string textInLine = string.Empty;
            ResetFormatting(ref rtxtScript);
            try
            {
                for (int i = 0; i < rtxtScript.Lines.Length; i++)
                {
                    textInLine = rtxtScript.Lines[i];
                    startIndex = rtxtScript.GetFirstCharIndexFromLine(i);
                    rtxtScript.Select(startIndex, textInLine.Length);
                    rtxtScript.SelectionColor = colorForLine(textInLine);
                }

                textInLine = string.Empty;
                for (int i = 0; i < rtxtScript.Lines.Length; i++)
                {
                    textInLine = rtxtScript.Lines[i];
                    rtxtScript.Select(rtxtScript.GetFirstCharIndexFromLine(i), textInLine.Length);
                    if (rtxtScript.SelectionColor == Color.Black)
                    {
                        var words = GetWords(textInLine);
                        foreach (string word in words)
                        {
                            startIndex = rtxtScript.GetFirstCharIndexFromLine(i) + textInLine.IndexOf(word);
                            rtxtScript.Select(startIndex, word.Length);
                            rtxtScript.SelectionColor = colorForWord(word);
                        }
                    }
                }

                textInLine = string.Empty;
                for (int i = 0; i < rtxtScript.Lines.Length; i++)
                {
                    textInLine = rtxtScript.Lines[i];
                    int noOfChar = NoOfCharInLine(textInLine, '\'');

                    if (noOfChar > 0)
                    {
                        for (int j = 1; j <= noOfChar; j += 2)
                        {
                            startIndex = rtxtScript.GetFirstCharIndexFromLine(i) + NthOccurence(textInLine, '\'', j);
                            endIndex = rtxtScript.GetFirstCharIndexFromLine(i) + NthOccurence(textInLine, '\'', j + 1);
                            rtxtScript.Select(startIndex, endIndex - startIndex);
                            rtxtScript.SelectionColor = Color.Red;
                        }
                    }
                }
                rtxtScript.Select(0, 0);
                rtxtScript.Focus();
            }
            catch (Exception ex) { }
        }

        public static void DecorateErrorMessage(ref RichTextBox rtxtScript)
        {
            ResetFormatting(ref rtxtScript);
            try
            {
                rtxtScript.SelectAll();
                rtxtScript.SelectionColor = Color.Red;
                rtxtScript.SelectionFont = new Font(rtxtScript.Font, FontStyle.Bold);
            }
            catch (Exception ex) { }
        }

        private static void ResetFormatting(ref RichTextBox rtxtScript)
        {
            try
            {
                rtxtScript.SelectAll();
                rtxtScript.SelectionColor = Color.Black;
                rtxtScript.SelectionFont = new Font(rtxtScript.Font, FontStyle.Regular);
            }
            catch (Exception ex) { }
        }

        private static string[] GetWords(string input)
        {
            MatchCollection matches = Regex.Matches(input, @"\b[\w']*\b");
            var words = matches.Cast<Match>().Where(w => !string.IsNullOrEmpty(w.Value)).Select(match => match.Value).ToArray();
            return words;
        }

        private static int NoOfCharInLine(string line, char chr)
        {
            return line.Count(l => l == chr);
        }

        private static int NthOccurence(string s, char t, int n)
        {
            return s.TakeWhile(c => (n -= (c == t ? 1 : 0)) > 0).Count();
        }

        private static Color colorForLine(string line)
        {
            if (line.Contains("--"))
                return Color.Green;

            return Color.Black;
        }

        private static Color colorForWord(string word)
        {
            if (
                word.ToUpper().Equals("DECLARE") ||
                word.ToUpper().Equals("SET") ||
                word.ToUpper().Equals("VARCHAR") ||
                word.ToUpper().Equals("NUMERIC") ||
                word.ToUpper().Equals("DATETIME") ||
                word.ToUpper().Equals("BEGIN") ||
                word.ToUpper().Equals("END") ||
                word.ToUpper().Equals("TRANSACTION") ||
                word.ToUpper().Equals("COMMIT") ||
                word.ToUpper().Equals("ROLLBACK") ||
                word.ToUpper().Equals("TRY") ||
                word.ToUpper().Equals("CATCH") ||
                word.ToUpper().Equals("IF") ||
                word.ToUpper().Equals("SELECT") ||
                word.ToUpper().Equals("FROM") ||
                word.ToUpper().Equals("WHERE") ||
                word.ToUpper().Equals("INSERT") ||
                word.ToUpper().Equals("INTO") ||
                word.ToUpper().Equals("VALUES") ||
                word.ToUpper().Equals("TOP") ||
                word.ToUpper().Equals("NOLOCK") ||
                word.ToUpper().Equals("PRINT")
               )
                return Color.Blue;
            else if (
                word.ToUpper().Equals("UPDATE") ||
                word.ToUpper().Equals("ERROR")
               )
                return Color.Magenta;
            else if (
                word.ToUpper().Equals("AND") ||
                word.ToUpper().Equals("NOT") ||
                word.ToUpper().Equals("EXISTS") ||
                word.ToUpper().Equals("NULL") ||
                word.ToUpper().Equals("IS")
               )
                return Color.DarkGray;

            return Color.Black;
        }

        # endregion
    }
}
