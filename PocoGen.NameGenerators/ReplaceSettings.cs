using System.ComponentModel;
using PocoGen.Common;

namespace PocoGen.NameGenerators
{
    public class ReplaceSettings : ISettings
    {
        public ReplaceSettings()
        {
            this.FindWhat = string.Empty;
            this.ReplaceWith = string.Empty;
        }

        [DisplayName("Find What")]
        public string FindWhat { get; set; }

        [DisplayName("Replace With")]
        public string ReplaceWith { get; set; }

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
    }
}