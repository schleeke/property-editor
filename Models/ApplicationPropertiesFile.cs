using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace ApplicationProperties.Models {

    /// <summary>
    /// Represents an application properties file.
    /// </summary>
    public class ApplicationPropertiesFile : BindableBase {
        private readonly Services.IConfigurationService _cfg;


        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationPropertiesFile"/> class.
        /// </summary>
        /// <param name="path">The full path to the file.</param>
        /// <param name="config">The configuration/settings.</param>
        /// <exception cref="System.IO.FileNotFoundException">Thrown if the specified file was not found.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the given path is NULL.</exception>
        /// <exception cref="ArgumentException">Thrown if the given path is empty.</exception>
        public ApplicationPropertiesFile(string path, Services.IConfigurationService config) {
            if(null == path) { throw new ArgumentNullException("The given path for the application properties file is NULL."); }
            if(string.IsNullOrEmpty(path)) { throw new ArgumentException("The given path for the application properties file is empty."); }
            if(!System.IO.File.Exists(path)) { throw new System.IO.FileNotFoundException("The application properties file could not be found."); }
            FullName = path;
            _cfg = config;
        }

        /// <summary>
        /// The full name/path of the application properties file.
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// A list of all configuration entries within the application properties file.
        /// </summary>
        public ObservableCollection<ApplicationPropertiesEntry  > Entries { get; set; }


        /// <summary>
        /// Reads the given application.properties file.
        /// </summary>
        public void Read() {
            Entries = new ObservableCollection<ApplicationPropertiesEntry>();
            if(!_cfg.UseSchemaFile) {
                LoadWithoutSchema();
                return;
            }
            var schemaName = GetSchema();
            if (string.IsNullOrEmpty(schemaName)) {
                LoadWithoutSchema();
                return;
            }
            var schemaFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), $"{schemaName}.xml");
            if (!System.IO.File.Exists(schemaFile)) {
                LoadWithoutSchema();
                return;
            }
            System.Collections.Generic.IEnumerable<ApplicationPropertiesEntry> entryDefinitions;
            try {
                var schema = new ApplicationPropertiesSchema(schemaFile, _cfg);
                entryDefinitions = schema.GetEntries();
            }
            catch (Exception) {
                entryDefinitions = null;
            }
            if(null == entryDefinitions || _cfg.UseSchemaFile == false) {
                LoadWithoutSchema();
            }
            else {
                LoadWithSchema(entryDefinitions);
            }
        }

        /// <summary>
        /// Saves the content to a file.
        /// </summary>
        /// <param name="fileName">The name of the file to save the content to.</param>
        public void Save(string fileName) {
            var bld = new System.Text.StringBuilder();
            foreach (var entry in Entries) { bld.AppendLine($"{entry.Identifier}={entry.Value}"); }
            System.IO.File.WriteAllText(fileName, bld.ToString(), System.Text.Encoding.UTF8);
            FullName = fileName;
        }

        private void LoadWithSchema(System.Collections.Generic.IEnumerable<ApplicationPropertiesEntry> entryDefinitions) {
            var lines = System.IO.File.ReadAllText(FullName).Split(Environment.NewLine.ToCharArray());
            var counter = 0;
            var validLines = 0;
            foreach (var line in lines) {
                counter++;
                if (string.IsNullOrEmpty(line.Trim())) { continue; }
                if (line.Trim().StartsWith("#")) { continue; }
                var separatorIndex = line.Trim().IndexOf('=');
                if (separatorIndex < 1) { continue; }
                var identifier = line.Trim().Substring(0, separatorIndex).Trim();
                if (entryDefinitions.Any(d => d.Identifier.Equals(identifier))) {
                    var newEntry = entryDefinitions.Single(d => d.Identifier.Equals(identifier));
                    var value = line.Trim().Substring(separatorIndex + 1).Trim();
                    validLines++;
                    newEntry.Value = value;
                    newEntry.TrackingActive = true;
                    newEntry.PropertyChanged += OnEntryChanged;
                    Entries.Add(newEntry);
                }
            }
            RaisePropertyChanged(nameof(Entries));
        }

        private void LoadWithoutSchema() {
            var lines = System.IO.File.ReadAllText(FullName).Split(Environment.NewLine.ToCharArray());
            var counter = 0;
            foreach (var line in lines) {
                counter++;
                if (string.IsNullOrEmpty(line.Trim())) { continue; }
                if (line.Trim().StartsWith("#")) { continue; }
                var separatorIndex = line.Trim().IndexOf('=');
                if (separatorIndex < 1) { continue; }
                var identifier = line.Trim().Substring(0, separatorIndex).Trim();
                var value = line.Trim().Substring(separatorIndex + 1).Trim();
                var newEntry = new ApplicationPropertiesEntry(this) {
                    Identifier = identifier,
                    Value = value,
                    IsArray = false,
                    EntryType = ApplicationPropertiesEntry.EntryTypeEnum.Text
                };
                Entries.Add(newEntry);
            }

        }

        private void OnEntryChanged(object sender, PropertyChangedEventArgs e) => RaisePropertyChanged(nameof(Entries));

        private string GetSchema() {
            var lines = System.IO.File.ReadAllText(FullName).Split(Environment.NewLine.ToCharArray());
            var rgEx = new Regex(@"^#\s?(?i)SCHEMA:\s?(?-i)\S+$");
            foreach (var line in lines) {
                if(!rgEx.IsMatch(line)) { continue; }
                var match = rgEx.Match(line);
                var parts = match.Value.Substring(1).Split(new char[] { ':' }, 2);
                if(!parts[0].Trim().Equals("SCHEMA", StringComparison.InvariantCultureIgnoreCase)) { continue; }
                return parts[1].Trim();
            }
            return string.Empty;
        }
    }
}
