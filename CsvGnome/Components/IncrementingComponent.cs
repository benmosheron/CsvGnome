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
        public string Command { get; }
        public List<Message> Summary => new List<Message> { new Message(Command, Program.SpecialColour) };
        /// <summary>
        /// Get value to write on ith row
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string GetValue(long i) => (start + (i * increment)).ToString(getFormat());
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

            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            if (start != DefaultStart) sb.Append(start);
            sb.Append("++");
            if (increment != DefaultIncrement) sb.Append(increment);
            sb.Append("]");

            Command = sb.ToString();
        }

        /// <summary>
        /// Format of integer (e.g. "D3" to always print 3 digits)
        /// </summary>
        private string getFormat()
        {
            int minDigits = (Math.Max(Math.Abs(start) + (Program.N * Math.Abs(increment)) - 1, 1)).ToString().Length;
            return "D" + minDigits.ToString();
        }
    }
}
