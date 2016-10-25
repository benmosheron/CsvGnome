using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.GnomeFiles
{
    /// <summary>
    /// Reads gnomefiles, but does not interpret them.
    /// </summary>
    public interface IReader
    {
        /// <summary>
        /// The last read file name.
        /// </summary>
        string LastLoadedFileName { get; }

        /// <summary>
        /// Read a gnome file and return the instructions.
        /// </summary>
        List<string> ReadGnomeFile(string path);
    }
}
