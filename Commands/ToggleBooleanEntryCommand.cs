using System;
using System.ComponentModel;
using System.Windows.Input;

namespace ApplicationProperties.Commands {

    /// <summary>
    /// Command for toggle the true/false value of the selected entry.
    /// </summary>
    public class ToggleBooleanEntryCommand : ICommand {
        private readonly Services.IContentService _content;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToggleBooleanEntryCommand"/> class.
        /// </summary>
        /// <param name="content">Service for accessing the files's content.</param>
        public ToggleBooleanEntryCommand(Services.IContentService content) {
            _content = content;
            _content.PropertyChanged += OnContentChanged;
        }

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc/>
        public bool CanExecute(object parameter) => _content.SelectedEntry != null && _content.SelectedEntry.EntryType == Models.ApplicationPropertiesEntry.EntryTypeEnum.Boolean && !_content.SelectedEntry.IsArray;

        /// <inheritdoc/>
        public void Execute(object parameter) => _content.SelectedEntry.Value = _content.SelectedEntry.Value.Equals("True", StringComparison.InvariantCultureIgnoreCase) ? "false" : "true";

        private void OnContentChanged(object sender, PropertyChangedEventArgs e) {
            if (!e.PropertyName.Equals("SelectedEntry")) { return; }
            var handle = CanExecuteChanged;
            handle?.Invoke(this, new EventArgs());
        }

    }
}
