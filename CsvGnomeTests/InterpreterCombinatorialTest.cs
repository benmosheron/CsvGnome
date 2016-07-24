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

        [TestMethod]
        public void InterpretComponents_ExtraSpaces()
        {
            const string groupId = "Patches";
            string ins = $"test:[     ++   #{groupId}/0  ]";
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
        public void InterpretComponents_MultiCombinatorials()
        {
            const string groupId = "Patches";
            string ins0 = $"test0:[cycle #{groupId}/0]{{hyena,spider}}[10++ #{groupId}/2]";
            string ins1 = $"test1:[++ #{groupId}/1]";

            Cache cache = new Cache();
            Factory factory = new Factory(cache);

            IComponent expectedRaw0 = new ArrayCycleComponent(new string[] { "hyena", "spider" });
            IComponent expectedRaw1 = new IncrementingComponent(0);
            IComponent expectedRaw2 = new IncrementingComponent(10);
            ICombinatorial expected0 = factory.Create(groupId, expectedRaw0);
            ICombinatorial expected1 = factory.Create(groupId, expectedRaw1);
            ICombinatorial expected2 = factory.Create(groupId, expectedRaw2);

            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);
            x.Interpret(ins0);
            x.Interpret(ins1);

            var componentsFirst = (fieldBrain.Fields[0] as ComponentField).Components;
            var componentsSecond = (fieldBrain.Fields[1] as ComponentField).Components;
            Assert.IsTrue((expected0 as ArrayCycleCombinatorial).Equals(componentsFirst[0]));
            Assert.IsTrue((expected1 as IncrementingCombinatorial).Equals(componentsSecond[0]));
            Assert.IsTrue((expected2 as IncrementingCombinatorial).Equals(componentsFirst[1]));
        }

    }
}
