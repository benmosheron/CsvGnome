using CsvGnomeScript;
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

            Dictionary<string, Func<long, string>> expected = new Dictionary<string, Func<long, string>>
            {
                ["getOne"] = i => 1.ToString(),
                ["getChaosBlade"] = i => "ChaosBlade",
                ["getI"] = i => i.ToString()
            };

            Manager m = new Manager();
            m.ReadFile(c_testFileName);

            foreach (string functionName in expected.Keys)
            {
                long i = 99;
                Assert.AreEqual(
                    expected[functionName](i),
                    m.GetValueFunction("lua",functionName)(i).First().ToString());
            }
        }
    }
}
