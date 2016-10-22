namespace CsvGnome.File
{
    public class DefaultOutputProvider : IProvider
    {
        const string Directory = "Output";
        const string Name = "CsvGnomeOutput";
        const string Extension = ".csv";

        static DefaultOutputProvider staticInstance = new DefaultOutputProvider();

        public static string DefaultPath => staticInstance.Path;

        public string Path => System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), $@"{Directory}\{Name}.{Extension}");
    }
}
