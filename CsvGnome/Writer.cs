using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    public class Writer
    {
        /// <summary>
        /// Write CSV file from the input fields.
        /// </summary>
        /// <param name="fields">Fields describing the csv to write.</param>
        /// <param name="pathAndFile">File to write to (will be overwritten).</param>
        /// <param name="n">Number of data lines to write (not including column line).</param>
        public void WriteToFile(Reporter reporter, List<IField> fields, string pathAndFile, int n)
        {
            // Reset the date/time to write in [date] components
            Program.UpdateTime();
            try {
                using (StreamWriter sw = new StreamWriter(pathAndFile))
                {
                    sw.WriteLine(GetFirstLine(fields));

                    for (int i = 0; i < n; i++)
                    {
                        sw.WriteLine(GetLine(fields, i));
                    }
                }
                reporter.AddMessage(new Message($"{n} lines written.", ConsoleColor.Green));
            }
            catch (DirectoryNotFoundException)
            {
                reporter.AddMessage(new Message("I couldn't find the directory:"));
                reporter.AddMessage(new Message(pathAndFile));
            }
            catch (FileNotFoundException)
            {
                reporter.AddMessage(new Message("I couldn't find the file:"));
                reporter.AddMessage(new Message(pathAndFile));
            }
            catch (Exception ex)
            {
                reporter.AddMessage(new Message("I don't know why that didn't work:"));
                reporter.AddMessage(new Message(ex.Message));
            }
        }

        private string GetFirstLine(List<IField> fields)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(fields.Select(f => f.Name).Aggregate((t,a) => $"{t},{a}"));

            return sb.ToString();
        }

        private string GetLine(List<IField> fields, int i)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(fields.Select(f => f.GetValue(i)).Aggregate((t, a) => $"{t},{a}"));

            return sb.ToString();
        }
    }
}
