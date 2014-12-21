using System;
using PocoGen.Common;

namespace PocoGen.Gui.Domain
{
    internal class SchemaReaderModel
    {
        private readonly Lazy<ISchemaReader, ISchemaReaderMetadata> information;
        private SchemaReaderPlugIn plugIn;

        public SchemaReaderModel(Lazy<ISchemaReader, ISchemaReaderMetadata> information)
        {
            if (information == null)
            {
                throw new ArgumentNullException("information", "information is null.");
            }

            this.information = information;
            this.plugIn = null;
        }

        public SchemaReaderPlugIn SchemaReader
        {
            get
            {
                if (this.plugIn == null)
                {
                    this.plugIn = this.CreatePlugIn();
                }

                return this.plugIn;
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

        public void SetInstance(SchemaReaderPlugIn instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance", "instance is null.");
            }

            if (instance.Guid != this.Guid)
            {
                throw new ArgumentException("The Guid doesn't match.", "instance");
            }

            this.plugIn = instance;
        }

        private SchemaReaderPlugIn CreatePlugIn()
        {
            return new SchemaReaderPlugIn(this.information);
        }
    }
}