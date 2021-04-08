using System.Windows;
using System.Windows.Controls;

namespace ApplicationProperties.Views {

    /// <summary>
    /// Interaction logic for MainToolbarView.xaml
    /// </summary>
    public partial class MainToolbarView : UserControl {

        /// <summary>
        /// Initializes a new instance of the <see cref="MainToolbarView"/> class.
        /// </summary>
        public MainToolbarView() => InitializeComponent();

        /// <summary>
        /// Hides the overflow grip.
        /// </summary>
        /// <param name="sender">The toolbar.</param>
        /// <param name="e">Arguments.</param>
        private void ToolBar_Loaded(object sender, RoutedEventArgs e) {
            var toolBar = sender as ToolBar;
            if (toolBar.Template.FindName("OverflowGrid", toolBar) is FrameworkElement overflowGrid) {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
            if (toolBar.Template.FindName("MainPanelBorder", toolBar) is FrameworkElement mainPanelBorder) {
                mainPanelBorder.Margin = new Thickness();
            }
        }

    }
}
