using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaveModel.ZDFEntry;
using ZaveGlobalSettings.Data_Structures;

namespace ZaveModel.ZDF
{
    public sealed class ZDFSingleton : IZDF
    {

        public event EventHandler<ModelEventArgs> ModelPropertyChanged;
        

        private void OnPropertyChanged(string description, ZDFEntry.IZDFEntry info)
        {
            SelectionState selState = info.Source;
            var handler = ModelPropertyChanged;
            System.Windows.Forms.MessageBox.Show("Inside Event!");
            if (handler != null)
            {
                System.Windows.Forms.MessageBox.Show("Event Fired!");
                handler(this, new ModelEventArgs(description, selState));
            }
        }

        private static ZDFSingleton instance;

        private ZDFSingleton()
        {
            isActive = true;
            
            EntryList = new List<ZDFEntry.IZDFEntry>();
        }

        public static ZDFSingleton Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new ZDFSingleton();
                }
                return instance;
            }
        }
        public bool isActive { get; set; }
        public List<ZDFEntry.IZDFEntry> EntryList { get; set; }

        public IEnumerable<IZDFEntry> ListEntries()
        {
            return EntryList.ToList<ZDFEntry.IZDFEntry>();
        }

        public void Add(IZDFEntry zEntry)
        {
            try {
                EntryList.Add(zEntry);                
                OnPropertyChanged(zEntry.Title, zEntry);
            }
            catch(ArgumentException ae)
            {
                throw ae;
            }
        }
    }
}
