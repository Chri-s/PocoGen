using System;
using System.Linq;

namespace PocoGen.Common
{
    public class TableCollection : EnhancedKeyedCollection<string, Table>, ICloneable
    {
        public TableCollection()
        {
        }

        public TableCollection Clone()
        {
            TableCollection clone = new TableCollection();

            clone.AddRange(from t in this
                           select t.Clone());

            return clone;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        protected override string GetKeyForItem(Table item)
        {
            return item.Name;
        }
    }
}