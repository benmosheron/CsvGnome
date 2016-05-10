using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    /// <summary>
    /// Interprets input from the console.
    /// </summary>
    class Interpreter
    {
        public Action Interpret(string input, FieldBrain fieldBrain)
        {
            // Empty string exits
            if (input == String.Empty) return Action.Exit;

            // "run" writes file and exits
            if (input.ToLower() == "run") return Action.Run;

            // "write" writes file
            if (input.ToLower() == "write") return Action.Write;

            // Int sets N
            int n;
            if (int.TryParse(input, out n)) Program.SetN(n);

            // ":" Indicates a data update
            if (input.Contains(":"))
            {
               var tokens = input.Split(new char[] { ':' }, 2);
               fieldBrain.Update(tokens[0], tokens[1]);
            }

            return Action.Continue;
        }
    }
}
