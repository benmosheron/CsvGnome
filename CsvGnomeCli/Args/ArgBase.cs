using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeCli.Args
{
    /// <summary>
    /// Base class for possible command line arguments.
    /// </summary>
    public class ArgBase : IArg
    {
        /// <summary>
        /// Different aliases for the command.
        /// </summary>
        public HashSet<string> Values { get; }

        public ArgBase(params string[] values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));
            if (values.Any(v => String.IsNullOrEmpty(v))) throw new InvalidOperationException("At least one value was null or empty.");
            Values = new HashSet<string>(values);
            if (Values.Count != values.Length) throw new InvalidOperationException("Duplicate values provided.");
        }

        public bool Is(string arg)
        {
            return Values.Any(v => v == arg);
        }

        public virtual bool Validate(int index, string[] args, out string failReason) { throw new NotImplementedException("Consumers should override Validate method."); }
    }
}
