using CsvGnomeScriptApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuaScript
{
    public class Args : IScriptArgs
    {
        private long? maybei;
        private long? maybeN;

        public long i
        {
            get
            {
                if (!maybei.HasValue) throw new ArgumentNullException(nameof(i), "i has not been assigned a value.");
                return maybei.Value;
            }
            set
            {
                maybei = value;
            }
        }

        public long N
        {
            get
            {
                if (!maybeN.HasValue) throw new ArgumentNullException(nameof(N), "N has not been assigned a value.");
                return maybeN.Value;
            }
            set
            {
                maybeN = value;
            }
        }
    }
}
