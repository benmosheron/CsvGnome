using CsvGnomeCli.Args;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CsvGnomeCliTest.Args
{
    [TestClass]
    public class ArgBaseTest
    {
        // Use a dummy class to test the abstract ArgBase class.
        private class DummyArg : ArgBase
        {
            public DummyArg(params string[] values) : base(values) { }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NoParams()
        {
            new DummyArg();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullParam()
        {
            new DummyArg(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EmptyStringParam()
        {
            new DummyArg(String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SomeEmptyStringParams()
        {
            new DummyArg("valid", String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DuplicateStringParam()
        {
            new DummyArg("dup", "dup");
        }

        [TestMethod]
        public void ValuesAsExpected()
        {
            string[] values = new string[] { "valid1", "valid2" };
            var expected = new HashSet<string>(values);
            IArg arg = new DummyArg(values);
            Assert.IsTrue(HashSetSame(expected, arg.Values));
        }

        bool HashSetSame(HashSet<string> expected, HashSet<string> actual)
        {
            if (expected.Count != actual.Count) return false;

            foreach(string e in expected)
            {
                if (!actual.Contains(e)) return false;
            }

            return true;
        }
    }
}
