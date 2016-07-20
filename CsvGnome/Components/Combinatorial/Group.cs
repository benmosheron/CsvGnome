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
        public IReadOnlyCollection<ICombinatorial> Components =>
            new ReadOnlyCollection<ICombinatorial>(idToComponent.Select(c=>c.Value).ToList());

        /// <summary>
        /// Maps the dimension of components to the components in this group. 
        /// </summary>
        /// <remarks>
        /// Dimensions should run 0,1,2...etc. but we must allow for higher dimensions being specified
        /// first when e.g. reading from a gnomefile.
        /// </remarks>
        private Dictionary<int, ICombinatorial> idToComponent = new Dictionary<int, ICombinatorial>();

        /// <summary>
        /// Maps a component in this group to its dimension.
        /// </summary>
        private Dictionary<ICombinatorial, int> componentToId => idToComponent.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

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
        }

        /// <summary>
        /// Get the dimension of a component in this group.
        /// </summary>
        /// <exception cref="Exception">Thrown if the component is not in this group.</exception>
        public int DimensionOf(ICombinatorial c)
        {
            if (!componentToId.ContainsKey(c)) throw new Exception($"Component [{c}] is not in group [{Id}].");

            return componentToId[c];
        }

        /// <summary>
        /// Get the next guaranteed available dimension.
        /// </summary>
        private int GetNextDimension()
        {
            if (!idToComponent.Any())
            {
                return 0;
            }

            return idToComponent.Keys.Max() + 1;
        }
    }
}
