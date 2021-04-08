using System;
using System.Windows.Input;

namespace ApplicationProperties.Commands {

    /// <summary>
    /// Exits the application.
    /// </summary>
    public class ExitApplicationCommand : ICommand {
        private readonly Services.IContentService _content;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExitApplicationCommand"/> class.
        /// </summary>
        /// <param name="content">The file's content.</param>
        public ExitApplicationCommand(Services.IContentService content) => _content = content;

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc/>
        public bool CanExecute(object parameter) => true;

        /// <inheritdoc/>
        public void Execute(object parameter) {
            if (_content.HasChanges) {
                var result = System.Windows.MessageBox.Show("Do you want to exit the application?\nAll changes will be discarded.", "Exit application", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Stop, System.Windows.MessageBoxResult.Cancel);
                if(result == System.Windows.MessageBoxResult.No) { return; }
            }
            System.Windows.Application.Current.Shutdown(0);
        }
    }
}
