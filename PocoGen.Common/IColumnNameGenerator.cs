namespace PocoGen.Common
{
    /// <summary>
    /// Provides a method to change the property name for a column.
    /// </summary>
    public interface IColumnNameGenerator
    {
        /// <summary>
        /// Returns the changed property name for a database column.
        /// </summary>
        /// <param name="table">The table of the column.</param>
        /// <param name="column">The column itself.</param>
        /// <param name="settings">The settings for this module if it specifies a settings type, otherwise null.</param>
        /// <returns>The changed property name.</returns>
        string GetPropertyName(Table table, Column column, ISettings settings);
    }
}