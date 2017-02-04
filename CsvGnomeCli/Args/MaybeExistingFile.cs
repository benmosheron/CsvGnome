using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeCli.Args
{
    /// <summary>
    /// A command line flag that must be followed by a path to a file. If the file may or may not yet exist.
    /// </summary>
    /// <example>
    /// > CsvGnome --output "C:\path\to\output"
    /// </example>
    public class MaybeExistingFile : ArgBase
    {
        public MaybeExistingFile(params string[] values) : base(values) { }

        /// <summary>
        /// Validate that the MaybeExistingFile arg (e.g. --file) is followed by a path.
        /// </summary>
        /// <param name="index">Index of the MaybeExistingFile argument in the input args.</param>
        /// <param name="args">The provided command line args.</param>
        /// <returns>True if MaybeExistingFile is followed by a path.</returns>
        public override bool Validate(int index, string[] args, out string failReason)
        {
            // Error if the index is out of range
            if (index >= args.Length) throw new IndexOutOfRangeException();

            string arg = args[index];

            // Check this isn't the last
            if (index + 1 >= args.Length) return Fail($"No file path provided after [{arg}] argument.", out failReason);

            failReason = String.Empty;

            return true;
        }

        private bool Fail(string message, out string failReason)
        {
            failReason = message;
            return false;
        }
    }
}
