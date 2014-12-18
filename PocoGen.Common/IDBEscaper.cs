namespace PocoGen.Common
{
    public interface IDBEscaper
    {
        string EscapeSchemaName(string schemaName);

        string EscapeTableName(string tableName);

        string EscapeColumnName(string columnName);
    }
}