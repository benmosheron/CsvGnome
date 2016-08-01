using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    public class LuaComponent : IComponent
    {
        public string Command
        {
            get
            {
                return $"[lua {FunctionName}]";
            }
        }

        public List<Message> Summary
        {
            get
            {
                return new List<Message>
                {
                    Message.NewSpecial("[lua "),
                    new Message($"{FunctionName}"),
                    Message.NewSpecial("]")
                };
            }
        }

        public bool EqualsComponent(IComponent x)
        {
            if (x == null) return false;
            var c = x as LuaComponent;
            if (c == null) return false;
            if (FunctionName != c.FunctionName) return false;
            if (ValueFunction != c.ValueFunction) return false;
            return true;
        }

        public string GetValue(long i)
        {
            return ValueFunction(i).First().ToString();
        }

        Func<long, object[]> ValueFunction;
        string FunctionName;

        public LuaComponent(string functionName, Func<long, object[]> valueFunction)
        {
            ValueFunction = valueFunction;
            FunctionName = functionName;
        }
    }
}
