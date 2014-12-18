namespace PocoGen.Common.FileFormat
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