using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components.Combinatorial
{
    /// <summary>
    /// Components which implement ICombinatorial can be created with a group ID.
    /// Values of components in the group cycle combinatorially.
    /// </summary>
    public interface ICombinatorial
    {
        /// <summary>
        /// Every combinatorial component must be in a group with other
        /// combinatorial components. It needs a reference to this
        /// group to generate its values.
        /// </summary>
        Group Group { get; }

        /// <summary>
        /// Must be unique in the group. Lower dimensions are
        /// cycled through first.
        /// </summary>
        int Dimension { get; }

        /// <summary>
        /// The number of possible values this component can take.
        /// Null corresponds to an infinite number, in which case
        /// all higher dimension components will be constant.
        /// </summary>
        int? Cardinality { get; }
    }
}
