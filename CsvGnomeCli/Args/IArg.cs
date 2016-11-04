using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeCli.Args
{
    /// <summary>
    /// Represents available command line arguments
    /// </summary>
    public interface IArg
    {
        /// <summary>
        /// Allowable values for this flag. E.g. "--file" and "-f".
        /// </summary>
        HashSet<String> Values { get; }

        /// <summary>
        /// True if the input arg represents this IArg
        /// </summary>
        bool Is(string arg);

        /// <summary>
        /// Check whether or not any extra conditions for this argument are satisfied.
        /// E.g. --file must be followed by a path to a file that exists.
        /// </summary>
        bool Validate(int index, string[] args, out string failReason);
    }
}
