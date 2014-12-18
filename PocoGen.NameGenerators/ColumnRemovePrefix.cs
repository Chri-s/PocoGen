using System;
using System.ComponentModel.Composition;
using PocoGen.Common;

namespace PocoGen.NameGenerators
{
    [Export(typeof(IColumnNameGenerator))]
    [ColumnNameGenerator("Remove Prefix", "{D2176813-033A-47E1-935B-E08BB2131E8B}", SettingsType = typeof(RemovePrefixSettings))]
    public class ColumnRemovePrefix : IColumnNameGenerator
    {
        public string GetPropertyName(Table table, Column column, ISettings settings)
        {
            RemovePrefixSettings namingSettings = (RemovePrefixSettings)settings;

            string name = column.PropertyName;
            StringComparison comparisonMode = namingSettings.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            if (name.StartsWith(namingSettings.Prefix, comparisonMode))
            {
                return name.Substring(namingSettings.Prefix.Length);
            }

            return name;
        }
    }
}