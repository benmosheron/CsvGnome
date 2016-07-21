﻿using CsvGnome;
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

            ICombinatorial c0 = factory.Create(c_groupId, raw1);
            ICombinatorial c1 = factory.Create(c_groupId, raw2);

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
            // dimension 0
            ArrayCycleComponent raw1 = new ArrayCycleComponent(null);
            // dimension 1
            ArrayCycleComponent raw2 = new ArrayCycleComponent(null);

            // Create four components with random ranks. The dimensions should go from lowest to highest.
            ICombinatorial c3 = factory.Create(c_groupId, raw2, 30);
            ICombinatorial c1 = factory.Create(c_groupId, raw1, -99);
            ICombinatorial c0 = factory.Create(c_groupId, raw2, -1000);
            ICombinatorial c2 = factory.Create(c_groupId, raw2, 0);

            Assert.AreEqual(0, c0.Dimension);
            Assert.AreEqual(1, c1.Dimension);
            Assert.AreEqual(2, c2.Dimension);
            Assert.AreEqual(3, c3.Dimension);
        }
    }
}
