using CsvGnome;
using CsvGnomeCli.Processor;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CsvGnomeCliTest
{
    [TestClass]
    public class ProcessorFactoryTest
    {
        private IProcessor GetProcessor(string[] args)
        {
            return GetProcessor(new ThrowReporter(), args);
        }

        private IProcessor GetProcessor(IReporter reporter, string[] args)
        {
            return Factory.Get(reporter, args);
        }

        [TestMethod]
        public void FileProvided()
        {
            var args = new string[] { "--file", "testFile" };
            IProcessor processor = GetProcessor(args);
            Assert.IsTrue(processor is FromFile);
        }

        [TestMethod]
        public void FileProvided2()
        {
            var args = new string[] { "-f", "testFile" };
            IProcessor processor = GetProcessor(args);
            Assert.IsTrue(processor is FromFile);
        }

        [TestMethod]
        public void FileProvidedLater()
        {
            var args = new string[] { "some", "other", "rubbish", "--file", "testFile", "with", "more", "stuff" };
            IProcessor processor = GetProcessor(args);
            Assert.IsTrue(processor is FromFile);
        }

        [TestMethod]
        public void FileProvidedLater2()
        {
            var args = new string[] { "some", "other", "rubbish", "-f", "testFile", "with", "more", "stuff" };
            IProcessor processor = GetProcessor(args);
            Assert.IsTrue(processor is FromFile);
        }

        [TestMethod]
        public void FileExpectedButNotProvided()
        {
            var args = new string[] { "--file" };
            IReporter reporter = new SingleMessageReporter();
            IProcessor processor = GetProcessor(reporter, args);
            Assert.IsTrue(processor == null);
            Assert.AreEqual(Factory.NoFileProvidedMessage, (reporter as SingleMessageReporter).msg);
        }

        [TestMethod]
        public void FileExpectedButNotProvided2()
        {
            var args = new string[] { "-f" };
            IReporter reporter = new SingleMessageReporter();
            IProcessor processor = GetProcessor(reporter, args);
            Assert.IsTrue(processor == null);
            Assert.AreEqual(Factory.NoFileProvidedMessage, (reporter as SingleMessageReporter).msg);
        }

        [TestMethod]
        public void OutputExpectedButNotProvided()
        {
            var args = new string[] { "--output" };
            IReporter reporter = new SingleMessageReporter();
            IProcessor processor = GetProcessor(reporter, args);
            Assert.IsTrue(processor == null);
            Assert.AreEqual(Factory.NoOutputProvidedMessage, (reporter as SingleMessageReporter).msg);
        }

        [TestMethod]
        public void OutputExpectedButNotProvided2()
        {
            var args = new string[] { "-o" };
            IReporter reporter = new SingleMessageReporter();
            IProcessor processor = GetProcessor(reporter, args);
            Assert.IsTrue(processor == null);
            Assert.AreEqual(Factory.NoOutputProvidedMessage, (reporter as SingleMessageReporter).msg);
        }
    }
}
