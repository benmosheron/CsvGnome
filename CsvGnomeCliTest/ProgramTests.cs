using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeCliTest
{
    [TestClass]
    public class ProgramTests
    {
        private const string testFilesDirectoryName = "TestFiles";
        private const string outputDirectoryName = "UnitTestOutput";
        private const string outputFileFormat = "UnitTestOutput_{0}.csv";
        private const string intputFileName = "Input1.gnome";
        private const string expectedOutputFileName = "ExpectedOutput1.csv";

        /// <summary>
        /// CsvGnomeCliTest\TestFiles
        /// </summary>
        private static readonly string TestFilesDirectory = Path.Combine(Directory.GetCurrentDirectory(), testFilesDirectoryName);

        /// <summary>
        /// CsvGnomeCliTest\TestFiles\UnitTestOutput
        /// All files created by unit tests should go into this directory.
        /// This directory is deleted after tests are run.
        /// </summary>
        private static readonly string OutputFilesDirectory = Path.Combine(TestFilesDirectory, outputDirectoryName);

        /// <summary>
        /// CsvGnomeCliTest\TestFiles\ExpectedOutput1.csv
        /// </summary>
        private static readonly string ExpectedOutputFilePath = Path.Combine(TestFilesDirectory, expectedOutputFileName);

        /// <summary>
        /// CsvGnomeCliTest\TestFiles\Input1.gnome
        /// Path of the input file used by unit tests.
        /// </summary>
        public static readonly string TestInputFilePath = Path.Combine(TestFilesDirectory, intputFileName);

        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            // Check the output directory was deleted by the last run.
            if (Directory.Exists(OutputFilesDirectory))
            {
                throw new UnitTestDirectoryNotClearedException(OutputFilesDirectory);
            }

            // Check that the test files directory exists
            if (!Directory.Exists(TestFilesDirectory))
            {
                throw new TestFileDirectoryNotFoundException(TestFilesDirectory);
            }

            // Create the unit test output directory
            Directory.CreateDirectory(OutputFilesDirectory);
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            // Delete all files created by unit tests
            Directory.Delete(OutputFilesDirectory, true);
        }

        private string GetOutputFileForTest(string testName)
        {
            string file = String.Format(outputFileFormat, testName);
            return Path.Combine(OutputFilesDirectory, file);
        }

        [TestMethod]
        public void TestOutputFileIsCreated()
        {
            string output = GetOutputFileForTest("TestOutputFileIsCreated");
            Assert.IsFalse(File.Exists(output));
            CsvGnomeCli.Program.Main(new[] { "--file", TestInputFilePath, "--output", output });
            Assert.IsTrue(File.Exists(output));
        }

        [TestMethod]
        public void TestOutputFileIsAsExpected()
        {
            string output = GetOutputFileForTest("TestOutputFileIsAsExpected");
            Assert.IsFalse(File.Exists(output));
            CsvGnomeCli.Program.Main(new[] { "--file", TestInputFilePath, "--output", output });

            string[] expectedOutput = File.ReadAllLines(ExpectedOutputFilePath);
            string[] actualOutput = File.ReadAllLines(output);

            Assert.AreEqual(expectedOutput.Length, actualOutput.Length, "Expetced and actual output files had different number of lines. Check the newline at the end.");

            for (int i = 0; i < expectedOutput.Length; i++)
            {
                Assert.AreEqual(expectedOutput[i], actualOutput[i], $"Lines [{i}] (zero-based) did not match.");
            }
        }
    }

    #region ProgramTest Exceptions
    class UnitTestDirectoryNotClearedException : Exception
    {
        public UnitTestDirectoryNotClearedException(string dir) 
            : base($"This directory was not cleared and deleted by previous test run [{dir}].")
        {

        }
    }

    class TestFileDirectoryNotFoundException : Exception
    {
        public TestFileDirectoryNotFoundException(string dir)
            : base($"The test file directory was not found [{dir}].")
        {

        }
    }
    #endregion ProgramTest Exceptions
}
