using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Windows.Input;

namespace ApplicationProperties.ViewModels {

    /// <summary>
    /// View model for the <see cref="Views.ContentView"/>.
    /// </summary>
    public class ContentViewModel : BindableBase {
        private readonly Services.IContentService _content;
        private string _arrayItemToRemove;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentViewModel"/> class.
        /// </summary>
        /// <param name="content">The file's content.</param>
        /// <param name="dialogs">The dialog service.</param>
        /// <param name="events">The application's event bus.</param>
        public ContentViewModel(Services.IContentService content, IDialogService dialogs, IEventAggregator events) {
            _content = content;
            ToggleBooleanCommand = new Commands.ToggleBooleanEntryCommand(_content);
            AddArrayItemCommand = new Commands.AddArrayItemCommand(dialogs, _content, events);
            RemoveArrayItemCommand = new Commands.RemoveArrayItemCommand(_content);
        }

        /// <summary>
        /// The file's content.
        /// </summary>
        public Services.IContentService Content { get => _content; }

        /// <summary>
        /// The array item to remove.
        /// </summary>
        public string ArrayItemToRemove { get => _arrayItemToRemove; set => SetProperty(ref _arrayItemToRemove, value); }


        /// <summary>
        /// Toggles the boolean value of the selected entry.
        /// </summary>
        public ICommand ToggleBooleanCommand { get; }

        /// <summary>
        /// Shows a dialog for adding a new item to the array.
        /// </summary>
        public ICommand AddArrayItemCommand { get; }

        /// <summary>
        /// Removes an item from the array.
        /// </summary>
        public ICommand RemoveArrayItemCommand { get; }

    }
}
