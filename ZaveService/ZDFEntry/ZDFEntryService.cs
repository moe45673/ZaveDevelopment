using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaveModel.ZDFEntry;

namespace ZaveService.ZDFEntry
{
    public class ZDFEntryService : IZDFEntryService
    {

        public ZDFEntryService(ZaveModel.ZDFEntry.ZDFEntry entry = default(ZaveModel.ZDFEntry.ZDFEntry))
        {
            ActiveZDFEntry = entry;
        }

        public ZaveModel.ZDFEntry.ZDFEntry ActiveZDFEntry { get; set; }

        public IZDFEntry getZDFEntry(string id)
        {

            return ZaveModel.ZDF.ZDFSingleton.GetInstance().ListEntries().FirstOrDefault(x => x.ID == Convert.ToInt64(id));
        }
    }
}
