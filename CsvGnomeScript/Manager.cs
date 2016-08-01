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
    public class Manager
    {
        private ScriptFunctionStore Functions = new ScriptFunctionStore();

        public void ReadFile(string path)
        {
            // For now, we can only do lua files.
            LuaScriptReader reader = new LuaScriptReader();
            Functions.UpdateWithFunctions("lua" ,reader.Read(path));
        }

        public Func<long, object[]> GetValueFunction(string language, string functionName)
        {
            return Functions.Languages[language].ValueFunctions[functionName];
        }
        
    }
}
