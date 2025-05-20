using Syddjurs.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syddjurs.Models
{
    public class EntryInfo
    {
        public string EntryId { get; set; } = "";
        public string EntryName { get; set; } = "";
        public CustomEntry EntryControl { get; set; }
    }
}
