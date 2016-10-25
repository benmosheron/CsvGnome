using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components
{
    public class TextComponent : BaseComponentWithMessageProvider, IComponent
    {
        
        public string Command => text;
        public List<IMessage> Summary => MessageProvider.New(text).ToList();
        public string GetValue(long i) => text;
        public bool EqualsComponent(IComponent x)
        {
            if (x == null) return false;
            var c = x as TextComponent;
            if (c == null) return false;
            if (text != c.text) return false;
            return true;
        }

        private string text;

        public TextComponent(string text, IMessageProvider messageProvider = null)
            :base(messageProvider)
        {
            if (text == null) text = String.Empty;
            this.text = text;
        }
    }
}
