using System.ComponentModel;

namespace PocoGen.Common
{
    /// <summary>
    /// Used to persist and load settings for modules.
    /// </summary>
    public interface ISettings : IChangeTracking
    {
        /// <summary>
        /// Creates a <see cref="SettingsRepository"/> instance and inserts the current implementor's settings into it.
        /// </summary>
        /// <returns>The SettingsRepository instance with the current options.</returns>
        SettingsRepository Serialize();

        /// <summary>
        /// Loads the current settings from the <paramref name="repository"/> parameter.
        /// </summary>
        /// <param name="repository">A SettingsRepository instance with the settings set from <see cref="Serialize()"/>.</param>
        void Deserialize(SettingsRepository repository);

        /// <summary>
        /// Sets the current options to the default values.
        /// </summary>
        void ResetToDefaults();
    }
}