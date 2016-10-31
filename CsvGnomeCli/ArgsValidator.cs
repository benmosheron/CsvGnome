using CsvGnome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeCli
{
    /// <summary>
    /// Check that the input args are valid.
    /// </summary>
    public class ArgsValidator
    {
        public const string NoArgsProvided = "No command line arguments were provided.";
        public const string DuplicateArgsProvided = "The following command line arguments were provided more than once:";
        public const string DuplicateArgValuesProvided = "The following command line arguments were provided, but they do the same thing:";
        public const string NoInputFileProvided = "The --file (-f) argument was specified, but no file path followed it.";
        public const string NoOutputFileProvided = "The --output (-o) argument was specified, but no file path followed it.";

        private IReporter Reporter;

        public ArgsValidator(IReporter reporter)
        {
            Reporter = reporter;
        }

        public bool Validate(string[] args)
        {
            return false;
        }
    }
}
