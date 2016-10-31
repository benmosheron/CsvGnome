using CsvGnome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeCli.Processor
{
    public static class Factory
    {
        public const string NoFileProvidedMessage = "No input file provided.";

        private static Func<string, bool> IsFileFlag = a => (a == "--file" || a == "-f");

        public static IProcessor Get(IReporter reporter, string[] commandArgs)
        {
            // Is an input file specified?
            if(commandArgs.Any(IsFileFlag))
            {
                // There should only be one
                // The following arg is the input file path and should not be null or empty

            }

            return new FromCommand();
        }
    }
}
