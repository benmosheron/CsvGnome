namespace CsvGnome.GnomeFiles
{
    /// <summary>
    /// Writes gnomefiles.
    /// </summary>
    public interface IWriter
    {
        /// <summary>
        /// Writes a gnomefile to a file at the provided path.
        /// </summary>
        /// <param name="path"></param>
        void Save(string path);
    }
}
