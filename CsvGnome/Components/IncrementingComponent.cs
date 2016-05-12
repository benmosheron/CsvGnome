using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{
    class IncrementingComponent : IComponent
    {
        public string Summary => Program.IncrementComponentString;
        /// <summary>
        /// Get value to write on ith row
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public string GetValue(int i) => (i + start).ToString(getFormat());

        /// <summary>
        /// Value to start incrementing from.
        /// </summary>
        private int start;

        public IncrementingComponent(int start)
        {
            this.start = start;
        }

        /// <summary>
        /// Format of integer (e.g. "D3" to always print 3 digits)
        /// </summary>
        private string getFormat()
        {
            int minDigits = (Math.Max(Program.N + start - 1, 1)).ToString().Length;
            return "D" + minDigits.ToString();
        }
    }
}
