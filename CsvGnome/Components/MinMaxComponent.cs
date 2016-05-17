using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    /// <summary>
    /// Component with a minimum and maximum value, and optional increment.
    /// E.g. from zero to ten in steps of two: "ZeroToTenField:[0 => 10, 2]" 
    /// 
    /// Can optionally be given an ID to form a combinatorial combination with another component, e.g.:
    /// "x:[1 => 10, 1 #position]"
    /// "y:[1 => 10, 1 #position]"
    /// will link x and y to cover all combinations.
    /// </summary>
    public class MinMaxComponent : IComponent
    {
        /// <summary>
        /// The command to create this component.
        /// </summary>
        /// <remarks>
        /// We do not add the Dim for the command.
        /// </remarks>
        public string Command
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"[{Info.Mins[Dim]}=>{Info.Maxs[Dim]}");
                if (incrementProvided) sb.Append($",{Info.Increments[Dim]}");
                if (Info.Id != null)
                {
                    sb.Append(" #");
                    sb.Append(Info.Id);
                }
                sb.Append("]");
                return sb.ToString();
            }
        }

        /// <summary>
        /// Summary of this component for display.
        /// </summary>
        public List<Message> Summary {
            get
            {
                List<Message> m = new List<Message>();
                m.Add(new Message("[", Program.SpecialColour));
                m.Add(new Message($"{Info.Mins[Dim]}"));
                m.Add(new Message("=>", Program.SpecialColour));
                m.Add(new Message($"{Info.Maxs[Dim]}"));
                if (incrementProvided) m.Add(new Message($",{Info.Increments[Dim]}", Program.SpecialColour));
                if(Info.Id != null)
                {
                    m.Add(new Message(" #", Program.SpecialColour));
                    m.Add(new Message(Info.Id, Info.IdColour));
                    if (Info.Dims > 1) m.Add(new Message($"/{Dim}", Program.SpecialColour));
                }
                m.Add(new Message("]", Program.SpecialColour));
                return m;
            }
        }

        public bool Equals(IComponent x)
        {
            if (x == null) return false;
            var c = x as MinMaxComponent;
            if (c == null) return false;
            if (Dim != c.Dim) return false;
            if (!Info.Equals(c.Info)) return false;
            return true;
        }

        public string GetValue(int i)
        {
            // If this is the lowest dimension (highest index :/...) use modulo
            if (Dim == Info.Dims - 1) return (Info.Mins[Dim] + ((i % Info.Cardinalities[Dim]) * Info.Increments[Dim])).ToString();

            // Higher dimensions have i reduced by the product of lower dimensions' cardinalities
            int productLowerDimCardinalities = Info.Cardinalities.Skip(Dim + 1).Aggregate(1, (t, n) => t * n);

            return (Info.Mins[Dim] + (((i / productLowerDimCardinalities) % Info.Cardinalities[Dim]) * Info.Increments[Dim])).ToString();
        }

        private const int DefaultIncrement = 1;
        private bool incrementProvided = true;
        /// <summary>
        /// The dimension of this component. Used as an index in the Info's arrays.
        /// </summary>
        public int Dim = 0;

        public MinMaxInfo Info;

        // MinMaxField - no combinatorics
        public MinMaxComponent(int min, int max)
            :this(min, max, DefaultIncrement)
        {
            incrementProvided = false;
        }

        public MinMaxComponent(int min, int max, int increment)
        {
            Info = new MinMaxInfo(min, max, increment);
        }

        // Combinatorial MinMaxField
        public MinMaxComponent(int min, int max, string id, MinMaxInfoCache cache)
            : this(min, max, DefaultIncrement, id, cache)
        {
            incrementProvided = false;
        }

        public MinMaxComponent(int min, int max, int increment, string id, MinMaxInfoCache cache)
        {
            if (cache.Cache.ContainsKey(id))
            {
                cache.Cache[id].AddComponent(min, max, increment);
                Info = cache.Cache[id];
                Dim = Info.Dims - 1;
            }
            else
            {
                cache.Cache.Add(id, new MinMaxInfo(min, max, increment, id));
                Info = cache.Cache[id];
            }
            cache.Components.Add(this);
        }
    }
}
