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
    public class ComponentField : IField
    {
        public string Name { get; }

        public string Command => Components.Select(c => c.Command).Aggregate((t, n) => $"{t}{n}");
        public List<Message> Summary => Components.Select(c => c.Summary).Aggregate((t, n) => t.Concat(n).ToList()).ToList();

        public string GetValue(int row)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Components.Length; i++)
            {
                sb.Append(Components[i].GetValue(row));
            }

            return sb.ToString();
        }

        public readonly IComponent[] Components;

        public ComponentField(String name, IComponent[] components)
        {
            Name = name;
            Components = components;
        }
    }
}
