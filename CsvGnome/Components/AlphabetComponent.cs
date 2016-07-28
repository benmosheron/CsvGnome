using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components
{
    /// <summary>
    /// Component which cycles through the alphabet.
    /// </summary>
    /// <example>
    /// [a=>c] = a,b,c
    /// [a=>C] = a,b,c,A,B,C
    /// [C=>a] = C,B,A,c,b,a
    /// </example>
    public class AlphabetComponent : IComponent
    {
        public char Start { get; private set; }
        public char End { get; private set; }

        public string Command => $"[{Start}=>{End}]";

        public List<Message> Summary => new Message(Command, Program.SpecialColour).ToList();

        private char[] Values;

        public AlphabetComponent(char start, char end)
        {
            Start = start;
            End = end;
            Values = GetArray(start, end);
        }

        private char[] GetArray(char start, char end)
        {
            string lower = new string(GetRange(start, end)).ToLowerInvariant();
            string upper = new string(GetRange(start, end)).ToUpperInvariant();

            List<char> first = new List<char>();
            if (IsLowerCase(start)) first = lower.ToCharArray().ToList();
            else first = upper.ToCharArray().ToList();

            List<char> second = new List<char>();
            if (IsLowerCase(end) && IsUpperCase(start)) second = lower.ToCharArray().ToList();
            if (IsUpperCase(end) && IsLowerCase(start)) second = upper.ToCharArray().ToList();

            return first.Union(second).ToArray();
        }

        private char[] GetRange(char startIn, char endIn)
        {
            char startLower = startIn.ToString().ToLowerInvariant()[0];
            char endLower = endIn.ToString().ToLowerInvariant()[0];

            bool reverse = startLower > endLower;

            char start = startLower;
            char end = endLower;

            if (reverse)
            {
                start = endLower;
                end = startLower;
            }


            if (start == end) return new char[] { start };

            List<char> values = new List<char>();

            while(start <= end)
            {
                values.Add(start++);
            }

            if (reverse) values.Reverse();

            return values.ToArray();
        }

        private bool IsLowerCase(char a)
        {
            return a >= 'a' && a <= 'z';
        }

        private bool IsUpperCase(char a)
        {
            return a >= 'A' && a <= 'Z';
        }

        public string GetValue(long i)
        {
            return Values[i % Values.Length].ToString();
        }

        public bool EqualsComponent(IComponent x)
        {
            if (x == null) return false;
            var c = x as AlphabetComponent;
            if (c == null) return false;
            if (Start != c.Start) return false;
            if (End != c.End) return false;
            return true;
        }
    }
}
