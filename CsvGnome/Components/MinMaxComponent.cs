using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
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
                if (incrementProvided) sb.Append($",{Increment}");
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
                List<Message> m = new List<Message>();
                m.Add(new Message("[", Program.SpecialColour));
                m.Add(new Message($"{Min}"));
                m.Add(new Message("=>", Program.SpecialColour));
                m.Add(new Message($"{Max}"));
                if (incrementProvided) m.Add(new Message($",{Increment}"));
                m.Add(new Message("]", Program.SpecialColour));
                return m;
            }
        }

        public bool Equals(IComponent x)
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
        private bool incrementProvided = true;
        public int Min { get; private set; }
        public int Max { get; private set; }
        public int Increment { get; private set; }
        public int Cardinality => ((Max - Min) / Increment) + 1;

        // MinMaxField - no combinatorics
        public MinMaxComponent(int min, int max)
            :this(min, max, DefaultIncrement)
        {
            incrementProvided = false;
        }

        public MinMaxComponent(int min, int max, int increment)
        {
            Min = min;
            Max = max;
            Increment = increment;
        }
    }
}
