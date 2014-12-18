namespace PocoGen.Common
{
    public interface ISchemaReader : IDBEscaper
    {
        TableCollection ReadSchema(string connectionString, ISettings settings);

        void TestConnectionString(string connectionString, ISettings settings);
    }
}