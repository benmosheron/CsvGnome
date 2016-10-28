using System;

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
        public int? IndexOfFirstInfiniteDimension;

        #region Useful Properties

        /// <summary>
        /// The first dimension is infinite.
        /// </summary>
        public bool FirstIsInfinite => IndexOfFirstInfiniteDimension == 0;

        /// <summary>
        /// There are no infinite dimensions.
        /// </summary>
        public bool AllAreFinite => !IndexOfFirstInfiniteDimension.HasValue;

        #endregion

        /// <summary>
        /// There are no finite dimensions.
        /// </summary>
        public GroupCardinality()
        {
            IndexOfFirstInfiniteDimension = 0;
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
        public GroupCardinality(long cardinality, int firstInfiniteDimension)
        {
            if (IndexOfFirstInfiniteDimension == 0) throw new ArgumentException($"Last finite dimension is 0, but a cardinality was provided.");
            Cardinality = cardinality;
            IndexOfFirstInfiniteDimension = firstInfiniteDimension;
        }
    }
}
