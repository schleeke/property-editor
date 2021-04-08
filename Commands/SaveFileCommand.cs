using System;
using System.ComponentModel;
using System.Windows.Input;

namespace ApplicationProperties.Commands {

    /// <summary>
    /// Command for saving the application propertiesfile.
    /// </summary>
    public class SaveFileCommand : ICommand {
        private readonly Services.IContentService _content;
        private readonly Services.IStatusService _status;
        private readonly Services.IConfigurationService _cfg;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveFileCommand"/> class.
        /// </summary>
        /// <param name="content">Service for accessing and monitoring the current file's content.</param>
        /// <param name="appStatus">The application's status.</param>
        /// <param name="config">The configuration/settings.</param>
        public SaveFileCommand(Services.IContentService content, Services.IStatusService appStatus, Services.IConfigurationService config) {
            _content = content;
            _status = appStatus;
            _cfg = config;
            _content.PropertyChanged += OnContentChanged;
        }

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc/>
        public bool CanExecute(object parameter) => _content.HasChanges;

        /// <inheritdoc/>
        public void Execute(object parameter) {
            var file = new Models.ApplicationPropertiesFile(_status.FilePath, _cfg) { Entries = _content.Entries };
            if(_cfg.BackupOnSave) {
                var fileInfo = new System.IO.FileInfo(_status.FilePath);
                var fileName = fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
                var datePart = DateTime.Now.ToString("yyyy-MM-dd_HH_mm");
                var newFileName = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(_status.FilePath), $"{fileName}.{datePart}{fileInfo.Extension}");
                fileInfo.MoveTo(newFileName);
            }
            file.Save(_status.FilePath);
            _content.HasChanges = false;
        }

        private void OnContentChanged(object sender, PropertyChangedEventArgs e) {
            if (!e.PropertyName.Equals("HasChanges")) { return; }
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

    }
}
