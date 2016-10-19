﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components
{
    /// <summary>
    /// Component which writes values from a user supplied array.
    /// </summary>
    public class ArrayCycleComponent : IComponent
    {
        public string Command => valueArrayCommand;

        public List<Message> Summary => new List<Message>()
        {
            new Message(Program.CycleComponentString, Program.SpecialColour),
            new Message("{"),
            ConfigurationProvider.ReportArrayContents 
            ? new Message(valueArray.Aggregate((t, n) => $"{t},{n}")) 
            : new Message($"{valueArray.Length} items", Program.SpecialColour),
            new Message("}"),
        };

        public bool EqualsComponent(IComponent x)
        {
            if (x == null) return false;
            var c = x as ArrayCycleComponent;
            if (c == null) return false;
            if (Command != c.Command) return false;
            return true;
        }

        public string GetValue(long i)
        {
            return valueArray[i % valueArray.Length];
        }

        public readonly Configuration.IProvider ConfigurationProvider;
        private readonly string[] valueArray;

        public ReadOnlyCollection<String> ValueArray => new ReadOnlyCollection<string>(valueArray);

        private string valueArrayCommand => Program.CycleComponentString + "{" + valueArray.Aggregate((t, n) => $"{t},{n}") + "}";

        public ArrayCycleComponent(string[] valueArray, Configuration.IProvider configurationProvider)
        {
            ConfigurationProvider = configurationProvider;
            Debug.WriteLineIf(valueArray == null || !valueArray.Any(), "Empty or null array supplied to array component.");
            if (valueArray == null || valueArray.Length == 0) valueArray = new string[] { String.Empty };
            this.valueArray = valueArray;
        }
    }
}
