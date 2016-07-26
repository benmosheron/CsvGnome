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
        public void Negative()
        {
            const int N = 100;
            const int start = 0;
            const int end = -2;
            MinMaxComponent m = new MinMaxComponent(start, end);

            // Explicitely state expected pattern to actually test GetValue logic
            int[] expect = { 0, -1, -2 };
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
        public void NegativeIncrement()
        {
            const int N = 100;
            const int start = 0;
            const int end = -4;
            const int increment = -2;
            MinMaxComponent m = new MinMaxComponent(start, end, increment);

            // Explicitely state expected pattern to actually test GetValue logic
            int[] expect = { 0, -2, -4 };
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
        public void ExceptionForInfinite()
        {
            const int N = 100;
            const int start = 0;
            const int end = -2;
            const int increment = 1;

            try
            {
                MinMaxComponent m = new MinMaxComponent(start, end, increment);
                Assert.Fail("Expected ArgumentOutOfRangeException");
            }
            catch (ArgumentOutOfRangeException)
            {
                // Success!
            }
        }

        [TestMethod]
        public void ExceptionForInfiniteNegativeIncrement()
        {
            const int N = 100;
            const int start = 0;
            const int end = 2;
            const int increment = -1;

            try
            {
                MinMaxComponent m = new MinMaxComponent(start, end, increment);
                Assert.Fail("Expected ArgumentOutOfRangeException");
            }
            catch (ArgumentOutOfRangeException)
            {
                // Success!
            }
        }


    }
}
