using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    /// <summary>
    /// Stores and and handles updates to the list of fields.
    /// </summary>
    class FieldBrain
    {
        private List<IField> fields = Defaults.GetFields();

        /// <summary>
        /// Fields (for reading).
        /// </summary>
        public List<IField> Fields => fields.ToList();

        public void Update(string name, string instruction)
        {
            if(fields.Any(f => f.Name == name))
            {
                UpdateField(name, instruction);
            }
            else
            {
                AddField(name, instruction);
            }
        }

        private void UpdateField(string name, string instruction)
        {
            fields[fields.FindIndex(f => f.Name == name)] = new ConstantField(name, instruction);
        }

        private void AddField(string name, string instruction)
        {
            fields.Add(new ConstantField(name, instruction));
        }
    }
}
