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

        private readonly FieldBrain FieldBrain;

        public Interpreter(FieldBrain fieldBrain)
        {
            FieldBrain = fieldBrain;
        }

        public Action Interpret(string input)
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
               string name = tokens[0];
               string instruction = tokens[1];
               InterpretInstruction(name, instruction);
            }

            return Action.Continue;
        }

        private void InterpretInstruction(string name, string instruction)
        {
            // Look for a special instruction enclosed in square brackets
            if (instruction.Contains("[++]"))
            {
                // Incremental field
                // e.g. fieldName:baseval_[++] 3
                var tokens = instruction.Split(new string[] { "[++]" }, 2, StringSplitOptions.None);
                string baseVal = tokens[0];
                string startString = tokens[1].Trim();
                int start = 0;
                int.TryParse(startString, out start);
                FieldBrain.AddOrUpdateIncrementingField(name, baseVal, start);
            }
            else
            {
                // Constant field
                string constantValue = instruction;
                FieldBrain.AddOrUpdateConstantField(name, constantValue);
            }
        }
    }
}
