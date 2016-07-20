using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components.Combinatorial
{
    /// <summary>
    /// <para>Creates, updates and deletes combinatorial components.</para>
    /// <para>Ensures the cache is up to date.</para>
    /// </summary>
    public class Factory
    {
        private const string c_arrayCycle = "CsvGnome.ArrayCycleComponent";
        Cache Cache;
        public Factory(Cache cache)
        {
            Cache = cache;
        }
        


        public ICombinatorial Create(string groupId, IComponent rawComponent)
        {
            string typeName = rawComponent.GetType().FullName;
            switch (typeName) {
                case c_arrayCycle:
                    return CreateArrayCycleCombinatorial(groupId, rawComponent as ArrayCycleComponent);
                default:
                    throw new Exception($"Cannot create an ICombinatorial from [{typeName}]");
            }
        }

        private ArrayCycleCombinatorial CreateArrayCycleCombinatorial(string groupId, ArrayCycleComponent rawComponent)
        {
            // Check if a group with this ID already exists.
            if (!Cache.Contains(groupId))
            {
                // if it doesn't exist, create it.
                Cache.CreateGroup(groupId);
            }

            // Get the group.
            Group group = Cache[groupId];

            // Create the component
            var arrayCycleCombinatorial = new ArrayCycleCombinatorial(group, rawComponent.ValueArray);

            // Update the group.
            Cache.AddComponentToGroup(groupId, arrayCycleCombinatorial);

            // Return the component.
            return arrayCycleCombinatorial;
        }
    }
}
