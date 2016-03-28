using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zave.ZDF
{
    public class ZDF
    {
        public ZDF()
        {
            isActive = true;
            EntryList = new List<ZDFEntry.ZDFEntry>();
        }
        public bool isActive { get; set; }
        public List<ZDFEntry.ZDFEntry> EntryList { get; set; }
    }
}
