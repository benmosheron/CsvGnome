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
    interface IComponent
    {
        string Summary { get; }
        string GetValue(int i);
    }
}
