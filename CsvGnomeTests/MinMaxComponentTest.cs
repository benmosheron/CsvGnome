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
    public class MinMaxComponentTest
    {
        [TestMethod]
        public void GetValue_ZeroToTwo()
        {
            const int N = 3;
            const int start = 0;
            const int end = 2;
            MinMaxComponent m = new MinMaxComponent(start, end);

            for (int i = 0; i < N; i++)
            {
                Assert.AreEqual(i, int.Parse(m.GetValue(i)));
            }
        }

        [TestMethod]
        public void GetValue_ZeroToTwoMoreLines()
        {
            const int N = 100;
            const int start = 0;
            const int end = 2;
            MinMaxComponent m = new MinMaxComponent(start,end);

            // Explicitely state expected pattern to actually test GetValue logic
            int[] expect = { 0, 1, 2 };
            int index = 0;
            int indexMax = expect.Length - 1;

            for (int i = 0; i < N; i++)
            {
                // expect 0,1,2,0,1,2,0,...
                int expected = expect[index];
                int actual = int.Parse(m.GetValue(i));
                Assert.AreEqual(expected, actual);
                index++;
                if (index > indexMax) index = 0;
            }
        }

        [TestMethod]
        public void GetValue_OneToThreeMoreLines()
        {
            const int N = 100;
            const int start = 1;
            const int end = 3;
            MinMaxComponent m = new MinMaxComponent(start, end);

            // Explicitely state expected pattern to actually test GetValue logic
            int[] expect = { 1, 2, 3 };
            int index = 0;
            int indexMax = expect.Length - 1;

            for (int i = 0; i < N; i++)
            {
                // expect 0,1,2,0,1,2,0,...
                int expected = expect[index];
                int actual = int.Parse(m.GetValue(i));
                Assert.AreEqual(expected, actual);
                index++;
                if (index > indexMax) index = 0;
            }
        }

        [TestMethod]
        public void GetValue_OneToNineIncTwo()
        {
            const int N = 100;
            const int start = 1;
            const int end = 9;
            const int inc = 2;
            MinMaxComponent m = new MinMaxComponent(start, end, inc);

            // Explicitely state expected pattern to actually test GetValue logic
            int[] expect = { 1, 3, 5, 7, 9 };
            int index = 0;
            int indexMax = expect.Length - 1;

            for (int i = 0; i < N; i++)
            {
                int expected = expect[index];
                int actual = int.Parse(m.GetValue(i));
                Assert.AreEqual(expected, actual);
                index++;
                if (index > indexMax) index = 0;
            }
        }

        [TestMethod]
        public void GetValue_CombinedOneToFiveIncTwo()
        {
            const int N = 100;
            const int start = 1;
            const int end = 5;
            const int inc = 2;
            const string id = "test";

            MinMaxInfoCache cache = new MinMaxInfoCache();
            MinMaxComponent m1 = new MinMaxComponent(start, end, inc, id, cache);
            MinMaxComponent m2 = new MinMaxComponent(start, end, inc, id, cache);

            // Explicitely state expected pattern to actually test GetValue logic
            int[] expect1 = { 1, 1, 1, 3, 3, 3, 5, 5, 5 };
            int[] expect2 = { 1, 3, 5, 1, 3, 5, 1, 3, 5 };
            int index = 0;
            int indexMax = expect1.Length - 1;

            for (int i = 0; i < N; i++)
            {
                int expected1 = expect1[index];
                int actual1 = int.Parse(m1.GetValue(i));
                Assert.AreEqual(expected1, actual1);

                int expected2 = expect2[index];
                int actual2 = int.Parse(m2.GetValue(i));
                Assert.AreEqual(expected2, actual2);

                index++;
                if (index > indexMax) index = 0;
            }
        }
    }
}
