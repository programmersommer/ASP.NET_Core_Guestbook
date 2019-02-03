using System.Collections.Generic;

namespace Guestbook.Models
{

    public class DataTableParameters
    {
        public List<DataTableColumn> Columns { get; set; }
        public int Draw { get; set; }
        public int iDisplayLength { get; set; }
        public List<DataOrder> Order { get; set; }
        public Search Search { get; set; }
        public int displayStart { get; set; }
    }

    public class Search
    {
        public bool Regex { get; set; }
        public string Value { get; set; }
    }

    public class DataTableColumn
    {
        public int Data { get; set; }
        public string Name { get; set; }
        public bool Orderable { get; set; }
        public bool Searchable { get; set; }
        public Search Search { get; set; }
    }

    public class DataOrder
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }

}
