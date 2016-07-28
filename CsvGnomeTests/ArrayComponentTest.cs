using CsvGnome;
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
            string expectedCommand = Program.SpreadComponentString + "{one,two,three,four,five}";
            ArraySpreadComponent s = new ArraySpreadComponent(Values);
            Assert.AreEqual(expectedCommand, s.Command);
        }

        [TestMethod]
        public void ArrayComponent_CycleCommand()
        {
            string expectedCommand = Program.CycleComponentString + "{one,two,three,four,five}";
            ArrayCycleComponent c = new ArrayCycleComponent(Values);
            Assert.AreEqual(expectedCommand, c.Command);
        }

        [TestMethod]
        public void ArrayComponent_SpreadEquals()
        {
            ArraySpreadComponent s1 = new ArraySpreadComponent(Values);
            ArraySpreadComponent s2 = new ArraySpreadComponent(Values);
            // Not same instance
            Assert.AreNotEqual(s1, s2);
            Assert.IsTrue(s1.EqualsComponent(s2));
        }

        [TestMethod]
        public void ArrayComponent_CycleEquals()
        {
            ArrayCycleComponent c1 = new ArrayCycleComponent(Values);
            ArrayCycleComponent c2 = new ArrayCycleComponent(Values);
            Assert.AreNotEqual(c1, c2);
            Assert.IsTrue(c1.EqualsComponent(c2));
        }

        [TestMethod]
        public void ArrayComponent_SpreadSummary()
        {
            List<Message> expectedSummary = new List<Message>()
            {
                new Message(Program.SpreadComponentString,
                Program.SpecialColour),
                new Message("{"),
                new Message("5 items", Program.SpecialColour),
                new Message("}")
            };

            var s = new ArraySpreadComponent(Values);
            Assert.IsTrue(s.Summary.Zip(expectedSummary, (a, e) => a.EqualsMessage(e)).All(z => z));
        }

        [TestMethod]
        public void ArrayComponent_CycleSummary()
        {
            List<Message> expectedSummary = new List<Message>()
            {
                new Message(Program.CycleComponentString,
                Program.SpecialColour),
                new Message("{"),
                new Message("5 items", Program.SpecialColour),
                new Message("}")
            };

            var c = new ArrayCycleComponent(Values);
            Assert.IsTrue(c.Summary.Zip(expectedSummary, (a, e) => a.EqualsMessage(e)).All(z => z));
        }

        [TestMethod]
        public void ArrayComponent_SpreadGetValue()
        {
            int n = 9;
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
            var s = new ArraySpreadComponent(Values);
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
            var s = new ArrayCycleComponent(Values);
            for (int i = 0; i < n; i++)
            {
                Assert.AreEqual(expected[i], s.GetValue(i));
            }
        }
    }
}
