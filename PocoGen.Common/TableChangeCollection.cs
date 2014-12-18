using System.Collections.Generic;

namespace PocoGen.Common
{
    public class TableChangeCollection : EnhancedKeyedCollection<string, TableChange>
    {
        public TableChangeCollection()
        {
        }

        public TableChangeCollection(IEnumerable<TableChange> collection)
        {
            this.AddRange(collection);
        }

        public void ApplyChanges(IEnumerable<Table> tables)
        {
            foreach (Table table in tables)
            {
                TableChange tableChange = this[table.Name];
                if (tableChange == null)
                {
                    continue;
                }

                tableChange.ApplyChanges(table);
            }
        }

        protected override string GetKeyForItem(TableChange item)
        {
            return item.Name;
        }
    }
}