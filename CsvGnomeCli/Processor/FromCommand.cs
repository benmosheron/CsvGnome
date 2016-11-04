using System;
using System.Collections.Generic;
using CsvGnome;
using System.Linq;

namespace CsvGnomeCli.Processor
{
    public class FromCommand : IProcessor
    {
        string[] Commands { get; }
        public FromCommand(string[] interpretArgs)
        {
            Commands = interpretArgs;
        }

        public List<string> Process(IReporter reporter)
        {
            return Commands.ToList();
        }
    }
}
