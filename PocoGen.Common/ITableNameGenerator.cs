namespace PocoGen.Common
{
    /// <summary>
    /// Provides a method to change the class name for a table or view.
    /// </summary>
    public interface ITableNameGenerator
    {
        /// <summary>
        /// Returns the changed class name for a table or view.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="settings">The settings for this module if it specifies a settings type, otherwise null.</param>
        /// <returns>The changed class name.</returns>
        string GetClassName(Table table, ISettings settings);
    }
}