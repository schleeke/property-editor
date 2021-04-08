using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Windows.Input;

namespace ApplicationProperties.ViewModels {

    /// <summary>
    /// View model for the <see cref="Views.SetPasswordView"/>.
    /// </summary>
    public class SetPasswordViewModel : BindableBase, IDialogAware {
        private SecureString _pwd;
        private IEventAggregator _events;


        /// <summary>
        /// Initializes a new instance of the <see cref="SetPasswordViewModel"/> class.
        /// </summary>
        public SetPasswordViewModel(IEventAggregator events) {
            SaveCommand = new DelegateCommand(OnSave, CanSave).ObservesProperty(() => Password);
            _events = events;
        }

        private bool CanSave() => null != Password;

        /// <summary>
        /// Command for saving the new password.
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <inheritdoc/>
        public string Title => "Set the crypto password";

        /// <summary>
        /// The password.
        /// </summary>
        public SecureString Password { get => _pwd; set => SetProperty(ref _pwd, value); }

        /// <inheritdoc/>
        public event Action<IDialogResult> RequestClose;

        /// <inheritdoc/>
        public bool CanCloseDialog() => true;

        /// <inheritdoc/>
        public void OnDialogClosed() {
            //throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void OnDialogOpened(IDialogParameters parameters) {
            //throw new NotImplementedException();
        }

        private void OnSave() {
            _events.GetEvent<Events.PasswordChangedEvent>().Publish(Password);
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }


    }
}
