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
        void CalculateMaxLength(long N);

        /// <summary>
        /// Name of the field, padded with spaces to fill the maximum length it will take up.
        /// </summary>
        string GetPaddedName();
    }
}
