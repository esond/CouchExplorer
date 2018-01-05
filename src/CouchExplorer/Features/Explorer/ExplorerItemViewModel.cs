using System.IO;

namespace CouchExplorer.Features.Explorer
{
    public class ExplorerItemViewModel
    {
        public ExplorerItemViewModel(string filePath) => FilePath = filePath;
        
        public string FilePath { get; }

        public string FileName => Path.GetFileName(FilePath);

        public bool IsFile => !File.GetAttributes(FilePath).HasFlag(FileAttributes.Directory);
    }
}
