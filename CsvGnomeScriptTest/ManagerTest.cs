using CsvGnomeScript;
using CsvGnomeScriptApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvGnomeScriptTest
{
    [TestClass]
    public class ManagerTest
    {
        [TestMethod]
        public void Lua()
        {
            const string c_testFileName = "functions.lua";

            Dictionary<string, Func<IScriptArgs, string>> expected = new Dictionary<string, Func<IScriptArgs, string>>
            {
                ["getOne"] = args => 1.ToString(),
                ["getChaosBlade"] = args => "ChaosBlade",
                ["getI"] = args => args.i.ToString(),
                ["rowXOfY"] = args => $"row {args.i + 1} of {args.N}"
            };

            Manager m = new Manager();
            m.ReadFile(c_testFileName);

            foreach (string functionName in expected.Keys)
            {
                LuaScript.Args args = new LuaScript.Args();
                args.i = 3;
                args.N = 10;
                Assert.AreEqual(
                    expected[functionName](args),
                    m.GetValueFunction("lua",functionName)(args).First().ToString());
            }
        }
    }
}
