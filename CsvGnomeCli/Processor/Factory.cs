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
        public const string NoOutputProvidedMessage = "No output file provided.";

        public static IProcessor Get(IReporter reporter, string[] commandArgs)
        {
            reporter.AddMessage(new Message("heh"));
            return new FromCommand();
        }
    }
}
