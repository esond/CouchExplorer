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
        private string _currentPath;
        private ExplorerItemViewModel _selectedItem;

        public ExplorerViewModel(string path) => CurrentPath = path;

        public string CurrentPath
        {
            get => _currentPath;
            set
            {
                _currentPath = value; 
                
                OnPropertyChanged();
                OnPropertyChanged(nameof(Items));

                SelectedItem = Items.FirstOrDefault();
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

        #region Commands

        public ICommand SelectItemCommand => new RelayCommand(SelectItem);

        private void SelectItem()
        {
            if (SelectedItem.IsFile)
            {
                Process.Start(SelectedItem.FilePath);
                return;
            }

            CurrentPath = SelectedItem.FilePath;
        }

        public ICommand GoBackCommand => new RelayCommand(GoBack);

        private void GoBack()
        {
            if (SelectedItem.DirectoryName == Path.GetPathRoot(SelectedItem.DirectoryName))
                return;

            CurrentPath = Path.GetDirectoryName(SelectedItem.DirectoryName);
        }

        public ICommand GoToRootCommand => new RelayCommand(GoToRoot);

        private void GoToRoot()
        {
            CurrentPath = ConfigurationManager.AppSettings["StartupDirectory"];
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
