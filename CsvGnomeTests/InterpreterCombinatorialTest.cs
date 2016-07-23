using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CsvGnome;
using CsvGnome.Components.Combinatorial;

namespace CsvGnomeTests
{
    [TestClass]
    public class InterpreterCombinatorialTest
    {

        [TestMethod]
        public void InterpretComponents_IncrementingEveryNoDim()
        {
            const string groupId = "Patches";
            string ins = $"test:[++ #{groupId}]";
            Cache cache = new Cache();
            Factory factory = new Factory(cache);

            IComponent expectedRaw = new IncrementingComponent(0);
            ICombinatorial expected = factory.Create(groupId, expectedRaw);

            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
            InterpreterTest.AssertSingleComponentField(fieldBrain, new IComponent[] { expected as IComponent });
        }

        [TestMethod]
        public void InterpretComponents_IncrementingEvery()
        {
            const string groupId = "Patches";
            string ins = $"test:[++ #{groupId}/0]";
            Cache cache = new Cache();
            Factory factory = new Factory(cache);

            IComponent expectedRaw = new IncrementingComponent(0);
            ICombinatorial expected = factory.Create(groupId, expectedRaw);

            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);

            Assert.AreEqual(x.Interpret(ins), CsvGnome.Action.Continue);
            InterpreterTest.AssertSingleComponentField(fieldBrain, new IComponent[] { expected as IComponent });
        }

    }
}
