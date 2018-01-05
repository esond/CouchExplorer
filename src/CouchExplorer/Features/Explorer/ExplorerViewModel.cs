using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using CouchExplorer.Infrastructure;

namespace CouchExplorer.Features.Explorer
{
    public class ExplorerViewModel : ViewModelBase
    {
        private readonly IExplorerHistory _history;

        private string _currentPath;
        private ExplorerItemViewModel _selectedItem;

        public ExplorerViewModel(string path, IExplorerHistory history)
        {
            _history = history;
            CurrentPath = path;
        }

        public string CurrentPath
        {
            get => _currentPath;
            set
            {
                _currentPath = value; 
                
                OnPropertyChanged();
                OnPropertyChanged(nameof(Items));

                SelectedItem = Items.FirstOrDefault();
                OnPropertyChanged(nameof(HistoryItems));
            }
        }

        public IEnumerable<ExplorerItemViewModel> Items
        {
            get
            {
                var items = new List<ExplorerItemViewModel>();
                items.AddRange(GetVisibleDirectories());
                items.AddRange(GetVisibleFiles());
                return items;
            }
        }

        public ExplorerItemViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value; 
                
                OnPropertyChanged();
            }
        }

        public IEnumerable<ExplorerHistoryItem> HistoryItems => _history.GetHistoryForPath(CurrentPath);

        #region Commands

        public ICommand SelectItemCommand => new RelayCommand(SelectItem);

        private void SelectItem()
        {
            if (SelectedItem.IsFile)
            {
                _history.AddOrUpdateItem(Path.GetDirectoryName(SelectedItem.FilePath), Path.GetFileName(SelectedItem.FilePath));
                OnPropertyChanged(nameof(HistoryItems));

                Process.Start(SelectedItem.FilePath);
                return;
            }

            _history.AddOrUpdateItem(CurrentPath, SelectedItem.FileName);
            OnPropertyChanged(nameof(HistoryItems));

            CurrentPath = SelectedItem.FilePath;
        }

        public ICommand GoBackCommand => new RelayCommand(GoBack);

        private void GoBack()
        {
            if (SelectedItem.FileName == Path.GetPathRoot(SelectedItem.FilePath))
                return;

<<<<<<< HEAD
            CurrentPath = Path.GetDirectoryName(SelectedItem.DirectoryName);
        }

        public ICommand GoToRootCommand => new RelayCommand(GoToRoot);

        private void GoToRoot()
        {
            CurrentPath = ConfigurationManager.AppSettings["StartupDirectory"];
=======
            CurrentPath = Path.GetDirectoryName(CurrentPath);
            SelectedItem = Items.FirstOrDefault();
>>>>>>> Display the selection history of directory items in descending order
        }

        #endregion

        #region Private Helpers

        private IEnumerable<ExplorerItemViewModel> GetVisibleDirectories()
        {
            return new DirectoryInfo(CurrentPath).GetDirectories()
                .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden))
                .Select(f => new ExplorerItemViewModel(f.FullName));
        }

        private IEnumerable<ExplorerItemViewModel> GetVisibleFiles()
        {
            return new DirectoryInfo(CurrentPath).GetFiles()
                .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden))
                .Select(f => new ExplorerItemViewModel(f.FullName));
        }

        #endregion
    }
}
