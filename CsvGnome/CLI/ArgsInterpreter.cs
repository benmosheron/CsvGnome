using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.CLI
{
    public class ArgsInterpreter
    {
        public static Option Do(string[] args, Reporter reporter)
        {
            if (args == null || args.Length == 0) return Option.RunStandalone;

            // For interpret mode, the first argument must strictly be either "--interpret" or "-i"
            string first = args[0];
            if (first == "--interpret" || first == "-i") return Option.Interpret;

            // For file mode, "--file" or "-f"
            if (first == "--file" || first == "-f") return Option.File;

            reporter.AddMessage(new Message($"Unexpected command line arguments were provided, starting with [{first}]. The first argument (if any are provided) should be one of either: --file, -f, --interpret or -i"));
            return Option.RunStandalone;
        }
    }
}
