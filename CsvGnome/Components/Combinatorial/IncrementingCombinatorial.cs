using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components.Combinatorial
{
    public class IncrementingCombinatorial: CombinatorialCore, IComponent
    {
        /// <summary>
        /// Do not use this! Use the Factory class, which will manage the cache.
        /// </summary>
        public IncrementingCombinatorial(
            Group group,
            IncrementingComponent rawComponent)
            : base(group, rawComponent)
        {

        }

        public static IncrementingCombinatorial Make(Group group, IncrementingComponent rawComponent)
        {
            return new IncrementingCombinatorial(group, rawComponent);
        }

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
            string startString = (start != IncrementingComponent.DefaultStart) ? start.ToString() : String.Empty;
            string incrementString = (increment != IncrementingComponent.DefaultIncrement) ? increment.ToString() : String.Empty;
            string everyString = (every != IncrementingComponent.DefaultEvery) ? $" every {every}" : String.Empty;

            return Message.NewSpecial($"[{startString}++{incrementString}{everyString} ").ToList();
        }

        protected override List<Message> GetPostGroupMessage()
        {
            return Message.NewSpecial("]").ToList();
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
        /// Format of integer (e.g. "D3" to always print 3 digits)
        /// </summary>
        public string getFormat()
        {
            return RawIncrementingComponent.getFormat();
        }
    }
}
