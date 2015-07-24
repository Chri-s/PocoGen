namespace PocoGen.Common
{
    /// <summary>
    /// Returns the tables, columns and views of a database.
    /// </summary>
    public interface ISchemaReader : IDBEscaper
    {
        /// <summary>
        /// Returns the tables and views of a database.
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="settings">The settings instance if the module specified a settings type, otherwise null.</param>
        /// <returns>A <see cref="TableCollection"/> of all tables and views in the database.</returns>
        TableCollection ReadTables(string connectionString, ISettings settings);

        /// <summary>
        /// Returns the foreign keys of a database.
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="settings">The settings instance if the module specified a settings type, otherwise null.</param>
        /// <returns>A <see cref="ForeignKeyCollection"/> of all foreign keys in the database.</returns>
        ForeignKeyCollection ReadForeignKeys(string connectionString, ISettings settings);

        /// <summary>
        /// Tries to open a connection to the database with the specified connection string. This call either succeeds or throws an exception.
        /// </summary>
        /// <param name="connectionString">The connection string to check.</param>
        /// <param name="settings">The settings instance if the module specified a settings type, otherwise null.</param>
        void TestConnectionString(string connectionString, ISettings settings);
    }
}