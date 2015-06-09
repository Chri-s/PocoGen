using System.Collections.Generic;
using System.Linq;

namespace PocoGen.Common
{
    public class ColumnCollection : EnhancedKeyedCollection<string, Column>
    {
        public ColumnCollection()
        {
        }

        protected override string GetKeyForItem(Column item)
        {
            return item.Name;
        }
    }
}