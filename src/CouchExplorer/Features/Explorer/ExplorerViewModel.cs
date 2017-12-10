using System.Collections.Generic;
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
            }
        }

        public IEnumerable<ExplorerItemViewModel> Items
        {
            get
            {
                var items = new List<ExplorerItemViewModel>();
                items.AddRange(Directory.GetDirectories(CurrentPath).Select(fse => new ExplorerItemViewModel(fse)));
                items.AddRange(Directory.GetFiles(CurrentPath).Select(fse => new ExplorerItemViewModel(fse)));
                return items;
            }
        }

        public ExplorerItemViewModel SelectedItem
        {
            get => _selectedItem ?? Items.FirstOrDefault();
            set
            {
                _selectedItem = value; 
                
                OnPropertyChanged();
            }
        }

        public ICommand SelectItemCommand => new RelayCommand(SelectItem);

        private void SelectItem()
        {
            if (SelectedItem.IsFile)
            {
                Process.Start(SelectedItem.FilePath);
                return;
            }

            CurrentPath = SelectedItem.FilePath;
            SelectedItem = Items.FirstOrDefault();
        }

        public ICommand GoBackCommand => new RelayCommand(GoBack);

        private void GoBack()
        {
            if (SelectedItem.DirectoryName == Path.GetPathRoot(SelectedItem.DirectoryName))
                return;

            CurrentPath = Path.GetDirectoryName(SelectedItem.DirectoryName);
            SelectedItem = Items.FirstOrDefault();
        }
    }
}
