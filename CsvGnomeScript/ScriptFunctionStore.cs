using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeScript
{
    public class ScriptFunctionStore
    {
        public Dictionary<string, IScriptFunctions> Languages = new Dictionary<string, IScriptFunctions>();

        /// <summary>
        /// Update the function store with a set of functions for the provided language. The store will be initialsed if it has not been already.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="functionsToAdd"></param>
        public void UpdateWithFunctions(string language, IScriptFunctions functionsToAdd)
        {
            if (!Languages.ContainsKey(language))
            {
                switch (language)
                {
                    case "lua":
                        Languages[language] = new LuaScriptFunctions();
                        break;
                    default:
                        throw new ArgumentException($"Language [{language}] is not supported.");
                }
            }

            foreach (string valueFunctionName in functionsToAdd.ValueFunctions.Keys)
            {
                Languages[language].ValueFunctions[valueFunctionName] = functionsToAdd.ValueFunctions[valueFunctionName];
            }
        }
    }
}
