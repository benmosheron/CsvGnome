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
                    ArrayCycleComponent raw = rawComponent as ArrayCycleComponent;
                    // Check if a group with this ID already exists.
                    // if it doesn't exist, create it.
                    // Get the group.
                    ArrayCycleCombinatorial arrayCycleCombinatorial = new ArrayCycleCombinatorial(new Group("test"), raw.ValueArray);
                    return arrayCycleCombinatorial;
                default:
                    throw new Exception($"Cannot create an ICombinatorial from [{typeName}]");
            }

        }
    }
}
