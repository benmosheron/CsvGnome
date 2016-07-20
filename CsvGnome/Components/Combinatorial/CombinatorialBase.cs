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
        public Group Group { get; protected set; }

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
    }
}
