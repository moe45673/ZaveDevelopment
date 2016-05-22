using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using ZaveModel.ZDFEntry;
using ZaveGlobalSettings.Data_Structures;
using System.IO;
using Newtonsoft.Json;



namespace ZaveModel.ZDF
{


    public sealed class ZDFSingleton : ObservableObject, IZDF
    {

        //Needs to be protected virtual with private set
       

        private static ZDFSingleton instance;
        private static readonly object syncRoot = new Object();
        private static int _iDTracker;
        //private string _date = DateTime.Now.ToShortTimeString();
        //FileSystemWatcher watcher;

        public static int setID()
        {          
            
            return ++_iDTracker;
        }



        private ZDFSingleton()
        {
            isActive = true;

           
            
            EntryList = new List<ZDFEntry.IZDFEntry>();
            
            
            if (EntryList.Count.Equals(0))
                _iDTracker = 0;
            else
            {
                //get highest existing id and set _iDtracker to it
            }
            //CreateFileWatcher(Path.GetTempPath());
        }



        public static ZDFSingleton Instance
        {
            get
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new ZDFSingleton();
                    }
                }
                return instance;
            }
        }

        public static int IDTracker { get { return _iDTracker; } }

        public bool isActive { get; set; }

        private List<ZDFEntry.IZDFEntry> _entryList;
        
        public List<ZDFEntry.IZDFEntry> EntryList
        {
            get { return _entryList; }
            set { _entryList = value; OnPropertyChanged("EntryList"); }
        }

        public IEnumerable<IZDFEntry> ListEntries()
        {
            return EntryList.ToList<ZDFEntry.IZDFEntry>();
        }

        public void Add(IZDFEntry zEntry)
        {
            try {
                EntryList.Add(zEntry);

                OnPropertyChanged("EntryList");
            }
            catch(ArgumentException ae)
            {
                throw ae;
            }
        }

        
    }
}
