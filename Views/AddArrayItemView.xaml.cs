using System.Windows.Controls;

namespace ApplicationProperties.Views {
    /// <summary>
    /// Interaction logic for AddArrayItemView.xaml
    /// </summary>
    public partial class AddArrayItemView : UserControl {

        /// <summary>
        /// The name of the dialog.
        /// </summary>
        public static readonly string DIALOG_NAME = "AddArrayItemDialog";
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AddArrayItemView"/> class.
        /// </summary>
        public AddArrayItemView() {
            InitializeComponent();
            ItemText.Focus();
        }
    }
}
