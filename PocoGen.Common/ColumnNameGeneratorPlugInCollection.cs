using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PocoGen.Common
{
    public class ColumnNameGeneratorPlugInCollection : Collection<ColumnNameGeneratorPlugIn>
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