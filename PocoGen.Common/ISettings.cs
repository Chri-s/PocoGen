using System.ComponentModel;

namespace PocoGen.Common
{
    public interface ISettings : IChangeTracking
    {
        SettingsRepository Serialize();

        void Deserialize(SettingsRepository repository);

        void ResetToDefaults();
    }
}