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
        private readonly Reporter Reporter;

        public Interpreter(FieldBrain fieldBrain, Reporter reporter)
        {
            FieldBrain = fieldBrain;
            Reporter = reporter;
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
            if (input.StartsWith("combine")) CombineFields(input);

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

        /// <summary>
        /// Attempt to perform a combine.
        /// </summary>
        /// <param name="input"></param>
        private void CombineFields(string input)
        {
            string fieldsToCombineMaybe = input.Remove(0, "combine".Length).Trim();
            // Remove whitespace and duplicates
            List<string> fieldsMaybe = fieldsToCombineMaybe.Split(' ').Select(fm => fm.Trim()).Distinct().ToList();

            // Check fields were provided
            if (!CheckCombineFieldsProvided(fieldsMaybe)) return;

            // The last element may be a set name e.g. "[chickens]"
            string last = fieldsMaybe.Last();
            string setName = null;
            if(last.StartsWith("[") && last.EndsWith("]"))
            {
                setName = last.Substring(1, last.Length - 2);
                fieldsMaybe.Remove(last);
            }

            // Check fields were provided (other than set name)
            if (!CheckCombineFieldsProvided(fieldsMaybe)) return;

            if (fieldsMaybe.All(fm => FieldBrain.FieldValidForCombine(fm)))
            {
                // woohoo! get the ICombinableFields for the brain to eat
                var fieldsDefinitely = fieldsMaybe.Select(fm => FieldBrain.CombinableFields.First(f => f.Name == fm)).ToList();

                // Actually combine the fields
                FieldBrain.CombineFields(fieldsDefinitely, setName);
            }
            else
            {
                ReportInvalidFields(fieldsMaybe);
            }
        }

        /// <summary>
        /// Attempt to update or create a field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="instruction"></param>
        private void InterpretInstruction(string name, string instruction)
        {
            // Try and create a component field
            // Break instruction into components
            IComponent[] components = GetComponents(instruction);

            // If there is only one component and it is text, it might be a MinMaxField
            if (components.Length == 1 && components[0] is TextComponent)
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
            else
            {
                // Create component field
                FieldBrain.AddOrUpdateComponentField(name, components);
            }
            
        }

        private IComponent[] GetComponents(string instruction)
        {
            List<IComponent> components = new List<IComponent>();

            ExtractComponents(instruction, components);

            return components.ToArray();
        }

        private void ExtractComponents(string instruction, List<IComponent> components)
        {
            int i = 0;
            string match = null;
            while(match == null && i < instruction.Length)
            {
                // try to match a component command to a substring starting at i
                match = Program.ComponentStrings.FirstOrDefault(C => instruction.Substring(i).StartsWith(C));
                if (match == null) i++;
            }
            // Either a match was found, or we didn't match a command
            // Add everything to the left as text
            if (i > 0) components.Add(ComponentFactory.Create(instruction.Substring(0, i)));

            if(match != null)
            {
                // Add the token
                components.Add(ComponentFactory.Create(match));

                // Recursively look for more matches
                ExtractComponents(instruction.Substring(i + match.Length), components);
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

        /// <summary>
        /// Check that > 0 non null or whitespace fields were provided.
        /// </summary>
        /// <param name="fieldsMaybe"></param>
        /// <returns></returns>
        private bool CheckCombineFieldsProvided(List<string> fieldsMaybe)
        {
            if (fieldsMaybe.Count == 0 || fieldsMaybe.All(fm => String.IsNullOrWhiteSpace(fm)))
            {
                Reporter.AddMessage(new Message("You must provide fields to combine. E.g:"));
                Reporter.AddMessage(new Message("combine field1 field2 [NameOfSet]"));
                return false;
            }
            return true;
        }

        /// <summary>
        /// Report to console any invalid fields.
        /// </summary>
        /// <param name="fieldsMaybe"></param>
        private void ReportInvalidFields(List<string> fieldsMaybe)
        {
            // Report
            // Which fields weren't valid?
            var invalidFields = fieldsMaybe.Where(fm => !FieldBrain.FieldValidForCombine(fm)).ToList();
            Message m;
            if (invalidFields.Count == 1) m = new Message($"The field \"{invalidFields.First()}\" is invalid for combining");
            else m = new Message($"The following fields are invalid for combining: {invalidFields.Aggregate((t, n) => $"\"{t}\", \"{n}\"")}");
            Reporter.AddMessage(m);
        }
    }
}
