using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Components.Combinatorial
{
    public class MinMaxCombinatorial : CombinatorialBase, IComponent
    {
        #region IComponent

        public string Command
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Equals(IComponent x)
        {
            return base.Equals(x);
        }

        #endregion

        public MinMaxCombinatorial(Group group, MinMaxComponent rawComponent) 
            : base(group, rawComponent)
        {
        }
    }
}
