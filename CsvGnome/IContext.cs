using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    /// <summary>
    /// Provides contextual information such as N, the number of rows that will be written.
    /// </summary>
    public interface IContext
    {
        /// <summary>
        /// The number of rows of data to write (doesn't include the columns row).
        /// </summary>
        int N { get; set; }
    }
}
