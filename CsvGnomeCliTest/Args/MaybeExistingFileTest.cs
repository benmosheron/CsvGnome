﻿using CsvGnomeCli.Args;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvGnomeCliTest.Args
{
    [TestClass]
    public class MaybeExistingFileTest
    {
        private IArg GetTestArg => new MaybeExistingFile(new[] { "-m" });
        [TestMethod]
        public void ValidateFileExists()
        {
            IArg arg = GetTestArg;

            // Test with a file that exists.
            // These are the command line args, e.g.:
            //    >CsvGnome -e filePath
            string[] args = new[] { "-e", ProgramTests.TestInputFilePath };

            string failReason = String.Empty;
            Assert.IsTrue(arg.Validate(0, args, out failReason));
            Assert.AreSame(String.Empty, failReason);
        }

        [TestMethod]
        public void ValidateFileNotExists()
        {
            IArg arg = GetTestArg;

            // Test with a file that doesn't exist.
            // These are the command line args, e.g.:
            //    >CsvGnome -e filePath
            string[] args = new[] { "-e", ProgramTests.TestInputFilePath + "someExtraRubbish" };

            string failReason = String.Empty;
            Assert.IsTrue(arg.Validate(0, args, out failReason));
            Assert.AreSame(String.Empty, failReason);
        }
    }
}
