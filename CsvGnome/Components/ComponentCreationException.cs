using System;

namespace CsvGnome.Components
{
    public class ComponentCreationException : Exception
    {
        public ComponentCreationException(string message, Exception inner) : base(message, inner) { }
    }
}
