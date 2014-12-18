using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PocoGen.Common
{
    public class TableNameGeneratorPlugInCollection : Collection<TableNameGeneratorPlugIn>
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