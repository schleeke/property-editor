using Prism.Services.Dialogs;
using System;
using System.Windows.Input;

namespace ApplicationProperties.Commands {

    /// <summary>
    /// Sets the crypto password.
    /// </summary>
    public class SetCryptoPasswordCommand : ICommand {
        private readonly IDialogService _dlg;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetCryptoPasswordCommand"/> class.
        /// </summary>
        /// <param name="dialogs">The application's dialog service.</param>
        public SetCryptoPasswordCommand(IDialogService dialogs) {
            _dlg = dialogs;
        }

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc/>
        public bool CanExecute(object parameter) => true;

        /// <inheritdoc/>
        public void Execute(object parameter) {
            _dlg.ShowDialog(Views.SetPasswordView.DIALOG_NAME);
        }
    }
}
