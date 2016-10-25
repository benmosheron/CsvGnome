using CsvGnome.Components;
using CsvGnome.Fields;
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
        /// <summary>
        /// E.g. [something]
        /// or   [something]{something else}
        /// </summary>
        public const string CommandRegexPattern = @"(\[.+?\](?:{.*?})?)";
        private Regex CommandRegex = new Regex(CommandRegexPattern);

        private readonly FieldBrain FieldBrain;
        private readonly IReporter Reporter;
        private readonly IContext Context;
        private readonly Configuration.IProvider ConfigurationProvider;

        private readonly ComponentFactory Factory;
        // Only used to pass into ComponentFactory
        private readonly CsvGnomeScriptApi.IManager ScriptManager;
        private readonly Date.IProvider DateProvider;
        
        // To remain null for NoIO interpreters
        private readonly GnomeFiles.IWriter GnomeFileWriter;
        private readonly GnomeFiles.IReader GnomeFileReader;

        private Components.Combinatorial.Factory CombinatorialFactory => FieldBrain.CombinatorialFactory;

        /// <summary>
        /// Overload with no I/O for reading from gnomefiles and unit tests.
        /// </summary>
        /// <param name="fieldBrain"></param>
        /// <param name="reporter"></param>
        /// <param name="cache"></param>
        public Interpreter(FieldBrain fieldBrain, IReporter reporter, CsvGnomeScriptApi.IManager scriptManager, IContext context, Date.IProvider dateProvider, Configuration.IProvider configurationProvider)
            :this(fieldBrain, reporter, scriptManager, context, dateProvider, configurationProvider, null, null)
        { }

        /// <summary>
        /// Create a new interpreter that can update fields, read and write gnomefiles.
        /// </summary>
        /// <param name="fieldBrain"></param>
        /// <param name="reporter"></param>
        /// <param name="cache"></param>
        /// <param name="gnomeFileWriter"></param>
        /// <param name="gnomeFileReader"></param>
        public Interpreter(
            FieldBrain fieldBrain, 
            IReporter reporter,
            CsvGnomeScriptApi.IManager scriptManager,
            IContext context,
            Date.IProvider dateProvider,
            Configuration.IProvider configurationProvider,
            GnomeFiles.IWriter gnomeFileWriter,
            GnomeFiles.IReader gnomeFileReader)
        {
            FieldBrain = fieldBrain;
            Reporter = reporter;
            ScriptManager = scriptManager;
            Context = context;
            DateProvider = dateProvider;
            ConfigurationProvider = configurationProvider;
            Factory = new ComponentFactory(context, dateProvider, configurationProvider, fieldBrain.CombinatorialFactory, scriptManager, Reporter.MessageProvider);
            GnomeFileWriter = gnomeFileWriter;
            GnomeFileReader = gnomeFileReader;
        }

        /// <summary>
        /// Interprets commands without altering program flow. Used when reading from a gnomefile.
        /// </summary>
        /// <param name="input"></param>
        public void InterpretSilent(string input)
        {
            Action temp = Interpret(input);
        }

        /// <summary>
        /// Interpret commands.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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

            // "preview" display a preview of the data to generate in the console window
            if (input.ToLower() == "preview") return Action.Preview;

            // "clear" clears all fields
            if (input.ToLower() == "clear")
            {
                FieldBrain.ClearFields();
                return Action.Continue;
            }

            // "save fileName" writes a new GnomeFile to the gnomefile directory
            if (input.StartsWith("save"))
            {
                // Interpreter unit tests use a null writer
                if (GnomeFileWriter != null)
                {
                    string fileName = input.Remove(0, "save".Length);
                    if (String.IsNullOrWhiteSpace(fileName))
                    {
                        if(GnomeFileReader.LastLoadedFileName != null)
                            fileName = GnomeFileReader.LastLoadedFileName;
                    }
                    GnomeFileWriter.Save(fileName);
                }
                return Action.Continue;
            }

            // "load fileName" loads a GnomeFile from the gnomefile directory
            if (input.StartsWith("load"))
            {
                // Use a separate interpreter with no I/O for reading files
                // Otherwise the interpreter could read a "load" instruction
                if (GnomeFileReader != null)
                {
                    Interpreter interpreterNoIO = new Interpreter(FieldBrain, Reporter, ScriptManager, Context, DateProvider, ConfigurationProvider);
                    string fileToRead = input.Remove(0, "load".Length).Trim();
                    List<string> parsedFile = GnomeFileReader.ReadGnomeFile(fileToRead);
                    
                    // We will assume the file contains some valid instructions, if it has anything at all!
                    if(parsedFile != null && parsedFile.Count > 0 && parsedFile.Any(l => !String.IsNullOrWhiteSpace(l)))
                    {
                        // Clear fields and cache
                        FieldBrain.ClearFields();
                        parsedFile.ForEach(interpreterNoIO.InterpretSilent);
                        Reporter.AddMessage($"{fileToRead} loaded.", ConsoleColor.Green);
                    }
                    
                }
                return Action.Continue;
            }

            if (input.StartsWith("delete"))
            {
                FieldBrain.DeleteField(input.Remove(0, "delete".Length));
                return Action.Continue;
            }

            if (input.StartsWith("output"))
            {
                Context.SetOutputFile(input.Remove(0, "output".Length));
                return Action.Continue;
            }

            // Int sets N
            int n;
            if (int.TryParse(input, out n))
            {
                Context.N = n;
                return Action.Continue;
            }
            
            // "full on" sets array fields to display contents in full
            if(input == "full on")
            {
                ConfigurationProvider.SetReportArrayContents(true);
                return Action.Continue;
            }

            // "full off" sets array fields to report only the number of items
            if (input == "full off")
            {
                ConfigurationProvider.SetReportArrayContents(false);
                return Action.Continue;
            }

            // "pad on" turns on padding of output to align columns
            if(input == "pad on")
            {
                ConfigurationProvider.SetPadOutput(true);
                Reporter.AddMessage("Padding enabled.", ConsoleColor.Green);
                return Action.Continue;
            }

            // "pad off" turns off padding of output to align columns
            if (input == "pad off")
            {
                ConfigurationProvider.SetPadOutput(false);
                Reporter.AddMessage("Padding disabled.", ConsoleColor.Green);
                return Action.Continue;
            }

            // ":" Indicates a data update
            if (input.Contains(":"))
            {
                var tokens = input.Split(new char[] { ':' }, 2);
                string name = tokens[0];
                string instruction = tokens[1];

                try
                {
                    InterpretInstruction(name, instruction);
                }
                catch (InfiniteMinMaxException infiniteMinMaxException)
                {
                    // Thrown by MinMaxComponent() if it would be infinite
                    Reporter.AddError(infiniteMinMaxException.Message);
                }
                return Action.Continue;
            }

            // Someone's consfused
            if (!String.IsNullOrEmpty(input)) Reporter.AddMessage("Enter \"help\" for help.", ConsoleColor.Green);

            return Action.Continue;
        }

        /// <summary>
        /// Attempt to update or create a field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="instruction"></param>
        private void InterpretInstruction(string name, string instruction)
        {
            // If we are overwriting a field, explicitely delete the existing information first.
            // This prevents a can of worms when overwriting a field with combined minMax components
            if (FieldBrain.Fields.Any(f => f.Name == name)) FieldBrain.DeleteField(name);

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
            IComponent[] results;
            try
            {
                results = CommandRegex
                    .Split(instruction)
                    .Where(i => !String.IsNullOrEmpty(i))
                    .Select(i => Factory.Create(i))
                    .ToArray();
            }
            catch(ComponentCreationException ex)
            {
                Reporter.AddError($"Error interpreting instruction {instruction}. [{ex.Message}]");
                results = new IComponent[0];
            }
            return results;
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
                Reporter.AddMessage("You must provide fields to combine. E.g:");
                Reporter.AddMessage("combine field1 field2 [NameOfSet]");
                return false;
            }
            return true;
        }
    }
}
