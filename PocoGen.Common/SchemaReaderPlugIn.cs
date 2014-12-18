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

        public TableCollection ReadSchema(string connectionString)
        {
            return this.Instance.ReadSchema(connectionString, this.Settings);
        }

        public void TestConnectionString(string connectionString)
        {
            this.Instance.TestConnectionString(connectionString, this.Settings);
        }
    }
}