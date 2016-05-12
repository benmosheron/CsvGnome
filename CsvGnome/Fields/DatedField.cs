using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    /// <summary>
    /// Field prefixed with the date and time. Can include a postfix, and an incrementing postfix.
    /// </summary>
    class DatedField : IField
    {
        /// <summary>
        /// Field name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Summary reported to console.
        /// </summary>
        public string Summary { get; }

        /// <summary>
        /// Get value for ith row.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string GetValue(int i)
        {
            StringBuilder sb = new StringBuilder(GetDateTime());

            if (!String.IsNullOrEmpty(postfix)) sb.Append(postfix);

            if (start.HasValue) sb.Append((i + start.Value).ToString(getFormat()));

            return sb.ToString();
        }

        public DatedField(string name, string postfix, int? start)
        {
            Name = name;
            this.start = start;
            this.postfix = postfix;
        }

        /// <summary>
        /// String representation of date/time at runtime.
        /// </summary>
        private string GetDateTime()
        {
            return DateTime.Now.ToString(Program.DateTimeFormat);
        }

        /// <summary>
        /// String to append to date. Can be null or empty.
        /// </summary>
        private string postfix;

        /// <summary>
        /// Value to start incrementing from, if incrementing postfix is desired.
        /// </summary>
        private int? start;

        /// <summary>
        /// Format of integer, if incrementing postfix is desired. (e.g. "D3" to always print 3 digits).
        /// </summary>
        private string getFormat()
        {
            int minDigits = (Math.Max(Program.N + start.Value - 1, 1)).ToString().Length;
            return "D" + minDigits.ToString();
        }
    }
}
