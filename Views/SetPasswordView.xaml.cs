using ApplicationProperties.ViewModels;
using System.Windows.Controls;

namespace ApplicationProperties.Views {

    /// <summary>
    /// Interaction logic for SetPasswordView.xaml
    /// </summary>
    public partial class SetPasswordView : UserControl {

        /// <summary>
        /// The name of the dialog.
        /// </summary>
        public static readonly string DIALOG_NAME = "PasswordDialog";


        /// <summary>
        /// Initializes a new instance of the <see cref="SetPasswordView"/> class.
        /// </summary>
        public SetPasswordView() => InitializeComponent();

        private void pwdBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e) {
            if(null == DataContext) { return; }
            if(typeof(SetPasswordViewModel) != DataContext.GetType()) { return; }
            ((SetPasswordViewModel)DataContext).Password = ((PasswordBox)sender).SecurePassword;
        }
    }
}
