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

        public MinMaxInfoCache()
        {
            Cache = new Dictionary<string, MinMaxInfo>();
        }
    }
}
