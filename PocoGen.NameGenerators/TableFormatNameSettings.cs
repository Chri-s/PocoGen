using System.ComponentModel;
using PocoGen.Common;

namespace PocoGen.NameGenerators
{
    public class TableFormatNameSettings : ChangeTrackingBase, ISettings
    {
        private const string DefaultFormatString = "%TABLE%";

        public TableFormatNameSettings()
        {
            this.ResetToDefaults();
        }

        private string formatString;
        [DisplayName("Format string")]
        [Description("Format of the table name. Use %TABLE% and %SCHEMA% as placeholders for the table name and schema name")]
        public string FormatString
        {
            get { return this.formatString; }
            set { this.ChangeProperty(ref this.formatString, value); }
        }

        private string excludeSchema;
        [DisplayName("Exclude Schema")]
        [Description("The default schema name which should not be included. When the current schema equals this setting, %SCHEMA% will be set to \"\".")]
        public string ExcludeSchema
        {
            get { return this.excludeSchema; }
            set { this.ChangeProperty(ref this.excludeSchema, value); }
        }

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

        public void ResetToDefaults()
        {
            this.FormatString = TableFormatNameSettings.DefaultFormatString;
            this.ExcludeSchema = string.Empty;

            this.AcceptChanges();
        }
    }
}