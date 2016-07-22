using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components.Combinatorial
{
    public class IncrementingCombinatorial: CombinatorialBase, IComponent
    {
        public const int DefaultStart = 0;
        public const int DefaultIncrement = 1;
        public string Command { get; }
        public List<Message> Summary => new List<Message> { new Message(Command, Program.SpecialColour) };

        /// <summary>
        /// Get value to write on ith row.
        /// </summary>
        /// <remarks>
        /// Incrementing components do not have a finite cardinality (they just go on forever).
        /// We just need to account for any lower dimensions.
        /// </remarks>
        /// <param name="i">The row number (zero indexed).</param>
        /// <returns></returns>
        public string GetValue(int i)
        {
            int reducedRow;
            
            // If this is the first dimension, increment it as usual
            if(Dimension == 0)
            {
                reducedRow = i;
            }
            else if(Dimension > Group.GroupCardinality.IndexOfFirstInfiniteDimension)
            {
                // If it's got another infinite below it, it will stay at i = 0
                reducedRow = 0;
            }
            else
            {
                reducedRow = i;
            }

            return (start + (reducedRow * increment)).ToString(getFormat());
        }

        public bool Equals(IComponent x)
        {
            if (x == null) return false;
            var c = x as IncrementingCombinatorial;
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

        /// <summary>
        /// Do not use this! Use the Factory class, which will manage the cache.
        /// </summary>
        public IncrementingCombinatorial(
            Group group,
            IncrementingComponent rawComponent,
            int start,
            int increment)
            :base(group, rawComponent)
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
