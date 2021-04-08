using Prism.Services.Dialogs;
using System;
using System.Windows.Input;

namespace ApplicationProperties.Commands {

    /// <summary>
    /// Command for showing the about dialog.
    /// </summary>
    public class ShowAboutCommand : ICommand {
        private readonly IDialogService _dlg;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowAboutCommand"/> class.
        /// </summary>
        /// <param name="dialogs">The application's dialog service.</param>
        public ShowAboutCommand(IDialogService dialogs) => _dlg = dialogs;

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc/>
        public bool CanExecute(object parameter) => true;

        /// <inheritdoc/>
        public void Execute(object parameter) => _dlg.ShowDialog(Views.AboutView.DIALOG_NAME);
    }
}
