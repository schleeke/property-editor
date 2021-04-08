using System.Windows.Controls;

namespace ApplicationProperties.Views {

    /// <summary>
    /// Interaction logic for ContentView.xaml
    /// </summary>
    public partial class ContentView : UserControl {

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentView"/> class.
        /// </summary>
        public ContentView() => InitializeComponent();


        /// <summary>
        /// Prevents entering alphabetical characters into a textbox.
        /// </summary>
        /// <param name="sender">The <see cref="TextBox"/> that fired the event.</param>
        /// <param name="e">Event parameters.</param>
        private void NumericTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e) {
            var regex = new System.Text.RegularExpressions.Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}
