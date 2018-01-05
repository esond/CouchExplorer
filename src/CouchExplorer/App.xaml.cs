using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using CouchExplorer.Common;
using CouchExplorer.Features.Explorer;
using CouchExplorer.Infrastructure;

namespace CouchExplorer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private ExplorerHistory _explorerHistory;

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            var startupDir = ConfigurationManager.AppSettings["StartupDirectory"];

            var historyPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Assembly.GetExecutingAssembly().GetName().Name, "history.json");

            _explorerHistory = ExplorerHistory.Load(historyPath);

            var viewModel = new MainViewModel(new ExplorerViewModel(startupDir, _explorerHistory));
            var mainWindow = new MainWindow(viewModel);

            mainWindow.Show();

            Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            Current.Exit += HandleExit;
        }

        private void HandleExit(object sender, ExitEventArgs e)
        {
            _explorerHistory.Save();
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
