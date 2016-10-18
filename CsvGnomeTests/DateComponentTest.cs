using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CsvGnome;

namespace CsvGnomeTests
{
    [TestClass]
    public class DateComponentTest
    {
        private const string OldFormat = "yyyy-MM-ddTHH:mm:ss";

        [TestMethod]
        public void BasicNoFormat()
        {
            CsvGnome.Date.Provider provider = new CsvGnome.Date.Provider();
            DateComponent d = new DateComponent(provider);
            
            Assert.AreEqual(provider.Get().ToString(), d.GetValue(0));
        }

        [TestMethod]
        public void CommandNoFormat()
        {
            CsvGnome.Date.Provider provider = new CsvGnome.Date.Provider();
            DateComponent d = new DateComponent(provider);

            Assert.AreEqual("[date]", d.Command);
        }

        [TestMethod]
        public void BasicOldFormat()
        {
            CsvGnome.Date.Provider provider = new CsvGnome.Date.Provider();
            DateComponent d = new DateComponent(provider, OldFormat);

            Assert.AreEqual(provider.Get().ToString(OldFormat), d.GetValue(0));
        }

        [TestMethod]
        public void CommandOldFormat()
        {
            CsvGnome.Date.Provider provider = new CsvGnome.Date.Provider();
            DateComponent d = new DateComponent(provider, OldFormat);

            Assert.AreEqual($"[date \"{OldFormat}\"]", d.Command);
        }
    }
}
