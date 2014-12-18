using System.ComponentModel.Composition;
using PocoGen.Common;

namespace PocoGen.NameGenerators
{
    [Export(typeof(IColumnNameGenerator))]
    [ColumnNameGenerator("Replace", "{E9276672-0E49-453D-9595-0C946ED818C9}", SettingsType = typeof(ReplaceSettings))]
    public class ColumnReplace : IColumnNameGenerator
    {
        public string GetPropertyName(Table table, Column column, ISettings settings)
        {
            ReplaceSettings generatorSettings = (ReplaceSettings)settings;

            if (string.IsNullOrEmpty(generatorSettings.FindWhat))
            {
                return column.PropertyName;
            }

            return column.PropertyName.Replace(generatorSettings.FindWhat, generatorSettings.ReplaceWith);
        }
    }
}