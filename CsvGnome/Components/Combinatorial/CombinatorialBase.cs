using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components.Combinatorial
{
    /// <summary>
    /// Common ICombinatorial behaviour.
    /// Consumers should override Cardinality.
    /// </summary>
    public class CombinatorialBase : ICombinatorial
    {
        /// <summary>
        /// The group to which this component belongs.
        /// </summary>
        public Group Group { get; private set; }

        /// <summary>
        /// The dimension of this component within the group.
        /// </summary>
        public int Dimension => Group.DimensionOf(this);

        /// <summary>
        /// The number of possible values for this component.
        /// </summary>
        public virtual int? Cardinality { get { throw new NotImplementedException("CombinatorialBase Cardinality should be overridden."); } }

        /// <summary>
        /// Create a new component with a group.
        /// </summary>
        public CombinatorialBase(Group group)
        {
            Group = group;
        }

        /// <summary>
        /// Get the index of this value from its group, for the ith row.
        /// </summary>
        /// <param name="i">Row number.</param>
        /// <returns>Index of the return value.</returns>
        protected int GetValueIndex(int i)
        {
            return -99;
            // Copied from MinMaxComponent - we will follow the same method.
            // If this is the lowest dimension (highest index :/...) use modulo
            //if (Dim == Info.Dims - 1) return (Info.Mins[Dim] + ((i % Info.Cardinalities[Dim]) * Info.Increments[Dim])).ToString();

            //// Higher dimensions have i reduced by the product of lower dimensions' cardinalities
            //int productLowerDimCardinalities = Info.Cardinalities.Skip(Dim + 1).Aggregate(1, (t, n) => t * n);

            //return (Info.Mins[Dim] + (((i / productLowerDimCardinalities) % Info.Cardinalities[Dim]) * Info.Increments[Dim])).ToString();
        }
    }
}
