using System.Collections.Generic;

namespace SpeedRunApp.Model
{
    public class SearchResult
    {
        public string Label { get; set; }
        public string Value { get; set; }
        public IEnumerable<SearchResult> SubItems { get; set; }
    }
}
