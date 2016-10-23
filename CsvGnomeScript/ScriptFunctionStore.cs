using CsvGnomeScriptApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeScript
{
    public class ScriptFunctionStore
    {
        private Dictionary<string, IScriptFunctions> store = new Dictionary<string, IScriptFunctions>();

        public void AddFunctions(string language, IScriptFunctions f)
        {
            // If this language already has functions store, add the new functions to the existing store
            if (ContainsLanguage(language))
            {
                store[language].Combine(f);
            }
            else
            {
                store[language] = f;
            }
        }

        public bool ContainsLanguage(string language)
        {
            return store.ContainsKey(language);
        }

        public IScriptFunctions this[string language]
        {
            get
            {
                return store[language];
            }
        }
    }
}
