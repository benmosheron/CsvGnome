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
            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);
            Assert.AreEqual(x.Interpret(String.Empty), new GnomeActionInfo(GnomeAction.Continue));
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretExit()
        {
            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);
            Assert.AreEqual(x.Interpret("exit"), new GnomeActionInfo(GnomeAction.Exit));
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretRun()
        {
            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);
            Assert.AreEqual(x.Interpret("run"), new GnomeActionInfo(GnomeAction.Run));
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretWrite()
        {
            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);
            Assert.AreEqual(x.Interpret("write"), new GnomeActionInfo(GnomeAction.Write));
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretHelp()
        {
            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);
            Assert.AreEqual(x.Interpret("help"), new GnomeActionInfo(GnomeAction.Help));
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretHelpSpecial()
        {
            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);
            Assert.AreEqual(x.Interpret("help special"), new GnomeActionInfo(GnomeAction.HelpSpecial));
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretSave()
        {
            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);
            Assert.AreEqual(x.Interpret("save fileNameOMGWOW"), new GnomeActionInfo(GnomeAction.Save, "fileNameOMGWOW"));
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretComponents_Text()
        {
            const string ins = "test:meowmeowmeow";
            IComponent[] expected = new IComponent[] { new TextComponent("meowmeowmeow") };

            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);

            Assert.AreEqual(x.Interpret(ins), new GnomeActionInfo(GnomeAction.Continue));
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_EmptyText()
        {
            const string ins = "test:";
            IComponent[] expected = new IComponent[] { new TextComponent(String.Empty) };

            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);

            Assert.AreEqual(x.Interpret(ins), new GnomeActionInfo(GnomeAction.Continue));
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_Incrementing()
        {
            const string ins = "test:[++]";
            IComponent[] expected = new IComponent[] { new IncrementingComponent(0) };

            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);

            Assert.AreEqual(x.Interpret(ins), new GnomeActionInfo(GnomeAction.Continue));
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_IncrementingWithStart()
        {
            const string ins = "test:[39++]";
            IComponent[] expected = new IComponent[] { new IncrementingComponent(39, 1) };

            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);

            Assert.AreEqual(x.Interpret(ins), new GnomeActionInfo(GnomeAction.Continue));
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_IncrementingWithIncrement()
        {
            const string ins = "test:[++254]";
            IComponent[] expected = new IComponent[] { new IncrementingComponent(0, 254) };

            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);

            Assert.AreEqual(x.Interpret(ins), new GnomeActionInfo(GnomeAction.Continue));
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_IncrementingBoth()
        {
            const string ins = "test:[718++218]";
            IComponent[] expected = new IComponent[] { new IncrementingComponent(718, 218) };

            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);

            Assert.AreEqual(x.Interpret(ins), new GnomeActionInfo(GnomeAction.Continue));
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_IncrementingNegative()
        {
            const string ins = "test:[++][++-11][-110++-2]";
            IComponent[] expected = new IComponent[] 
            {
                new IncrementingComponent(0),
                new IncrementingComponent(0, -11),
                new IncrementingComponent(-110, -2)
            };

            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);

            Assert.AreEqual(x.Interpret(ins), new GnomeActionInfo(GnomeAction.Continue));
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_IncrementingSpaces()
        {
            const string ins = "test:[-110++-2][ -110 ++ -2 ]";
            IComponent[] expected = new IComponent[]
            {
                new IncrementingComponent(-110, -2),
                new IncrementingComponent(-110, -2)
            };

            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);

            Assert.AreEqual(x.Interpret(ins), new GnomeActionInfo(GnomeAction.Continue));
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_Date()
        {
            const string ins = "test:[date]";
            IComponent[] expected = new IComponent[] { new DateComponent() };

            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);

            Assert.AreEqual(x.Interpret(ins), new GnomeActionInfo(GnomeAction.Continue));
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_MinMax()
        {
            const string ins = "test:[0=>10]";
            IComponent[] expected = new IComponent[] { new MinMaxComponent(0,10) };

            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);

            Assert.AreEqual(x.Interpret(ins), new GnomeActionInfo(GnomeAction.Continue));
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_MinMaxIncrement()
        {
            const string ins = "test:[1=>11,2]";
            IComponent[] expected = new IComponent[] { new MinMaxComponent(1, 11, 2) };

            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);

            Assert.AreEqual(x.Interpret(ins), new GnomeActionInfo(GnomeAction.Continue));
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_MinMaxId()
        {
            const string ins = "test:[1=>3 #testId]";

            MinMaxInfoCache temp = new MinMaxInfoCache();
            IComponent[] expected = new IComponent[] { new MinMaxComponent(1, 3, "testId", temp) };

            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);

            Assert.AreEqual(x.Interpret(ins), new GnomeActionInfo(GnomeAction.Continue));
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_MinMaxIncrementId()
        {
            const string ins = "test:[11=>21,2#testId]";
            MinMaxInfoCache temp = new MinMaxInfoCache();
            IComponent[] expected = new IComponent[] { new MinMaxComponent(11, 21, 2, "testId", temp) };

            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);

            Assert.AreEqual(x.Interpret(ins), new GnomeActionInfo(GnomeAction.Continue));
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_MinMaxSpaces()
        {
            const string ins = "test:[ 11 => 21 , 2 #testId ]";
            MinMaxInfoCache temp = new MinMaxInfoCache();
            IComponent[] expected = new IComponent[] { new MinMaxComponent(11, 21, 2, "testId", temp) };

            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);

            Assert.AreEqual(x.Interpret(ins), new GnomeActionInfo(GnomeAction.Continue));
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_MinMaxSameId()
        {
            const string ins = "test:[1=>9,2 #testId][0=>9 #testId][0=>1 #testId]";
            MinMaxInfoCache temp = new MinMaxInfoCache();
            IComponent[] expected = new IComponent[]
            {
                new MinMaxComponent(1, 9, 2, "testId", temp),
                new MinMaxComponent(0, 9, "testId", temp),
                new MinMaxComponent(0, 1, "testId", temp)
            };

            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);

            Assert.AreEqual(x.Interpret(ins), new GnomeActionInfo(GnomeAction.Continue));
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_Compound1()
        {
            const string ins = "test:1[date]meow[++]xxx";
            IComponent[] expected = new IComponent[]
            {
                new TextComponent("1"),
                new DateComponent(),
                new TextComponent("meow"),
                new IncrementingComponent(0),
                new TextComponent("xxx")
            };

            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);

            Assert.AreEqual(x.Interpret(ins), new GnomeActionInfo(GnomeAction.Continue));
            AssertSingleComponentField(fieldBrain, expected);
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
