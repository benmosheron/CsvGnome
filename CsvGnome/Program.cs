using CsvGnome.Fields;
using CsvGnome.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    public class Program
    {
        public const string FileExt = ".csv";

        /// <summary>
        /// Set the value of the TimeAtWrite field to the current time.
        /// </summary>
        public static void UpdateTime()
        {
            DateProvider.UpdateNow();
        }

        /// <summary>
        /// Path to output file.
        /// </summary>
        public static string FilePath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"Output\");

        /// <summary>
        /// Name of output file.
        /// </summary>
        public static string FileName = "CsvGnomeOutput";

        /// <summary>
        /// Full path of the file to be written.
        /// </summary>
        public static string File => $"{FilePath}{FileName}{FileExt}";

        /// <summary>
        /// Path of the file containing lua functions.
        /// </summary>
        public static string LuaFunctionsFile = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), @"Scripts\functions.lua");

        /// <summary>
        /// Public access to the program's gnomefilecache.
        /// </summary>
        public static GnomeFileCache GetGnomeFileCache => GnomeFileCache;
        
        static readonly Reporter Reporter = new Reporter();
        static readonly CsvGnomeScriptApi.IManager ScriptManager = new CsvGnomeScript.Manager();
        static readonly Components.Combinatorial.Cache CombinatorialCache = new Components.Combinatorial.Cache();
        static readonly PaddedFieldFactory PaddedFieldFactory = new PaddedFieldFactory();
        static readonly Date.IProvider DateProvider = new Date.Provider();
        static readonly IContext Context = new Context();
        static readonly Components.Combinatorial.Factory CombinatorialFactory = new Components.Combinatorial.Factory(CombinatorialCache);
        static readonly Components.Combinatorial.Deleter CombinatorialDeleter = new Components.Combinatorial.Deleter(CombinatorialCache);
        static readonly FieldBrain FieldBrain = new FieldBrain(CombinatorialFactory, CombinatorialDeleter);
        static readonly Configuration.IProvider ConfigurationProvider = new Configuration.Provider(Reporter);
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

            Action nextAction = Action.Continue;

            Action[] ContinueWhile = new Action[]
            {
                Action.Continue,
                Action.Help,
                Action.HelpSpecial,
                Action.ShowGnomeFiles,
                Action.Preview
            };

            while(ContinueWhile.Contains(nextAction))
            {
                // Before user entry - display information
                DisplayInfo(nextAction);

                // Interpret user entry
                nextAction = Interpreter.Interpret(Console.ReadLine());

                // after user entry
                nextAction = WriteIfNeeded(nextAction);
            }
        }

        public static void SetN(int nToSet)
        {
            Context.N = nToSet;
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

        private static void DisplayInfo(Action action)
        {
            switch (action)
            {
                case Action.Help:
                    Reporter.Help();
                    break;
                case Action.HelpSpecial:
                    Reporter.HelpSpecial();
                    break;
                case Action.ShowGnomeFiles:
                    Reporter.ShowGnomeFiles();
                    break;
                case Action.Preview:
                    Previewer.Preview(Reporter, FieldBrain.Fields, PaddedFieldFactory, Context.N);
                    Reporter.Report(FieldBrain.Fields, Context.N, File);
                    break;
                default:
                    Reporter.Report(FieldBrain.Fields, Context.N, File);
                    break;
            }
        }

        /// <summary>
        /// If required by the current action, write output data to a csv file.
        /// </summary>
        private static Action WriteIfNeeded(Action action)
        {
            switch (action)
            {
                case Action.Run:
                    Writer.WriteToFile(Reporter, FieldBrain.Fields, PaddedFieldFactory, File, Context.N);
                    break;
                case Action.Write:
                    Writer.WriteToFile(Reporter, FieldBrain.Fields, PaddedFieldFactory, File, Context.N);
                    action = Action.Continue;
                    break;
                default:
                    break;
            }
            return action;
        }
    }
}
