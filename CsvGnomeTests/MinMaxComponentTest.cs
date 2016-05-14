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
            const int N = 10;
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
            int indexMax = 2;

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
            int indexMax = 2;

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
    }
}
