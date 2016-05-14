using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CsvGnome;

namespace CsvGnomeTests
{
    [TestClass]
    public class MinMaxInfoCacheTest
    {
        [TestMethod]
        public void UpdateCacheForDelete()
        {
            // Create cache with three components
            MinMaxInfoCache cache = new MinMaxInfoCache();
            MinMaxComponent c1 = new MinMaxComponent(0, 10, "id", cache);
            MinMaxComponent c2 = new MinMaxComponent(10, 20, "id", cache);
            MinMaxComponent c3 = new MinMaxComponent(20, 30, "id", cache);

            Assert.AreEqual(3, cache.Components.Count);

            // delete the middle
            cache.UpdateCacheForDelete(new MinMaxComponent[]{ c2 });

            int[] expectedMins = new int[] { 0, 20 };
            int[] expectedMaxs = new int[] { 10, 30 };
            int[] expectedIncrements = new int[] { 1, 1 };
            int[] expectedCardinalities = new int[] { 11, 11 };
            Assert.IsTrue(cache.Cache.Count == 1);
            Assert.IsTrue(cache.Cache["id"].Dims == 2);
            Assert.IsTrue(cache.Cache["id"].Mins.SequenceEqual(expectedMins));
            Assert.IsTrue(cache.Cache["id"].Maxs.SequenceEqual(expectedMaxs));
            Assert.IsTrue(cache.Cache["id"].Increments.SequenceEqual(expectedIncrements));
            Assert.IsTrue(cache.Cache["id"].Cardinalities.SequenceEqual(expectedCardinalities));
            Assert.AreEqual(2, cache.Components.Count);

        }
    }
}
