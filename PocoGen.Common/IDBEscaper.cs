namespace PocoGen.Common
{
    /// <summary>
    /// Provides methods to escape database object names for a database engine.
    /// </summary>
    public interface IDBEscaper
    {
        /// <summary>
        /// Escapes a schema name if needed.
        /// </summary>
        /// <param name="schemaName">The name of the schema.</param>
        /// <returns>The escaped schema name if escaping is needed, otherwise the unescaped schema name.</returns>
        string EscapeSchemaName(string schemaName);

        /// <summary>
        /// Escapes a table name if needed.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <returns>The escaped table name if escaping is needed, otherwise the unescaped table name.</returns>
        string EscapeTableName(string tableName);

        /// <summary>
        /// Escapes a column name if needed.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <returns>The escaped column name if escaping is needed, otherwise the unescaped column name.</returns>
        string EscapeColumnName(string columnName);
    }
}