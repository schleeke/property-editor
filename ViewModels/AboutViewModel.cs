using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;

namespace ApplicationProperties.ViewModels {

    /// <summary>
    /// The view model for the <see cref="Views.AboutView"/>.
    /// </summary>
    public class AboutViewModel : BindableBase, IDialogAware {

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutViewModel"/> class.
        /// </summary>
        public AboutViewModel() {

        }

        /// <inheritdoc/>
        public string Title => "About...";

        /// <inheritdoc/>
        public event Action<IDialogResult> RequestClose;

        /// <inheritdoc/>
        public bool CanCloseDialog() => true;

        /// <inheritdoc/>
        public void OnDialogClosed() { }

        /// <inheritdoc/>
        public void OnDialogOpened(IDialogParameters parameters) { }
    }
}
