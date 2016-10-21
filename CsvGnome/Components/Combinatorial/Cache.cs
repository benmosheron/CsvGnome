using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components.Combinatorial
{
    /// <summary>
    /// Tracks all of the current combinatorial components and groups.
    /// </summary>
    public class Cache
    {
        private HashSet<string> ids => new HashSet<string>(Groups.Keys);
        private Dictionary<string, Group> Groups = new Dictionary<string, Group>();

        /// <summary>
        /// True if the cache contains a group with the given id. False otherwise.
        /// </summary>
        public bool Contains(string id) => ids.Contains(id);

        /// <summary>
        /// Create a new group and add it to the cache.
        /// </summary>
        /// <exception cref="Exception">Thrown if a group with the same id already exists.</exception>
        public void CreateGroup(string id)
        {
            if (Contains(id)) throw new Exception($"Group [{id}] already exists");
            Groups[id] = new Group(id, Colour.Get());
        }

        /// <summary>
        /// Adds a component to a group with a specifed rank, and updates the cache.
        /// </summary>
        /// <param name="id">Id of the group to add the component to.</param>
        public void AddComponentToGroup(string id, ICombinatorial component, int rank)
        {
            if (!Groups.ContainsKey(id)) throw new Exception($"Group with id [{id}] does not exist in the cache");

            // If the group already contains a component of this rank, we need to make room for it.
            Groups[id].InsertRank(rank);

            Groups[id].AddComponent(rank, component);
        }

        public void Clear()
        {
            // Do we need to delete every component's group reference?
            // GC should handle it as long as all fields/components are deleted.
            Groups.Clear();
        }


        public Group this[string id] => Groups[id];
    }
}
