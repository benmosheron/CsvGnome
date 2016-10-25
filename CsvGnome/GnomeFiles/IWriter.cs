using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.GnomeFiles
{
    /// <summary>
    /// Writes gnomefiles.
    /// </summary>
    public interface IWriter
    {
        /// <summary>
        /// Writes a gnomefile to a file at the provided path.
        /// </summary>
        /// <param name="path"></param>
        void Save(string path);
    }
}
