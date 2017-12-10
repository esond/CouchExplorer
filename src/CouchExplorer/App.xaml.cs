using System.Windows;
using CouchExplorer.Features.Explorer;

namespace CouchExplorer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindow(new MainViewModel(new ExplorerViewModel()));

            mainWindow.Show();
        }
    }
}
