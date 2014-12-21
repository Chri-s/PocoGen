using System.Collections.Generic;

namespace PocoGen.Common
{
    public class TableNameGeneratorPlugInCollection : ChangeTrackingCollection<TableNameGeneratorPlugIn>
    {
        public TableNameGeneratorPlugInCollection()
        {
        }

        public void AddRange(IEnumerable<TableNameGeneratorPlugIn> collection)
        {
            foreach (TableNameGeneratorPlugIn tableNameGeneratorPlugIn in collection)
            {
                this.Add(tableNameGeneratorPlugIn);
            }
        }
    }
}