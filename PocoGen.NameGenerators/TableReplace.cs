using System.ComponentModel.Composition;
using PocoGen.Common;

namespace PocoGen.NameGenerators
{
    [Export(typeof(ITableNameGenerator))]
    [TableNameGenerator("Replace", "{EFED26F5-CAAA-43A5-9D44-8952DFADB4A2}", SettingsType = typeof(ReplaceSettings))]
    public class TableReplace : ITableNameGenerator
    {
        public string GetClassName(Table table, ISettings settings)
        {
            ReplaceSettings generatorSettings = (ReplaceSettings)settings;

            if (string.IsNullOrEmpty(generatorSettings.FindWhat))
            {
                return table.GeneratedClassName;
            }

            return table.GeneratedClassName.Replace(generatorSettings.FindWhat, generatorSettings.ReplaceWith);
        }
    }
}