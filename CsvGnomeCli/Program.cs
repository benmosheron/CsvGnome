using CsvGnome;
using CsvGnomeCli.Processor;
using System;
using System.Collections.Generic;
using System.IO;
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

            // Validate command line args
            if (!new ArgsValidator(reporter).Validate(args)) return;

            // Get a processor, depending on whether a --file is specified.
            IProcessor processor = Factory.Get(reporter, args);

            // If the processer is null, something went wrong with the input args.
            if (processor == null) return;

            // Get the work to do
            List<string> commands = processor.Process(reporter);

            // Interpret the commands
            FieldBrain fieldBrain;
            Context context;
            CsvGnome.Date.IProvider dateProvider;
            CsvGnome.Configuration.IProvider configurationProvider;
            Interpreter interpreterNoIO = GetInterpreter(
                reporter,
                out fieldBrain,
                out context,
                out dateProvider, 
                out configurationProvider);
            commands.ForEach(interpreterNoIO.InterpretSilent);

            // Set the output file
            string outputFile;
            if (!Args.Args.TryGetOutputFilePath(args, out outputFile)) outputFile = Path.Combine(Directory.GetCurrentDirectory(), "Output.csv");
            context.SetOutputFile(outputFile);

            // Write the file
            Writer writer = new Writer(configurationProvider);
            writer.WriteToFile(
                dateProvider,
                reporter,
                fieldBrain.Fields,
                new CsvGnome.Fields.PaddedFieldFactory(),
                context.Path,
                context.N);
        }

        private static Interpreter GetInterpreter(
            IReporter reporter,
            out FieldBrain fieldBrain,
            out Context context,
            out CsvGnome.Date.IProvider dateProvider,
            out CsvGnome.Configuration.IProvider configurationProvider)
        {
            var combinatorialCache = new CsvGnome.Components.Combinatorial.Cache();
            var combinatorialDeleter = new CsvGnome.Components.Combinatorial.Deleter(combinatorialCache);
            var combinatorialFactory = new CsvGnome.Components.Combinatorial.Factory(combinatorialCache);
            fieldBrain = new FieldBrain(combinatorialFactory, combinatorialDeleter);

            context = new Context();
            dateProvider = new CsvGnome.Date.Provider();
            configurationProvider = new DefaultConfigurationProvider();

            return new Interpreter(
                fieldBrain: fieldBrain,
                reporter: reporter,
                scriptManager: null,
                context: context,
                dateProvider: dateProvider,
                configurationProvider: configurationProvider);
        }
    }
}
