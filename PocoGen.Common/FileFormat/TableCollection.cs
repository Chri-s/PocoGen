namespace PocoGen.Common.FileFormat
{
    internal class TableCollection : EnhancedKeyedCollection<string, Table>
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