using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Configuration
{
    /// <summary>
    /// Default values, to be used when configuration is either not required, or not found.
    /// </summary>
    public static class Defaults
    {
        public const bool PadOutput = false;
        public const bool ReportArrayContents = false;
    }
}
