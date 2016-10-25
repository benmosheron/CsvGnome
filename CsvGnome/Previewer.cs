using CsvGnome.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    /// <summary>
    /// Displays a preview of the CSV file that will be generated in the console window.
    /// </summary>
    public class Previewer
    {
        Configuration.IProvider ConfigurationProvider;

        public Previewer(Configuration.IProvider configurationProvider)
        {
            ConfigurationProvider = configurationProvider;
        }

        public void Preview(IReporter reporter, List<IField> fields, PaddedFieldFactory paddedFieldFactory, long n)
        {
            Console.Clear();

            if (ConfigurationProvider.PadOutput)
            {
                Console.WriteLine("Preparing preview...");
                // Replace IFields with IPaddedFields
                fields = fields.Select(f => paddedFieldFactory.GetPaddedField(f) as IField).ToList();
                foreach (var field in fields)
                {
                    (field as IPaddedField).CalculateMaxLength(n);
                }
            }

            // Write the preview
            int timesPPressed = 0;
            bool cont = true;
            cont = WritePreviewAfterNPPresses(fields, n, timesPPressed);

            // Preview another line if "p" is pressed
            while (Console.ReadKey(true).Key == ConsoleKey.P)
            {
                if (!cont) continue;
                cont = WritePreviewAfterNPPresses(fields, n, ++timesPPressed);
            }

            Console.Clear();
        }

        private bool WritePreviewAfterNPPresses(List<IField> fields, long n, int np)
        {
            long rowsToWrite = Math.Min(Console.WindowHeight - 4, n);
            bool pHasBeenPressed = np > 0;
            bool nextWillBeEnd = np + 1 + rowsToWrite > n;

            Console.Clear();

            // Always write the columns
            Console.WriteLine(GetFirstLine(fields));

            // If p has been pressed
            if (pHasBeenPressed)
            {
                Console.WriteLine("...");
                rowsToWrite--;
            }

            WriteLinesAToB(fields, np, np + rowsToWrite);

            // Either write "[End]" or instructions on continuing.
            
            if (nextWillBeEnd)
            {
                Console.WriteLine("[End]");
                return false;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Press [p] to continue preview.");
                Console.ResetColor();
            }

            return true;
        }

        private void WriteLinesAToB(List<IField> fields, long a, long b)
        {
            for (long i = a; i <= b; i++)
            {
                Console.WriteLine(GetLine(fields, i));
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

            sb.Append(fields.Select(GetName).Aggregate((t, a) => $"{t},{a}"));

            return sb.ToString();
        }

        private string GetLine(List<IField> fields, long i)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(fields.Select(f => f.GetValue(i)).Aggregate((t, a) => $"{t},{a}"));

            return sb.ToString();
        }
    }
}
