using System.ComponentModel;
using PocoGen.Common;

namespace PocoGen.NameGenerators
{
    public class RemovePrefixSettings : ChangeTrackingBase, ISettings
    {
        public RemovePrefixSettings()
        {
            this.Prefix = string.Empty;
            this.CaseSensitive = false;

            this.AcceptChanges();
        }

        private string prefix;
        public string Prefix
        {
            get { return this.prefix; }
            set { this.ChangeProperty(ref this.prefix, value); }
        }

        private bool caseSensitive;
        [DisplayName("Case sensitive")]
        public bool CaseSensitive
        {
            get { return this.caseSensitive; }
            set { this.ChangeProperty(ref this.caseSensitive, value); }
        }

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