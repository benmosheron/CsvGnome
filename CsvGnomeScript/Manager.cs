using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeScript
{
    /// <summary>
    /// Manages reading of script files, and retrieval of function delegates.
    /// </summary>
    /// <remarks>
    /// Intended to be the single public access point for CsvGnome.
    /// </remarks>
    public class Manager
    {
        private ScriptFunctionStore Functions = new ScriptFunctionStore();

        public void ReadFile(string path)
        {
            throw new NotImplementedException();
        }

        public Func<long, string> GetValueFunction(string language, string function)
        {
            throw new NotImplementedException();
        }
        
    }
}
