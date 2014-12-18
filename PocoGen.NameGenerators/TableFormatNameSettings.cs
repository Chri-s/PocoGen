using System.ComponentModel;
using PocoGen.Common;

namespace PocoGen.NameGenerators
{
    public class TableFormatNameSettings : ISettings
    {
        private const string DefaultFormatString = "%TABLE%";

        public TableFormatNameSettings()
        {
            this.FormatString = TableFormatNameSettings.DefaultFormatString;
            this.ExcludeSchema = string.Empty;
        }

        [DisplayName("Format string")]
        [Description("Format of the table name. Use %TABLE% and %SCHEMA% as placeholders for the table name and schema name")]
        public string FormatString { get; set; }

        [DisplayName("Exclude Schema")]
        [Description("The default schema name which should not be included. When the current schema equals this setting, %SCHEMA% will be set to \"\".")]
        public string ExcludeSchema { get; set; }

        public SettingsRepository Serialize()
        {
            SettingsRepository repository = new SettingsRepository();
            repository.SetOption("FormatString", this.FormatString);
            repository.SetOption("ExcludeSchema", this.ExcludeSchema);

            return repository;
        }

        public void Deserialize(SettingsRepository repository)
        {
            string stringValue;

            this.FormatString = repository.TryGetValue("FormatString", out stringValue) ? stringValue : TableFormatNameSettings.DefaultFormatString;
            this.ExcludeSchema = repository.TryGetValue("ExcludeSchema", out stringValue) ? stringValue : string.Empty;
        }
    }
}