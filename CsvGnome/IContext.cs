namespace CsvGnome
{
    /// <summary>
    /// Provides contextual information such as N, the number of rows that will be written.
    /// </summary>
    public interface IContext
    {
        /// <summary>
        /// The number of rows of data to write (doesn't include the columns row).
        /// </summary>
        long N { get; set; }

        /// <summary>
        /// Change the location that the csv file will be written to.
        /// </summary>
        /// <param name="path"></param>
        void SetOutputFile(string path);
    }
}
