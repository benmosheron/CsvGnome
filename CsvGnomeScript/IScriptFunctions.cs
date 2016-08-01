using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeScript
{
    public interface IScriptFunctions
    {
        Dictionary<string, Func<long, object[]>> ValueFunctions { get; }
    }
}
