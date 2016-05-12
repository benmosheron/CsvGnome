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

        public List<ICombinableField> CombinableFields => fields.Where(f => f is ICombinableField).Cast<ICombinableField>().ToList();

        /// <summary>
        /// True if a field with matching name is present;
        /// </summary>
        public bool ContainsName(string name)
        {
            return fields.Any(f => f.Name == name);
        }

        /// <summary>
        /// True if a field with matching name is ICombinable and uniquely identifiable by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool FieldValidForCombine(string name)
        {
            // Each ICombinableField must be uniquely identifiable
            return CombinableFields.Count(f => f.Name == name) == 1;
        }

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

        #region MinMaxField

        /// <summary>
        /// Looks for a fields with matching <paramref name="name"/>. If found, that field is updated. Otherwise a new MinMax field is created.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="min">Minimum (starting) value.</param>
        /// <param name="max">Written values will not increase past this value.</param>
        /// <param name="increment">Amount to increase.</param>
        public void AddOrUpdateMinMaxField(string name, int min, int max, int increment)
        {
            if (fields.Any(f => f.Name == name))
            {
                UpdateMinMaxField(name, min, max, increment);
            }
            else
            {
                AddMinMaxField(name, min, max, increment);
            }
        }

        private void UpdateMinMaxField(string name, int min, int max, int increment)
        {
            fields[fields.FindIndex(f => f.Name == name)] = new MinMaxField(name, min, max, increment);
        }

        private void AddMinMaxField(string name, int min, int max, int increment)
        {
            fields.Add(new MinMaxField(name, min, max, increment));
        }

        #endregion

        public void CombineFields(List<ICombinableField> fieldsToCombine, string setName)
        {
            // Create the info that will bind the combine fields
            CombinatorialFieldInfo info = new CombinatorialFieldInfo(fieldsToCombine, setName);

            // Relevant information is contained in the info
            // Remove existing fields
            var names = fieldsToCombine.Select(ftc => ftc.Name).ToList();
            names.ForEach(name => fields.Remove(CombinableFields.First(f => f.Name == name) as IField));

            // Add new combinatorial fields
            for(int i = 0; i<fieldsToCombine.Count; i++)
            {
                fields.Add(new CombinatorialField(fieldsToCombine[i].Name, info, i));
            }
        }
    }
}
