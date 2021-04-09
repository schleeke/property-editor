using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ApplicationProperties.Views {

    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    public partial class AboutView : UserControl {

        /// <summary>
        /// The name of the dialog.
        /// </summary>
        public static readonly string DIALOG_NAME = "AboutDialog";


        /// <summary>
        /// Initializes a new instance of the <see cref="AboutView"/> clas.
        /// </summary>
        public AboutView() => InitializeComponent();

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            using(var memStream = new MemoryStream(Encoding.UTF8.GetBytes(MainResource.aboutRTF))) {
                AboutText.Selection.Load(memStream, DataFormats.Rtf);
            }
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e) {
            System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }
    }
}
