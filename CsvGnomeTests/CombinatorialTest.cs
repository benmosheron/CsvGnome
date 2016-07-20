using CsvGnome;
using CsvGnome.Components.Combinatorial;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvGnomeTests
{
    [TestClass]
    public class CombinatorialTest
    {
        [TestMethod]
        public void TestCombinatorialCycle_Basic()
        {
            Cache cache = new Cache();
            Factory factory = new Factory(cache);

            const string c_groupId = "Siegward";
            // dimension 0
            ArrayCycleComponent raw1 = new ArrayCycleComponent(new string[] { "pickle", "pee" });
            // dimension 1
            ArrayCycleComponent raw2 = new ArrayCycleComponent(new string[] { "pump", "a", "rum" });

            ICombinatorial c1 = factory.Create(c_groupId, raw1);
            ICombinatorial c2 = factory.Create(c_groupId, raw2);

            var c1Expected = new string[] { "pickle", "pee", "pickle", "pee", "pickle", "pee", "pickle" };
            var c2Expected = new string[] { "pump", "pump", "a", "a", "rum", "rum", "pump" };

            for (int i = 0; i < c1Expected.Length; i++)
            {
                Assert.AreEqual(c1Expected[i], (c1 as IComponent).GetValue(i));
                Assert.AreEqual(c2Expected[i], (c2 as IComponent).GetValue(i));
            }
        }
    }
}
