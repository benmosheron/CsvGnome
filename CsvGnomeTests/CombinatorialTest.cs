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
            ArrayCycleComponent raw0 = new ArrayCycleComponent(new string[] { "pickle", "pee" });
            // dimension 1
            ArrayCycleComponent raw1 = new ArrayCycleComponent(new string[] { "pump", "a", "rum" });

            ICombinatorial c0 = factory.Create(c_groupId, raw0);
            ICombinatorial c1 = factory.Create(c_groupId, raw1);

            var c1Expected = new string[] { "pickle", "pee", "pickle", "pee", "pickle", "pee", "pickle" };
            var c2Expected = new string[] { "pump", "pump", "a", "a", "rum", "rum", "pump" };

            for (int i = 0; i < c1Expected.Length; i++)
            {
                Assert.AreEqual(c1Expected[i], (c0 as IComponent).GetValue(i));
                Assert.AreEqual(c2Expected[i], (c1 as IComponent).GetValue(i));
            }
        }

        [TestMethod]
        public void TestCombinatorialCycle_DimensionOf()
        {
            Cache cache = new Cache();
            Factory factory = new Factory(cache);

            const string c_groupId = "Siegward";
            ArrayCycleComponent raw0 = new ArrayCycleComponent(null);

            // Create four components with random ranks. The dimensions should go from lowest to highest.
            ICombinatorial c3 = factory.Create(c_groupId, raw0, 30);
            ICombinatorial c1 = factory.Create(c_groupId, raw0, -99);
            ICombinatorial c0 = factory.Create(c_groupId, raw0, -1000);
            ICombinatorial c2 = factory.Create(c_groupId, raw0, 0);

            Assert.AreEqual(0, c0.Dimension);
            Assert.AreEqual(1, c1.Dimension);
            Assert.AreEqual(2, c2.Dimension);
            Assert.AreEqual(3, c3.Dimension);
        }

        [TestMethod]
        public void TestCombinatorialCycle_GroupCardinality_AllFinite()
        {
            Cache cache = new Cache();
            Factory factory = new Factory(cache);

            const string c_groupId = "Siegward";
            // dimension 0, cardinality 2
            ArrayCycleComponent raw0 = new ArrayCycleComponent(new string[] { "0", "1" });
            // dimension 1, cardinality 3
            ArrayCycleComponent raw1 = new ArrayCycleComponent(new string[] { "0", "1", "2" });
            // dimension 2, cardinality 4
            ArrayCycleComponent raw2 = new ArrayCycleComponent(new string[] { "0", "1", "2", "3" });
            // Create four components with random ranks. The dimensions should go from lowest to highest.
            ICombinatorial c3 = factory.Create(c_groupId, raw0);
            ICombinatorial c1 = factory.Create(c_groupId, raw1);
            ICombinatorial c0 = factory.Create(c_groupId, raw2);

            Assert.IsNotNull(cache[c_groupId].GroupCardinality.Cardinality);
            Assert.IsNull(cache[c_groupId].GroupCardinality.IndexOfFinalDimension);
            Assert.AreEqual(24, cache[c_groupId].GroupCardinality.Cardinality.Value);
        }
    }
}
