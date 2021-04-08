using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Windows.Input;

namespace ApplicationProperties.ViewModels {

    /// <summary>
    /// The view model for the <see cref="Views.AddArrayItemView"/>.
    /// </summary>
    public class AddArrayItemViewModel : BindableBase, IDialogAware {
        private readonly IEventAggregator _events;
        private string _text;
        private string _title;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddArrayItemViewModel"/> class.
        /// </summary>
        /// <param name="events">The application's events.</param>
        public AddArrayItemViewModel(IEventAggregator events) {
            _events = events;
            CancelCommad = new DelegateCommand(OnCancel);
            SaveCommand = new DelegateCommand(OnSave, CanSave).ObservesProperty(() => Text);
            Title = "Add value to array...";
        }


        /// <summary>
        /// The new text/value for the array.
        /// </summary>
        public string Text { get => _text; set => SetProperty(ref _text, value); }

        /// <summary>
        /// Command for canceling the dialog.
        /// </summary>
        public ICommand CancelCommad { get; }

        /// <summary>
        /// Command for saving the new array item.
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <inheritdoc/>
        public string Title { get => _title; private set => SetProperty(ref _title, value); }

        /// <inheritdoc/>
        public event Action<IDialogResult> RequestClose;

        /// <inheritdoc/>
        public bool CanCloseDialog() => true;

        /// <inheritdoc/>
        public void OnDialogClosed() {
            //throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void OnDialogOpened(IDialogParameters parameters) { }

        private void OnCancel() => RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));

        private bool CanSave() => !Text.IsNullOrEmpty();

        private void OnSave() {
            _events.GetEvent<Events.ArrayItemAddedEvent>().Publish(new Events.ArrayItemAddedEventArgs { NewItem = Text });
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }


    }
}
