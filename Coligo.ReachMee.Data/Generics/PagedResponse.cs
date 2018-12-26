using System.Collections.Generic;

namespace Coligo.ReachMee.Data.Generics
{
    public class PagedResponse<T>
    {
        public int pages { get; set; }
        public int total_count { get; set; }
        public List<T> result { get; set; }
        public string prev { get; set; }
        public string next { get; set; }
    }
}
