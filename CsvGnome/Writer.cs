using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    class Writer
    {
        public void WriteToFile(List<IField> fields, string pathAndFile, int n)
        {
            using (StreamWriter sw = new StreamWriter(pathAndFile))
            {
                sw.WriteLine(GetFirstLine(fields));

                for (int i = 0; i < n; i++)
                {
                    sw.WriteLine(GetLine(fields, i));
                }
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
