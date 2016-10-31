﻿using CsvGnome;
using CsvGnomeCli.Processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeCli
{
    public class Program
    {
        // Possible flags:
        // Read from file: --file,      -f
        // Set output:     --output,    -o
        // Interpret:      --interpret, -i
        public static void Main(string[] args)
        {
            // CLI entry point
            IReporter reporter = new Reporter();
            IProcessor processor = Factory.Get(reporter, args);

            // If the processer is null, something went wrong with the input args.
            if (processor == null) return;

            // Do the work

        }
    }
}