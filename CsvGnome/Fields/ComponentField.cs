using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    /// <summary>
    /// Field consisting of one or more components, in any order.
    /// E.g. "CatInfo:Cat_Number_[++]_of_[date]"
    /// </summary>
    class ComponentField : IField
    {
        public string Name { get; }

        public string Summary { get; }

        public string GetValue(int row)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Components.Length; i++)
            {
                sb.Append(Components[i].GetValue(row));
            }

            return sb.ToString();
        }

        protected IComponent[] Components;

        public ComponentField(String name, IComponent[] components)
        {
            Name = name;
            Summary = components.Select(c => c.Summary).Aggregate((t, n) => $"{t}{n}");
            Components = components;
        }
    }
}
