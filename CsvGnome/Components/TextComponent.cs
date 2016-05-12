using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    class TextComponent : IComponent
    {
        public string Summary => text;
        public string GetValue(int i) => text;

        private string text;

        public TextComponent(string text)
        {
            if (text == null) text = String.Empty;
            this.text = text;
        }
    }
}
