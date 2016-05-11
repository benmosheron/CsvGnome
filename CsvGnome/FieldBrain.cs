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
        /// Fields (read only, updates will not be committed).
        /// </summary>
        public List<IField> Fields => fields.ToList();

        #region ConstantField
        /// <summary>
        /// Looks for a fields with matching <paramref name="name"/>. If found, that field is updated. Otherwise a new constant field is created.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddOrUpdateConstantField(string name, string value)
        {
            if (fields.Any(f => f.Name == name))
            {
                UpdateConstantField(name, value);
            }
            else
            {
                AddConstantField(name, value);
            }
        }

        private void UpdateConstantField(string name, string value)
        {
            fields[fields.FindIndex(f => f.Name == name)] = new ConstantField(name, value);
        }

        private void AddConstantField(string name, string value)
        {
            fields.Add(new ConstantField(name, value));
        }
        #endregion

        #region IncrementingField

        /// <summary>
        /// Looks for a fields with matching <paramref name="name"/>. If found, that field is updated. Otherwise a new incrementing field is created.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddOrUpdateIncrementingField(string name, string value, int start)
        {
            if (fields.Any(f => f.Name == name))
            {
                UpdateIncrementingField(name, value, start);
            }
            else
            {
                AddIncrementingField(name, value, start);
            }
        }

        private void UpdateIncrementingField(string name, string value, int start)
        {
            fields[fields.FindIndex(f => f.Name == name)] = new IncrementingField(name, value, start);
        }

        private void AddIncrementingField(string name, string value, int start)
        {
            fields.Add(new IncrementingField(name, value, start));
        }

        #endregion

    }
}
