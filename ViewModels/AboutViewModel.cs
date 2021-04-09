using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Reflection;

namespace ApplicationProperties.ViewModels {

    /// <summary>
    /// The view model for the <see cref="Views.AboutView"/>.
    /// </summary>
    public class AboutViewModel : BindableBase, IDialogAware {

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutViewModel"/> class.
        /// </summary>
        public AboutViewModel() {
            ProductName = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title;
            Version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
        }

        /// <inheritdoc/>
        public string Title => "About...";

        /// <inheritdoc/>
        public event Action<IDialogResult> RequestClose;

        /// <inheritdoc/>
        public bool CanCloseDialog() => true;

        /// <inheritdoc/>
        public void OnDialogClosed() { }

        /// <summary>
        /// The name of the application.
        /// </summary>
        public string ProductName { get; }

        /// <summary>
        /// The version of the application.
        /// </summary>
        public string Version { get; }

        /// <inheritdoc/>
        public void OnDialogOpened(IDialogParameters parameters) { }
    }
}
