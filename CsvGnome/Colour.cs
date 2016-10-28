using System;

namespace CsvGnome
{
    /// <summary>
    /// Manages console friendly colours.
    /// </summary>
    public static class Colour
    {
        /// <summary>
        /// List of colours which look good.
        /// </summary>
        public static ConsoleColor[] Colours =
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

        private static int index = 0;

        public static ConsoleColor Get()
        {
            int i = index++;
            if (index >= Colours.Length) index = 0;
            return Colours[i];
        }

    }
}
