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
            Assert.AreEqual(ArgsValidator.NoArgsProvided, reporter.msg.Text);
        }

        [TestMethod]
        public void NullArgsProvided()
        {
            bool result;
            SingleMessageReporter reporter = new SingleMessageReporter();
            ArgsValidator argsValidator = GetValidator(reporter);

            result = argsValidator.Validate(new string[] { null });

            Assert.IsFalse(result);
            Assert.AreEqual(ArgsValidator.NoArgsProvided, reporter.msg.Text);
        }

        [TestMethod]
        public void EmptyArgsProvided()
        {
            bool result;
            SingleMessageReporter reporter = new SingleMessageReporter();
            ArgsValidator argsValidator = GetValidator(reporter);

            result = argsValidator.Validate(new[] { String.Empty });

            Assert.IsFalse(result);
            Assert.AreEqual(ArgsValidator.NoArgsProvided, reporter.msg.Text);
        }

        [TestMethod]
        public void DuplicateArgsProvided()
        {
            bool result;
            MultiMessageReporter reporter = new MultiMessageReporter();
            ArgsValidator argsValidator = GetValidator(reporter);

            result = argsValidator.Validate(new[] { "--file", "test", "--file", "--output", "--output" });

            Assert.IsFalse(result);

            var expected = new List<string>()
            {
                ArgsValidator.DuplicateArgsProvided,
                "--file",
                "--output"
            };

            // Ignore all the invalid file errors
            Assert.IsTrue(expected.SequenceEqual(reporter.Text.Take(3)));
        }

        [TestMethod]
        public void DuplicateArgsDifferentValuesProvided()
        {
            bool result;
            MultiMessageReporter reporter = new MultiMessageReporter();
            ArgsValidator argsValidator = GetValidator(reporter);

            result = argsValidator.Validate(new[] { "--file", "test", "-f", "--output", "-o" });

            Assert.IsFalse(result);

            var expected = new List<string>()
            {
                ArgsValidator.DuplicateArgValuesProvided,
                "--file and -f",
                "--output and -o"
            };

            // Ignore all the invalid file errors
            Assert.IsTrue(expected.SequenceEqual(reporter.Text.Take(3)));
        }

        [TestMethod]
        public void FileExpected()
        {
            bool result;
            SingleMessageReporter reporter = new SingleMessageReporter();
            ArgsValidator argsValidator = GetValidator(reporter);

            result = argsValidator.Validate(new[] { "--file" });

            Assert.IsFalse(result);
            Assert.AreEqual("No file path provided after [--file] argument.", reporter.msg.Text);
        }

        [TestMethod]
        public void OutputExpected()
        {
            bool result;
            SingleMessageReporter reporter = new SingleMessageReporter();
            ArgsValidator argsValidator = GetValidator(reporter);

            result = argsValidator.Validate(new[] { "--output" });

            Assert.IsFalse(result);
            Assert.AreEqual("No file path provided after [--output] argument.", reporter.msg.Text);
        }
    }
}
