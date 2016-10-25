using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components.Combinatorial
{
    public class ArrayCycleCombinatorial : CombinatorialCore, IComponent
    {
        /// <summary>
        /// Do not use this! Use the Factory class, which will manage the cache.
        /// </summary>
        public ArrayCycleCombinatorial(
            Group group,
            ArrayCycleComponent rawComponent,
            IMessageProvider messageProvider = null)
            : base(group, rawComponent, messageProvider)
        {

        }

        public static ArrayCycleCombinatorial Make(Group group, ArrayCycleComponent rawComponent, IMessageProvider messageProvider)
        {
            return new ArrayCycleCombinatorial(group, rawComponent, messageProvider);
        }

        public string Command
        {
            get
            {
                return $"[cycle {GetGroupString()}]{{{valueArray.Aggregate((t, n) => $"{t},{n}")}}}";
            }
        }

        /// <summary>
        /// Number of elements in the raw component's value array.
        /// </summary>
        public override long? Cardinality => valueArray.Count;

        protected override List<IMessage> GetPreGroupMessage()
        {
            return new List<IMessage>() { MessageProvider.NewSpecial("[cycle ") };
        }

        protected override List<IMessage> GetPostGroupMessage()
        {
            return new List<IMessage>()
            {
                MessageProvider.NewSpecial("]{"),
                RawArrayCycleComponent.ConfigurationProvider.ReportArrayContents
                ? MessageProvider.New(valueArray.Aggregate((t, n) => $"{t},{n}"))
                : MessageProvider.NewSpecial($"{valueArray.Count} items"),
                MessageProvider.NewSpecial("}"),
            };
        }

        /// <summary>
        /// Access the raw component.
        /// </summary>
        private ArrayCycleComponent RawArrayCycleComponent => RawComponent as ArrayCycleComponent;

        /// <summary>
        /// Access the raw component's value array.
        /// </summary>
        private ReadOnlyCollection<string> valueArray => RawArrayCycleComponent.ValueArray;
    }
}
