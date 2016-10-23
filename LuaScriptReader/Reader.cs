using NLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CsvGnomeScriptApi;

namespace LuaScript
{
    /// <summary>
    /// Reads the specified lua script file and extracts valid functions, which take a single parameter "i".
    /// Functions declarations must be in the exact form: 
    /// <para>function nameOfFunction(i)</para>
    /// <para>[body]</para>
    /// <para>return [returnValue]</para>
    /// <para>end</para>
    /// </summary>
    /// <remarks>
    /// Does not swallow any exceptions (e.g. file access exceptions).
    /// </remarks>
    public class Reader : IScriptReader
    {
        public const string lua = "lua";

        /// <summary>
        /// "lua"
        /// </summary>
        public string Extension => lua;

        /// <summary>
        /// Read a lua script file and gather valid functions into a LuaScriptFunctions object.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IScriptFunctions Read(string path)
        {
            string luaScript = GetScript(path);

            // Get the every possible valid function by examining the text of the file.
            HashSet<string> functions = GetValidFunctionNames(luaScript);

            // Read everything from the file, using the NLua interpreter.
            Lua state = GetLuaState(luaScript);

            IScriptFunctions luaScriptFunctions = new Functions();

            // Match the text-read and state-read functions.
            foreach (string functionName in functions)
            {
                luaScriptFunctions.ValueFunctions[functionName] = i => state.GetFunction(functionName).Call(i);
            }

            return luaScriptFunctions;
        }

        /// <summary>
        /// Read the file into a string in memory.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string GetScript(string path)
        {
            return System.IO.File.ReadAllText(path);
        }

        HashSet<string> GetValidFunctionNames(string script)
        {
            Regex luaFunction = new Regex(@"(function )((?:[a-zA-Z]|_)\w*)(\(i\))");
            var functions = luaFunction.Matches(script);
            var functionNames = new HashSet<string>();
            for (int i = 0; i < functions.Count; i++)
            {
                // Group will be
                // 0: full regex match
                // 1: "function "
                // 2: <functionName>
                // 3: "(i)"
                // we are after 2.
                functionNames.Add(functions[i].Groups[2].Value);
            }
            return functionNames;
        }

        Lua GetLuaState(string script)
        {
            Lua state = new Lua();
            state.DoString(script);
            return state;
        }
    }
}
