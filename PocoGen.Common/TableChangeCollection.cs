using System.Collections.Generic;
using System.Linq;

namespace PocoGen.Common
{
    public class TableChangeCollection : EnhancedKeyedCollection<string, TableChange>
    {
        public TableChangeCollection()
        {
        }

        public override bool IsChanged
        {
            get
            {
                return base.IsChanged ? true : this.Any(c => c.IsChanged);
            }
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

        public override void AcceptChanges()
        {
            base.AcceptChanges();

            foreach (TableChange item in this)
            {
                item.AcceptChanges();
            }
        }

        protected override string GetKeyForItem(TableChange item)
        {
            return item.Name;
        }
    }
}