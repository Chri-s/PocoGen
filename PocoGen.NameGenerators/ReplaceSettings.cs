using System.ComponentModel;
using PocoGen.Common;

namespace PocoGen.NameGenerators
{
    public class ReplaceSettings : ChangeTrackingBase, ISettings
    {
        public ReplaceSettings()
        {
            this.ResetToDefaults();
        }

        private string findWhat;
        [DisplayName("Find What")]
        public string FindWhat
        {
            get { return this.findWhat; }
            set { this.ChangeProperty(ref this.findWhat, value); }
        }

        private string replaceWith;
        [DisplayName("Replace With")]
        public string ReplaceWith
        {
            get { return this.replaceWith; }
            set { this.ChangeProperty(ref this.replaceWith, value); }
        }

        public SettingsRepository Serialize()
        {
            SettingsRepository repository = new SettingsRepository();

            repository.SetOption("FindWhat", this.FindWhat);
            repository.SetOption("ReplaceWith", this.ReplaceWith);

            return repository;
        }

        public void Deserialize(SettingsRepository repository)
        {
            string stringValue;

            this.FindWhat = repository.TryGetValue("FindWhat", out stringValue) ? stringValue : string.Empty;
            this.ReplaceWith = repository.TryGetValue("ReplaceWith", out stringValue) ? stringValue : string.Empty;
        }

        public void ResetToDefaults()
        {
            this.FindWhat = string.Empty;
            this.ReplaceWith = string.Empty;

            this.AcceptChanges();
        }
    }
}