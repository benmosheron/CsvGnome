using CsvGnome;
using CsvGnome.Fields;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvGnomeTests
{
    [TestClass]
    public class PaddedFieldTest
    {
        private static IComponent[] components = new IComponent[]
        {
            // Use an array component which will have different max lengths for when N = 1 -> 5 (i = 0 -> 4).
            new ArrayCycleComponent(new string[]{"", "x", "xx", "xxx", "xxxx"}, new TestConfigurationProvider())
        };

        private ComponentField InnerField = new ComponentField("test", components);

        private PaddedField Get()
        {
            return new PaddedField(InnerField);
        }

        [TestMethod]
        public void TestPadding()
        {
            var expected = new Dictionary<int, string>()
            {
                {0, "    "},
                {1, "x   "},
                {2, "xx  "},
                {3, "xxx "},
                {4, "xxxx"}
            };

            int N = expected.Count;

            PaddedField f = Get();
            
            f.CalculateMaxLength(N);

            for (int i = 0; i < N; i++)
            {
                Assert.AreEqual(expected[i], f.GetValue(i));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(PaddedLengthNotCalculatedException))]
        public void ThrowsIfMaxNotCalculated()
        {
            PaddedField f = Get();
            f.GetValue(0);
        }

        [TestMethod]
        [ExpectedException(typeof(PaddedLengthExceededException))]
        public void ThrowsIfMaxExceeded()
        {
            PaddedField f = Get();

            f.CalculateMaxLength(1); //0

            f.GetValue(1); // "x", which has length 1
        }
    }
}
