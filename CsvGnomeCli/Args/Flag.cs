using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeCli.Args
{
    /// <summary>
    /// A flag that alters program flow with its mere presence!
    /// </summary>
    public class Flag : ArgBase
    {
        public override bool Validate(string[] args)
        {
            // No other validation required.
            return true;
        }
    }
}
