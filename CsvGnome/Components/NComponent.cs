using System.Collections.Generic;

namespace CsvGnome.Components
{
    /// <summary>
    /// Returns the number of rows that will be written.
    /// </summary>
    public class NComponent : IComponent
    {
        public const string CommandString = "[N]";
        IContext Context;
        public NComponent(IContext context)
        {
            Context = context;
        }
        public string Command => CommandString;

        public List<Message> Summary => Message.NewSpecial(Command).ToList();

        public bool EqualsComponent(IComponent x)
        {
            if (x is NComponent) return true;
            return false;
        }

        public string GetValue(long i)
        {
            return GetValue();
        }

        private string GetValue()
        {
            return Context.N.ToString();
        }
    }
}
