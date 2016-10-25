using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components
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
            Args.i = i;
            Args.N = Context.N;
            return ValueFunction(Args).First().ToString();
        }

        Func<CsvGnomeScriptApi.IScriptArgs, object[]> ValueFunction;
        string FunctionName;
        CsvGnomeScriptApi.IScriptArgs Args;
        IContext Context;
        public LuaComponent(IContext context, string functionName, Func<CsvGnomeScriptApi.IScriptArgs, object[]> valueFunction, CsvGnomeScriptApi.IScriptArgs args)
        {
            Context = context;
            Args = args;
            ValueFunction = valueFunction;
            FunctionName = functionName;
        }
    }
}
