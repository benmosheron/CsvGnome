using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    /// <summary>
    /// Field for which the summary is used for every value
    /// </summary>
    class ConstantField : IField
    {
        /// <summary>
        /// Field name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Value for each row.
        /// </summary>
        public string Summary { get; }

        public string GetValue(int i) => Summary;

        public ConstantField(string name, string value)
        {
            Name = name;
            Summary = value;
        }
    }
}
