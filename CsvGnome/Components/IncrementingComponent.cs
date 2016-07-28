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
        public const int DefaultEvery = 1;
        public string Command { get; }
        public List<Message> Summary => new List<Message> { new Message(Command, Program.SpecialColour) };
        /// <summary>
        /// Get value to write on ith row
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string GetValue(long i) => GetValueNumeric(i).ToString(getFormat());
        private long GetValueNumeric(long i) => start + ((i / every) * increment);
        public bool EqualsComponent(IComponent x)
        {
            if (x == null) return false;
            var c = x as IncrementingComponent;
            if (c == null) return false;
            if (start != c.start) return false;
            if (increment != c.increment) return false;
            return true;
        }

        public int Start => start;
        public int Increment => increment;
        public int Every => every;

        /// <summary>
        /// Value to start incrementing from. Default 0.
        /// </summary>
        int start;

        /// <summary>
        /// Value to add each row. Default 1;
        /// </summary>
        private int increment;

        /// <summary>
        /// Apply the increment every [every] rows.
        /// </summary>
        private int every;

        public IncrementingComponent(int start)
            : this(start, DefaultIncrement)
        { }

        public IncrementingComponent(int start, int increment)
            :this(start, increment, DefaultEvery)
        { }

        public IncrementingComponent(int start, int increment, int every)
        {
            this.start = start;
            this.increment = increment;
            this.every = every;

            // Sanity check on every
            if (every < 1) every = 1;

            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            if (start != DefaultStart) sb.Append(start);
            sb.Append("++");
            if (increment != DefaultIncrement) sb.Append(increment);
            if (every != DefaultEvery) sb.Append($" every {every}");
            sb.Append("]");

            Command = sb.ToString();
        }

        /// <summary>
        /// Format of integer (e.g. "D3" to always print 3 digits)
        /// </summary>
        public string getFormat()
        {
            // Either the first or the last will have the most digits, so calculate them both.
            int minD = 1;

            // Strip out any "-"s
            long first = Math.Abs(GetValueNumeric(0));
            long last = Math.Abs(GetValueNumeric(Program.N - 1));

            int firstD = first.ToString().Length;
            int lastD = last.ToString().Length;

            int minOfBoth = Math.Max(firstD, lastD);
            int min = Math.Max(minOfBoth, minD);

            //int minDigits = (Math.Max(Math.Abs(start) + (Program.N * Math.Abs(increment)) - 1, 1)).ToString().Length;
            return "D" + min.ToString();
        }
    }
}
