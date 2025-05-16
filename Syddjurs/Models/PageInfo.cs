using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syddjurs.Models
{
    public class PageInfo
    {
        public string PageName { get; set; } = "";
        public ContentPage PageInstance { get; set; }
        public Type PageType { get; set; }
        public List<EntryInfo> Entries { get; set; } = new();
    }
}
