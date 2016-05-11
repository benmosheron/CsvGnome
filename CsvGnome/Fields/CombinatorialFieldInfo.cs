using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    class CombinatorialFieldInfo
    {
        private static int _id = 0;

        public int Id { get; }

        /// <summary>
        /// Name of the set.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Number of dimensions.
        /// </summary>
        public int Dims { get; }

        /// <summary>
        /// Array of mininmum values for each field in this set.
        /// </summary>
        public int[] Mins { get; }

        /// <summary>
        /// Array of maximum values for each field in this set.
        /// </summary>
        public int[] Maxs { get; }

        /// <summary>
        /// Array of increment values for each field in this set.
        /// </summary>
        public int[] Increments { get; }

        /// <summary>
        /// The number of elements possible for each of the fields in this set.
        /// </summary>
        public int[] Cardinalities { get; }

        /// <summary>
        /// Total lines that can be written by all fields in this set.
        /// </summary>
        public int Length => Cardinalities.Aggregate((t, n) => t * n);

        /// <summary>
        /// Create a new set of information for combined fields. The order of the input list is preserved adn replicated in this classes arrays.
        /// </summary>
        /// <param name="dims"></param>
        /// <param name="fields"></param>
        public CombinatorialFieldInfo(List<ICombinableField> fields, string name)
        {
            Id = _id;
            _id++;
            Name = name;
            Dims = fields.Count;
            Mins = fields.Select(f => f.Min).ToArray();
            Maxs = fields.Select(f => f.Max).ToArray();
            Increments = fields.Select(f => f.Increment).ToArray();
            Cardinalities = new int[Dims];
            for (int i = 0; i < Dims; i++)
            {
                Cardinalities[i] = ((Maxs[i] - Mins[i]) / Increments[i]) + 1;
            }
        }
    }
}
