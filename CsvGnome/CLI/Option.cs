using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.CLI
{
    /// <summary>
    /// Three options are provided by the CLI.
    /// </summary>
    public enum Option
    {
        /// <summary>
        /// Run the standalone CsvGnome console.
        /// </summary>
        RunStandalone,

        /// <summary>
        /// A file path is expected to be provided, interpret the file silently and exit.
        /// </summary>
        File,

        /// <summary>
        /// Interpret a set of instructions supplied directly to the command line silently and exit.
        /// </summary>
        Interpret
    }
}
