using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaveModel.ZDFEntry;

namespace ZaveService.ZDFEntry
{
    public class ZDFEntryHandler : IZDFEntryHandler
    {
        public ZaveModel.IZDFEntry zdfEntry { get; set; }

        public ZDFEntryHandler(ZaveGlobalSettings.Data_Structures.SelectionData selDat)
        {
            zdfEntry = new ZaveModel.ZDFEntry.ZDFEntry()
        }
    }
}
