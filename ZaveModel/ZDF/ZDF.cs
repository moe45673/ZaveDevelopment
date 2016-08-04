using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using ZaveModel.ZDFEntry;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ZaveGlobalSettings.Data_Structures;
using System.Collections.Immutable;
using System.Threading;
using System.Windows.Forms;
using JetBrains.ReSharper.Psi.Resx.Utils;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Events;
using ZaveGlobalSettings.Data_Structures.ZaveObservableCollection;


namespace ZaveModel.ZDF
{

    [JsonObject]
    public sealed class ZDFSingleton : BindableBase, IZDF
    {

        //Needs to be protected virtual with private set
        
        [JsonIgnore]
        private static ZDFSingleton instance;
        [JsonIgnore]
        private static readonly object syncRoot = new Object();
        [JsonIgnore]
        private static int _iDTracker;
        [JsonIgnore]
        private IEventAggregator _eventAggregator;
        //private string _date = DateTime.Now.ToShortTimeString();
        //FileSystemWatcher watcher;

        public static int setID()
        {          
            
            return ++_iDTracker;
        }



        private ZDFSingleton()
        {


            
            
            EntryList = new ObservableImmutableList<IZDFEntry>();
            
            
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
                
                return GetInstance();
                
                
            }
        }
        
        public static ZDFSingleton GetInstance(IEventAggregator eventAgg = null)
        {
            lock (syncRoot)
            {
                if (eventAgg != null && instance == null)
                {
                    instance = new ZDFSingleton();
                    instance._eventAggregator = eventAgg;
                    instance._eventAggregator.GetEvent<EntryCreatedEvent>().Subscribe(Add);
                }          
                if (instance != null)
                {
                    if (instance._eventAggregator == null)
                        instance._eventAggregator = new EventAggregator();

                    instance._eventAggregator.GetEvent<EntryCreatedEvent>().Subscribe(Add);
                }

            }
            return instance;


        }

        [JsonProperty]
        public static int IDTracker { get { return _iDTracker; } }

       
        [JsonIgnore]
        private ObservableImmutableList<IZDFEntry> _entryList;

        //public event NotifyCollectionChangedEventHandler CollectionChanged;

        public ObservableImmutableList<IZDFEntry> EntryList
        {
            get { return _entryList; }
            set { SetProperty(ref _entryList, value); }
        }

        public SelectionStateList toSelectionStateList()
        {
            SelectionStateList selStateList = SelectionStateList.Instance;

            selStateList.Clear();

            foreach (var item in EntryList)
            {
                selStateList.Add(item.toSelectionState());
            }

            return selStateList;


        }

        public IEnumerable<IZDFEntry> ListEntries()
        {
            return EntryList.ToList<ZDFEntry.IZDFEntry>();
        }

        public void Add(IZDFEntry zEntry)
        {
            try
            {
                EntryList.Add(zEntry);
                
                

            }
            catch (ArgumentException ae)
            {
                System.Windows.Forms.MessageBox.Show(ae.Message);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

        public static void Add(Object obj)
        {
            Instance.Add(obj as IZDFEntry);
        }

        public void Clear()
        {
            IEventAggregator ea = this._eventAggregator;
            instance = null;
            GetInstance(ea);
        }

        
    }
}
