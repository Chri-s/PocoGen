using System.Collections.Generic;
using System.Linq;

namespace PocoGen.Common
{
    public class ColumnChangeCollection : EnhancedKeyedCollection<string, ColumnChange>
    {
        public ColumnChangeCollection()
        {
        }

        public override bool IsChanged
        {
            get
            {
                return base.IsChanged ? true : this.Any(c => c.IsChanged);
            }
        }

        public void ApplyChanges(IEnumerable<Column> columns)
        {
            foreach (Column column in columns)
            {
                ColumnChange columnChange = this[column.Name];
                if (columnChange == null)
                {
                    continue;
                }

                columnChange.ApplyChanges(column);
            }
        }

        public override void AcceptChanges()
        {
            base.AcceptChanges();

            foreach (ColumnChange item in this)
            {
                item.AcceptChanges();
            }
        }

        protected override string GetKeyForItem(ColumnChange item)
        {
            return item.Name;
        }
    }
}