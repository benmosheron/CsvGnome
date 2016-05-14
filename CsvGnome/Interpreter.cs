using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CsvGnome
{
    /// <summary>
    /// Interprets input from the console.
    /// </summary>
    public class Interpreter
    {

        private readonly FieldBrain FieldBrain;
        private readonly Reporter Reporter;
        private readonly ComponentFactory Factory;
        private readonly GnomeFileWriter GnomeFileWriter;

        public Interpreter(FieldBrain fieldBrain, Reporter reporter, MinMaxInfoCache cache, GnomeFileWriter gnomeFileWriter)
        {
            FieldBrain = fieldBrain;
            Reporter = reporter;
            Factory = new ComponentFactory(cache);
            GnomeFileWriter = gnomeFileWriter;
        }

        public Action Interpret(string input)
        {
            // "//" indicates a comment (in a GnomeFile).
            if (input.TrimStart().StartsWith("//")) return Action.Continue;

            // Empty string does nothing
            if (input == String.Empty) return Action.Continue;

            // "exit" exist program
            if (input.ToLower() == "exit") return Action.Exit;

            // "run" writes file and exits
            if (input.ToLower() == "run") return Action.Run;

            // "write" writes file and continues
            if (input.ToLower() == "write") return Action.Write;

            // "help" write help to console and continues
            if (input.ToLower() == "help") return Action.Help;

            // "help special" write help to console and continues
            if (input.ToLower() == "help special") return Action.HelpSpecial;

            // "gnomefiles" write information about gnomefiles
            if (input.ToLower() == "gnomefiles") return Action.ShowGnomeFiles;

            // "save fileName" writes a new GnomeFile to the gnomefile directory
            if (input.StartsWith("save"))
            {
                // Interpreter nit tests use a null writer
                if(GnomeFileWriter != null) GnomeFileWriter.Save(input.Remove(0, "save".Length));
                return Action.Continue;
            }

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
        /// Attempt to update or create a field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="instruction"></param>
        private void InterpretInstruction(string name, string instruction)
        {
            // Try and create a component field
            // Break instruction into components
            IComponent[] components = GetComponents(instruction);

            // If no components could be created, add an empty field
            // e.g. "emptyField:"
            if (components.Length == 0) components = new IComponent[] { new TextComponent(String.Empty) };

            // Create component field
            FieldBrain.AddOrUpdateComponentField(name, components);
            
            
        }

        private IComponent[] GetComponents(string instruction)
        {
            Regex r = new Regex(Program.CommandRegexPattern);
            return r
                .Split(instruction)
                .Where(i => !String.IsNullOrEmpty(i))
                .Select(i => Factory.Create(i))
                .ToArray();
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
    }
}
