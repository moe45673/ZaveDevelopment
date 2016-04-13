using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaveModel.ZDFEntry;

namespace ZaveModel.ZDF
{
    public class ZDF : IZDF
    {
        public ZDF()
        {
            isActive = true;
            
            EntryList = new List<ZDFEntry.IZDFEntry>();
        }
        public bool isActive { get; set; }
        public List<ZDFEntry.IZDFEntry> EntryList { get; set; }

        public IEnumerable<IZDFEntry> ListEntries()
        {

        }
    }
}
