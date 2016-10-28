namespace CsvGnome
{
    /// <summary>
    /// Default implementation.
    /// </summary>
    public class Context : IContext
    {
        long n = 10;
        public long N
        {
            get
            {
                return n;
            }
            set
            {
                if (value >= 0) n = value;
                else n = 0;
            }
        }

        private string path;
        public string Path => path;
        public void SetOutputFile(string path)
        {
            this.path = path;
        }
    }
}
