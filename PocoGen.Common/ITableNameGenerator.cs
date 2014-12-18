namespace PocoGen.Common
{
    public interface ITableNameGenerator
    {
        string GetClassName(Table table, ISettings settings);
    }
}