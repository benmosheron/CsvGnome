using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    public class Program
    {
        public const string FileExt = ".csv";
        public const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
        public const string CommandRegexPattern = @"(\[.+?\](?:{.*?})*)";
        public const string DateComponentString = "[date]";
        public const string SpreadComponentString = "[spread]";
        public const string CycleComponentString = "[cycle]";
        public const ConsoleColor DefaultColour = ConsoleColor.Gray;
        public const ConsoleColor SpecialColour = ConsoleColor.Cyan;

        private const string ReportArrayContentsConfigKey = "ReportArrayContents";

        public static string TimeAtWrite = DateTime.Now.ToString(DateTimeFormat);

        /// <summary>
        /// Set the value of the TimeAtWrite field to the current time.
        /// </summary>
        public static void UpdateTime()
        {
            TimeAtWrite = DateTime.Now.ToString(DateTimeFormat);
        }

        private static int n = 10; 

        /// <summary>
        /// Number of data lines to generate.
        /// </summary>
        public static int N
        {
            get { return n; }
        }

        private static bool reportArrayContents = GetConfigured_ReportArrayContent();

        /// <summary>
        /// If true, array contents will be written to the console. If false, only the number of items will be displayed.
        /// </summary>
        public static bool ReportArrayContents { get { return reportArrayContents; } }

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
        /// Public access to the program's gnomefilecache.
        /// </summary>
        public static GnomeFileCache GetGnomeFileCache => GnomeFileCache;

        static readonly MinMaxInfoCache MinMaxInfoCache = new MinMaxInfoCache();
        static readonly FieldBrain FieldBrain = new FieldBrain();
        static readonly Reporter Reporter = new Reporter();
        static readonly GnomeFileCache GnomeFileCache = new GnomeFileCache(Reporter);
        static readonly GnomeFileWriter GnomeFileWriter = new GnomeFileWriter(Reporter, FieldBrain, GnomeFileCache);
        static readonly GnomeFileReader GnomeFileReader = new GnomeFileReader(Reporter, GnomeFileCache);
        static readonly Interpreter Interpreter = new Interpreter(FieldBrain, Reporter, MinMaxInfoCache, GnomeFileWriter, GnomeFileReader);
        static readonly Interpreter InterpreterNoIO = new Interpreter(FieldBrain, Reporter, MinMaxInfoCache);
        static readonly Writer Writer = new Writer();

        static void Main(string[] args)
        {
            // Read defaults from file
            GnomeFileReader.ReadDefaultGnomeFile().ForEach(InterpreterNoIO.InterpretSilent);

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

        public static void SetReportFullArrayContents(bool b)
        {
            SetConfigured_ReportArrayContent(b);
            reportArrayContents = b;
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

        private static bool GetConfigured_ReportArrayContent()
        {
            bool b = false;
            try
            {
                b = ConfigurationManager.AppSettings[ReportArrayContentsConfigKey] == true.ToString();
            }
            catch(ConfigurationException ex)
            {
                Reporter.AddMessage(new Message(ex.Message, ConsoleColor.Red));
            }
            return b;
        }

        private static void SetConfigured_ReportArrayContent(bool b)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings.Remove(ReportArrayContentsConfigKey);
                config.AppSettings.Settings.Add(ReportArrayContentsConfigKey, b.ToString());
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch(ConfigurationErrorsException ex)
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

        private static Action WriteIfNeeded(Action action)
        {
            switch (action)
            {
                case Action.Run:
                    Writer.WriteToFile(Reporter, FieldBrain.Fields, File, N);
                    break;
                case Action.Write:
                    Writer.WriteToFile(Reporter, FieldBrain.Fields, File, N);
                    action = Action.Continue;
                    break;
                default:
                    break;
            }
            return action;
        }
    }
}
