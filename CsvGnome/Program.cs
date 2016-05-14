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
        public const string GnomeFileExt = ".gnome";
        public const string DefaultGnomeFileName = "default";
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

        static readonly FieldBrain FieldBrain = new FieldBrain();
        static readonly Reporter Reporter = new Reporter();
        static readonly MinMaxInfoCache MinMaxInfoCache = new MinMaxInfoCache();
        static readonly Interpreter Interpreter = new Interpreter(FieldBrain, Reporter, MinMaxInfoCache);
        static readonly GnomeFileReader GnomeFileReader = new GnomeFileReader(Interpreter, Reporter);
        static readonly Writer Writer = new Writer();

        static void Main(string[] args)
        {
            // Read defaults from file
            GnomeFileReader.ReadDefaultGnomeFile();

            Action nextAction = Action.Continue;
            while(
                nextAction == Action.Continue 
                || nextAction == Action.Help
                || nextAction == Action.HelpSpecial)
            {
                if (nextAction == Action.Help)
                    Reporter.Help();
                else if (nextAction == Action.HelpSpecial)
                    Reporter.HelpSpecial();
                else
                    Reporter.Report(FieldBrain.Fields, N, File);

                nextAction = Interpreter.Interpret(Console.ReadLine());

                switch (nextAction)
                {
                    case Action.Exit:
                        break;
                    case Action.Continue:
                        break;
                    case Action.Run:
                        Writer.WriteToFile(FieldBrain.Fields, File, N);
                        break;
                    case Action.Write:
                        Writer.WriteToFile(FieldBrain.Fields, File, N);
                        nextAction = Action.Continue;
                        break;
                    case Action.Help:
                        break;
                    case Action.HelpSpecial:
                        break;
                    default:
                        nextAction = Action.Continue;
                        break;
                }
            }
        }

        public static void SetN(int nToSet)
        {
            n = nToSet;
        }
    }
}
