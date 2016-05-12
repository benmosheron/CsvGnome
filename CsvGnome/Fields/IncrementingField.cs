using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    /// <summary>
    /// Field with a base string (can be null) and an incrementing integer.
    /// </summary>
    class IncrementingField : IField
    {
        /// <summary>
        /// Field name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Summary: "baseValue[++]"
        /// </summary>
        public string Summary { get; }

        /// <summary>
        /// Get value to write on ith row
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string GetValue(int i) => baseValue + (i + start).ToString(getFormat());

        /// <summary>
        /// Value to which the incremented integer is appended.
        /// </summary>
        private string baseValue = String.Empty;

        /// <summary>
        /// Format of integer (e.g. "D3" to always print 3 digits)
        /// </summary>
        private string getFormat()
        {
            int minDigits = (Math.Max(Program.N + start - 1, 1)).ToString().Length;
            return "D" + minDigits.ToString();
        }

        /// <summary>
        /// Value to start incrementing from.
        /// </summary>
        private int start;

        /// <summary>
        /// Create a new incrementing field, which has a base value followed by a number that counts up, padded with zeros.
        /// eg.
        /// base001
        /// base002
        /// base003
        /// ...
        /// base998
        /// base999
        /// </summary>
        /// <param name="name"></param>
        /// <param name="baseValue"></param>
        /// <param name="start"></param>
        public IncrementingField(string name, string baseValue, int start)
        {
            Name = name;
            Summary = baseValue + $"[++] [start: {start}]";
            this.baseValue = baseValue;
            this.start = start;
        }
    }
}
