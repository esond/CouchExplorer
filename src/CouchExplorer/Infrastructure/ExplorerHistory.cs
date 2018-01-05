using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace CouchExplorer.Infrastructure
{
    public class ExplorerHistory : IExplorerHistory
    {
        private readonly string _path;
        private readonly Dictionary<string, IEnumerable<ExplorerHistoryItem>> _history;

        private ExplorerHistory(string path, Dictionary<string, IEnumerable<ExplorerHistoryItem>> history)
        {
            _path = path;
            _history = history ?? new Dictionary<string, IEnumerable<ExplorerHistoryItem>>();
        }

        public static ExplorerHistory Load(string filePath)
        {
            if (!File.Exists(filePath))
                return new ExplorerHistory(filePath, new Dictionary<string, IEnumerable<ExplorerHistoryItem>>());

            var serialized = File.ReadAllText(filePath);

            var history =
                JsonConvert.DeserializeObject<Dictionary<string, IEnumerable<ExplorerHistoryItem>>>(serialized);

            return new ExplorerHistory(filePath, history);
        }

        public void Save()
        {
            if (!File.Exists(_path))
                Directory.CreateDirectory(Path.GetDirectoryName(_path) ?? throw new InvalidOperationException());

            File.WriteAllText(_path, JsonConvert.SerializeObject(_history, Formatting.Indented));
        }

        public IEnumerable<ExplorerHistoryItem> GetHistoryForPath(string path)
        {
            return _history.TryGetValue(path, out var history)
                ? history.OrderByDescending(h => h.SelectedDateTime)
                : Enumerable.Empty<ExplorerHistoryItem>();
        }

        public void AddOrUpdateItem(string path, string itemName)
        {
            var pathHistory = GetHistoryForPath(path);
            var items = pathHistory.ToList();

            var existingItem = items.SingleOrDefault(i => i.Name == itemName);

            if (existingItem != null)
                existingItem.SelectedDateTime = DateTime.Now;
            else
                items.Add(new ExplorerHistoryItem(itemName, DateTime.Now));

            _history[path] = items.OrderByDescending(i => i.SelectedDateTime);
        }
    }
}
