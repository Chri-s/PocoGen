using System.ComponentModel.Composition;
using PocoGen.Common;

namespace PocoGen.NameGenerators
{
    [Export(typeof(ITableNameGenerator))]
    [TableNameGenerator("Pascal Casing", "{3A78208A-19FF-4495-B858-6B8CD5123AF6}")]
    public class TablePascalCasing : ITableNameGenerator
    {
        public string GetClassName(Table table, ISettings settings)
        {
            return Utilities.ToPascalCase(table.ClassName);
        }
    }
}