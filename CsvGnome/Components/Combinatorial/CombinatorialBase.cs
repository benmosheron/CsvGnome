using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components.Combinatorial
{
    /// <summary>
    /// Common ICombinatorial behaviour.
    /// Consumers should implement CombinatorialCore.
    /// </summary>
    public class CombinatorialBase : ICombinatorial
    {
        protected readonly IMessageProvider MessageProvider;
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
        public virtual long? Cardinality { get { throw new NotImplementedException("CombinatorialBase Cardinality should be overridden."); } }

        /// <summary>
        /// The non-combinatorial version of this component.
        /// </summary>
        protected IComponent RawComponent { get; private set; }

        /// <summary>
        /// Create a new component with a group.
        /// </summary>
        public CombinatorialBase(Group group, IComponent rawComponent, IMessageProvider messageProvider)
        {
            Group = group;
            RawComponent = rawComponent;
            MessageProvider = messageProvider;
        }

        /// <summary>
        /// Use the raw component to calculate the value by passing it the value index of the row.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual string GetValue(long i)
        {
            return RawComponent.GetValue(GetValueIndex(i));
        }

        /// <summary>
        /// Get the index of this value from its group, for the ith row.
        /// </summary>
        /// <param name="i">Row number.</param>
        /// <returns>Index of the return value.</returns>
        protected long GetValueIndex(long i)
        {
            GroupCardinality groupCardinality = Group.GroupCardinality;
            // If the group has a single infinite component, it will just increase forever.
            // All higher dimensions will stay at the zeroth element.
            if (groupCardinality.FirstIsInfinite)
            {
                if (Dimension > 0) return 0;
                else return i;
            }

            // If the group has finite cardinality, we are good to go!
            if (groupCardinality.AllAreFinite)
            {
                return FiniteValueIndex(i);
            }

            // We've got a mix of finite and infinite dimensions.

            // If this component has higher dimension than an infinite, it will be stuck at element zero.
            if (Dimension > groupCardinality.IndexOfFirstInfiniteDimension)
            {
                return 0;
            }
            else
            {
                // We are either a finite dimension, or the first infinite dimension.
                return FiniteValueIndex(i);
            }
        }

        /// <summary>
        /// Get the index that this component will be at, given row i.
        /// </summary>
        private long FiniteValueIndex(long i)
        {
            // If this is the lowest dimension, use the modulo
            if (Dimension == 0) return i % Cardinality.Value;

            // Higher dimensions have i reduced by the cardinalities of lower dimensions.
            long lowerDimensionCardinality = Group.FiniteCardinalityOfLowerDimensions(Dimension);

            if (Cardinality.HasValue)
            {
                return (i / lowerDimensionCardinality) % Cardinality.Value;
            }
            else
            {
                // This component is the lowest dimension infinite component.
                // Return i divided by the lower dimensions.
                return i / lowerDimensionCardinality;
            }
        }

        /// <summary>
        /// Get the summary for this component. Combines a pre- and post-group message with the group.
        /// </summary>
        public List<IMessage> Summary
        {
            get
            {
                List<IMessage> summary = MessageProvider.EmptyList();
                summary.AddRange(GetPreGroupMessage());
                summary.AddRange(GetGroupMessage());
                summary.AddRange(GetPostGroupMessage());
                return summary;
            }
        }

        protected virtual List<IMessage> GetPreGroupMessage()
        {
            throw new NotImplementedException("Override PreGroupMessage in child class.");
        }

        public List<IMessage> GetGroupMessage()
        {
            return new List<IMessage>()
            {
                MessageProvider.NewSpecial("#"),
                MessageProvider.New(Group.Id, Group.Colour),
                MessageProvider.NewSpecial($"/{Group.RankOf(this)}")
            };
        }

        protected virtual List<IMessage> GetPostGroupMessage()
        {
            throw new NotImplementedException("Override PostGroupMessage in child class.");
        }

        public string GetGroupString()
        {
            return $"#{Group.Id}/{Group.RankOf(this)}";
        }

        public bool EqualsComponent(IComponent x)
        {
            if (x == null) return false;
            var c = x as CombinatorialBase;
            if (c == null) return false;
            if (Group.Id != c.Group.Id) return false;
            if (Cardinality != c.Cardinality) return false;
            if (!RawComponent.EqualsComponent(c.RawComponent)) return false;
            return true;
        }
    }
}
