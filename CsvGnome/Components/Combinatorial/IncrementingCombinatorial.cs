using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components.Combinatorial
{
    public class IncrementingCombinatorial: CombinatorialBase, IComponent
    {
        public override long? Cardinality => null;

        public string Command { get; }
        public List<Message> Summary => new List<Message> { new Message(Command, Program.SpecialColour) };

        public bool Equals(IComponent x)
        {
            if (x == null) return false;
            var c = x as IncrementingCombinatorial;
            if (c == null) return false;
            if (start != c.start) return false;
            if (increment != c.increment) return false;
            return true;
        }

        private IncrementingComponent RawIncrementingComponent => RawComponent as IncrementingComponent;

        /// <summary>
        /// Value to start incrementing from. Default 0.
        /// </summary>
        private int start => RawIncrementingComponent.Start;

        /// <summary>
        /// Value to add each row. Default 1;
        /// </summary>
        private int increment => RawIncrementingComponent.Increment;

        /// <summary>
        /// Do not use this! Use the Factory class, which will manage the cache.
        /// </summary>
        public IncrementingCombinatorial(
            Group group,
            IncrementingComponent rawComponent)
            :base(group, rawComponent)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            if (start != IncrementingComponent.DefaultStart) sb.Append(start);
            sb.Append("++");
            if (increment != IncrementingComponent.DefaultIncrement) sb.Append(increment);
            sb.Append("]");

            Command = sb.ToString();
        }

        /// <summary>
        /// Format of integer (e.g. "D3" to always print 3 digits)
        /// </summary>
        public string getFormat()
        {
            return RawIncrementingComponent.getFormat();
        }
    }
}
