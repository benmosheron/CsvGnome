﻿using System;
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
            ArrayCycleComponent rawComponent)
            : base(group, rawComponent)
        {

        }

        public static ArrayCycleCombinatorial Make(Group group, ArrayCycleComponent rawComponent)
        {
            return new ArrayCycleCombinatorial(group, rawComponent);
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

        protected override List<Message> GetPreGroupMessage()
        {
            return new List<Message>() { new Message("[cycle ", Program.SpecialColour) };
        }

        protected override List<Message> GetPostGroupMessage()
        {
            return new List<Message>()
            {
                new Message("]", Program.SpecialColour),
                new Message("{"),
                RawArrayCycleComponent.ConfigurationProvider.ReportArrayContents
                ? new Message(valueArray.Aggregate((t, n) => $"{t},{n}"))
                : new Message($"{valueArray.Count} items", Program.SpecialColour),
                new Message("}"),
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
