using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Fields
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
        /// The command used to create the field.
        /// </summary>
        string Command { get; }

        /// <summary>
        /// Summary of the field to display in the console.
        /// </summary>
        List<IMessage> Summary { get; }

        /// <summary>
        /// Get the value to be written on the ith line.
        /// </summary>
        /// <param name="i">Line number (zero based, ignoring the columns line).</param>
        /// <returns></returns>
        string GetValue(long i);
    }
}
