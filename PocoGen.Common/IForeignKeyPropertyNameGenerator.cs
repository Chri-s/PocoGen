namespace PocoGen.Common
{
    /// <summary>
    /// Provides a method to change the property names for a foreign key.
    /// </summary>
    public interface IForeignKeyPropertyNameGenerator
    {
        /// <summary>
        /// Returns the changed parent property name for a foreign key.
        /// </summary>
        /// <param name="foreignKey">The foreign key.</param>
        /// <param name="settings">The settings for this module if it specifies a settings type, otherwise null.</param>
        /// <returns>The changed parent property name.</returns>
        string GetParentPropertyName(ForeignKey foreignKey, ISettings settings);

        /// <summary>
        /// Returns the changed child property name for a foreign key.
        /// </summary>
        /// <param name="foreignKey">The foreign key.</param>
        /// <param name="settings">The settings for this module if it specifies a settings type, otherwise null.</param>
        /// <returns>The changed child property name.</returns>
        string GetChildPropertyName(ForeignKey foreignKey, ISettings settings);
    }
}