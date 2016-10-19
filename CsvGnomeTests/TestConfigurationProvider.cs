using CsvGnome.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeTests
{
    /// <summary>
    /// Provides only default values. No "Set..." methods implemented.
    /// </summary>
    public class TestConfigurationProvider : IProvider
    {
        public static TestConfigurationProvider Get => new TestConfigurationProvider();

        public bool PadOutput => Defaults.PadOutput;

        public bool ReportArrayContents => Defaults.ReportArrayContents;

        public void SetPadOutput(bool value)
        {
            throw new NotImplementedException();
        }

        public void SetReportArrayContents(bool value)
        {
            throw new NotImplementedException();
        }
    }
}
