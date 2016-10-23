using CsvGnomeScriptApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaScript
{
    public class Functions : IScriptFunctions
    {
        public Functions()
        {
            ValueFunctions = new Dictionary<string, Func<IScriptArgs, object[]>>();
        }

        public Dictionary<string, Func<IScriptArgs, object[]>> ValueFunctions { get; }
        public bool ContainsFunction(string name) => ValueFunctions.ContainsKey(name);
        public void Combine(IScriptFunctions functionsToAdd)
        {
            foreach (var kvp in functionsToAdd.ValueFunctions)
            {
                if (ContainsFunction(kvp.Key)) throw new ArgumentException($"A function with name [{kvp.Key}] already exists.");

                ValueFunctions.Add(kvp.Key, kvp.Value);
            }
        }
    }
}
