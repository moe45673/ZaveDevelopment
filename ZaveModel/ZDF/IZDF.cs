using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaveGlobalSettings.Data_Structures;

namespace ZaveModel.ZDF
{
    public interface IZDF
    {
        //static event EventHandler<ModelEventArgs> PropertyChanged;
        void Add(ZaveModel.ZDFEntry.IZDFEntry zEntry);

        List<ZDFEntry.IZDFEntry> EntryList { get; set; }

        IEnumerable<ZDFEntry.IZDFEntry> ListEntries();

    }
}
