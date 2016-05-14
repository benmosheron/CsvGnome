using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    /// <summary>
    /// Stores current MinMaxInfo IDs.
    /// Only infos with IDs (names) are tracked.
    /// </summary>
    public class MinMaxInfoCache
    {
        public Dictionary<string, MinMaxInfo> Cache;

        public List<MinMaxComponent> Components;

        public MinMaxInfoCache()
        {
            Cache = new Dictionary<string, MinMaxInfo>();
            Components = new List<MinMaxComponent>();
        }

        public void UpdateCacheForDelete(IEnumerable<MinMaxComponent> toRemove)
        {
            // Store a dict of the id of each info, and a list of all the dimensions that will be removed from it.
            Dictionary<string, HashSet<int>> affected = new Dictionary<string, HashSet<int>>();
            foreach(var c in toRemove)
            {
                string id = c.Info.Id;
                int dim = c.Dim;
                if (!affected.ContainsKey(id)) affected.Add(id, new HashSet<int> { dim });
                else affected[id].Add(dim);
            }

            // The remove all affected dims from each affected info
            foreach(string id in affected.Keys)
            {
                // Sort to ensure we remove higher dimensions first
                List<int> dims = affected[id].ToList();
                dims.Sort();
                dims.Reverse();
                foreach (int dim in dims)
                {
                    Cache[id].RemoveComponent(dim);
                }
            }

            // For each component being removed, update the dims of any higher order components
            // and remove the component from the component cache
            foreach (var mmc in toRemove)
            {
                Components.Remove(mmc);

                // Find higher order components with the same Info.id
                var toLower = Components
                    .Where(c => c.Info.Id == mmc.Info.Id)
                    .Where(c => c.Dim > mmc.Dim)
                    .ToList();

                toLower.ForEach(c => c.Dim--);
            }
            

        }
    }
}
