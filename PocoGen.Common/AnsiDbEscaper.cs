namespace PocoGen.Common
{
    internal class AnsiDbEscaper : IDBEscaper
    {
        public string EscapeSchemaName(string schemaName)
        {
            return "\"" + schemaName + "\"";
        }

        public string EscapeTableName(string tableName)
        {
            return "\"" + tableName + "\"";
        }

        public string EscapeColumnName(string columnName)
        {
            return "\"" + columnName + "\"";
        }
    }
}