using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components.Combinatorial
{
    /// <summary>
    /// Group of combinatorial components.
    /// </summary>
    public class Group
    {
        /// <summary>
        /// Identifier of the group. Input via command as e.g. #idOfGroup
        /// </summary>
        public String Id { get; private set; }

        /// <summary>
        /// Collection of components in this group. Ordered by dimension.
        /// </summary>
        public IReadOnlyCollection<ICombinatorial> Components =>
            new ReadOnlyCollection<ICombinatorial>(rankToComponent.Select(c => c.Value).OrderBy(c => c.Dimension).ToList());

        /// <summary>
        /// Maps the rank of components to the components in this group. 
        /// </summary>
        /// <remarks>
        /// Ranks should run 0,1,2...etc. but we must allow for higher ranks being specified
        /// first when e.g. reading from a gnomefile.
        /// </remarks>
        private Dictionary<int, ICombinatorial> rankToComponent = new Dictionary<int, ICombinatorial>();

        /// <summary>
        /// Maps a component in this group to its rank.
        /// </summary>
        private Dictionary<ICombinatorial, int> componentToRank => rankToComponent.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

        /// <summary>
        /// If a new component is added to this group, it will have this rank.
        /// </summary>
        public int NextRank => GetNextRank();

        /// <summary>
        /// The total number of unique combinations
        /// </summary>
        public GroupCardinality GroupCardinality
        {
            get
            {
                // If every component has a finite cardinality, return the product.
                if (Components.All(c => c.Cardinality.HasValue))
                {
                    return new GroupCardinality(Components.Select(c => c.Cardinality.Value).Aggregate((t, n) => t * n));
                }
                else
                {
                    // We should return the product of cardinalities of every component with a 
                    // dimension lower than the first infinite dimension.

                    // First get the first infinite dimension.
                    int firstInfiniteDimension = DimensionOf(Components.First(c => !c.Cardinality.HasValue));

                    // If this was the first (or only) then we are cycling forever
                    if (firstInfiniteDimension == 0) return new GroupCardinality();

                    // Every lower dimension will have a finite cardinality - let's get them!
                    long finiteCardinalities = Components
                        .Where(c => c.Dimension < firstInfiniteDimension)
                        .Select(c => c.Cardinality.Value)
                        .Aggregate((t, n) => t * n);

                    return new GroupCardinality(finiteCardinalities, firstInfiniteDimension);
                }
            }
        }

        public ConsoleColor Colour { get; }

        /// <summary>
        /// The product of cardinalities of every component with a dimension lower than the input dimension.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">If dimension is zero.</exception>
        /// <exception cref="InvalidOperationException">If any lower dimensions are infinite.</exception>
        public long FiniteCardinalityOfLowerDimensions(int dimension)
        {
            if (dimension == 0) throw new ArgumentOutOfRangeException(nameof(dimension), "Cannot calculate cardinality for dimensions lower than zero.");

            var lowerDimensions = Components.Where(c => c.Dimension < dimension);

            if (lowerDimensions.Any(c => !c.Cardinality.HasValue)) throw new InvalidOperationException("Lower dimension is infinite.");

            return lowerDimensions.Select(c => c.Cardinality.Value).Aggregate((t, n) => t * n);
        }

        /// <summary>
        /// Create a new group with the input id.
        /// </summary>
        public Group(string id, ConsoleColor colour)
        {
            Id = id;
            Colour = colour;
        }

        /// <summary>
        /// Get the dimension of a component in this group. Dimensions are the ranks of each component
        /// mapped to {0, 1, ..., N }.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public int DimensionOf(ICombinatorial c)
        {
            List<int> ranks = rankToComponent.Keys.OrderBy(r => r).ToList();

            return ranks.IndexOf(componentToRank[c]);
        }

        /// <summary>
        /// Get the rank of a component in this group.
        /// </summary>
        /// <exception cref="Exception">Thrown if the component is not in this group.</exception>
        public int RankOf(ICombinatorial c)
        {
            if (!componentToRank.ContainsKey(c)) throw new Exception($"Component [{c}] is not in group [{Id}].");

            return componentToRank[c];
        }

        /// <summary>
        /// Get the next guaranteed available rank.
        /// </summary>
        private int GetNextRank()
        {
            if (!rankToComponent.Any())
            {
                return 0;
            }

            // Don't return a negative number, even if the previous rank was -99 or something.
            return Math.Max(0, rankToComponent.Keys.Max() + 1);
        }

        /// <summary>
        /// Do not use this! Use the method in the cache.
        /// Add a component to this group, with the specified rank.
        /// </summary>
        /// <exception cref="CombinatorialGroupException">Thrown if a component with this rank already exists in this group.</exception>        
        public void AddComponent(int rank, ICombinatorial component)
        {
            if (rankToComponent.ContainsKey(rank)) throw new CombinatorialGroupException($"Group already has component with rank [{rank}].");
            if (componentToRank.ContainsKey(component)) throw new CombinatorialGroupException($"Group already has this component in.");
            rankToComponent[rank] = component;
        }

        /// <summary>
        /// Do not use this! Use the method in the cache.
        /// Delete a component to this group, with the specified rank.
        /// </summary>
        public void DeleteComponent(ICombinatorial component)
        {
            int rankOfComponentToRemove = RankOf(component);
            rankToComponent.Remove(rankOfComponentToRemove);
        }

        public void InsertRank(int rank)
        {
            if (rankToComponent.Keys.Contains(rank))
            {
                // Ensure there is space in which to insert the offender
                InsertRank(rank + 1);
                // Increase the rank of the offender
                rankToComponent[rank + 1] = rankToComponent[rank];
                // Clear a space
                rankToComponent.Remove(rank);
            }
            else
            {
                // It's too easy!
                return;
            }
        }
    }
}
