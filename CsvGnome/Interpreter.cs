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
        public Action Interpret(string input, List<IField> fields)
        {
            // Empty string exits
            if (input == String.Empty) return Action.Exit;

            // "run" writes file and exits
            if (input.ToLower() == "run") return Action.Run;

            // "write" writes file
            if (input.ToLower() == "write") return Action.Write;

            // Int sets N
            int n;
            if (int.TryParse(input, out n)) Program.N = n;

            // ":" Indicates a data update
            if (input.Contains(":"))
            {
               var tokens = input.Split(new char[] { ':' }, 2);
               UpdateFields(tokens[0], tokens[1], fields);
            }

            return Action.Continue;
        }

        private void UpdateFields(string name, string value, List<IField> fields)
        {
            // Find a matching field
            IField match = fields.FirstOrDefault(f => f.Name == name);

            // Update field
            if (fields.Any(f => f.Name == name)) fields[fields.FindIndex((f) => f.Name == name)] = new ConstantField(name, value);
            // No match, create new field
            else
            {
                fields.Add(new ConstantField(name, value));
            }
        }
    }
}
