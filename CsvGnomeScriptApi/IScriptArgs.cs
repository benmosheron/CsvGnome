namespace CsvGnomeScriptApi
{
    /// <summary>
    /// Encompasses arguments that can be passed to script value generating functions.
    /// </summary>
    public interface IScriptArgs
    {
        /// <summary>
        /// The zero-based index of the row (not including the columns row).
        /// </summary>
        /// <remarks>
        /// E.g.
        /// 
        /// i
        /// __________________
        ///   | Field1, Field2
        /// 0 |      a,      b
        /// 1 |      c,      d
        /// </remarks>
        long i { get; set; }

        /// <summary>
        /// The total number of rows to write (not including the columns row).
        /// </summary>
        /// <remarks>
        /// E.g. for N=2
        /// __________________
        ///  Field1, Field2
        ///       a,      b
        ///       c,      d
        /// </remarks>
        long N { get; set; }
    }
}
