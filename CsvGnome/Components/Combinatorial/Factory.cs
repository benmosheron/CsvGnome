﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components.Combinatorial
{
    /// <summary>
    /// <para>Creates combinatorial components.</para>
    /// <para>Ensures the cache is up to date.</para>
    /// </summary>
    public class Factory
    {
        // Supported raw components
        private const string c_arrayCycle = "CsvGnome.ArrayCycleComponent";
        private const string c_increment = "CsvGnome.IncrementingComponent";
        private const string c_minMax = "CsvGnome.MinMaxComponent";

        Cache Cache;
        public Factory(Cache cache)
        {
            Cache = cache;
        }

        /// <summary>
        /// Create an ICombinatorial component from a regular component, by assigning it to a group.
        /// A rank will be generated.
        /// </summary>
        public ICombinatorial Create(string groupId, IComponent rawComponent)
        {
            return Create(groupId, rawComponent, null);
        }

        /// <summary>
        /// Create an ICombinatorial component from a regular component, by assigning it to a group and specifying a rank.
        /// </summary>
        public ICombinatorial Create(string groupId, IComponent rawComponent, int? maybeRank)
        {
            string typeName = rawComponent.GetType().FullName;
            switch (typeName) {
                case c_arrayCycle:
                    return CreateArrayCycleCombinatorial(groupId, rawComponent as ArrayCycleComponent, maybeRank);
                case c_increment:
                    return CreateIncrementingCombinatorial(groupId, rawComponent as IncrementingComponent, maybeRank);
                case c_minMax:
                    return CreateMinMaxCombinatorial(groupId, rawComponent as MinMaxComponent, maybeRank);
                default:
                    throw new Exception($"Cannot create an ICombinatorial from [{typeName}]");
            }
        }

        private Group InitGroup(string groupId, int? maybeRank, out int rank)
        {
            // Check if a group with this ID already exists.
            if (!Cache.Contains(groupId))
            {
                // if it doesn't exist, create it.
                Cache.CreateGroup(groupId);
            }

            // Get the group.
            Group group = Cache[groupId];

            // If we don't know the rank, get the next rank
            if (!maybeRank.HasValue)
            {
                rank = group.NextRank;
            }
            else
            {
                rank = maybeRank.Value;
            }

            return group;
        }

        /// <summary>
        /// Create an ArrayCycleCombinatorial, adding it to a group and ensure the cache is up to date
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="rawComponent"></param>
        /// <returns></returns>
        private ArrayCycleCombinatorial CreateArrayCycleCombinatorial(string groupId, ArrayCycleComponent rawComponent, int? maybeRank)
        {
            int rank;
            Group group = InitGroup(groupId, maybeRank, out rank);

            // Create the component. Register the group with the component (but beware, the group does not know about the component yet!).
            var arrayCycleCombinatorial = new ArrayCycleCombinatorial(group, rawComponent);

            // Update the group.
            Cache.AddComponentToGroup(groupId, arrayCycleCombinatorial, rank);

            // Return the component.
            return arrayCycleCombinatorial;
        }

        private IncrementingCombinatorial CreateIncrementingCombinatorial(string groupId, IncrementingComponent rawComponent, int? maybeRank)
        {
            int rank;
            Group group = InitGroup(groupId, maybeRank, out rank);

            // Create the component. Register the group with the component (but beware, the group does not know about the component yet!).
            var arrayCycleCombinatorial = new IncrementingCombinatorial(group, rawComponent);

            // Update the group.
            Cache.AddComponentToGroup(groupId, arrayCycleCombinatorial, rank);

            // Return the component.
            return arrayCycleCombinatorial;
        }

        private MinMaxCombinatorial CreateMinMaxCombinatorial(string groupId, MinMaxComponent rawComponent, int? maybeRank)
        {
            int rank;
            Group group = InitGroup(groupId, maybeRank, out rank);

            // Create the component. Register the group with the component (but beware, the group does not know about the component yet!).
            var minMaxCombinatorial = new MinMaxCombinatorial(group, rawComponent);

            // Update the group.
            Cache.AddComponentToGroup(groupId, minMaxCombinatorial, rank);

            // Return the component.
            return minMaxCombinatorial;
        }

    }
}
