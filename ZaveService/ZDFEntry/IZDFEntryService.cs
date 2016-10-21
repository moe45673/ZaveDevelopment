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
        IZDFEntry getZDFEntry(string id);
    }
}
