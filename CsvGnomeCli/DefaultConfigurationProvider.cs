using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnomeCli
{
    public class DefaultConfigurationProvider : CsvGnome.Configuration.IProvider
    {
        private bool padOutput = CsvGnome.Configuration.Defaults.PadOutput;
        private bool reportArrayContents = CsvGnome.Configuration.Defaults.ReportArrayContents;
        public bool PadOutput => padOutput;

        public bool ReportArrayContents => reportArrayContents;

        public void SetPadOutput(bool value)
        {
            padOutput = value;
        }

        public void SetReportArrayContents(bool value)
        {
            reportArrayContents = value;
        }
    }
}
