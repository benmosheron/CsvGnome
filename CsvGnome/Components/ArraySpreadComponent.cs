﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    /// <summary>
    /// Component which writes values from a user supplied array.
    /// </summary>
    public class ArraySpreadComponent : IComponent
    {
        public string Command => valueArrayCommand;

        public List<Message> Summary => new List<Message>()
        {
            new Message(Program.SpreadComponentString, Program.SpecialColour),
            new Message("{"),
            ConfigurationProvider.ReportArrayContents
            ? new Message(valueArray.Aggregate((t, n) => $"{t},{n}"))
            : new Message($"{valueArray.Length} items", Program.SpecialColour),
            new Message("}"),
        };

        public bool EqualsComponent(IComponent x)
        {
            if (x == null) return false;
            var c = x as ArraySpreadComponent;
            if (c == null) return false;
            if (Command != c.Command) return false;
            return true;
        }

        public string GetValue(long i)
        {
            long size = valueArray.Length;
            long rowsPer = Math.Max((Program.N + 1) / size, 1);
            long index = (i / rowsPer) % size;
            return valueArray[index];
        }

        public readonly Configuration.IProvider ConfigurationProvider;
        private readonly string[] valueArray;

        private string valueArrayCommand => Program.SpreadComponentString + "{" + valueArray.Aggregate((t, n) => $"{t},{n}") + "}";

        public ArraySpreadComponent(string[] valueArray, Configuration.IProvider configurationProvider)
        {
            ConfigurationProvider = configurationProvider;
            Debug.WriteLineIf(valueArray == null || !valueArray.Any(), "Empty or null array supplied to array component.");
            if (valueArray == null || valueArray.Length == 0) valueArray = new string[] { String.Empty };
            this.valueArray = valueArray;
        }
    }
}
