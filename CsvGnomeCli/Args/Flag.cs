using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeCli.Args
{
    /// <summary>
    /// A command line flag that alters program flow with its mere presence!
    /// </summary>
    public class Flag : ArgBase
    {
        public Flag(params string[] values) : base(values) { }
        public override bool Validate(int index, string[] args, out string failReason)
        {
            failReason = String.Empty;
            // No other validation required.
            return true;
        }
    }
}
