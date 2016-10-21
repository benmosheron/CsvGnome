using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components
{
    /// <summary>
    /// Component with a minimum and maximum value, and optional increment.
    /// E.g. from zero to ten in steps of two: "ZeroToTenField:[0 => 10, 2]" 
    /// </summary>
    /// <remarks>
    /// The maximum will never be exceeded, so e.g.
    /// [0=>9, 100] will give 0,0,0,...
    /// </remarks>
    public class MinMaxComponent : IComponent
    {
        /// <summary>
        /// The command to create this component.
        /// </summary>
        public string Command
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"[{Min}=>{Max}");
                sb.Append($",{Increment}");
                sb.Append("]");
                return sb.ToString();
            }
        }

        /// <summary>
        /// Summary of this component for display.
        /// </summary>
        public List<Message> Summary {
            get
            {
                return Message.NewSpecial($"[{Min}=>{Max},{Increment}]").ToList();
            }
        }

        public bool EqualsComponent(IComponent x)
        {
            if (x == null) return false;
            var c = x as MinMaxComponent;
            if (c == null) return false;
            if (Min != c.Min) return false;
            if (Max != c.Max) return false;
            if (Increment != c.Increment) return false;
            return true;
        }

        public string GetValue(long i)
        {
            return (Min + (i % Cardinality) * Increment).ToString();
        }

        private const int DefaultIncrement = 1;
        public int Min { get; private set; }
        public int Max { get; private set; }
        public int Increment { get; private set; }
        public long Cardinality => (Math.Abs(Max - Min) / Math.Abs(Increment)) + 1;

        public MinMaxComponent(int min, int max)
            :this(min, max, DefaultIncrement * (min < max ? 1 : -1))
        {

        }

        /// <summary>
        /// Create a new MinMaxComponenet
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the min, max and increment provided would result in an infinite cardinality.</exception>
        public MinMaxComponent(int min, int max, int increment)
        {
            Min = min;
            Max = max;
            Increment = increment;
            // Check for infinite ranges.
            if (max < min && increment > 0) throw new InfiniteMinMaxException($"Cannot create MinMaxComponent {Command}, it would have an infinite range. Try using an incrementing component [{Min}++{Increment}].");
            if (min < max && increment < 0) throw new InfiniteMinMaxException($"Cannot create MinMaxComponent {Command}, it would have an infinite range. Try using an incrementing component [{Min}++{Increment}].");
        }
    }

    public class InfiniteMinMaxException : Exception
    {
        public InfiniteMinMaxException(string message) : base(message) { }
    }
}
