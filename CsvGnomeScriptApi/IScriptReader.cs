using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeScriptApi
{
    /// <summary>
    /// Reads script files.
    /// </summary>
    public interface IScriptReader
    {
        /// <summary>
        /// The extension of files which are readable by this reader.
        /// </summary>
        string Extension { get; }

        /// <summary>
        /// Read the provided script files and extract all valid functions.
        /// </summary>
        IScriptFunctions Read(string path);
    }
}
