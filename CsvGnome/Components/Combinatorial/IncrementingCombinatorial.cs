using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components.Combinatorial
{
    public class IncrementingCombinatorial: CombinatorialCore, IComponent
    {
        public override long? Cardinality => null;

        public string Command
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("[");
                if (start != IncrementingComponent.DefaultStart) sb.Append(start);
                sb.Append("++");
                if (increment != IncrementingComponent.DefaultIncrement) sb.Append(increment);
                if (every != IncrementingComponent.DefaultEvery) sb.Append($" every {every}");
                sb.Append($" {GetGroupString()}");
                sb.Append("]");

                return sb.ToString();
            }
        }

        protected override List<Message> GetPreGroupMessage()
        {
            List<Message> m = new List<Message>();
            m.Add(new Message("[", Program.SpecialColour));
            if (start != IncrementingComponent.DefaultStart) m.Add(new Message(start.ToString(), Program.SpecialColour));
            m.Add(new Message("++", Program.SpecialColour));
            if (increment != IncrementingComponent.DefaultIncrement) m.Add(new Message(increment.ToString(), Program.SpecialColour));
            if (every != IncrementingComponent.DefaultEvery) m.Add(new Message($" every {every}", Program.SpecialColour));
            m.Add(new Message(" ", Program.SpecialColour));

            return m;
        }

        protected override List<Message> GetPostGroupMessage()
        {
            return new Message("]", Program.SpecialColour).ToList();
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
        /// Rows to wait before incrementing.
        /// </summary>
        private int every => RawIncrementingComponent.Every;

        /// <summary>
        /// Do not use this! Use the Factory class, which will manage the cache.
        /// </summary>
        public IncrementingCombinatorial(
            Group group,
            IncrementingComponent rawComponent)
            :base(group, rawComponent)
        {

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
