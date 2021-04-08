using Microsoft.Win32;
using Prism.Events;
using System;
using System.Windows.Input;

namespace ApplicationProperties.Commands {

    /// <summary>
    /// Shows a dialog for chosing the file to load.
    /// </summary>
    public class LoadFileCommand : ICommand {
        private readonly IEventAggregator _events;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadFileCommand"/> class.
        /// </summary>
        /// <param name="events"></param>
        public LoadFileCommand(IEventAggregator events) {
            _events = events;
        }

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc/>
        public bool CanExecute(object parameter) => true;

        /// <inheritdoc/>
        public void Execute(object parameter) {
            var dlg = new OpenFileDialog {
                Filter = "All files|*.*|Application properties|*.properties",
                DefaultExt = "*.properties",
                Title = "Select application properties file",
                AddExtension = true,
                CheckFileExists = true,
                FilterIndex = 2,
                Multiselect = false
            };
            var result = dlg.ShowDialog(App.Current.MainWindow);
            if (!result.HasValue || result.Value == false) { return; }
            _events.GetEvent<Events.NewFileSelectedEvent>().Publish(new Events.NewFileSelectedEventArgs { FullName = dlg.FileName });
        }
    }
}
