using CsvGnome;
using CsvGnome.Fields;
using CsvGnome.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeStandAlone
{
    public class Program
    {
        private const string FileExt = ".csv";

        /// <summary>
        /// Path to output file.
        /// </summary>
        private static string FilePath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"Output\");

        /// <summary>
        /// Name of output file.
        /// </summary>
        private static string FileName = "CsvGnomeOutput";

        /// <summary>
        /// Full path of the file to be written.
        /// </summary>
        private static string File => $"{FilePath}{FileName}{FileExt}";

        /// <summary>
        /// Path of the file containing lua functions.
        /// </summary>
        private static string LuaFunctionsFile = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"Scripts\functions.lua");

        /// <summary>
        /// Public access to the program's gnomefilecache.
        /// </summary>
        public static GnomeFileCache GetGnomeFileCache => GnomeFileCache;

        static readonly Reporter Reporter = new Reporter();
        static readonly CsvGnomeScriptApi.IManager ScriptManager = new CsvGnomeScript.Manager();
        static readonly CsvGnome.Components.Combinatorial.Cache CombinatorialCache = new CsvGnome.Components.Combinatorial.Cache();
        static readonly PaddedFieldFactory PaddedFieldFactory = new PaddedFieldFactory();
        static readonly CsvGnome.Date.IProvider DateProvider = new CsvGnome.Date.Provider();
        static readonly IContext Context = new Context();
        static readonly CsvGnome.Components.Combinatorial.Factory CombinatorialFactory = new CsvGnome.Components.Combinatorial.Factory(CombinatorialCache);
        static readonly CsvGnome.Components.Combinatorial.Deleter CombinatorialDeleter = new CsvGnome.Components.Combinatorial.Deleter(CombinatorialCache);
        static readonly FieldBrain FieldBrain = new FieldBrain(CombinatorialFactory, CombinatorialDeleter);
        static readonly CsvGnome.Configuration.IProvider ConfigurationProvider = new CsvGnome.Configuration.Provider(Reporter);
        static readonly Writer Writer = new Writer(ConfigurationProvider);
        static readonly Previewer Previewer = new Previewer(ConfigurationProvider);
        static readonly GnomeFileCache GnomeFileCache = new GnomeFileCache(Reporter);
        static readonly GnomeFileWriter GnomeFileWriter = new GnomeFileWriter(Context, Reporter, FieldBrain, GnomeFileCache);
        static readonly GnomeFileReader GnomeFileReader = new GnomeFileReader(Reporter, GnomeFileCache);
        static readonly Interpreter Interpreter = new Interpreter(FieldBrain, Reporter, ScriptManager, Context, DateProvider, ConfigurationProvider, GnomeFileWriter, GnomeFileReader);
        static readonly Interpreter InterpreterNoIO = new Interpreter(FieldBrain, Reporter, ScriptManager, Context, DateProvider, ConfigurationProvider);


        public static void Main(string[] args)
        {
            // Read defaults from file
            GnomeFileReader.ReadDefaultGnomeFile().ForEach(InterpreterNoIO.InterpretSilent);

            // Read script files
            ReadScriptFiles();

            SetConsoleTitle();

            CsvGnome.Action nextAction = CsvGnome.Action.Continue;

            CsvGnome.Action[] ContinueWhile = new CsvGnome.Action[]
            {
                CsvGnome.Action.Continue,
                CsvGnome.Action.Help,
                CsvGnome.Action.HelpSpecial,
                CsvGnome.Action.ShowGnomeFiles,
                CsvGnome.Action.Preview
            };

            while (ContinueWhile.Contains(nextAction))
            {
                // Before user entry - display information
                DisplayInfo(nextAction);

                // Interpret user entry
                nextAction = Interpreter.Interpret(Console.ReadLine());

                // after user entry
                nextAction = WriteIfNeeded(nextAction);
            }
        }

        /// <summary>
        /// Set the location of the output csv file.
        /// </summary>
        /// <param name="input">E.g. C:\path\file.csv</param>
        public static void SetOutputFile(string input)
        {
            string inputTrimmed = input.Trim().Replace("\"", String.Empty);
            string name = System.IO.Path.GetFileNameWithoutExtension(inputTrimmed);
            string path = System.IO.Path.GetDirectoryName(inputTrimmed);

            if (!System.IO.Directory.Exists(path))
            {
                Reporter.AddMessage(new Message("Could not find directory:", ConsoleColor.Red));
                Reporter.AddMessage(new Message(path, ConsoleColor.Red));
            }
            else
            {
                FilePath = path + @"\";
            }

            if (String.IsNullOrEmpty(name))
            {

            }
            else
            {
                FileName = name;
            }
        }

        /// <summary>
        /// Set the title of the console window
        /// </summary>
        public static void SetConsoleTitle()
        {
            string title = "CsvGnome";
            if (GnomeFileReader.LastLoadedFileName != null) title += $": {GnomeFileReader.LastLoadedFileName}";
            Console.Title = title;
        }

        private static void ReadScriptFiles()
        {
            try
            {
                ScriptManager.ReadFile(LuaFunctionsFile);
            }
            catch (Exception ex)
            {
                // Swallow all exceptions here, script functions will not work.
                Reporter.Error(new Message($"Error loading functions from file: {LuaFunctionsFile}. [{ex.Message}]").ToList());
            }
        }

        private static void DisplayInfo(CsvGnome.Action action)
        {
            switch (action)
            {
                case CsvGnome.Action.Help:
                    Reporter.Help();
                    break;
                case CsvGnome.Action.HelpSpecial:
                    Reporter.HelpSpecial();
                    break;
                case CsvGnome.Action.ShowGnomeFiles:
                    Reporter.ShowGnomeFiles();
                    break;
                case CsvGnome.Action.Preview:
                    Previewer.Preview(Reporter, FieldBrain.Fields, PaddedFieldFactory, Context.N);
                    Reporter.Report(FieldBrain.Fields, Context.N, File);
                    break;
                default:
                    Reporter.Report(FieldBrain.Fields, Context.N, File);
                    break;
            }
        }

        /// <summary>
        /// If required by the current CsvGnome.Action, write output data to a csv file.
        /// </summary>
        private static CsvGnome.Action WriteIfNeeded(CsvGnome.Action action)
        {
            switch (action)
            {
                case CsvGnome.Action.Run:
                    Writer.WriteToFile(DateProvider, Reporter, FieldBrain.Fields, PaddedFieldFactory, File, Context.N);
                    break;
                case CsvGnome.Action.Write:
                    Writer.WriteToFile(DateProvider, Reporter, FieldBrain.Fields, PaddedFieldFactory, File, Context.N);
                    action = CsvGnome.Action.Continue;
                    break;
                default:
                    break;
            }
            return action;
        }
    }
}
