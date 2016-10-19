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
    /// <remarks>
    /// It is the responsibility of the class using the fields (e.g. Writer) to check config and decide whether
    /// or not to use a PaddedFieldFactory to turn IFields into IPaddedFields.
    /// 
    /// It could go either way, but they would have to ensure than the size is recalculated anyway.
    /// </remarks>
    public class PaddedFieldFactory
    {
        public IPaddedField GetPaddedField(IField field)
        {
            return new PaddedField(field);
        }
    }
}
