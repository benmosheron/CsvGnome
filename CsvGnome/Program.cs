using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    class Program
    {
        public const string FileExt = ".csv";
        public const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
        public const string CommandRegexPattern = @"(\[.+?\])";
        public const string DateComponentString = "[date]";

        public static string TimeAtWrite = DateTime.Now.ToString(DateTimeFormat);

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

        /// <summary>
        /// Path to output file.
        /// </summary>
        public static string FilePath = @"C:\Users\revol\Desktop\csvGnomeOut\";

        /// <summary>
        /// Name of output file.
        /// </summary>
        public static string FileName = "CsvGnome";

        public static string File => $"{FilePath}{FileName}{FileExt}";

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
                    Writer.WriteToFile(FieldBrain.Fields, File, N);
                    break;
                case Action.Write:
                    Writer.WriteToFile(FieldBrain.Fields, File, N);
                    action = Action.Continue;
                    break;
                default:
                    break;
            }
            return action;
        }
    }
}
