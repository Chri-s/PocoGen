using System.ComponentModel.Composition;
using PocoGen.Common;

namespace PocoGen.NameGenerators
{
    [Export(typeof(IColumnNameGenerator))]
    [ColumnNameGenerator("Pascal Casing", "{957A07B3-A49A-4E5B-BC02-CAB5B91F8677}")]
    public class ColumnPascalCasing : IColumnNameGenerator
    {
        public string GetPropertyName(Table table, Column column, ISettings settings)
        {
            return Utilities.ToPascalCase(column.PropertyName);
        }
    }
}