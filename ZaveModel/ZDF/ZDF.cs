using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using ZaveModel.ZDFEntry;
using ZaveGlobalSettings.Data_Structures;



namespace ZaveModel.ZDF
{
    public sealed class ZDFSingleton : IZDF
    {

        public static event EventHandler<ModelEventArgs> ModelPropertyChanged;
        

        private void OnPropertyChanged(string description, ZDFEntry.IZDFEntry info)
        {
            this.VerifyPropertyName(description);
            SelectionState selState = info.Source;
            var handler = ModelPropertyChanged;
#if DEBUG
            System.Windows.Forms.MessageBox.Show("Inside Event!");
#endif
            if (handler != null)
            {
#if DEBUG
                System.Windows.Forms.MessageBox.Show("Event Fired!");
#endif
                handler(this, new ModelEventArgs(description, selState));
            }
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                if (this.ThrowOnInvalidPropertyName)
                    throw new Exception(msg);
                else
                    Debug.Fail(msg);
            }
        }

        //Needs to be protected virtual with private set
        private bool ThrowOnInvalidPropertyName { get; set; }

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
                OnPropertyChanged("EntryList", zEntry);
            }
            catch(ArgumentException ae)
            {
                throw ae;
            }
        }
    }
}
