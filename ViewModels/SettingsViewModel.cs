using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Windows.Input;

namespace ApplicationProperties.ViewModels {

    /// <summary>
    /// The view model for the <see cref="Views.SettingsView"/>.
    /// </summary>
    public class SettingsViewModel : BindableBase, IDialogAware {
        private readonly IEventAggregator _events;
        private readonly Services.IConfigurationService _cfg;
        private string _jasyptPath;
        private bool _useSchema;
        private bool _createBackup;
        private string _pathValue;
        private bool _schemaValue;
        private bool _backupValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        /// <param name="events">The application's event bus.</param>
        /// <param name="config">The configuration/settings.</param>
        public SettingsViewModel(IEventAggregator events, Services.IConfigurationService config) {
            _events = events;
            _cfg = config;
            CancelCommand = new DelegateCommand(OnCancel);
            SaveCommand = new DelegateCommand(OnSave, CanSave)
                .ObservesProperty(() => JasyptDirectory)
                .ObservesProperty(() => UseSchema)
                .ObservesProperty(() => CreateBackup);
            BrowseDirectoryCommand = new DelegateCommand(OnOpenBrowser);
        }


        /// <summary>
        /// The jasypt directory. Used for crypto stuff.
        /// </summary>
        public string JasyptDirectory { get => _jasyptPath; set => SetProperty(ref _jasyptPath, value); }

        /// <summary>
        /// Indicates if the schema file should be used.
        /// </summary>
        public bool UseSchema { get => _useSchema; set => SetProperty(ref _useSchema, value); }

        /// <summary>
        /// Indicates if a backup is created when the application.properties file is saved.
        /// </summary>
        public bool CreateBackup { get => _createBackup; set => SetProperty(ref _createBackup, value); }

        /// <summary>
        /// Command for closing the dialog.
        /// </summary>
        public ICommand CancelCommand { get; }

        /// <summary>
        /// Command for saving the settings.
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Shows the directory browser dialog for the jasypt directory.
        /// </summary>
        public ICommand BrowseDirectoryCommand { get; }

        /// <inheritdoc/>
        public string Title => "Application settings";

        /// <inheritdoc/>
        public event Action<IDialogResult> RequestClose;

        /// <inheritdoc/>
        public bool CanCloseDialog() => true;

        /// <inheritdoc/>
        public void OnDialogClosed() {
            //throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void OnDialogOpened(IDialogParameters parameters) {
            _pathValue = _cfg.CryptoBinariesDirectory;
            _schemaValue = _cfg.UseSchemaFile;
            _backupValue = _cfg.BackupOnSave;
            JasyptDirectory = _cfg.CryptoBinariesDirectory;
            UseSchema = _cfg.UseSchemaFile;
            CreateBackup = _cfg.BackupOnSave;
        }

        private void OnCancel() => RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));

        private bool CanSave() => JasyptDirectory?.Equals(_pathValue, StringComparison.InvariantCultureIgnoreCase) == false
                               || UseSchema != _schemaValue || CreateBackup != _backupValue;

        private void OnSave() {
            var settings = new Services.JsonConfigurationService(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "app-settings.json")) {
                CryptoBinariesDirectory = JasyptDirectory,
                UseSchemaFile = UseSchema,
                BackupOnSave = CreateBackup
            };
            settings.Save();
            _cfg.BackupOnSave = CreateBackup;
            _cfg.CryptoBinariesDirectory = JasyptDirectory;
            _cfg.UseSchemaFile = UseSchema;
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }

        private void OnOpenBrowser() {
            var dlg = new OpenFileDialog {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = ".bat",
                Filter = "Encryption binary|encrypt.bat",
                FilterIndex = 0,
                Multiselect = false,
                Title = "Select jasypt encryption binary"                
            };
            var result = dlg.ShowDialog();
            if(result.HasValue == false || result.Value == false) { return; }
            JasyptDirectory = System.IO.Path.GetDirectoryName(dlg.FileName);
        }

    }
}
