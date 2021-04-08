using Prism.Services.Dialogs;
using System;
using System.Windows.Input;

namespace ApplicationProperties.Commands {

    /// <summary>
    /// Command for showing the settings dialog.
    /// </summary>
    public class ShowSettingsCommand : ICommand {
        private readonly IDialogService _dlg;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowSettingsCommand"/> class.
        /// </summary>
        /// <param name="dlg">The dialog service.</param>
        public ShowSettingsCommand(IDialogService dlg) {
            _dlg = dlg;
        }

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc/>
        public bool CanExecute(object parameter) => true;

        /// <inheritdoc/>
        public void Execute(object parameter) => _dlg.ShowDialog(Views.SettingsView.DIALOG_NAME);
    }
}
