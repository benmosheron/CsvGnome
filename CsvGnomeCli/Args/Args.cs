using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeCli.Args
{
    public static class Args
    {
        public static IArg File = new ExistingFile("--file", "-f");
        public static IArg Output = new ExistingFile("--output", "-o");
        public static IArg Interpret = new Flag("--interpret", "-i");
        //public static CliArg Preview = new CliArg("--preview", "-p");
        //public static CliArg CreateDirectories = new CliArg("--file", "-f");

        /// <summary>
        /// Get a list of all definied command line arguments.
        /// </summary>
        public static List<IArg> GetAll()
        {
            return new List<IArg>()
            {
                File,
                Output,
                Interpret
            };
        }

        /// <summary>
        /// True if the input arg is a predefined command like arg such as --file.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static bool IsAny(string arg)
        {
            return GetAll().Any(a => a.Is(arg));
        }

        /// <summary>
        /// Get the IArg that the string represents.
        /// </summary>
        public static IArg Get(string arg)
        {
            return GetAll().Single(a => a.Is(arg));
        }

        /// <summary>
        /// Get the IArg that the string represents, or null if it doesn't represent anything.
        /// </summary>
        public static IArg TryGet(string arg)
        {
            return GetAll().SingleOrDefault(a => a.Is(arg));
        }

        /// <summary>
        /// Get the index of the --interpret argument. If there isn't one, return -1.
        /// </summary>
        public static int TryGetInterpretIndex(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (Interpret.Is(args[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// If a --file arg is specified, get the file path that follows it.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool TryGetInputFilePath(string[] args, out string filePath)
        {
            filePath = String.Empty;
            return (TryGetFollowingArg(File, args, out filePath));
        }

        /// <summary>
        /// If a --file arg is specified, get the file path that follows it.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool TryGetOutputFilePath(string[] args, out string filePath)
        {
            filePath = String.Empty;
            return (TryGetFollowingArg(Output, args, out filePath));
        }

        private static bool TryGetFollowingArg(IArg fileArg, string[] args, out string filePath)
        {
            filePath = String.Empty;
            for (int i = 0; i < args.Length; i++)
            {
                if (fileArg.Is(args[i]))
                {
                    // Validation has already ensured safety here.
                    filePath = args[i + 1];
                    return true;
                }
            }
            return false;
        }
    }
}
