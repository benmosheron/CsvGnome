﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CsvGnome
{
    public class ComponentFactory
    {
        // * zero or more
        // + one or more
        // ? zero or one
        private const string IncrementingPattern = @"\[ *\-?\d* *\+\+ *\-?\d* *(every +\d+ *)?\]";
        private Regex IncrementingRegex = new Regex(IncrementingPattern);

        private const string MinMaxPattern = @"\[ *\-?\d+ *=> *\-?\d+ *,* *\-?\d* *(?:#.+?)*\]";
        private Regex MinMaxRegex = new Regex(MinMaxPattern);

        private MinMaxInfoCache cache;

        public ComponentFactory(MinMaxInfoCache cache)
        {
            this.cache = cache;
        }

        public IComponent Create(string prototype)
        {
            // e.g.
            // [++]
            // [1++2]
            // [-99++-109]
            if (IncrementingRegex.IsMatch(prototype))
            {
                // remove "[" and "]"
                string protoInc = prototype.Substring(1, prototype.Length - 2);

                // split on "++" and "every" for the start, increment and every.
                string[] tokens = protoInc.Split(new string[] { "++", "every" },StringSplitOptions.None);

                int start;
                int increment;
                int every;
                if (!int.TryParse(tokens[0], out start)) start = IncrementingComponent.DefaultStart;
                if (!int.TryParse(tokens[1], out increment)) increment = IncrementingComponent.DefaultIncrement;
                if (tokens.Length <= 2 || !int.TryParse(tokens[2], out every)) every = IncrementingComponent.DefaultEvery;

                return new IncrementingComponent(start, increment, every);
            }
            // e.g. [11=>21,2 #testId]
            else if (MinMaxRegex.IsMatch(prototype))
            {
                // remove "[" and "]"
                string protoMinMax = prototype.Substring(1, prototype.Length - 2);
                string protoId = null;
                int? increment = null;
                // Does it have an ID at the end? (signified by #)
                if (protoMinMax.Contains("#"))
                {
                    // Everything right of # is the id (the gnome removes whitespace in the ids!)
                    string[] tokens = protoMinMax.Split(new string[] { "#" }, StringSplitOptions.None);
                    protoMinMax = tokens[0];
                    protoId = tokens[1].Trim();
                }

                // Does it specify an increment? (signified by ,)
                if (protoMinMax.Contains(","))
                {
                    string[] tokens = protoMinMax.Split(new string[] { "," }, StringSplitOptions.None);
                    protoMinMax = tokens[0];
                    string protoIncrement = tokens[1];
                    int temp;
                    if (int.TryParse(protoIncrement, out temp)) increment = temp;
                }

                // Extract the min and max (presence assured by regex)
                string[] tokens2 = protoMinMax.Split(new string[] { "=>" }, StringSplitOptions.None);
                int min = int.Parse(tokens2[0]);
                int max = int.Parse(tokens2[1]);

                if (String.IsNullOrWhiteSpace(protoId))
                {
                    if (!increment.HasValue)
                        return new MinMaxComponent(min, max);
                    else return new MinMaxComponent(min, max, increment.Value);
                }
                else
                {
                    if (!increment.HasValue)
                        return new MinMaxComponent(min, max, protoId, cache);
                    else return new MinMaxComponent(min, max, increment.Value, protoId, cache);
                }
            }
            else if (prototype.StartsWith(Program.SpreadComponentString))
            {
                // remove "[spread]"
                var array = prototype.Substring(Program.SpreadComponentString.Length);
                return new ArraySpreadComponent(GetArray(array));
            }
            else if (prototype.StartsWith(Program.CycleComponentString))
            {
                // remove "[cycle]"
                var array = prototype.Substring(Program.CycleComponentString.Length);
                return new ArrayCycleComponent(GetArray(array));
            }
            else if(prototype == Program.DateComponentString)
            {
                return new DateComponent();
            }
            else
            {
                return new TextComponent(prototype);
            }
        }

        private string[] GetArray(string prototypeArray)
        {
            // Remove { and } from end
            var trimmedArray = prototypeArray.Trim(new char[] { '{', '}' });
            var tokens = trimmedArray.Split(new string[] { "," }, StringSplitOptions.None);
            string[] values;
            if (tokens == null || !tokens.Any()) values = new string[] { String.Empty };
            else values = tokens;
            return values;
        }
    }
}
