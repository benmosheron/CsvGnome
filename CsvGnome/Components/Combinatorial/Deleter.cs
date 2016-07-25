using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components.Combinatorial
{
    public class Deleter
    {
        Cache cache;
        public Deleter(Cache cache)
        {
            this.cache = cache;
        }

        public void Delete(ICombinatorial component)
        {
            // the component must be removed from it's group
            string groupId = component.Group.Id;
            Group group = cache[groupId];
            group.DeleteComponent(component);
        }

        public void Clear()
        {
            cache.Clear();
        }
    }
}
