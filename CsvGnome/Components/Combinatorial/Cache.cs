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
        private HashSet<string> ids = new HashSet<string>();
        private Dictionary<string, Group> Groups = new Dictionary<string, Group>();

        public bool Contains(string id) => ids.Contains(id);

    }
}
