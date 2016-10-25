using CsvGnome;
using CsvGnome.Components;
using CsvGnome.Fields;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeTests
{
    [TestClass]
    public class ArrayComponentTest
    {
        private readonly string[] Values = new string[]
        {
            "one",
            "two",
            "three",
            "four",
            "five"
        };


        [TestMethod]
        public void ArrayComponent_SpreadCommand()
        {
            string expectedCommand = ArraySpreadComponent.CommandInitString + "{one,two,three,four,five}";
            ArraySpreadComponent s = NewSpread();
            Assert.AreEqual(expectedCommand, s.Command);
        }

        [TestMethod]
        public void ArrayComponent_CycleCommand()
        {
            string expectedCommand = ArrayCycleComponent.CommandInitString + "{one,two,three,four,five}";
            ArrayCycleComponent c = NewCycle();
            Assert.AreEqual(expectedCommand, c.Command);
        }

        [TestMethod]
        public void ArrayComponent_SpreadEquals()
        {
            ArraySpreadComponent s1 = NewSpread();
            ArraySpreadComponent s2 = NewSpread();
            // Not same instance
            Assert.AreNotEqual(s1, s2);
            Assert.IsTrue(s1.EqualsComponent(s2));
        }

        [TestMethod]
        public void ArrayComponent_CycleEquals()
        {
            ArrayCycleComponent c1 = NewCycle();
            ArrayCycleComponent c2 = NewCycle();
            Assert.AreNotEqual(c1, c2);
            Assert.IsTrue(c1.EqualsComponent(c2));
        }

        [TestMethod]
        public void ArrayComponent_SpreadSummary()
        {
            List<TestMessage> expectedSummary = new List<TestMessage>()
            {
                TestMessage.NewSpecial(ArraySpreadComponent.CommandInitString),
                new TestMessage("{"),
                TestMessage.NewSpecial("5 items"),
                new TestMessage("}")
            };

            var s = NewSpread();
            Assert.IsTrue(s.Summary.Zip(expectedSummary, (a, e) => (a as TestMessage).EqualsMessage(e)).All(z => z));
        }

        [TestMethod]
        public void ArrayComponent_CycleSummary()
        {
            List<TestMessage> expectedSummary = new List<TestMessage>()
            {
                TestMessage.NewSpecial(ArrayCycleComponent.CommandInitString),
                new TestMessage("{"),
                TestMessage.NewSpecial("5 items"),
                new TestMessage("}")
            };

            var c = NewCycle();
            Assert.IsTrue(c.Summary.Zip(expectedSummary, (a, e) => (a as TestMessage).EqualsMessage(e)).All(z => z));
        }

        [TestMethod]
        public void ArrayComponent_SpreadGetValue()
        {
            int n = 9;

            var ctx = new TestContext();
            ctx.N = n;

            string[] expected = new string[]
            {
                "one",
                "one",
                "two",
                "two",
                "three",
                "three",
                "four",
                "four",
                "five"
            };

            var s = NewSpread(ctx);
            for (int i = 0; i < n; i++)
            {
                Assert.AreEqual(expected[i], s.GetValue(i));
            }
        }

        [TestMethod]
        public void ArrayComponent_CycleGetValue()
        {
            int n = 9;
            string[] expected = new string[]
            {
                "one",
                "two",
                "three",
                "four",
                "five",
                "one",
                "two",
                "three",
                "four"
            };
            var s = NewCycle();
            for (int i = 0; i < n; i++)
            {
                Assert.AreEqual(expected[i], s.GetValue(i));
            }
        }

        private ArraySpreadComponent NewSpread()
        {
            return NewSpread(new TestContext());
        }

        private ArraySpreadComponent NewSpread(IContext ctx)
        {
            return new ArraySpreadComponent(Values, ctx, new TestConfigurationProvider());
        }

        private ArrayCycleComponent NewCycle()
        {
            return new ArrayCycleComponent(Values, new TestConfigurationProvider());
        }
    }
}
