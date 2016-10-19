using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome.Fields
{
    /// <summary>
    /// Creates IPaddedFields from IFields.
    /// </summary>
    public class PaddedFieldFactory
    {
        public IPaddedField GetPaddedField(IField field)
        {
            return new PaddedField(field);
        }
    }
}
