using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.File
{
    /// <summary>
    /// Provides file location, name and extension.
    /// </summary>
    public interface IProvider
    {
        /// <summary>
        /// Path to the file. Including folder, name and extension.
        /// </summary>
        string Path { get; }
    }
}
