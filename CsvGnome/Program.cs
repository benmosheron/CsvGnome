using CsvGnome.Fields;
using System;
using System.Collections.Generic;
//using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    public class Program
    {
        public const string FileExt = ".csv";
        public const string DateComponentString = "[date]";
        public const string SpreadComponentString = "[spread]";
        public const string CycleComponentString = "[cycle]";
        public const ConsoleColor DefaultColour = ConsoleColor.Gray;
        public const ConsoleColor SpecialColour = ConsoleColor.Cyan;

        private const string ReportArrayContentsConfigKey = "ReportArrayContents";

        /// <summary>
        /// Set the value of the TimeAtWrite field to the current time.
        /// </summary>
        public static void UpdateTime()
        {
            DateProvider.UpdateNow();
        }

        private static int n = 10; 

        /// <summary>
        /// Number of data lines to generate.
        /// </summary>
        public static int N
        {
            get { return n; }
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
        static readonly CsvGnomeScript.Manager ScriptManager = new CsvGnomeScript.Manager();
        static readonly Components.Combinatorial.Cache CombinatorialCache = new Components.Combinatorial.Cache();
        static readonly PaddedFieldFactory PaddedFieldFactory = new PaddedFieldFactory();
        static readonly Date.IProvider DateProvider = new Date.Provider();
        static readonly Components.Combinatorial.Factory CombinatorialFactory = new Components.Combinatorial.Factory(CombinatorialCache);
        static readonly Components.Combinatorial.Deleter CombinatorialDeleter = new Components.Combinatorial.Deleter(CombinatorialCache);
        static readonly FieldBrain FieldBrain = new FieldBrain(CombinatorialFactory, CombinatorialDeleter);
        static readonly Configuration.IProvider ConfigurationProvider = new Configuration.Provider(Reporter);
        static readonly Writer Writer = new Writer(ConfigurationProvider);
        static readonly GnomeFileCache GnomeFileCache = new GnomeFileCache(Reporter);
        static readonly GnomeFileWriter GnomeFileWriter = new GnomeFileWriter(Reporter, FieldBrain, GnomeFileCache);
        static readonly GnomeFileReader GnomeFileReader = new GnomeFileReader(Reporter, GnomeFileCache);
        static readonly Interpreter Interpreter = new Interpreter(FieldBrain, Reporter, ScriptManager, DateProvider, ConfigurationProvider, GnomeFileWriter, GnomeFileReader);
        static readonly Interpreter InterpreterNoIO = new Interpreter(FieldBrain, Reporter, ScriptManager, DateProvider, ConfigurationProvider);


        static void Main(string[] args)
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
            n = nToSet;
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

        private static bool GetConfigured_ReportArrayContent()
        {
            bool b = false;
            try
            {
                b = System.Configuration.ConfigurationManager.AppSettings[ReportArrayContentsConfigKey] == true.ToString();
            }
            catch(System.Configuration.ConfigurationException ex)
            {
                Reporter.AddMessage(new Message(ex.Message, ConsoleColor.Red));
            }
            return b;
        }

        private static void SetConfigured_ReportArrayContent(bool b)
        {
            try
            {
                System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                config.AppSettings.Settings.Remove(ReportArrayContentsConfigKey);
                config.AppSettings.Settings.Add(ReportArrayContentsConfigKey, b.ToString());
                config.Save(System.Configuration.ConfigurationSaveMode.Modified);
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");
            }
            catch(System.Configuration.ConfigurationErrorsException ex)
            {
                Reporter.AddMessage(new Message(ex.Message, ConsoleColor.Red));
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
                default:
                    Reporter.Report(FieldBrain.Fields, N, File);
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
                    Writer.WriteToFile(Reporter, FieldBrain.Fields, PaddedFieldFactory, File, N);
                    break;
                case Action.Write:
                    Writer.WriteToFile(Reporter, FieldBrain.Fields, PaddedFieldFactory, File, N);
                    action = Action.Continue;
                    break;
                default:
                    break;
            }
            return action;
        }
    }
}
