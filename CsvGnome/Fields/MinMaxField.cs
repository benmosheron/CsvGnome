using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    /// <summary>
    /// Field for which values lie between a minimum and maximum (inclusive) value, and are incremented by a constant amount.
    /// Values outside this range will be written as null.
    /// </summary>
    class MinMaxField : IField, ICombinableField
    {
        public string Name { get; }

        public string Summary { get; }

        public string GetValue(int i)
        {
            int val = Min + (Increment * i);
            if (Math.Abs(val) > Math.Abs(Max)) return null;
            return val.ToString();
        }

        public int Min { get; }
        public int Max { get; }
        public int Increment { get; }

        /// <summary>
        /// The number of elements possible, AKA the number of lines that can be written before writing nulls.
        /// </summary>
        public int Cardinality { get; }

        public MinMaxField(string name, int min, int max, int increment)
        {
            Name = name;
            Min = min;
            Max = max;
            Increment = increment;

            // make use of integer truncation
            Cardinality = ((max - min) / increment) + 1;

            Summary = $"{Min} -> {Max} [{Increment}]";
        }
    }
}
