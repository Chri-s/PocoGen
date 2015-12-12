using System;
using System.Collections.Generic;
using System.Linq;

namespace PocoGen.Common
{
    public class TableCollection : EnhancedKeyedCollection<KeyWithSchema, Table>
    {
        public TableCollection()
        {
        }

        public Table this[string schema, string name]
        {
            get
            {
                return this[new KeyWithSchema(schema, name)];
            }
        }

        public bool Contains(string schema, string name)
        {
            return this.Contains(new KeyWithSchema(schema, name));
        }

        public bool Remove(string schema, string name)
        {
            return this.Remove(new KeyWithSchema(schema, name));
        }

        protected override KeyWithSchema GetKeyForItem(Table item)
        {
            return new KeyWithSchema(item.Schema, item.Name);
        }
    }
}