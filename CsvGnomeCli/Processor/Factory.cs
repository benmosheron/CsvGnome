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
        public static IProcessor Get(IReporter reporter, string[] commandArgs)
        {
            string filePath;
            // Is an input file specified?
            if(Args.Args.TryGetInputFilePath(commandArgs, out filePath))
            {
                return new FromFile(filePath);
            }

            // Is an interpret flag specified?
            int interpretArgsAfter = Args.Args.TryGetInterpretIndex(commandArgs);
            return new FromCommand(commandArgs.Skip(interpretArgsAfter + 1).ToArray());
        }
    }
}
