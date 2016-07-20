using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    public class MinMaxInfo
    {
        /// <summary>
        /// Name of the set. Null if this info is for a single MinMaxField.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Number of dimensions.
        /// </summary>
        public int Dims { get; private set; }

        /// <summary>
        /// Array of mininmum values for each component in this set.
        /// </summary>
        public List<int> Mins { get; }

        /// <summary>
        /// Array of maximum values for each component in this set.
        /// </summary>
        public List<int> Maxs { get; }

        /// <summary>
        /// Array of increment values for each component in this set.
        /// </summary>
        public List<int> Increments { get; }

        /// <summary>
        /// The number of elements possible for each of the components in this set.
        /// </summary>
        public List<int> Cardinalities { get; }

        /// <summary>
        /// Total lines that can be written by all components in this set.
        /// </summary>
        public int Lines => Cardinalities.Aggregate((t, n) => t * n);

        public ConsoleColor IdColour { get; }

        public MinMaxInfo(int min, int max, int increment)
            :this(min, max, increment, null)
        { }

        public MinMaxInfo(int min, int max, int increment, string name)
        {
            Id = name;
            Dims = 1;
            Mins = new List<int> { min };
            Maxs = new List<int> { max };
            Increments = new List<int> { increment };
            Cardinalities = new List<int> { ((max - min) / increment) + 1 };
            IdColour = GetNextColour();
        }

        /// <summary>
        /// Add a compatible MinMaxField  to this set.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public void AddComponent(int min, int max, int increment)
        {
            Dims++;
            Mins.Add(min);
            Maxs.Add(max);
            Increments.Add(increment);
            Cardinalities.Add(((max - min) / increment) + 1);
        }

        /// <summary>
        /// Remove a component from the info.
        /// </summary>
        /// <param name="dim"></param>
        public void RemoveComponent(int dim)
        {
            Dims--;
            Mins.RemoveAt(dim);
            Maxs.RemoveAt(dim);
            Increments.RemoveAt(dim);
            Cardinalities.RemoveAt(dim);
        }

        public bool Equals(MinMaxInfo x)
        {
            if (Id != null && Id != x.Id) return false;
            if (x.Id != null && Id == null) return false;
            if (!Mins.SequenceEqual(x.Mins)) return false;
            if (!Maxs.SequenceEqual(x.Maxs)) return false;
            if (!Increments.SequenceEqual(x.Increments)) return false;
            if (!Cardinalities.SequenceEqual(x.Cardinalities)) return false;
            return true;
        }

        #region Colour

        private static ConsoleColor[] Colours =
        {
            ConsoleColor.Green,
            ConsoleColor.Magenta,
            ConsoleColor.White,
            ConsoleColor.Yellow,
            ConsoleColor.Blue,
            ConsoleColor.DarkCyan,
            ConsoleColor.DarkGreen,
            ConsoleColor.DarkMagenta,
            ConsoleColor.DarkRed,
            ConsoleColor.DarkYellow
        };

        private static int ColourIndex = 0;

        private static ConsoleColor GetNextColour()
        {
            ConsoleColor c = Colours[ColourIndex];
            ColourIndex++;
            if (ColourIndex >= Colours.Length) ColourIndex = 0;
            return c;
        }

        #endregion
    }
}
