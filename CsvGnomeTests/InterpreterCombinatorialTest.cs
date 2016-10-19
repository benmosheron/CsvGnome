using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CsvGnome;
using CsvGnome.Components;
using CsvGnome.Fields;
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

            IComponent expectedRaw0 = new ArrayCycleComponent(new string[] { "hyena", "spider" }, new TestConfigurationProvider());
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
            Assert.IsTrue((expected0 as ArrayCycleCombinatorial).EqualsComponent(componentsFirst[0]));
            Assert.IsTrue((expected1 as IncrementingCombinatorial).EqualsComponent(componentsSecond[0]));
            Assert.IsTrue((expected2 as IncrementingCombinatorial).EqualsComponent(componentsFirst[1]));
        }

        [TestMethod]
        public void Delete()
        {
            const string groupId = "Siegward";
            string ins = $"test:[++ #{groupId}]";

            Cache cache;
            Interpreter interpreter;
            Utilties.InterpreterTestInit(out cache, out interpreter);

            // Create the combinatorial field
            interpreter.Interpret(ins);

            // delete it
            interpreter.Interpret("delete test");

            // The group should contain no components, and the next rank should be zero
            Group group = cache[groupId];
            Assert.IsNotNull(group);
            Assert.IsFalse(group.Components.Any());
            Assert.AreEqual(0, group.NextRank);
        }

        [TestMethod]
        public void Clear()
        {
            const string groupId0 = "Gwynevere";
            const string groupId1 = "Gwyndolin";
            string ins0 = $"test:[++ #{groupId0}/0]";
            string ins1 = $"test:[++ #{groupId0}/1]";
            string ins2 = $"test:[++ #{groupId1}]";

            Cache cache;
            Interpreter interpreter;
            Utilties.InterpreterTestInit(out cache, out interpreter);

            // Create the combinatorial field
            interpreter.Interpret(ins0);
            interpreter.Interpret(ins1);

            // clear
            interpreter.Interpret("clear");

            // The group should contain no components, and the next rank should be zero
            Assert.IsFalse(cache.Contains(groupId0));
            Assert.IsFalse(cache.Contains(groupId1));
        }
        [TestMethod]
        public void InterpretComponents_MinMaxCombinatorial()
        {
            const string groupId0 = "Reah";
            const string groupId1 = "Astraea";
            string ins0 = $"test0:[1=>10 #{groupId0}/0][3=>7, 3 #{groupId0}/2]";
            string ins1 = $"test1:[9=>3 #{groupId0}/1][  1  =>  -2  #{groupId1} ]";

            Cache cache = new Cache();
            Factory factory = new Factory(cache);

            IComponent expectedRaw0 = new MinMaxComponent(1, 10);
            IComponent expectedRaw1 = new MinMaxComponent(9, 3);
            IComponent expectedRaw2 = new MinMaxComponent(3, 7, 3);
            IComponent expectedRaw3 = new MinMaxComponent(1, -2);
            ICombinatorial expected0 = factory.Create(groupId0, expectedRaw0);
            ICombinatorial expected1 = factory.Create(groupId0, expectedRaw1);
            ICombinatorial expected2 = factory.Create(groupId0, expectedRaw2);
            ICombinatorial expected3 = factory.Create(groupId1, expectedRaw3);

            FieldBrain fieldBrain;
            Interpreter x;
            Utilties.InterpreterTestInit(out fieldBrain, out x);
            x.Interpret(ins0);
            x.Interpret(ins1);

            var componentsFirst = (fieldBrain.Fields[0] as ComponentField).Components;
            var componentsSecond = (fieldBrain.Fields[1] as ComponentField).Components;
            Assert.IsTrue((expected0 as MinMaxCombinatorial).EqualsComponent(componentsFirst[0]));
            Assert.IsTrue((expected1 as MinMaxCombinatorial).EqualsComponent(componentsSecond[0]));
            Assert.IsTrue((expected2 as MinMaxCombinatorial).EqualsComponent(componentsFirst[1]));
            Assert.IsTrue((expected3 as MinMaxCombinatorial).EqualsComponent(componentsSecond[1]));
        }

    }
}
