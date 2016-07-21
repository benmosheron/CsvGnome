using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components.Combinatorial
{
    public class ArrayCycleCombinatorial : CombinatorialBase, IComponent
    {
        #region IComponent

        public string Command => valueArrayCommand;

        public List<Message> Summary => new List<Message>()
        {
            new Message(Program.CycleComponentString, Program.SpecialColour),
            new Message("{"),
            Program.ReportArrayContents
            ? new Message(valueArray.Aggregate((t, n) => $"{t},{n}"))
            : new Message($"{valueArray.Length} items", Program.SpecialColour),
            new Message("}"),
        };

        public bool Equals(IComponent x)
        {
            if (x == null) return false;
            var c = x as ArrayCycleComponent;
            if (c == null) return false;
            if (Command != c.Command) return false;
            return true;
        }

        public string GetValue(int i)
        {
            return valueArray[i % valueArray.Length];
        }

        #endregion IComponent

        #region ICombinatorial

        public override int? Cardinality => valueArray.Length;

        #endregion

        private readonly string[] valueArray;

        private string valueArrayCommand => Program.CycleComponentString + "{" + valueArray.Aggregate((t, n) => $"{t},{n}") + "}";

        /// <summary>
        /// Do not use this! Use the Factory class, which will manage the cache.
        /// </summary>
        public ArrayCycleCombinatorial(
            Group group,
            IEnumerable<string> valueArray)
            :base(group)
        {
            Debug.WriteLineIf(valueArray == null || !valueArray.Any(), "Empty or null array supplied to array component.");
            this.valueArray = valueArray.ToArray();
            if (this.valueArray == null || this.valueArray.Length == 0) this.valueArray = new string[] { String.Empty };
        }
    }
}
