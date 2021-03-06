﻿namespace PocoGen.Common.FileFormat
{
    internal class ColumnCollection : EnhancedKeyedCollection<string, Column>
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