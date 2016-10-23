using CsvGnomeScriptApi;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CsvGnomeScriptTest
{
    [TestClass]
    public class LuaTest
    {
        [TestMethod]
        public void ScriptReader()
        {
            const string c_testFileName = "functions.lua";

            Dictionary<string, Func<IScriptArgs, string>> expected = new Dictionary<string, Func<IScriptArgs, string>>
            {
                ["getOne"] = args => 1.ToString(),
                ["getChaosBlade"] = args => "ChaosBlade",
                ["getI"] = args => args.i.ToString(),
                ["rowXOfY"] = args => $"row {args.i + 1} of {args.N}"
            };

            IScriptReader reader = new LuaScript.Reader();
            IScriptFunctions f = reader.Read(c_testFileName);

            foreach (string functionName in expected.Keys)
            {
                IScriptArgs args = new LuaScript.Args();
                args.i = 99;
                args.N = 100;
                Assert.AreEqual(
                    expected[functionName](args),
                    f.ValueFunctions[functionName](args).First().ToString());
            }
        }
    }
}
