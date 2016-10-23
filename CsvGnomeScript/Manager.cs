using CsvGnomeScriptApi;
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
    public class Manager : IManager
    {
        private ScriptFunctionStore GlobalFunctionStore = new ScriptFunctionStore();

        private Dictionary<string, IScriptReader> Readers = new Dictionary<string, IScriptReader>();

        public Manager()
        {
            var luaReader = new LuaScript.Reader();
            // For now, we can only do lua files.
            Readers.Add(luaReader.Extension, luaReader);
        }

        public void ReadFile(string path)
        {
            // Read the file with the reader that matches the file's extension
            string ext = System.IO.Path.GetExtension(path).TrimStart('.');

            // Extract functions
            IScriptFunctions f = Readers[ext].Read(path);

            // Store functions.
            GlobalFunctionStore.AddFunctions(ext, f);
        }

        /// <summary>
        /// Get the value generating function for a given language and function.
        /// </summary>
        /// <exception cref="InvalidLanguageException">Thrown if the given language does not have any functions stored.</exception>
        /// <exception cref="InvalidFunctionException">Thrown if the given function does not exist in the given language's store.</exception> 
        public Func<long, object[]> GetValueFunction(string language, string functionName)
        {
            if (!GlobalFunctionStore.ContainsLanguage(language))
            {
                throw new InvalidLanguageException(language);
            }

            if (!GlobalFunctionStore[language].ContainsFunction(functionName))
            {
                throw new InvalidFunctionException(language, functionName);
            }
            return GlobalFunctionStore[language].ValueFunctions[functionName];
        }

    }
}
