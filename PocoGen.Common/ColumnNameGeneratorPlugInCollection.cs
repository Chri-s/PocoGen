using System.Collections.Generic;

namespace PocoGen.Common
{
    public class ColumnNameGeneratorPlugInCollection : ChangeTrackingCollection<ColumnNameGeneratorPlugIn>
    {
        public ColumnNameGeneratorPlugInCollection()
        {
        }

        public void AddRange(IEnumerable<ColumnNameGeneratorPlugIn> collection)
        {
            foreach (ColumnNameGeneratorPlugIn columnNameGeneratorPlugIn in collection)
            {
                this.Add(columnNameGeneratorPlugIn);
            }
        }
    }
}