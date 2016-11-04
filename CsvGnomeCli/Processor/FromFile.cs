using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvGnome;
using System.IO;

namespace CsvGnomeCli.Processor
{
    /// <summary>
    /// Get a set of CSvGnome commands by reading them from an existing file.
    /// </summary>
    public class FromFile : IProcessor
    {
        string Path;

        public FromFile(string path)
        {
            Path = path;
        }

        public List<string> Process(IReporter reporter)
        {
            return File.ReadAllLines(Path).ToList();
        }
    }
}
