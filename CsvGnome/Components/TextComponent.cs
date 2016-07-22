using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    public class TextComponent : IComponent
    {
        public string Command => text;
        public List<Message> Summary => new List<Message> { new Message(text) };
        public string GetValue(long i) => text;
        public bool Equals(IComponent x)
        {
            if (x == null) return false;
            var c = x as TextComponent;
            if (c == null) return false;
            if (text != c.text) return false;
            return true;
        }

        private string text;

        public TextComponent(string text)
        {
            if (text == null) text = String.Empty;
            this.text = text;
        }
    }
}
