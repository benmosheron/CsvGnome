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
    public class Manager : CsvGnomeScriptApi.IManager
    {
        private ScriptFunctionStore Functions = new ScriptFunctionStore();

        public void ReadFile(string path)
        {
            // For now, we can only do lua files.
            LuaScriptReader reader = new LuaScriptReader();
            Functions.UpdateWithFunctions("lua" ,reader.Read(path));
        }

        /// <summary>
        /// Get the value generating function for a given language and function.
        /// </summary>
        /// <exception cref="InvalidLanguageException">Thrown if the given language does not have any functions stored.</exception>
        /// <exception cref="InvalidFunctionException">Thrown if the given function does not exist in the given language's store.</exception> 
        public Func<long, object[]> GetValueFunction(string language, string functionName)
        {
            if (!Functions.Languages.ContainsKey(language))
            {
                throw new InvalidLanguageException(language);
            }

            if (!Functions.Languages[language].ValueFunctions.ContainsKey(functionName))
            {
                throw new InvalidFunctionException(language, functionName);
            }
            return Functions.Languages[language].ValueFunctions[functionName];
        }
    }
}
