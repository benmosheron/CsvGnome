using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components.Combinatorial
{
    /// <summary>
    /// Group of combinatorial components.
    /// </summary>
    public class Group
    {
        /// <summary>
        /// Identifier of the group. Input via command as e.g. #idOfGroup
        /// </summary>
        public String Id { get; private set; }

        /// <summary>
        /// Collection of components in this group.
        /// </summary>
        public IReadOnlyCollection<ICombinatorial> Components => new ReadOnlyCollection<ICombinatorial>(componentList);

        /// <summary>
        /// List of components in this group. The index of every component in this list must be strictly
        /// equal to its dimension.
        /// </summary>
        private List<ICombinatorial> componentList { get; set; }

        /// <summary>
        /// If a new component is added to this group, it will have this dimension.
        /// </summary>
        public int NextDimension => GetNextDimension();

        /// <summary>
        /// Create a new group with the input id.
        /// </summary>
        public Group(string id)
        {
            Id = id;
            componentList = new List<ICombinatorial>();
        }

        /// <summary>
        /// Get the dimension of a component in this group.
        /// </summary>
        /// <exception cref="Exception">Thrown if the component is not in this group.</exception>
        public int DimensionOf(ICombinatorial c)
        {
            int index = componentList.IndexOf(c);

            if (index == -1) throw new Exception($"Component [{c}] is not in group [{Id}].");

            return index;
        }

        private int GetNextDimension()
        {
            if (componentList == null)
            {
                throw new Exception("componentList for this group should not be null.");
            }

            if (!componentList.Any())
            {
                return 0;
            }

            return componentList.Count;
        }
    }
}
