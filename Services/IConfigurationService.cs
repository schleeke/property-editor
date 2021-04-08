using System.ComponentModel;

namespace ApplicationProperties.Services {

    /// <summary>
    /// Interface for accessing the application's configuration/settings.
    /// </summary>
    public interface IConfigurationService : INotifyPropertyChanged {
        
        /// <summary>
        /// The directory that contains the binaries for the crypto operations.
        /// </summary>
        string CryptoBinariesDirectory { get; set; }

        /// <summary>
        /// Flag for enabling/disabling the usage of schema files for the application.properties file.
        /// </summary>
        bool UseSchemaFile { get; set; }

        /// <summary>
        /// Controls the creation (true) of a backup file before saving/rewriting the application.properties file.
        /// </summary>
        bool BackupOnSave { get; set; }

        /// <summary>
        /// The full path to the file that contains the configuration/settings.
        /// </summary>
        string ConfigurationFilePath { get; }

        /// <summary>
        /// Loads the configuration/settings from the <see cref="ConfigurationFilePath"/>.
        /// </summary>
        /// <returns></returns>
        IConfigurationService Load();

        /// <summary>
        /// Saves the configuration to the <see cref="ConfigurationFilePath"/>.
        /// </summary>
        void Save();
    }
}
