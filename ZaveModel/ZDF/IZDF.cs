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

    /// <summary>
    /// All ZDFs inherit from this
    /// </summary>
    public interface IZDF
    {
        //static event EventHandler<ModelEventArgs> PropertyChanged;
        /// <summary>
        /// Wraps the List<T>.Add(T) method
        /// </summary>
        /// <param name="zEntry">The Entry to add</param>
        void Add(ZaveModel.ZDFEntry.IZDFEntry zEntry);
        
        /// <summary>
        /// List of Entries
        /// </summary>
        ObservableImmutableList<ZDFEntry.IZDFEntry> EntryList { get;}

        /// <summary>
        /// Unused
        /// </summary>
        /// <returns></returns>
        SelectionStateList toSelectionStateList();
        

    }
}
