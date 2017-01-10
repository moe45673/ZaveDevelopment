using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaveModel.ZDFEntry;

namespace ZaveService.ZDFEntry
{
    public interface IZDFEntryService
    {
        int ActiveZDFEntryId { get; set; }
        T getZDFEntry<T>(string id) where T : IZDFEntry, new();

        IZDFEntry getZDFEntry(int id);
    }
}
