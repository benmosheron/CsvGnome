﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components.Combinatorial
{
    public class IncrementingCombinatorial: CombinatorialBase, IComponent
    {
        #region ICombinatorial

        public override long? Cardinality => null;

        #endregion

        public const int DefaultStart = 0;
        public const int DefaultIncrement = 1;
        public string Command { get; }
        public List<Message> Summary => new List<Message> { new Message(Command, Program.SpecialColour) };

        public bool Equals(IComponent x)
        {
            if (x == null) return false;
            var c = x as IncrementingCombinatorial;
            if (c == null) return false;
            if (start != c.start) return false;
            if (increment != c.increment) return false;
            return true;
        }

        /// <summary>
        /// Value to start incrementing from. Default 0.
        /// </summary>
        private int start;

        /// <summary>
        /// Value to add each row. Default 1;
        /// </summary>
        private int increment;

        /// <summary>
        /// Do not use this! Use the Factory class, which will manage the cache.
        /// </summary>
        public IncrementingCombinatorial(
            Group group,
            IncrementingComponent rawComponent)
            :base(group, rawComponent)
        {
            start = rawComponent.Start;
            increment = rawComponent.Increment;

            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            if (start != DefaultStart) sb.Append(start);
            sb.Append("++");
            if (increment != DefaultIncrement) sb.Append(increment);
            sb.Append("]");

            Command = sb.ToString();
        }

        /// <summary>
        /// Format of integer (e.g. "D3" to always print 3 digits)
        /// </summary>
        public string getFormat()
        {
            int minDigits = (Math.Max(Math.Abs(start) + (Program.N * Math.Abs(increment)) - 1, 1)).ToString().Length;
            return "D" + minDigits.ToString();
        }
    }
}
