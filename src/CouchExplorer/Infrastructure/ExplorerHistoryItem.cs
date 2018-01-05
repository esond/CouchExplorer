using System;

namespace CouchExplorer.Infrastructure
{
    public class ExplorerHistoryItem
    {
        public ExplorerHistoryItem(string name, DateTime selectedDateTime)
        {
            Name = name;
            SelectedDateTime = selectedDateTime;
        }

        public string Name { get; set; }

        public DateTime SelectedDateTime { get; set; }
    }
}