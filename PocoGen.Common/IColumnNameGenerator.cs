namespace PocoGen.Common
{
    public interface IColumnNameGenerator
    {
        string GetPropertyName(Table table, Column column, ISettings settings);
    }
}