using System;
using PocoGen.Common;

namespace PocoGen.Gui.Domain
{
    internal class SchemaReaderModel
    {
        private readonly Lazy<ISchemaReader, ISchemaReaderMetadata> information;
        private readonly Lazy<SchemaReaderPlugIn> plugIn;

        public SchemaReaderModel(Lazy<ISchemaReader, ISchemaReaderMetadata> information)
        {
            if (information == null)
            {
                throw new ArgumentNullException("information", "information is null.");
            }

            this.information = information;
            this.plugIn = new Lazy<SchemaReaderPlugIn>(this.CreatePlugIn);
        }

        public SchemaReaderPlugIn SchemaReader
        {
            get
            {
                return this.plugIn.Value;
            }
        }

        public string DatabaseType
        {
            get
            {
                return this.information.Metadata.Name;
            }
        }

        public string ConnectionStringHelpUrl
        {
            get
            {
                return this.information.Metadata.ConnectionStringDocumentationUrl;
            }
        }

        public string Guid
        {
            get
            {
                return this.information.Metadata.Guid;
            }
        }

        public bool HasSettings
        {
            get
            {
                return this.information.Metadata.SettingsType != null;
            }
        }

        private SchemaReaderPlugIn CreatePlugIn()
        {
            return new SchemaReaderPlugIn(this.information);
        }
    }
}