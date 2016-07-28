using CsvGnome;
using CsvGnome.Components;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvGnomeTests
{
    [TestClass]
    public class AlphabetComponentTest
    {
        [TestMethod]
        public void Basic()
        {
            AlphabetComponent a = new AlphabetComponent('a', 'z');
            Assert.AreEqual("a", a.GetValue(0));
            Assert.AreEqual("z", a.GetValue(25));
        }

        [TestMethod]
        public void Reverse()
        {
            AlphabetComponent a = new AlphabetComponent('z', 'a');
            Assert.AreEqual("z", a.GetValue(0));
            Assert.AreEqual("a", a.GetValue(25));
        }

        [TestMethod]
        public void UpperCaseReverse()
        {
            AlphabetComponent a = new AlphabetComponent('Z', 'A');
            Assert.AreEqual("Z", a.GetValue(0));
            Assert.AreEqual("A", a.GetValue(25));
        }

        [TestMethod]
        public void UpperCase()
        {
            AlphabetComponent a = new AlphabetComponent('A', 'Z');
            Assert.AreEqual("A", a.GetValue(0));
            Assert.AreEqual("Z", a.GetValue(25));
        }

        [TestMethod]
        public void MixedCase()
        {
            AlphabetComponent a = new AlphabetComponent('a', 'Z');
            Assert.AreEqual("a", a.GetValue(0));
            Assert.AreEqual("z", a.GetValue(25));
            Assert.AreEqual("A", a.GetValue(26));
            Assert.AreEqual("Z", a.GetValue(51));
        }

        [TestMethod]
        public void MixedCaseReverse()
        {
            AlphabetComponent a = new AlphabetComponent('Z', 'a');
            Assert.AreEqual("Z", a.GetValue(0));
            Assert.AreEqual("A", a.GetValue(25));
            Assert.AreEqual("z", a.GetValue(26));
            Assert.AreEqual("a", a.GetValue(51));
        }
    }
}
