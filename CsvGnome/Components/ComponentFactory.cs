using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    public static class ComponentFactory
    {
        public static IComponent Create(string prototype)
        {
            switch (prototype)
            {
                case (Program.IncrementComponentString):
                    return new IncrementingComponent(0);
                case (Program.DateComponentString):
                    return new DateComponent();
                default:
                    return new TextComponent(prototype);
            }
        }
    }
}
