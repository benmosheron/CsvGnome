using CsvGnome;
using CsvGnome.Components;
using CsvGnome.Fields;
using CsvGnome.Components.Combinatorial;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CsvGnomeTests
{
    [TestClass]
    public class CombinatorialTest
    {
        [TestMethod]
        public void Cycle_Basic()
        {
            Cache cache = new Cache();
            Factory factory = new Factory(cache);

            const string c_groupId = "Siegward";
            // dimension 0
            ArrayCycleComponent raw0 = new ArrayCycleComponent(new string[] { "pickle", "pee" }, new TestConfigurationProvider());
            // dimension 1
            ArrayCycleComponent raw1 = new ArrayCycleComponent(new string[] { "pump", "a", "rum" }, new TestConfigurationProvider());

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
        public void Cycle_DimensionOf()
        {
            Cache cache = new Cache();
            Factory factory = new Factory(cache);

            const string c_groupId = "Siegward";
            ArrayCycleComponent raw0 = new ArrayCycleComponent(null, new TestConfigurationProvider());
            ArrayCycleComponent raw1 = new ArrayCycleComponent(null, new TestConfigurationProvider());
            ArrayCycleComponent raw2 = new ArrayCycleComponent(null, new TestConfigurationProvider());
            ArrayCycleComponent raw3 = new ArrayCycleComponent(null, new TestConfigurationProvider());

            // Create four components with random ranks. The dimensions should go from lowest to highest.
            ICombinatorial c3 = factory.Create(c_groupId, raw3, 30);
            ICombinatorial c1 = factory.Create(c_groupId, raw1, -99);
            ICombinatorial c0 = factory.Create(c_groupId, raw0, -1000);
            ICombinatorial c2 = factory.Create(c_groupId, raw2, 0);

            Assert.AreEqual(0, c0.Dimension);
            Assert.AreEqual(1, c1.Dimension);
            Assert.AreEqual(2, c2.Dimension);
            Assert.AreEqual(3, c3.Dimension);
        }

        [TestMethod]
        public void Cycle_GroupCardinality_AllFinite()
        {
            Cache cache = new Cache();
            Factory factory = new Factory(cache);

            const string c_groupId = "Siegward";
            // dimension 0, cardinality 2
            ArrayCycleComponent raw0 = new ArrayCycleComponent(new string[] { "0", "1" }, new TestConfigurationProvider());
            // dimension 1, cardinality 3
            ArrayCycleComponent raw1 = new ArrayCycleComponent(new string[] { "0", "1", "2" }, new TestConfigurationProvider());
            // dimension 2, cardinality 4
            ArrayCycleComponent raw2 = new ArrayCycleComponent(new string[] { "0", "1", "2", "3" }, new TestConfigurationProvider());
            // Create four components with random ranks. The dimensions should go from lowest to highest.
            ICombinatorial c0 = factory.Create(c_groupId, raw0);
            ICombinatorial c1 = factory.Create(c_groupId, raw1);
            ICombinatorial c2 = factory.Create(c_groupId, raw2);

            Assert.IsNotNull(cache[c_groupId].GroupCardinality.Cardinality);
            Assert.IsNull(cache[c_groupId].GroupCardinality.IndexOfFirstInfiniteDimension);
            Assert.AreEqual(24, cache[c_groupId].GroupCardinality.Cardinality.Value);
        }

        [TestMethod]
        public void Cycle_Command()
        {
            Cache cache = new Cache();
            Factory factory = new Factory(cache);

            const string c_groupId = "Siegward";
            // dimension 0
            ArrayCycleComponent raw0 = new ArrayCycleComponent(new string[] { "pickle", "pee" }, new TestConfigurationProvider());
            // dimension 1
            ArrayCycleComponent raw1 = new ArrayCycleComponent(new string[] { "pump", "a", "rum" }, new TestConfigurationProvider());

            ICombinatorial c0 = factory.Create(c_groupId, raw0);
            ICombinatorial c1 = factory.Create(c_groupId, raw1);

            string expected0 = "[cycle #Siegward/0]{pickle,pee}";
            string expected1 = "[cycle #Siegward/1]{pump,a,rum}";

            Assert.AreEqual(expected0, (c0 as IComponent).Command);
            Assert.AreEqual(expected1, (c1 as IComponent).Command);
        }

        [TestMethod]
        public void Incrementing_Single()
        {
            Cache cache = new Cache();
            Factory factory = new Factory(cache);

            const string c_groupId = "Sieglinde";
            // dimension 0
            IncrementingComponent raw0 = Utilties.NewIncrementingComponent(0, 1);

            ICombinatorial c0 = factory.Create(c_groupId, raw0);

            string[] expectedValues = new string[] { "0", "1", "2" };

            for (int i = 0; i < expectedValues.Length; i++)
            {
                Assert.AreEqual(expectedValues[i], (c0 as IComponent).GetValue(i));
            }
        }

        [TestMethod]
        public void Incrementing_Command()
        {
            Cache cache = new Cache();
            Factory factory = new Factory(cache);

            const string c_groupId = "Sieglinde";
            // dimension 0
            IncrementingComponent raw0 = Utilties.NewIncrementingComponent(0,1);
            // dimension 1
            IncrementingComponent raw1 = Utilties.NewIncrementingComponent(23,-9);
            IncrementingComponent raw2 = Utilties.NewIncrementingComponent(23, -9, 10);


            ICombinatorial c0 = factory.Create(c_groupId, raw0);
            ICombinatorial c1 = factory.Create(c_groupId, raw1);
            ICombinatorial c2 = factory.Create(c_groupId, raw2);

            string expected0 = "[++ #Sieglinde/0]";
            string expected1 = "[23++-9 #Sieglinde/1]";
            string expected2 = "[23++-9 every 10 #Sieglinde/2]";


            Assert.AreEqual(expected0, (c0 as IComponent).Command);
            Assert.AreEqual(expected1, (c1 as IComponent).Command);
            Assert.AreEqual(expected2, (c2 as IComponent).Command);
        }

        [TestMethod]
        public void IncrementingAfterCycle()
        {
            Cache cache = new Cache();
            Factory factory = new Factory(cache);

            const string c_groupId = "Sieglinde";
            const int c_rows = 100;

            ArrayCycleComponent raw0 = new ArrayCycleComponent(new string[] { "snuggly", "sparkly" }, new TestConfigurationProvider());
            IncrementingComponent raw1 = Utilties.NewIncrementingComponent(0, 1);

            ICombinatorial c0 = factory.Create(c_groupId, raw0);
            ICombinatorial c1 = factory.Create(c_groupId, raw1);

            // c0 (zeroth dimension) should alternate its two possiblities.
            Func<long, string> expected0 = (i) => (i % 2 == 0) ? "snuggly" : "sparkly";
            // c1 (first dimension) should increase every time c0 covers all possiblities
            Func<long, string> expected1 = (i) => (i / 2).ToString((c1 as IncrementingCombinatorial).getFormat());

            for (long i = 0; i < c_rows; i++)
            {
                var expected0i = expected0(i);
                var expected1i = expected1(i);

                Assert.AreEqual(expected0i, ((IComponent)c0).GetValue(i));
                Assert.AreEqual(expected1i, ((IComponent)c1).GetValue(i));
            }
        }

        [TestMethod]
        public void IncrementingAfterTwoCycles()
        {
            Cache cache = new Cache();
            Factory factory = new Factory(cache);

            const string c_groupId = "Sieglinde";
            const int c_rows = 100;

            ArrayCycleComponent raw0 = new ArrayCycleComponent(new string[] { "snuggly", "sparkly" }, new TestConfigurationProvider());
            ArrayCycleComponent raw1 = new ArrayCycleComponent(new string[] { "petrus", "vince", "nico" }, new TestConfigurationProvider());
            IncrementingComponent raw2 = Utilties.NewIncrementingComponent(0, 1);

            ICombinatorial c0 = factory.Create(c_groupId, raw0);
            ICombinatorial c1 = factory.Create(c_groupId, raw1);
            ICombinatorial c2 = factory.Create(c_groupId, raw2);

            // c0 (zeroth dimension) should alternate its two possiblities.
            Func<long, string> expected0 = (i) => (i % 2 == 0) ? "snuggly" : "sparkly";
            // c1 (first dimension) 
            string[] e1Array = new string[] { "petrus", "petrus", "vince", "vince", "nico", "nico" };
            Func<long, string> expected1 = (i) => e1Array[(i % (2 * 3))];
            // c2 should increase every time c0 and c1 cover all possiblities between them
            Func<long, string> expected2 = (i) => (i / (2 * 3)).ToString((c2 as IncrementingCombinatorial).getFormat());

            for (long i = 0; i < c_rows; i++)
            {
                var expected0i = expected0(i);
                var expected1i = expected1(i);
                var expected2i = expected2(i);

                Assert.AreEqual(expected0i, ((IComponent)c0).GetValue(i));
                Assert.AreEqual(expected1i, ((IComponent)c1).GetValue(i));
                Assert.AreEqual(expected2i, ((IComponent)c2).GetValue(i));
            }
        }
    

        [TestMethod]
        public void CycleAfterIncrementing()
        {
            Cache cache = new Cache();
            Factory factory = new Factory(cache);

            const string c_groupId = "Sieglinde";
            const int c_rows = 100;

            IncrementingComponent raw0 = Utilties.NewIncrementingComponent(0, 1);
            ArrayCycleComponent raw1 = new ArrayCycleComponent(new string[] { "snuggly", "sparkly" }, new TestConfigurationProvider());

            ICombinatorial c0 = factory.Create(c_groupId, raw0);
            ICombinatorial c1 = factory.Create(c_groupId, raw1);

            // c0 (zeroth dimension) should increase every time c0 covers all possiblities
            Func<long, string> expected0 = (i) => (i).ToString((c0 as IncrementingCombinatorial).getFormat());
            // c1 (first dimension) is after an infinite dimension, it will stay at the zeroth element
            Func<long, string> expected1 = (i) => "snuggly";

            for (long i = 0; i < c_rows; i++)
            {
                var expected0i = expected0(i);
                var expected1i = expected1(i);

                Assert.AreEqual(expected0i, ((IComponent)c0).GetValue(i));
                Assert.AreEqual(expected1i, ((IComponent)c1).GetValue(i));
            }
        }

        [TestMethod]
        public void IncrementingIntermediateDimension()
        {
            Cache cache = new Cache();
            Factory factory = new Factory(cache);

            const string c_groupId = "Sieglinde";
            const int c_rows = 100;

            ArrayCycleComponent raw0 = new ArrayCycleComponent(new string[] { "snuggly", "sparkly" }, new TestConfigurationProvider());
            ArrayCycleComponent raw1 = new ArrayCycleComponent(new string[] { "petrus", "vince", "nico" }, new TestConfigurationProvider());
            IncrementingComponent raw2 = Utilties.NewIncrementingComponent(0, 1);
            IncrementingComponent raw3 = Utilties.NewIncrementingComponent(11, 1);
            ArrayCycleComponent raw4 = new ArrayCycleComponent(new string[] { "frampt", "kaathe" }, new TestConfigurationProvider());

            ICombinatorial c0 = factory.Create(c_groupId, raw0);
            ICombinatorial c1 = factory.Create(c_groupId, raw1);
            ICombinatorial c2 = factory.Create(c_groupId, raw2);
            ICombinatorial c3 = factory.Create(c_groupId, raw3);
            ICombinatorial c4 = factory.Create(c_groupId, raw4);

            // c0 (zeroth dimension) should alternate its two possiblities.
            Func<long, string> expected0 = (i) => (i % 2 == 0) ? "snuggly" : "sparkly";
            // c1 (first dimension) 
            string[] e1Array = new string[] { "petrus", "petrus", "vince", "vince", "nico", "nico" };
            Func<long, string> expected1 = (i) => e1Array[(i % (2 * 3))];
            // c2 should increase every time c0 and c1 cover all possiblities between them
            Func<long, string> expected2 = (i) => (i / (2 * 3)).ToString((c2 as IncrementingCombinatorial).getFormat());
            // c4 and c5 should stay at element zero.
            Func<long, string> expected3 = (i) => (11).ToString((c2 as IncrementingCombinatorial).getFormat());
            Func<long, string> expected4 = (i) => "frampt";

            for (long i = 0; i < c_rows; i++)
            {
                var expected0i = expected0(i);
                var expected1i = expected1(i);
                var expected2i = expected2(i);
                var expected3i = expected3(i);
                var expected4i = expected4(i);

                Assert.AreEqual(expected0i, ((IComponent)c0).GetValue(i));
                Assert.AreEqual(expected1i, ((IComponent)c1).GetValue(i));
                Assert.AreEqual(expected2i, ((IComponent)c2).GetValue(i));
                Assert.AreEqual(expected3i, ((IComponent)c3).GetValue(i));
                Assert.AreEqual(expected4i, ((IComponent)c4).GetValue(i));
            }
        }

        [TestMethod]
        public void MinMax_Single()
        {
            Cache cache = new Cache();
            Factory factory = new Factory(cache);

            const string c_groupId = "Capra";
            // dimension 0
            MinMaxComponent raw0 = new MinMaxComponent(0, 1);

            ICombinatorial c0 = factory.Create(c_groupId, raw0);

            string[] expectedValues = new string[] { "0", "1" };

            int index = 0;
            int maxIndex = expectedValues.Length - 1;
            for (int i = 0; i < 20; i++)
            {
                var expected = expectedValues[index];
                var actual = (c0 as IComponent).GetValue(i);
                Assert.AreEqual(expected, actual);

                index++;
                if (index > maxIndex) index = 0;
            }
        }

        [TestMethod]
        public void MinMax_Multi()
        {
            Cache cache = new Cache();
            Factory factory = new Factory(cache);

            const string c_groupId = "Capra";
            MinMaxComponent raw0 = new MinMaxComponent(0, 1);
            ArrayCycleComponent raw1 = new ArrayCycleComponent(new string[] {"washing", "pole" }, new TestConfigurationProvider());
            MinMaxComponent raw2 = new MinMaxComponent(0, 4, 2);

            ICombinatorial c0 = factory.Create(c_groupId, raw0);
            ICombinatorial c1 = factory.Create(c_groupId, raw1);
            ICombinatorial c2 = factory.Create(c_groupId, raw2);

            string[] expectedValues0 = new string[] { "0", "1", "0", "1", "0", "1", "0", "1", "0", "1", "0", "1" };
            string[] expectedValues1 = new string[] { "washing", "washing", "pole", "pole", "washing", "washing", "pole", "pole", "washing", "washing", "pole", "pole" };
            string[] expectedValues2 = new string[] { "0", "0", "0", "0", "2", "2", "2", "2", "4", "4", "4", "4" };

            int index = 0;
            int maxIndex = expectedValues0.Length - 1;
            for (int i = 0; i < 20; i++)
            {
                Assert.AreEqual(expectedValues0[index], (c0 as IComponent).GetValue(i));
                Assert.AreEqual(expectedValues1[index], (c1 as IComponent).GetValue(i));
                Assert.AreEqual(expectedValues2[index], (c2 as IComponent).GetValue(i));

                index++;
                if (index > maxIndex) index = 0;
            }
        }


        [TestMethod]
        public void MinMax_Command()
        {
            Cache cache = new Cache();
            Factory factory = new Factory(cache);

            const string c_groupId = "Capra";
            // dimension 0
            MinMaxComponent raw0 = new MinMaxComponent(0, 1);
            MinMaxComponent raw1 = new MinMaxComponent(0, -1);
            MinMaxComponent raw2 = new MinMaxComponent(0, -10, -3);

            ICombinatorial c0 = factory.Create(c_groupId, raw0);
            ICombinatorial c1 = factory.Create(c_groupId, raw1);
            ICombinatorial c2 = factory.Create(c_groupId, raw2);

            string expectedCommand0 = $"[0=>1,1 #{c_groupId}/0]";
            string expectedCommand1 = $"[0=>-1,-1 #{c_groupId}/1]";
            string expectedCommand2 = $"[0=>-10,-3 #{c_groupId}/2]";

            Assert.AreEqual(expectedCommand0, (c0 as IComponent).Command);
            Assert.AreEqual(expectedCommand1, (c1 as IComponent).Command);
            Assert.AreEqual(expectedCommand2, (c2 as IComponent).Command);
        }

    }
}
