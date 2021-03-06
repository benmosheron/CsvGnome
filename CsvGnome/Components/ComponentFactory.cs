﻿using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace CsvGnome.Components
{
    public class ComponentFactory
    {
        // * zero or more
        // + one or more
        // ? zero or one
        // (?:) non-capturing group
        //
        // In case you don't know about it already:
        // https://regex101.com/
        //
        private const string DatePattern = @"\[ *date *(?<format>"".*"")* *\]"; // Note the named (name = format) capturing group for a format.
        private Regex DateRegex = new Regex(DatePattern);
        
        private const string GroupPattern = @"( *#\w+(?:\/\d+)? *)";
        private Regex GroupRegex = new Regex(GroupPattern);

        private const string IncrementingPattern = @"(?:\[ *\-?\d* *\+\+ *\-?\d* *(?:every +\d+ *)?\])|(?:\[i\])";
        private Regex IncrementingRegex = new Regex(IncrementingPattern);

        private const string MinMaxPattern = @"\[ *\-?\d+ *=> *\-?\d+ *,* *\-?\d* *\]";
        private Regex MinMaxRegex = new Regex(MinMaxPattern);

        private const string AlphabetPattern = @"\[ *[a-zA-Z] *=> *[a-zA-Z] *\]";
        private Regex AlphabetRegex = new Regex(AlphabetPattern);

        private const string LuaPattern = @"\[ *lua (?:[a-zA-Z]|_)\w* *\]";
        private Regex LuaRegex = new Regex(LuaPattern);

        private readonly IContext Context;
        private readonly Date.IProvider DateProvider;
        private readonly Configuration.IProvider ConfigurationProvider;
        private readonly Combinatorial.Factory CombinatorialFactory;
        private readonly CsvGnomeScriptApi.IManager ScriptManager;

        public ComponentFactory(
            IContext context,
            Date.IProvider dateProvider,
            Configuration.IProvider configurationProvider,
            Combinatorial.Factory combinatorialFactory,
            CsvGnomeScriptApi.IManager scriptManager)
        {
            Context = context;
            DateProvider = dateProvider;
            ConfigurationProvider = configurationProvider;
            CombinatorialFactory = combinatorialFactory;
            ScriptManager = scriptManager;
        }

        /// <summary>
        /// Create a component from the prototype command.
        /// </summary>
        /// <exception cref="ComponentCreationException">Thrown if a lua component throws an error on creation due to an invalid function.</exception>
        public IComponent Create(string rawPrototype)
        {
            string groupPrototype = null;
            string prototype = ExtractGroup(rawPrototype, out groupPrototype);
            // [N]
            if(prototype == NComponent.CommandString)
            {
                return new NComponent(Context);
            }
            // e.g.
            // [++]
            // [1++2]
            // [-99++-109]
            // [1++3 every 10]
            // [i]
            else if (IncrementingRegex.IsMatch(prototype))
            {
                return CreateIncrementingComponent(prototype, groupPrototype);
            }
            // e.g. [11=>21,2 #testId]
            else if (MinMaxRegex.IsMatch(prototype))
            {
                return CreateMinMaxComponent(prototype, groupPrototype);
            }
            else if (AlphabetRegex.IsMatch(prototype))
            {
                return CreateAlphabetComponent(prototype, groupPrototype);
            }
            else if (LuaRegex.IsMatch(prototype))
            {
                return CreateLuaComponent(prototype);
            }
            else if (DateRegex.IsMatch(prototype))
            {
                return CreateDateComponent(prototype);
            }
            else if (prototype.StartsWith(ArraySpreadComponent.CommandInitString))
            {
                // remove "[spread]"
                var array = prototype.Substring(ArraySpreadComponent.CommandInitString.Length);
                return new ArraySpreadComponent(GetArray(array), Context, ConfigurationProvider);
            }
            else if (prototype.StartsWith(ArrayCycleComponent.CommandInitString))
            {
                return CreateArrayCycleComponent(prototype, groupPrototype);
            }
            else
            {
                return new TextComponent(prototype);
            }
        }

        /// <summary>
        /// If the prototype has a group command, remove it and save it.
        /// </summary>
        /// <param name="groupProtoType">Null unless the prototype contains a group.</param>
        /// <returns></returns>
        private string ExtractGroup(string prototype, out string groupProtoType)
        {
            groupProtoType = null;

            // Does it contain a group ID?
            if (GroupRegex.IsMatch(prototype))
            {
                string[] infoAndGroup = GroupRegex.Split(prototype);

                // A single member should be the group
                groupProtoType = infoAndGroup.Single(s => GroupRegex.IsMatch(s)).Trim();

                // Return the prototype with the group removed
                return infoAndGroup.Where(s => !GroupRegex.IsMatch(s)).Aggregate((t, n) => t + n);
            }

            // Otherwise there was no group, so return the prototype as is.
            return prototype;
        }

        private IComponent CreateIncrementingComponent(string prototype, string groupPrototype)
        {
            // remove "[" and "]"
            string protoInc = prototype.Substring(1, prototype.Length - 2);

            // split on "++" and "every" for the start, increment and every.
            string[] tokens = protoInc.Split(new string[] { "++", "every" }, StringSplitOptions.None);

            // if the input was "[i]", none of tokens will parse, and all default values will be used.
            int start;
            int increment;
            int every;
            if (!int.TryParse(tokens[0], out start)) start = IncrementingComponent.DefaultStart;
            if (tokens.Length <= 1 || !int.TryParse(tokens[1], out increment)) increment = IncrementingComponent.DefaultIncrement;
            if (tokens.Length <= 2 || !int.TryParse(tokens[2], out every)) every = IncrementingComponent.DefaultEvery;

            IncrementingComponent rawComponent = new IncrementingComponent(Context, start, increment, every);

            return Choose(rawComponent, groupPrototype);
        }

        private MinMaxComponent GetRawMinMax(int min, int max, int? increment)
        {
            if (!increment.HasValue)
            {
                return new MinMaxComponent(min, max);

            }
            else
            {
                return new MinMaxComponent(min, max, increment.Value);
            }
        }

        private IComponent CreateMinMaxComponent(string prototype, string groupPrototype)
        {
            // remove "[" and "]"
            string protoMinMax = prototype.Substring(1, prototype.Length - 2);
            int? increment = null;

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
            MinMaxComponent raw = GetRawMinMax(min, max, increment);
            return Choose(raw, groupPrototype);
        }

        private IComponent CreateArrayCycleComponent(string prototype, string groupPrototype)
        {
            // remove "[cycle]"
            var array = prototype.Substring(ArrayCycleComponent.CommandInitString.Length);
            ArrayCycleComponent rawComponent = new ArrayCycleComponent(GetArray(array), ConfigurationProvider);

            return Choose(rawComponent, groupPrototype);
        }

        private IComponent CreateAlphabetComponent(string prototype, string groupPrototype)
        {
            // remove "[" and "]"
            string protoInc = prototype.Substring(1, prototype.Length - 2);

            // split on "=>" for the start, and end.
            string[] tokens = protoInc.Split(new string[] { "=>" }, StringSplitOptions.None);
            AlphabetComponent rawComponent = new AlphabetComponent(tokens[0].Trim().First(), tokens[1].Trim().First());

            return Choose(rawComponent, groupPrototype);
        }

        /// <summary>
        /// Create a component which evaluates a function from functions.lua.
        /// </summary>
        /// <exception cref="ComponentCreationException">Thrown if the function does not exist.</exception>
        private IComponent CreateLuaComponent(string prototype)
        {
            // If no scriptmanager has been provided, just create a text component.
            if (ScriptManager == null) return new TextComponent(prototype);

            // remove "[" and "]"
            string protoInc = prototype.Substring(1, prototype.Length - 2);

            // remove "lua" and trim
            string protoNoLua = protoInc.Replace("lua", String.Empty).Trim();

            IComponent component;
            try
            {
                component = new LuaComponent(Context, protoNoLua, ScriptManager.GetValueFunction("lua", protoNoLua), ScriptManager.GetArgs("lua"));
                return component;
            }
            catch (CsvGnomeScriptApi.InvalidFunctionException ex)
            {
                throw new ComponentCreationException($"Function \"{ex.Function}\" was not found, or could not be parsed from functions.lua.", ex);
            }
        }

        /// <summary>
        /// Create a DateComponent which will output the date/time at writing.
        /// </summary>
        private IComponent CreateDateComponent(string prototype)
        {
            // Is a format provided?
            Group formatGroup = DateRegex.Match(prototype).Groups["format"];
            if (formatGroup.Success)
            {
                // remove the ""s
                string format = formatGroup.Value.Substring(1, formatGroup.Value.Length - 2);
                return new DateComponent(DateProvider, format);
            }
            else
            {
                return new DateComponent(DateProvider);
            }
        }

        private string GetGroupId(string groupPrototype, out int? rank)
        {
            rank = null;
            // groupPrototype is a match for the GroupRegex, so we know it is like:
            //  #groupId   or   #groupId/<n>
            // with no whitespace
            // remove the #
            string proto = groupPrototype.Remove(0, 1);

            if (proto.Contains("/"))
            {
                string[] tokens = proto.Split(new string[] { "/" }, StringSplitOptions.None);

                int parsedRank;
                if (int.TryParse(tokens[1], out parsedRank)) rank = parsedRank;
                return tokens[0];
            }

            return proto;
        }

        private IComponent Choose(IComponent rawComponent, string groupPrototype)
        {
            if (groupPrototype == null)
            {
                return rawComponent;
            }
            else
            {
                int? rank = null;
                string groupId = GetGroupId(groupPrototype, out rank);
                return CombinatorialFactory.Create(groupId, rawComponent, rank) as IComponent;
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
