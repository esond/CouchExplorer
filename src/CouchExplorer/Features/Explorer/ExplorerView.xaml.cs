using System.Windows;
using System.Windows.Input;

namespace CouchExplorer.Features.Explorer
{
    /// <summary>
    /// Interaction logic for Explorer.xaml
    /// </summary>
    public partial class ExplorerView
    {
        public ExplorerView()
        {
            InitializeComponent();
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            Explorer.Focus();
            Keyboard.Focus(Explorer);
        }
    }
}
