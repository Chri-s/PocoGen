using System;

namespace PocoGen.Common
{
    public class SchemaReaderPlugIn : PlugIn<ISchemaReader, ISchemaReaderMetadata>
    {
        public SchemaReaderPlugIn(Lazy<ISchemaReader, ISchemaReaderMetadata> instance)
            : base(instance)
        {
        }

        public IDBEscaper DBEscaper
        {
            get
            {
                return this.Instance;
            }
        }

        public TableCollection ReadTables(string connectionString)
        {
            return this.Instance.ReadTables(connectionString, this.Settings);
        }

        public ForeignKeyCollection ReadForeignKeys(string connectionString)
        {
            return this.Instance.ReadForeignKeys(connectionString, this.Settings);
        }

        public void TestConnectionString(string connectionString)
        {
            this.Instance.TestConnectionString(connectionString, this.Settings);
        }
    }
}