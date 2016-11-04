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
        string defaultOutput = Path.Combine(Directory.GetCurrentDirectory(), "Output.csv");

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(defaultOutput)) File.Delete(defaultOutput);
        }

        [TestMethod]
        public void TestInputFile()
        {
            Assert.IsFalse(File.Exists(defaultOutput));
            CsvGnomeCli.Program.Main(new[] { "--file", Path.Combine(Directory.GetCurrentDirectory(), "TestFiles", "Input1.gnome") });
            Assert.IsTrue(File.Exists(defaultOutput));
        }
    }
}
