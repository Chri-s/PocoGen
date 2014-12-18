namespace PocoGen.Common
{
    public interface ISettings
    {
        SettingsRepository Serialize();

        void Deserialize(SettingsRepository repository);
    }
}