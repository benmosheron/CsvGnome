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
            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);
            Assert.AreEqual(x.Interpret(String.Empty), CsvGnome.Action.Continue);
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretExit()
        {
            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);
            Assert.AreEqual(x.Interpret("exit"), CsvGnome.Action.Exit);
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretRun()
        {
            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);
            Assert.AreEqual(x.Interpret("run"), CsvGnome.Action.Run);
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretWrite()
        {
            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);
            Assert.AreEqual(x.Interpret("write"), CsvGnome.Action.Write);
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretHelp()
        {
            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);
            Assert.AreEqual(x.Interpret("help"), CsvGnome.Action.Help);
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretHelpSpecial()
        {
            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);
            Assert.AreEqual(x.Interpret("help special"), CsvGnome.Action.HelpSpecial);
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretShowGnomeFiles()
        {
            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);
            Assert.AreEqual(x.Interpret("gnomefiles"), CsvGnome.Action.ShowGnomeFiles);
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretSave()
        {
            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);
            Assert.AreEqual(CsvGnome.Action.Continue, x.Interpret("save test"));
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretLoad()
        {
            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);
            Assert.AreEqual(CsvGnome.Action.Continue, x.Interpret("load test"));
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void Interpret_Delete()
        {
            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);

            // Create a field to delete
            IComponent[] c = new IComponent[] { new TextComponent("moo") };
            fieldBrain.AddOrUpdateComponentField("test", c);
            Assert.IsTrue(fieldBrain.Fields.Any());
            Assert.AreEqual(CsvGnome.Action.Continue, x.Interpret("delete test"));
            Assert.IsTrue(!fieldBrain.Fields.Any());
        }

        [TestMethod]
        public void InterpretComponents_Text()
        {
            const string ins = "test:meowmeowmeow";
            IComponent[] expected = new IComponent[] { new TextComponent("meowmeowmeow") };

            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_EmptyText()
        {
            const string ins = "test:";
            IComponent[] expected = new IComponent[] { new TextComponent(String.Empty) };

            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_Incrementing()
        {
            const string ins = "test:[++]";
            IComponent[] expected = new IComponent[] { new IncrementingComponent(0) };

            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_IncrementingWithStart()
        {
            const string ins = "test:[39++]";
            IComponent[] expected = new IComponent[] { new IncrementingComponent(39, 1) };

            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_IncrementingWithIncrement()
        {
            const string ins = "test:[++254]";
            IComponent[] expected = new IComponent[] { new IncrementingComponent(0, 254) };

            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_IncrementingBoth()
        {
            const string ins = "test:[718++218]";
            IComponent[] expected = new IComponent[] { new IncrementingComponent(718, 218) };

            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_IncrementingEvery()
        {
            const string ins = "test:[3++2 every 7]";
            IComponent[] expected = new IComponent[] { new IncrementingComponent(3, 2, 7) };

            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);

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

            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);

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

            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_Date()
        {
            const string ins = "test:[date]";
            IComponent[] expected = new IComponent[] { new DateComponent(new CsvGnome.Date.Provider()) };

            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_DateWithFormat()
        {
            const string format = "HH:mm";
            string ins = $"test:[date \"{format}\"]";
            IComponent[] expected = new IComponent[] { new DateComponent(new CsvGnome.Date.Provider(), format) };
            
            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_MinMax()
        {
            const string ins = "test:[0=>10]";
            IComponent[] expected = new IComponent[] { new MinMaxComponent(0,10) };

            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_MinMaxIncrement()
        {
            const string ins = "test:[1=>11,2]";
            IComponent[] expected = new IComponent[] { new MinMaxComponent(1, 11, 2) };

            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);

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

            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);

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

            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
            AssertSingleComponentField(fieldBrain, expected);
        }

        [TestMethod]
        public void InterpretComponents_AlphabetComponent()
        {
            const string ins = "test:[a=>z]";
            IComponent[] expected = new IComponent[]
            {
                new AlphabetComponent('a','z')
            };

            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);

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
                new DateComponent(new CsvGnome.Date.Provider()),
                new TextComponent("meow"),
                new IncrementingComponent(0),
                new TextComponent("xxx")
            };

            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
            AssertSingleComponentField(fieldBrain, expected);
        }

        public static void AssertSingleComponentField(FieldBrain fb, IComponent[] expected)
        {
            Assert.IsTrue(fb.Fields.Count == 1);
            Assert.IsTrue(fb.Fields.First() is ComponentField);
            Assert.IsTrue(fb.Fields.First().Name == "test");
            Assert.AreEqual(expected.Length, (fb.Fields.First() as ComponentField).Components.Length);
            Assert.IsTrue((fb.Fields.First() as ComponentField).Components.Zip(expected, (a, e) => a.EqualsComponent(e)).All(r => r == true));
        }
    }
}
