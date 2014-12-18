using System;

namespace PocoGen.Common
{
    public class ColumnNameGeneratorPlugIn : PlugIn<IColumnNameGenerator, IColumnNameGeneratorMetadata>
    {
        public ColumnNameGeneratorPlugIn(Lazy<IColumnNameGenerator, IColumnNameGeneratorMetadata> instance)
            : base(instance)
        {
        }

        public string GetPropertyName(Table table, Column column)
        {
            return this.Instance.GetPropertyName(table, column, this.Settings);
        }

        public ColumnNameGeneratorPlugIn Clone()
        {
            return new ColumnNameGeneratorPlugIn(this.LazyInstance);
        }
    }
}