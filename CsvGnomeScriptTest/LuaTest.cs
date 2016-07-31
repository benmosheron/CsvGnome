using CsvGnomeScript;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvGnomeScriptTest
{
    [TestClass]
    public class LuaTest
    {
        [TestMethod]
        public void ScriptReader()
        {
            const string c_testFileName = "functions.lua";

            Dictionary<string, Func<long, string>> expected = new Dictionary<string, Func<long, string>>
            {
                ["getOne"] = i => 1.ToString(),
                ["getChaosBlade"] = i => "ChaosBlade",
                ["getI"] = i => i.ToString()
            };

            LuaScriptReader reader = new LuaScriptReader();
            LuaScriptFunctions f = reader.Read(c_testFileName);

            foreach (string functionName in expected.Keys)
            {
                long i = 99;
                Assert.AreEqual(
                    expected[functionName](i),
                    f.GetValue(functionName, i));
            }
        }
    }
}
