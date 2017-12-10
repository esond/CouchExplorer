using System.Configuration;
using System.Windows;
using System.Windows.Threading;
using CouchExplorer.Features.Common;
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

            Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        private void HandleUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var viewModel = new UnhandledExceptionViewModel(e.Exception);
            var window = new Window
            {
                Title = "Couch Explorer - Error",
                Height = 400,
                Width = 500,
                SizeToContent = SizeToContent.Manual,
                DataContext = viewModel,
                Content = viewModel,
                ContentTemplate = new DataTemplate(viewModel.GetType())
                {
                    VisualTree = new FrameworkElementFactory(typeof(UnhandledExceptionView))
                }
            };

            viewModel.PropertyChanged += (propSender, args) =>
            {
                if (viewModel.DialogResult.HasValue)
                    window.Close();
            };

            window.ShowDialog();
            e.Handled = true;
        }
    }
}
