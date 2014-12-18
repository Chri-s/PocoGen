using System;

namespace PocoGen.Common
{
    public class TableNameGeneratorPlugIn : PlugIn<ITableNameGenerator, ITableNameGeneratorMetadata>
    {
        public TableNameGeneratorPlugIn(Lazy<ITableNameGenerator, ITableNameGeneratorMetadata> instance)
            : base(instance)
        {
        }

        public string GetClassName(Table table)
        {
            return this.Instance.GetClassName(table, this.Settings);
        }

        public TableNameGeneratorPlugIn Clone()
        {
            return new TableNameGeneratorPlugIn(this.LazyInstance);
        }
    }
}