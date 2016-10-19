using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Fields
{
    public interface IPaddedField : IField
    {
        /// <summary>
        /// Calculate the maximum length of all the values that this field will produce, given that it will write N rows.
        /// </summary>
        void CalculateMaxLength(int N);
    }
}
