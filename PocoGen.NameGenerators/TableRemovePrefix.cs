using System;
using System.ComponentModel.Composition;
using PocoGen.Common;

namespace PocoGen.NameGenerators
{
    [Export(typeof(ITableNameGenerator))]
    [TableNameGenerator("Remove Prefix", "{FA896A96-30E5-42EC-9C53-F8503F009867}", SettingsType = typeof(RemovePrefixSettings))]
    public class TableRemovePrefix : ITableNameGenerator
    {
        public string GetClassName(Table table, ISettings settings)
        {
            RemovePrefixSettings namingSettings = (RemovePrefixSettings)settings;

            string name = table.ClassName;
            StringComparison comparisonMode = namingSettings.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            if (name.StartsWith(namingSettings.Prefix, comparisonMode))
            {
                return name.Substring(namingSettings.Prefix.Length);
            }

            return name;
        }
    }
}