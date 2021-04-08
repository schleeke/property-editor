using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Windows.Input;

namespace ApplicationProperties.ViewModels {

    /// <summary>
    /// The view model for the <see cref="Views.MainMenuView"/>.
    /// </summary>
    public class MainMenuViewModel : BindableBase {

        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenuViewModel"/> class.
        /// </summary>
        /// <param name="events">The application's event bus.</param>
        /// <param name="content">The file's content.</param>
        /// <param name="status">The application' status.</param>
        /// <param name="crypto">The application's crypto service.</param>
        /// <param name="dialogs">The dialog service.</param>
        /// <param name="config">The configuration/settings service.</param>
        public MainMenuViewModel(IEventAggregator events, Services.IContentService content, Services.IStatusService status, Services.ICryptoService crypto, IDialogService dialogs, Services.IConfigurationService config) {
            LoadFileCommand = new Commands.LoadFileCommand(events);
            SaveFileCommand = new Commands.SaveFileCommand(content, status, config);
            ExitApplicationCommand = new Commands.ExitApplicationCommand(content);
            EncryptValueCommand = new Commands.EncryptCommand(crypto, content);
            DecryptValueCommand = new Commands.DecryptCommand(crypto, content);
            SetPasswordCommand = new Commands.SetCryptoPasswordCommand(dialogs);
            ShowPreferencesCommand = new Commands.ShowSettingsCommand(dialogs);
            ShowAboutDialogCommand = new Commands.ShowAboutCommand(dialogs);
        }

        /// <summary>
        /// Command for loading an application.properties file.
        /// </summary>
        public ICommand LoadFileCommand { get; set; }

        /// <summary>
        /// Command for saving the currently loaded application.properties file.
        /// </summary>
        public ICommand SaveFileCommand { get; set; }

        /// <summary>
        /// Command for exiting the application.
        /// </summary>
        public ICommand ExitApplicationCommand { get; set; }

        /// <summary>
        /// Command for encrypting the currently selected value.
        /// </summary>
        public ICommand EncryptValueCommand { get; set; }

        /// <summary>
        /// Command for decrypting the currently selected value.
        /// </summary>
        public ICommand DecryptValueCommand { get; set; }

        /// <summary>
        /// Command for setting the crypto password.
        /// </summary>
        public ICommand SetPasswordCommand { get; set; }
        
        /// <summary>
        /// Command for showing the settings dialog.
        /// </summary>
        public ICommand ShowPreferencesCommand { get; set; }

        /// <summary>
        /// Command for showing the application's about dialog.
        /// </summary>
        public ICommand ShowAboutDialogCommand { get; set; }

    }
}
