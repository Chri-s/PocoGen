using System.ComponentModel.Composition;
using PocoGen.Common;

namespace PocoGen.NameGenerators
{
    [Export(typeof(ITableNameGenerator))]
    [TableNameGenerator("Format", "{B26667B0-D6CD-4A5A-ADF9-F95DCD8E8832}", SettingsType = typeof(TableFormatNameSettings))]
    public class TableFormatName : ITableNameGenerator
    {
        public string GetClassName(Table table, ISettings settings)
        {
            TableFormatNameSettings namingSettings = (TableFormatNameSettings)settings;

            string schemaName = (table.Schema == namingSettings.ExcludeSchema) ? string.Empty : table.Schema;

            return namingSettings.FormatString.Replace("%SCHEMA%", schemaName).Replace("%TABLE%", table.ClassName);
        }
    }
}