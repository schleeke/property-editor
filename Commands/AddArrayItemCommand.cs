using ApplicationProperties.Events;
using Prism.Events;
using Prism.Services.Dialogs;
using System;
using System.Windows.Input;

namespace ApplicationProperties.Commands {


    /// <summary>
    /// Command for editing the selected entries array values.
    /// </summary>
    public class AddArrayItemCommand : ICommand {
        private readonly IDialogService _dlg;
        private readonly Services.IContentService _content;
        private readonly IEventAggregator _events;
        internal readonly static string ENTRY_PARAMETER_NAME = "selectedEntry";

        /// <summary>
        /// Initializes a new instance of the <see cref="AddArrayItemCommand"/> class.
        /// </summary>
        public AddArrayItemCommand(IDialogService dialogs, Services.IContentService content, IEventAggregator events) {
            _dlg = dialogs;
            _content = content;
            _events = events;
            _events.GetEvent<Events.ArrayItemAddedEvent>().Subscribe(OnItemAdded);
        }

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc/>
        public bool CanExecute(object parameter) => true;

        /// <inheritdoc/>
        public void Execute(object parameter) {
            if(null == _content.SelectedEntry) { return; }
            var dlgParams = new DialogParameters { { ENTRY_PARAMETER_NAME, _content.SelectedEntry } };
            _dlg.ShowDialog(Views.AddArrayItemView.DIALOG_NAME, dlgParams, OnDialogClosed);
        }

        private void OnDialogClosed(IDialogResult result) { }

        private void OnItemAdded(ArrayItemAddedEventArgs args) {
            if(null == _content.SelectedEntry) { return; }
            _content.SelectedEntry.ArrayValues.Add(args.NewItem);
            _content.SelectedEntry.Value = string.Join(", ", _content.SelectedEntry.ArrayValues);
        }

    }
}
