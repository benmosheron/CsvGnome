using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeScript
{
    public class LuaScriptFunctions : IScriptFunctions
    {
        private Dictionary<string, Func<long, object[]>> valueFunctions = new Dictionary<string, Func<long, object[]>>();
        public Dictionary<string, Func<long, object[]>> ValueFunctions => valueFunctions;
    }
}
