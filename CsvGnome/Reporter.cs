using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    /// <summary>
    /// Reports data to the console.
    /// </summary>
    class Reporter
    {
        public void Report(List<IField> fields, int N, string pathAndFile)
        {
            Console.Clear();

            var names = fields.Select(f => f.Name);
            var values = fields.Select(f => f.Summary).ToList();

            int maxLength = names.Max(f => f.Length);
            var namesForDisplay = names.Select(n => RightPad(n, maxLength)).ToList();


            Console.WriteLine($"Writing {N} rows to {pathAndFile}");
            for(int i=0; i<fields.Count; i++)
            {
                Console.WriteLine($"{namesForDisplay[i]}: {values[i]}");
            }
        }

        private string RightPad(string s, int endLength)
        {
            StringBuilder sb = new StringBuilder(s);
            sb.Append(' ', endLength - sb.Length);
            return sb.ToString();
        }
    }
}
