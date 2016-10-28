using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CsvGnome.Components
{
    /// <summary>
    /// Component which writes values from a user supplied array.
    /// </summary>
    public class ArraySpreadComponent : IComponent
    {
        public const string CommandInitString = "[spread]";
        public string Command => valueArrayCommand;

        public List<Message> Summary => new List<Message>()
        {
            Message.NewSpecial(CommandInitString),
            new Message("{"),
            ConfigurationProvider.ReportArrayContents
            ? new Message(valueArray.Aggregate((t, n) => $"{t},{n}"))
            : Message.NewSpecial($"{valueArray.Length} items"),
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
            long rowsPer = Math.Max((Context.N + 1) / size, 1);
            long index = (i / rowsPer) % size;
            return valueArray[index];
        }

        public readonly Configuration.IProvider ConfigurationProvider;
        private readonly IContext Context;
        private readonly string[] valueArray;

        private string valueArrayCommand => CommandInitString + "{" + valueArray.Aggregate((t, n) => $"{t},{n}") + "}";

        public ArraySpreadComponent(string[] valueArray, IContext context, Configuration.IProvider configurationProvider)
        {
            Context = context;
            ConfigurationProvider = configurationProvider;
            Debug.WriteLineIf(valueArray == null || !valueArray.Any(), "Empty or null array supplied to array component.");
            if (valueArray == null || valueArray.Length == 0) valueArray = new string[] { String.Empty };
            this.valueArray = valueArray;
        }
    }
}
