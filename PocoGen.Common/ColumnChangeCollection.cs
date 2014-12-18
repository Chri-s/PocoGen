using System.Collections.Generic;

namespace PocoGen.Common
{
    public class ColumnChangeCollection : EnhancedKeyedCollection<string, ColumnChange>
    {
        public ColumnChangeCollection()
        {
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

        protected override string GetKeyForItem(ColumnChange item)
        {
            return item.Name;
        }
    }
}