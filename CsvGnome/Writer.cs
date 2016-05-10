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
        public void WriteToFile(List<IField> fields, string pathAndFile)
        {
            using (StreamWriter sw = new StreamWriter(pathAndFile))
            {
                sw.WriteLine(GetFirstLine(fields));
            }
        }

        private string GetFirstLine(List<IField> fields)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(fields.Select(f => f.Name).Aggregate((t,a) => $"{t},{a}"));

            return sb.ToString();
        }
    }
}
