using System;
using PocoGen.Common;

namespace PocoGen.Gui.Domain
{
    internal class TableNameGeneratorInstance
    {
        private readonly Lazy<ITableNameGenerator, ITableNameGeneratorMetadata> information;
        private readonly Lazy<TableNameGeneratorPlugIn> plugIn;

        public TableNameGeneratorInstance(Lazy<ITableNameGenerator, ITableNameGeneratorMetadata> information)
        {
            if (information == null)
            {
                throw new ArgumentNullException("information", "information is null.");
            }

            this.information = information;
            this.plugIn = new Lazy<TableNameGeneratorPlugIn>(() => new TableNameGeneratorPlugIn(this.information));
        }

        public string Guid
        {
            get
            {
                return this.information.Metadata.Guid;
            }
        }

        public string Name
        {
            get
            {
                return this.information.Metadata.Name;
            }
        }

        public TableNameGeneratorPlugIn TableNameGenerator
        {
            get
            {
                return this.plugIn.Value;
            }
        }

        public bool HasSettings
        {
            get
            {
                return this.information.Metadata.SettingsType != null;
            }
        }
    }
}