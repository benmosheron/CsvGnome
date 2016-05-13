using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    public class IncrementingComponent : IComponent
    {
        public const int DefaultStart = 0;
        public const int DefaultIncrement = 1;

        public string Summary => Program.IncrementComponentString;
        /// <summary>
        /// Get value to write on ith row
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string GetValue(int i) => (i + start).ToString(getFormat());
        public bool Equals(IComponent x)
        {
            if (x == null) return false;
            var c = x as IncrementingComponent;
            if (c == null) return false;
            if (start != c.start) return false;
            if (increment != c.increment) return false;
            return true;
        }
        /// <summary>
        /// Value to start incrementing from. Default 0.
        /// </summary>
        private int start;

        /// <summary>
        /// Value to add each row. Default 1;
        /// </summary>
        private int increment;

        public IncrementingComponent(int start)
            : this(start, 1)
        { }

        public IncrementingComponent(int start, int increment)
        {
            this.start = start;
            this.increment = increment;
        }

        /// <summary>
        /// Format of integer (e.g. "D3" to always print 3 digits)
        /// </summary>
        private string getFormat()
        {
            int minDigits = (Math.Max(Program.N + start - 1, 1)).ToString().Length;
            return "D" + minDigits.ToString();
        }
    }
}
