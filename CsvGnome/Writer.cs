using CsvGnome.Fields;
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
        Configuration.IProvider ConfigurationProvider;

        public Writer(Configuration.IProvider configurationProvider)
        {
            ConfigurationProvider = configurationProvider;
        }

        /// <summary>
        /// Write CSV file from the input fields.
        /// </summary>
        /// <param name="fields">Fields describing the csv to write.</param>
        /// <param name="pathAndFile">File to write to (will be overwritten).</param>
        /// <param name="n">Number of data lines to write (not including column line).</param>
        public void WriteToFile(Date.IProvider dateProvider, Reporter reporter, List<IField> fields, PaddedFieldFactory paddedFieldFactory, string pathAndFile, long n)
        {
            // Reset the date/time to write in [date] components
            dateProvider.UpdateNow();

            if (ConfigurationProvider.PadOutput)
            {
                // Replace IFields with IPaddedFields
                fields = fields.Select(f => paddedFieldFactory.GetPaddedField(f) as IField).ToList();
                foreach (var field in fields)
                {
                    (field as IPaddedField).CalculateMaxLength(n);
                }
            }

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

        /// <summary>
        /// Get the first line of column names.
        /// </summary>
        /// <remarks>
        /// Remember that padded fields must use GetPaddedName() instead of name!
        /// </remarks>
        private string GetFirstLine(List<IField> fields)
        {
            StringBuilder sb = new StringBuilder();

            Func<IField, string> GetName = f =>
            {
                if (f is IPaddedField) return (f as IPaddedField).GetPaddedName();
                else return f.Name;
            };

            sb.Append(fields.Select(GetName).Aggregate((t,a) => $"{t},{a}"));

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
