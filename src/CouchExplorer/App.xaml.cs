using System.Configuration;
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
            var startupDir = ConfigurationManager.AppSettings["StartupDirectory"];

            var viewModel = new MainViewModel(new ExplorerViewModel(startupDir));
            var mainWindow = new MainWindow(viewModel);

            mainWindow.Show();
        }
    }
}
