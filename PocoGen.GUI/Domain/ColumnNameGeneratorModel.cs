using System;
using PocoGen.Common;

namespace PocoGen.Gui.Domain
{
    internal class ColumnNameGeneratorModel
    {
        private readonly Lazy<IColumnNameGenerator, IColumnNameGeneratorMetadata> information;
        private readonly Lazy<ColumnNameGeneratorPlugIn> plugIn;

        public ColumnNameGeneratorModel(Lazy<IColumnNameGenerator, IColumnNameGeneratorMetadata> information)
        {
            if (information == null)
            {
                throw new ArgumentNullException("information", "information is null.");
            }

            this.information = information;
            this.plugIn = new Lazy<ColumnNameGeneratorPlugIn>(() => new ColumnNameGeneratorPlugIn(this.information));
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

        public ColumnNameGeneratorPlugIn ColumnNameGenerator
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