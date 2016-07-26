﻿using System;
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
        /// E.g. [something]{something else}
        /// </summary>
        public const string CommandRegexPattern = @"(\[.+?\](?:{.*?})?)";
        private Regex CommandRegex = new Regex(CommandRegexPattern);

        private readonly FieldBrain FieldBrain;
        private readonly Reporter Reporter;
        private readonly ComponentFactory Factory;
        private readonly GnomeFileWriter GnomeFileWriter;
        private readonly GnomeFileReader GnomeFileReader;
        private readonly MinMaxInfoCache MinMaxInfoCache;

        private Components.Combinatorial.Factory CombinatorialFactory => FieldBrain.CombinatorialFactory;


        /// <summary>
        /// Overload with no I/O for reading from gnomefiles and unit tests.
        /// </summary>
        /// <param name="fieldBrain"></param>
        /// <param name="reporter"></param>
        /// <param name="cache"></param>
        public Interpreter(FieldBrain fieldBrain, Reporter reporter, MinMaxInfoCache cache)
            :this(fieldBrain, reporter, cache, null, null)
        { }

        /// <summary>
        /// Create a new interpreter that can update fields, read and write gnomefiles.
        /// </summary>
        /// <param name="fieldBrain"></param>
        /// <param name="reporter"></param>
        /// <param name="cache"></param>
        /// <param name="gnomeFileWriter"></param>
        /// <param name="gnomeFileReader"></param>
        public Interpreter(FieldBrain fieldBrain, Reporter reporter, MinMaxInfoCache cache, GnomeFileWriter gnomeFileWriter, GnomeFileReader gnomeFileReader)
        {
            FieldBrain = fieldBrain;
            Reporter = reporter;
            Factory = new ComponentFactory(CombinatorialFactory, cache);
            MinMaxInfoCache = cache;
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

            // "clear" clears all fields
            if (input.ToLower() == "clear")
            {
                FieldBrain.ClearFields(MinMaxInfoCache);
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
                    Interpreter interpreterNoIO = new Interpreter(FieldBrain, Reporter, MinMaxInfoCache);
                    string fileToRead = input.Remove(0, "load".Length).Trim();
                    List<string> parsedFile = GnomeFileReader.ReadGnomeFile(fileToRead);
                    
                    // We will assume the file contains some valid instructions, if it has anything at all!
                    if(parsedFile != null && parsedFile.Count > 0 && parsedFile.Any(l => !String.IsNullOrWhiteSpace(l)))
                    {
                        // Clear fields and cache
                        FieldBrain.ClearFields(MinMaxInfoCache);
                        parsedFile.ForEach(interpreterNoIO.InterpretSilent);
                        Reporter.AddMessage(new Message($"{fileToRead} loaded.", ConsoleColor.Green));
                    }
                    
                }
                return Action.Continue;
            }

            if (input.StartsWith("delete"))
            {
                FieldBrain.DeleteField(input.Remove(0, "delete".Length), MinMaxInfoCache);
                return Action.Continue;
            }

            if (input.StartsWith("output"))
            {
                Program.SetOutputFile(input.Remove(0, "output".Length));
                return Action.Continue;
            }

            // Int sets N
            int n;
            if (int.TryParse(input, out n))
            {
                Program.SetN(n);
                return Action.Continue;
            }
            
            // "full on" sets array fields to display contents in full
            if(input == "full on")
            {
                Program.SetReportFullArrayContents(true);
                return Action.Continue;
            }

            // "full off" sets array fields to report only the number of items
            if (input == "full off")
            {
                Program.SetReportFullArrayContents(false);
                return Action.Continue;
            }

            // ":" Indicates a data update
            if (input.Contains(":"))
            {
               var tokens = input.Split(new char[] { ':' }, 2);
               string name = tokens[0];
               string instruction = tokens[1];
               InterpretInstruction(name, instruction);
               return Action.Continue;
            }

            // Someone's consfused
            if (!String.IsNullOrEmpty(input)) Reporter.AddMessage(new Message("Enter \"help\" for help.", ConsoleColor.Green));

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
            if (FieldBrain.Fields.Any(f => f.Name == name)) FieldBrain.DeleteField(name, MinMaxInfoCache);

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
            var results = CommandRegex
                .Split(instruction)
                .Where(i => !String.IsNullOrEmpty(i))
                .Select(i => Factory.Create(i))
                .ToArray();
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
                Reporter.AddMessage(new Message("You must provide fields to combine. E.g:"));
                Reporter.AddMessage(new Message("combine field1 field2 [NameOfSet]"));
                return false;
            }
            return true;
        }
    }
}
