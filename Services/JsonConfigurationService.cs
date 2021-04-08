using Newtonsoft.Json;
using Prism.Mvvm;

namespace ApplicationProperties.Services {

    /// <summary>
    /// Implementation of the <see cref="IConfigurationService"/> using JSON for (de)serialization.
    /// </summary>
    public class JsonConfigurationService : BindableBase, IConfigurationService {
        private string _binDir;
        private bool _useSchema;
        private bool _backup;
        private string _filePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonConfigurationService"/> class.
        /// </summary>
        public JsonConfigurationService(string filePath) {
            _filePath = filePath;
        }

        /// <inheritdoc/>
        public string CryptoBinariesDirectory { get => _binDir; set => SetProperty(ref _binDir, value); }

        /// <inheritdoc/>
        public bool UseSchemaFile { get => _useSchema; set => SetProperty(ref _useSchema, value); }

        /// <inheritdoc/>
        public bool BackupOnSave { get => _backup; set => SetProperty(ref _backup, value); }

        /// <inheritdoc/>
        public string ConfigurationFilePath => _filePath;

        /// <inheritdoc/>
        public IConfigurationService Load() {
            JsonConfigurationService settings;
            if (!System.IO.File.Exists(ConfigurationFilePath)) {
                settings = Default;
                settings.Save();
                CryptoBinariesDirectory = settings.CryptoBinariesDirectory;
                UseSchemaFile = settings.UseSchemaFile;
                BackupOnSave = settings.BackupOnSave;
                return settings;
            }
            var fileContent = System.IO.File.ReadAllText(ConfigurationFilePath, System.Text.Encoding.UTF8);
            settings = JsonConvert.DeserializeObject<JsonConfigurationService>(fileContent);
            CryptoBinariesDirectory = settings.CryptoBinariesDirectory;
            UseSchemaFile = settings.UseSchemaFile;
            BackupOnSave = settings.BackupOnSave;
            return settings;
        }

        /// <inheritdoc/>
        public void Save() {
            var fileContent = JsonConvert.SerializeObject(this, Formatting.Indented);
            System.IO.File.WriteAllText(ConfigurationFilePath, fileContent, System.Text.Encoding.UTF8);
        }

        private static JsonConfigurationService Default => new JsonConfigurationService(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "app-settings.json")) {
            BackupOnSave = true,
            UseSchemaFile = false,
            CryptoBinariesDirectory = "C:\\Program Files(x86)\\jasypt"
        };

    }

}
