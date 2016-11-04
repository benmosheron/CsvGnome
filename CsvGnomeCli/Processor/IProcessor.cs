using CsvGnome;
using System.Collections.Generic;

namespace CsvGnomeCli.Processor
{
    /// <summary>
    /// Generates a set of CsvGnome commands.
    /// </summary>
    public interface IProcessor
    {
        /// <summary>
        /// Generate a set of commands.
        /// </summary>
        List<string> Process(IReporter reporter);
    }
}
