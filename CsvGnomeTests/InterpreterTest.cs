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
    public class InterpreterTest
    {
        [TestMethod]
        public void InterpretEmpty()
        {
            var x = NewInterpreter();
            Assert.AreEqual(x.Interpret(String.Empty), CsvGnome.Action.Exit);
        }

        [TestMethod]
        public void InterpretComponents_Text()
        {
            const string ins = "test:meowmeowmeow";
            IComponent[] expected = new IComponent[] { new TextComponent("meowmeowmeow") };

            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            var x = new Interpreter(fieldBrain, reporter);

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_Incrementing()
        {
            const string ins = "test:[++]";
            IComponent[] expected = new IComponent[] { new IncrementingComponent(0) };

            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            var x = new Interpreter(fieldBrain, reporter);

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_Date()
        {
            const string ins = "test:[date]";
            IComponent[] expected = new IComponent[] { new DateComponent() };

            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            var x = new Interpreter(fieldBrain, reporter);

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
            AssertSingleComponentField(fieldBrain, expected);
        }

        private Interpreter NewInterpreter()
        {
            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            return new Interpreter(fieldBrain, reporter);
        }

        private void AssertSingleComponentField(FieldBrain fb, IComponent[] expected)
        {
            Assert.IsTrue(fb.Fields.Count == 1);
            Assert.IsTrue(fb.Fields.First() is ComponentField);
            Assert.IsTrue(fb.Fields.First().Name == "test");
            Assert.AreEqual(expected.Length, (fb.Fields.First() as ComponentField).Components.Length);
            Assert.IsTrue((fb.Fields.First() as ComponentField).Components.Zip(expected, (a, e) => a.Equals(e)).All(r => r == true));
        }
    }
}
