using System.ComponentModel;
using PocoGen.Common;

namespace PocoGen.NameGenerators
{
    public class RemovePrefixSettings : ISettings
    {
        public RemovePrefixSettings()
        {
            this.Prefix = string.Empty;
            this.CaseSensitive = false;
        }

        public string Prefix { get; set; }

        [DisplayName("Case sensitive")]
        public bool CaseSensitive { get; set; }

        public SettingsRepository Serialize()
        {
            SettingsRepository repository = new SettingsRepository();
            repository.SetOption("Prefix", this.Prefix);
            repository.SetOption("CaseSensitive", this.CaseSensitive);

            return repository;
        }

        public void Deserialize(SettingsRepository repository)
        {
            string stringValue;
            bool boolValue;

            this.Prefix = repository.TryGetValue("Prefix", out stringValue) ? stringValue : string.Empty;
            this.CaseSensitive = repository.TryGetValue("CaseSensitive", out boolValue) ? boolValue : false;
        }
    }
}