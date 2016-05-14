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
    public class FieldBrain
    {
        private List<IField> fields = Defaults.GetFields();

        /// <summary>
        /// Fields (read only, updates will not be committed).
        /// </summary>
        public List<IField> Fields => fields.ToList();

        /// <summary>
        /// True if a field with matching name is present;
        /// </summary>
        public bool ContainsName(string name)
        {
            return fields.Any(f => f.Name == name);
        }

        /// <summary>
        /// Delete all fields.
        /// </summary>
        public void ClearFields()
        {
            fields.Clear();
        }

        public void DeleteField(string name, MinMaxInfoCache cache)
        {
            IField toRemove = fields.LastOrDefault(f => f.Name == name.Trim());
            if (toRemove == null) return;

            // If we're removing a field with one or more combinatorial MinMix components, we must adjust the MinMaxInfo and Dims
            // of any other components in that set.
            ComponentField cfToRemove = toRemove as ComponentField;
            if (cfToRemove != null)
            {
                var minMaxsToRemove = cfToRemove.Components
                    .OfType<MinMaxComponent>()
                    .Where(c => c.Info.Id != null);

                if (minMaxsToRemove.Any())
                {
                    cache.UpdateCacheForDelete(minMaxsToRemove);
                }
            }

            fields.Remove(toRemove);
        }

        #region ComponentField

        public void AddOrUpdateComponentField(string name, IComponent[] components)
        {
            if (fields.Any(f => f.Name == name))
            {
                UpdateComponentField(name, components);
            }
            else
            {
                AddComponentField(name, components);
            }
        }

        private void UpdateComponentField(string name, IComponent[] components)
        {
            fields[fields.FindIndex(f => f.Name == name)] = new ComponentField(name, components);
        }

        private void AddComponentField(string name, IComponent[] components)
        {
            fields.Add(new ComponentField(name, components));
        }

        #endregion

    }
}
