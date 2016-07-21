using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components.Combinatorial
{
    /// <summary>
    /// Because a component may have infinite cardinality (if we want it to cycle indefinitely),
    /// we can optionally provide a final dimension. The cardinality then applies only to lower dimensions.
    /// </summary>
    public class GroupCardinality
    {
        /// <summary>
        /// The product of the cardinalities of every finite dimension.
        /// Null if there are no finite dimensions.
        /// </summary>
        public long? Cardinality;

        /// <summary>
        /// If provided, indicates that the cardinality only applies up to this dimension.
        /// Higher dimensions will never change.
        /// </summary>
        public int? IndexOfFinalDimension;

        /// <summary>
        /// There are no finite dimensions.
        /// </summary>
        public GroupCardinality()
        {
            IndexOfFinalDimension = 0;
        }

        /// <summary>
        /// There are no infinite dimensions.
        /// </summary>
        /// <param name="cardinality">Product of all the cardinalities.</param>
        public GroupCardinality(long cardinality)
        {
            Cardinality = cardinality;
        }

        /// <summary>
        /// There are finite dimensions below an infinite dimension.
        /// </summary>
        /// <param name="cardinality">Product of all the cardinalities with dimension lower than the first infinite dimension.</param>
        /// <param name="lastFiniteDimension">The dimension of the first infinite dimension.</param>
        public GroupCardinality(long cardinality, int lastFiniteDimension)
        {
            Cardinality = cardinality;
            IndexOfFinalDimension = lastFiniteDimension;
        }
    }
}
