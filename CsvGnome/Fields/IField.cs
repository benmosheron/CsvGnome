using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    /// <summary>
    /// A field for which value will be generated and written to the csv.
    /// </summary>
    public interface IField
    {
        /// <summary>
        /// Name of the field (column name).
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Summary to display in the console.
        /// </summary>
        string Summary { get; }

        /// <summary>
        /// Get the value to be written on the ith line.
        /// </summary>
        /// <param name="i">Line number (line 0 is the columns, line 1 is the first data line)</param>
        /// <returns></returns>
        string GetValue(int i);
    }
}
