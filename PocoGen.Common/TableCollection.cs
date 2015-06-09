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

        protected override string GetKeyForItem(Table item)
        {
            return item.Name;
        }
    }
}