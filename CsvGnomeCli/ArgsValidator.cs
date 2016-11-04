using CsvGnome;
using CsvGnomeCli.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeCli
{
    /// <summary>
    /// Check that the input args are valid.
    /// </summary>
    public class ArgsValidator
    {
        public const string NoArgsProvided = "No command line arguments were provided.";
        public const string DuplicateArgsProvided = "The following command line arguments were provided more than once:";
        public const string DuplicateArgValuesProvided = "The following command line arguments were provided, but they do the same thing:";

        private IReporter Reporter;

        public ArgsValidator(IReporter reporter)
        {
            Reporter = reporter;
        }

        public bool Validate(string[] args)
        {
            // return early if there are no arguments
            if (args == null || args.All(a => String.IsNullOrWhiteSpace(a)))
            {
                AddMessage(NoArgsProvided);
                return false;
            }

            bool result = true;
            result &= CheckForDuplicates(args);
            result &= CheckForRedundantArgs(args);
            result &= ValidateAll(args);
            return result;
        }

        private bool CheckForDuplicates(string[] args)
        {
            List<string> cliArgs = args.Where(a => Args.Args.IsAny(a)).ToList();

            // Check that no argument is provided more than once
            HashSet<string> duplicates = new HashSet<string>();
            foreach (string arg in cliArgs)
            {
                if(cliArgs.Count(a => (a == arg)) > 1)
                {
                    duplicates.Add(arg);
                }
            }

            // report duplicates
            if(duplicates.Count > 0)
            {
                AddMessage(DuplicateArgsProvided);
                duplicates.ToList().ForEach(AddMessage);
            }

            return duplicates.Count == 0;
        }

        private bool CheckForRedundantArgs (string[] args)
        {
            // Get all unique args
            HashSet<string> cliArgsSet = new HashSet<string>(args.Where(a => Args.Args.IsAny(a)));

            // Get their corresponding IArg
            List<IArg> iargs = cliArgsSet.Select(a => Args.Args.Get(a)).ToList();

            // Find duplicate IArgs
            HashSet<IArg> duplicates = new HashSet<IArg>();
            foreach (var iarg in iargs)
            {
                if (iargs.Count(a => a == iarg) > 1) duplicates.Add(iarg);
            }

            // report duplicates
            if (duplicates.Count > 0)
            {
                AddMessage(DuplicateArgValuesProvided);
                duplicates.ToList().ForEach(a => AddMessage($"{a.Values.First()} and {a.Values.Last()}"));
            }

            return duplicates.Count == 0;
        }

        private bool ValidateAll(string[] args)
        {
            bool success = true;
            // Call the validate method for each arg that is an IArg
            for (int i = 0; i < args.Length; i++)
            {
                IArg iarg = Args.Args.TryGet(args[i]);

                string failReason;
                if (iarg != null && !iarg.Validate(i, args, out failReason))
                {
                    AddMessage(failReason);
                    success = false;
                }
            }
            return success;
        }

        private void AddMessage(string message)
        {
            Reporter.AddMessage(new Message(message));
        }
    }
}
