using System.Collections.Generic;

namespace CouchExplorer.Infrastructure
{
    public interface IExplorerHistory
    {
        void Save();

        IEnumerable<ExplorerHistoryItem> GetHistoryForPath(string path);

        void AddOrUpdateItem(string path, string itemName);
    }
}