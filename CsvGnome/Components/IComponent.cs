using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    /// <summary>
    /// Represents a segment of text that may vary by row.
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// Command to write to gnomefiles.
        /// </summary>
        string Command { get; }

        /// <summary>
        /// Summary to write to console.
        /// </summary>
        List<Message> Summary { get; }

        /// <summary>
        /// Value for ith row.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        string GetValue(int i);

        bool Equals(IComponent x);
    }
}
