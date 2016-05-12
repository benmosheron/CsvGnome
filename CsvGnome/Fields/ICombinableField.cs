﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvGnome
{ 
    /// <summary>
    /// Indicates a field that can be transformed into a cominatorial field, with a set of other ICombinable fields.
    /// </summary>
    interface ICombinableField
    {
        string Name { get; }
        int Min { get; }
        int Max { get; }
        int Increment { get; }
    }
}