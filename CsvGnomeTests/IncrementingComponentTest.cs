using CsvGnome;
using CsvGnome.Components;
using CsvGnome.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvGnomeTests
{
    [TestClass]
    public class IncrementingComponentTest
    {
        [TestMethod]
        public void Basic()
        {
            const long n = 100;
            // Shortcut to a component's format function.
            Func<IncrementingComponent, long, string> f = (c, i) => i.ToString(c.getFormat());

            List<IncrementingComponent> testComponents = new List<IncrementingComponent>();
            List<Func<long, int, string>> expectedValueGenerators = new List<Func<long, int, string>>();

            // Just a start provided
            testComponents.Add(Utilties.NewIncrementingComponent(0));
            expectedValueGenerators.Add((i, componentNumber) => f(testComponents[componentNumber], i));

            testComponents.Add(Utilties.NewIncrementingComponent(100));
            expectedValueGenerators.Add((i, componentNumber) => f(testComponents[componentNumber], i + 100));

            testComponents.Add(Utilties.NewIncrementingComponent(-99));
            expectedValueGenerators.Add((i, componentNumber) => f(testComponents[componentNumber], i - 99));

            // Start and increment provided
            testComponents.Add(Utilties.NewIncrementingComponent(-99, -1));
            expectedValueGenerators.Add((i, componentNumber) => f(testComponents[componentNumber], -i - 99));

            testComponents.Add(Utilties.NewIncrementingComponent(99, -2));
            expectedValueGenerators.Add((i, componentNumber) => f(testComponents[componentNumber], -(2 * i) + 99));

            testComponents.Add(Utilties.NewIncrementingComponent(99, 2));
            expectedValueGenerators.Add((i, componentNumber) => f(testComponents[componentNumber], (2 * i) + 99));

            // Start, increment and every provided
            testComponents.Add(Utilties.NewIncrementingComponent(1, 1, 5));
            expectedValueGenerators.Add((i, componentNumber) => f(testComponents[componentNumber], i/5 + 1));

            testComponents.Add(Utilties.NewIncrementingComponent(-100, 7, 3));
            expectedValueGenerators.Add((i, componentNumber) => f(testComponents[componentNumber], ((i / 3) * 7) - 100));

            for (int componentNumber = 0; componentNumber < testComponents.Count; componentNumber++)
            {
                for (long i = 0; i < n; i++)
                {
                    string expected = expectedValueGenerators[componentNumber](i, componentNumber);
                    string actual = testComponents[componentNumber].GetValue(i);
                    Assert.AreEqual(expected, actual);
                }
            }
        }

        [TestMethod]
        public void Format()
        {
            // Dependent on N
            IContext ctx = new TestContext();
            ctx.N = 100;

            List<IncrementingComponent> testComponents = new List<IncrementingComponent>();
            List<string> expectedFormats = new List<string>();

            //00 - 99
            testComponents.Add(Utilties.NewIncrementingComponent(ctx, 0));
            expectedFormats.Add("D2");

            //001 - 100
            testComponents.Add(Utilties.NewIncrementingComponent(ctx, 1));
            expectedFormats.Add("D3");

            //-99 - 00
            testComponents.Add(Utilties.NewIncrementingComponent(ctx, -99));
            expectedFormats.Add("D2");

            //-10,-10,0,0,10,10,...,480
            testComponents.Add(Utilties.NewIncrementingComponent(ctx, -10, 10, 2));
            expectedFormats.Add("D3");

            for (int componentNumber = 0; componentNumber < testComponents.Count; componentNumber++)
            {
                string expected = expectedFormats[componentNumber];
                string actual = testComponents[componentNumber].getFormat();
                Assert.AreEqual(expected, actual);
            }
        }
    }
}
