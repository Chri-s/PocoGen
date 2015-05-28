using System;
using System.Collections.Generic;
using System.Linq;

namespace PocoGen.Common
{
    public class TableCollection : EnhancedKeyedCollection<string, Table>
    {
        public TableCollection()
        {
        }

        internal List<FileFormat.Table> GetFileFormatTables()
        {
            return this.Select(t => t.GetFileFormatTable()).Where(fft => fft != null).ToList();
        }

        protected override string GetKeyForItem(Table item)
        {
            return item.Name;
        }
    }
}