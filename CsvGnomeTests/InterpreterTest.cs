﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Assert.AreEqual(x.Interpret(String.Empty), CsvGnome.Action.Continue);
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretExit()
        {
            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);
            Assert.AreEqual(x.Interpret("exit"), CsvGnome.Action.Exit);
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretRun()
        {
            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);
            Assert.AreEqual(x.Interpret("run"), CsvGnome.Action.Run);
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretWrite()
        {
            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);
            Assert.AreEqual(x.Interpret("write"), CsvGnome.Action.Write);
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretHelp()
        {
            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);
            Assert.AreEqual(x.Interpret("help"), CsvGnome.Action.Help);
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretHelpSpecial()
        {
            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);
            Assert.AreEqual(x.Interpret("help special"), CsvGnome.Action.HelpSpecial);
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretShowGnomeFiles()
        {
            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);
            Assert.AreEqual(x.Interpret("gnomefiles"), CsvGnome.Action.ShowGnomeFiles);
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretSave()
        {
            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);
            Assert.AreEqual(CsvGnome.Action.Continue, x.Interpret("save test"));
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretLoad()
        {
            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);
            Assert.AreEqual(CsvGnome.Action.Continue, x.Interpret("load test"));
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretClear()
        {
            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);
            // Add some fields
            x.Interpret("woo:hoo");
            x.Interpret("x:[1=>3 #p]");
            x.Interpret("y:[1=>3 #p]");
            Assert.IsTrue(cache.Cache.Keys.Count == 1);
            Assert.IsTrue(fieldBrain.Fields.Count == 3);
            // Clear all
            Assert.AreEqual(CsvGnome.Action.Continue, x.Interpret("clear"));
            Assert.IsTrue(!fieldBrain.Fields.Any());
            Assert.IsTrue(!cache.Cache.Keys.Any());
        }

        [TestMethod]
        public void Interpret_Delete()
        {
            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);

            // Create a field to delete
            IComponent[] c = new IComponent[] { new TextComponent("moo") };
            fieldBrain.AddOrUpdateComponentField("test", c);
            Assert.IsTrue(fieldBrain.Fields.Any());
            Assert.AreEqual(CsvGnome.Action.Continue, x.Interpret("delete test"));
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretDelete_MinMaxSameIdOneDeleted()
        {
            const string ins1 = "test1:[1=>9,2 #testId]";
            const string ins2 = "test2:[10=>19,2 #testId]";
            const string ins3 = "test3:[20=>29,2 #testId]";
            const string ins4 = "delete test2";
            MinMaxInfoCache temp = new MinMaxInfoCache();

            IComponent[] expectedComponentsOfField1 = new IComponent[]
            {
                new MinMaxComponent(1, 9, 2, "testId", temp)
            };

            IComponent[] expectedComponentsOfField2 = new IComponent[]
            {
                new MinMaxComponent(20, 29, 2, "testId", temp)
            };

            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);

            x.Interpret(ins1);
            x.Interpret(ins2);
            x.Interpret(ins3);
            x.Interpret(ins4);

            Assert.IsTrue(fieldBrain.Fields.Count == 2);
            Assert.IsTrue((fieldBrain.Fields.First() as ComponentField).Components.Zip(expectedComponentsOfField1, (a, e) => a.Equals(e)).All(r => r == true));
            Assert.IsTrue((fieldBrain.Fields.Last() as ComponentField).Components.Zip(expectedComponentsOfField2, (a, e) => a.Equals(e)).All(r => r == true));

            // Assert Ranks
            Assert.AreEqual(0, (fieldBrain.Fields.First() as ComponentField).Components.OfType<MinMaxComponent>().First().Dim);
            Assert.AreEqual(1, (fieldBrain.Fields.Last() as ComponentField).Components.OfType<MinMaxComponent>().First().Dim);
        }

        [TestMethod]
        public void InterpretDelete_MinMaxSameIdOneDeletedMultiComponentFields()
        {
            const string ins1 = "test1:[1=>9,2 #testId][1=>92,2 #testId]";
            const string ins2 = "test2:[10=>19,2 #testId][1=>9,2 #testId]";
            const string ins3 = "test3:[20=>29,2 #testId][1=>9,2 #testId]";
            const string ins4 = "delete test2";
            MinMaxInfoCache temp = new MinMaxInfoCache();

            IComponent[] expectedComponentsOfField1 = new IComponent[]
            {
                new MinMaxComponent(1, 9, 2, "testId", temp),
                new MinMaxComponent(1, 92, 2, "testId", temp)
            };

            IComponent[] expectedComponentsOfField2 = new IComponent[]
            {
                new MinMaxComponent(20, 29, 2, "testId", temp),
                new MinMaxComponent(1, 9, 2, "testId", temp)
            };

            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);

            x.Interpret(ins1);
            x.Interpret(ins2);
            x.Interpret(ins3);
            x.Interpret(ins4);

            Assert.IsTrue(fieldBrain.Fields.Count == 2);
            Assert.IsTrue((fieldBrain.Fields.First() as ComponentField).Components.Zip(expectedComponentsOfField1, (a, e) => a.Equals(e)).All(r => r == true));
            Assert.IsTrue((fieldBrain.Fields.Last() as ComponentField).Components.Zip(expectedComponentsOfField2, (a, e) => a.Equals(e)).All(r => r == true));

            // Assert Ranks
            Assert.AreEqual(0, (fieldBrain.Fields.First() as ComponentField).Components.OfType<MinMaxComponent>().First().Dim);
            Assert.AreEqual(1, (fieldBrain.Fields.First() as ComponentField).Components.OfType<MinMaxComponent>().Last().Dim);
            Assert.AreEqual(2, (fieldBrain.Fields.Last() as ComponentField).Components.OfType<MinMaxComponent>().First().Dim);
            Assert.AreEqual(3, (fieldBrain.Fields.Last() as ComponentField).Components.OfType<MinMaxComponent>().Last().Dim);
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

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
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
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
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

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
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

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
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

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_IncrementingEvery()
        {
            const string ins = "test:[3++2 every 7]";
            IComponent[] expected = new IComponent[] { new IncrementingComponent(3, 2, 7) };

            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
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

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
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
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
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

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
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

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
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

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
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

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
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

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
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

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_SpreadComponent()
        {
            const string ins = "test:[spread]{one,two , three}";
            IComponent[] expected = new IComponent[]
            {
                new ArraySpreadComponent(new string[] {"one", "two ", " three"})
            };

            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_CycleComponent()
        {
            const string ins = "test:[cycle]{one,two , three}";
            IComponent[] expected = new IComponent[]
            {
                new ArrayCycleComponent(new string[] {"one", "two ", " three"})
            };

            FieldBrain fieldBrain = new FieldBrain();
            Reporter reporter = new Reporter();
            MinMaxInfoCache cache = new MinMaxInfoCache();
            var x = new Interpreter(fieldBrain, reporter, cache);

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
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

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
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
