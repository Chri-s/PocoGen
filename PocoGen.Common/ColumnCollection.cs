using System.Collections.Generic;
using System.Linq;

namespace PocoGen.Common
{
    public class ColumnCollection : EnhancedKeyedCollection<string, Column>
    {
        public ColumnCollection()
        {
        }

        internal List<FileFormat.Column> GetFileFormatColumns()
        {
            return this.Select(c => c.GetFileFormatColumn()).Where(ffc => ffc != null).ToList();
        }

        protected override string GetKeyForItem(Column item)
        {
            return item.Name;
        }
    }
}