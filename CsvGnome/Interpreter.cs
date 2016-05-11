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

            // "//" indicates a comment (in a GnomeFile).
            if (input.TrimStart().StartsWith("//")) return Action.Continue;

            // "run" writes file and exits
            if (input.ToLower() == "run") return Action.Run;

            // "write" writes file and continues
            if (input.ToLower() == "write") return Action.Write;

            // "combine x y z" initialises combinatorial fields
            if (input.StartsWith("combine ")) CombineFields(input);

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

        private void CombineFields(string input)
        {
            string fieldsToCombineMaybe = input.Remove(0, "combine ".Length).Trim();
            // Remove whitespace and duplicates
            List<string> fieldsMaybe = fieldsToCombineMaybe.Split(' ').Select(fm => fm.Trim()).Distinct().ToList();

            // The last element may be a set name e.g. "[chickens]"
            string last = fieldsMaybe.Last();
            string setName = null;
            if(last.StartsWith("[") && last.EndsWith("]"))
            {
                setName = last.Substring(1, last.Length - 2);
                fieldsMaybe.Remove(last);
            }

            if(fieldsMaybe.All(fm => FieldBrain.FieldValidForCombine(fm)))
            {
                // woohoo! get the ICombinableFields for the brain to eat
                var fieldsDefinitely = fieldsMaybe.Select(fm => FieldBrain.CombinableFields.First(f => f.Name == fm)).ToList();
                FieldBrain.CombineFields(fieldsDefinitely, setName);
            }
            else
            {
                // Do nothing, maybe say something?
            }
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
                // MinMax Field 
                // E.g. "0 10 2"
                int[] minMaxInc = ParseMinMax(instruction);
                if (minMaxInc != null)
                {
                    FieldBrain.AddOrUpdateMinMaxField(name, minMaxInc[0], minMaxInc[1], minMaxInc[2]);
                }
                else
                {
                    // Constant field
                    string constantValue = instruction;
                    FieldBrain.AddOrUpdateConstantField(name, constantValue);
                }
            }
        }

        /// <summary>
        /// E.g. "0 10 2"
        /// </summary>
        private int[] ParseMinMax(string instruction)
        {
            string trimmedInstruction = instruction.Trim();
            string[] tokens = trimmedInstruction.Split(' ');
            if (tokens.Length != 3) return null;
            int[] minMaxInc = new int[3];
            if (!int.TryParse(tokens[0], out minMaxInc[0])) return null;
            if (!int.TryParse(tokens[1], out minMaxInc[1])) return null;
            if (!int.TryParse(tokens[2], out minMaxInc[2])) return null;
            return minMaxInc;
        }
    }
}
