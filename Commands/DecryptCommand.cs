using System;
using System.ComponentModel;
using System.Windows.Input;

namespace ApplicationProperties.Commands {

    /// <summary>
    /// Command for decrypting a value.
    /// </summary>
    public class DecryptCommand : ICommand {
        private readonly Services.ICryptoService _crypto;
        private readonly Services.IContentService _content;

        /// <summary>
        /// Initializes a new instance of the <see cref="DecryptCommand"/> class.
        /// </summary>
        /// <param name="crypto">The application's crypto service.</param>
        /// <param name="content">The file's content.</param>
        public DecryptCommand(Services.ICryptoService crypto, Services.IContentService content) {
            _crypto = crypto;
            _content = content;
            _crypto.PropertyChanged += OnCryptoPropertyChanged;
            _content.PropertyChanged += OnContentChanged;
        }


        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc/>
        public bool CanExecute(object parameter) => _crypto.Password != null
                                                 && _crypto.IsReady
                                                 && _content.SelectedEntry != null
                                                 && _content.SelectedEntry.EntryType == Models.ApplicationPropertiesEntry.EntryTypeEnum.Text
                                                 && _content.SelectedEntry.IsEncrypted;

        /// <inheritdoc/>
        public void Execute(object parameter) {
            _content.SelectedEntry.Value = _crypto.Decrypt(_content.SelectedEntry.Value.Uncapsulate("ENC(", ")"));
            _content.SetChanged(Services.PropertyTypeEnum.SelectedEntry);
        }

        private void OnCryptoPropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (!e.PropertyName.Equals("IsReady")) { return; }
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        private void OnContentChanged(object sender, PropertyChangedEventArgs e) {
            if (!e.PropertyName.Equals("SelectedEntry")) { return; }
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

    }

}
