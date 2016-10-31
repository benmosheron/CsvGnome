using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CsvGnomeCli;
using CsvGnome;
using System.Collections.Generic;
using System.Linq;

namespace CsvGnomeCliTest
{
    [TestClass]
    public class ArgsValidatorTest
    {
        private ArgsValidator GetValidator(IReporter reporter)
        {
            return new ArgsValidator(reporter);
        }

        private ArgsValidator GetValidator()
        {
            return GetValidator(new ThrowReporter());
        }

        [TestMethod]
        public void NoArgsProvided()
        {
            bool result;
            SingleMessageReporter reporter = new SingleMessageReporter();
            ArgsValidator argsValidator = GetValidator(reporter);

            result = argsValidator.Validate(new[] { String.Empty });

            Assert.IsFalse(result);
            Assert.AreEqual(ArgsValidator.NoArgsProvided, reporter.msg);
        }

        [TestMethod]
        public void NullArgsProvided()
        {
            bool result;
            SingleMessageReporter reporter = new SingleMessageReporter();
            ArgsValidator argsValidator = GetValidator(reporter);

            result = argsValidator.Validate(new[] { String.Empty });

            Assert.IsFalse(result);
            Assert.AreEqual(ArgsValidator.NoArgsProvided, reporter.msg);
        }

        [TestMethod]
        public void DuplicateArgsProvided()
        {
            bool result;
            MultiMessageReporter reporter = new MultiMessageReporter();
            ArgsValidator argsValidator = GetValidator(reporter);

            result = argsValidator.Validate(new[] { "--file", "test", "--file", "--output", "--output" });

            Assert.IsFalse(result);

            var expected = new List<Message>()
            {
                new Message(ArgsValidator.DuplicateArgsProvided),
                new Message("--file"),
                new Message("--output")
            };

            Assert.IsTrue(expected.SequenceEqual(reporter.Messages));
        }

        [TestMethod]
        public void DuplicateArgsDifferentValuesProvided()
        {
            bool result;
            MultiMessageReporter reporter = new MultiMessageReporter();
            ArgsValidator argsValidator = GetValidator(reporter);

            result = argsValidator.Validate(new[] { "--file", "test", "-f", "--output", "-o" });

            Assert.IsFalse(result);

            var expected = new List<Message>()
            {
                new Message(ArgsValidator.DuplicateArgValuesProvided),
                new Message("--file and -f"),
                new Message("--output and -o")
            };

            Assert.IsTrue(expected.SequenceEqual(reporter.Messages));
        }

        [TestMethod]
        public void FileExpected()
        {
            bool result;
            SingleMessageReporter reporter = new SingleMessageReporter();
            ArgsValidator argsValidator = GetValidator(reporter);

            result = argsValidator.Validate(new[] { "--file" });

            Assert.IsFalse(result);
            Assert.AreEqual(ArgsValidator.NoInputFileProvided, reporter.msg);
        }

        [TestMethod]
        public void OutputExpected()
        {
            bool result;
            SingleMessageReporter reporter = new SingleMessageReporter();
            ArgsValidator argsValidator = GetValidator(reporter);

            result = argsValidator.Validate(new[] { "--output" });

            Assert.IsFalse(result);
            Assert.AreEqual(ArgsValidator.NoOutputFileProvided, reporter.msg);
        }
    }
}
