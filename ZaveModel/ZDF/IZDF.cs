using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaveGlobalSettings.Data_Structures;
using ZaveGlobalSettings.Data_Structures.ZaveObservableCollection;

namespace ZaveModel.ZDF
{
    public interface IZDF
    {
        //static event EventHandler<ModelEventArgs> PropertyChanged;
        void Add(ZaveModel.ZDFEntry.IZDFEntry zEntry);

        ObservableImmutableList<ZDFEntry.IZDFEntry> EntryList { get; set; }

        SelectionStateList toSelectionStateList();
        

    }
}
