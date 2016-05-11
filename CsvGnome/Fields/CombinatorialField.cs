using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    class CombinatorialField : IField
    {
        public string Name { get; }

        public string Summary { get; }

        public string GetValue(int i)
        {
            // If this is the lowest dimension (highest index :/...) use modulo
            if(Dim == Info.Dims - 1) return (i % Info.Cardinalities[Dim]).ToString();

            // Higher dimensions are affected by the product of lower dimensions' cardinalities
            int productLowerDimCardinalities = Info.Cardinalities.Skip(Dim + 1).Aggregate(1, (t, n) => t * n);

            return ((i / productLowerDimCardinalities) % Info.Cardinalities[Dim]).ToString();
        }

        public CombinatorialFieldInfo Info;

        /// <summary>
        /// The dimension of this field. Used as an index in the Info's arrays.
        /// </summary>
        public int Dim;

        public CombinatorialField(string name, CombinatorialFieldInfo info, int dim)
        {
            Name = name;
            Info = info;
            Dim = dim;

            Summary = $"{info.Mins[Dim]} -> {info.Maxs[Dim]} [{info.Increments[Dim]}] #{info.Name ?? info.Id.ToString()}/{Dim}";
        }
    }
}
