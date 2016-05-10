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
        public string GetValue(int i) => baseValue + (i + start).ToString(fmt);

        /// <summary>
        /// Value to which the incremented integer is appended.
        /// </summary>
        private string baseValue = String.Empty;

        /// <summary>
        /// Format of integer (e.g. "D3" to always print 3 digits)
        /// </summary>
        private string fmt;

        /// <summary>
        /// Value to start incrementing from.
        /// </summary>
        private int start;


        public IncrementingField(string name, string baseValue, int start = 0)
        {
            Name = name;
            Summary = baseValue + "[++]";
            this.baseValue = baseValue;
            this.start = start;

            int minDigits = (Math.Max(Program.N + start - 1, 1)).ToString().Length;
            fmt = "D" + minDigits.ToString();
        }
    }
}
