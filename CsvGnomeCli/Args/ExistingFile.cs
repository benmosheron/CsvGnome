using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeCli.Args
{
    /// <summary>
    /// A command line flag that should be followed by a path to an existing file.
    /// </summary>
    /// <example>
    /// > CsvGnome --file "C:\path\to\file"
    /// </example>
    public class ExistingFile : ArgBase
    {
        public ExistingFile(params string[] values) : base(values) { }

        /// <summary>
        /// Validate that the --file command is followed by a path to a file.
        /// </summary>
        /// <param name="index">Index of the --file argument in the input args.</param>
        /// <param name="args">The provided command line args.</param>
        /// <returns>True if --file is followed by a path.</returns>
        public override bool Validate(int index, string[] args, out string failReason)
        {
            // Error if the index is out of range
            if (index >= args.Length) throw new IndexOutOfRangeException();

            string arg = args[index];

            // Check this isn't the last
            if (index + 1 >= args.Length) return Fail($"No file path provided after [{arg}] argument.", out failReason);

            // Check that the following argument is a valid path
            string path = args[index + 1];
            if (!File.Exists(path)) return Fail($"Could not find the file [{path}] provided after [{arg}] argument.", out failReason);

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
