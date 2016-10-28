namespace CsvGnome.Components.Combinatorial
{
    /// <summary>
    /// Deletes components, ensuring the cache is kept up to date.
    /// </summary>
    public class Deleter
    {
        Cache cache;
        public Deleter(Cache cache)
        {
            this.cache = cache;
        }

        /// <summary>
        /// Delete a combinatorial component from the cache.
        /// </summary>
        public void Delete(ICombinatorial component)
        {
            // the component must be removed from it's group
            string groupId = component.Group.Id;
            Group group = cache[groupId];
            group.DeleteComponent(component);
        }

        /// <summary>
        /// Clear the cache of all components.
        /// </summary>
        public void Clear()
        {
            cache.Clear();
        }
    }
}
