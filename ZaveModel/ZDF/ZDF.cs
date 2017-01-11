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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZaveGlobalSettings.Data_Structures.ZaveObservableCollection;
using Prism.Mvvm;
using Prism.Events;

namespace ZaveModel.ZDF
{

    [JsonObject]
    [JsonConverter(typeof(ZDFConverter))]
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
        private static int _entryIDTracker;
        [JsonIgnore]
        private IEventAggregator _eventAggregator;
        //private string _date = DateTime.Now.ToShortTimeString();
        //FileSystemWatcher watcher;

        public static int setEntryID()
        {          
            
            return ++_entryIDTracker;
        }



        private ZDFSingleton()
        {


            
            
            EntryList = new ObservableImmutableList<ZDFEntry.IZDFEntry>();

            Name = "";


            
            _entryIDTracker = EntryList.Count;
            
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
                if (instance != null && instance._eventAggregator == null)
                {
                        instance._eventAggregator = new EventAggregator();
                }
            }
            return instance;


        }

        [JsonProperty]
        public static int EntryIDTracker { get { return _entryIDTracker; } }

        
       
        [JsonIgnore]
        private ObservableImmutableList<ZDFEntry.IZDFEntry> _entryList;

        //public event NotifyCollectionChangedEventHandler CollectionChanged;

        public static int RefreshEntryIDCounter()
        {
            int count = instance.EntryList.Count();
            if(count>0)
                _entryIDTracker = instance.EntryList.ElementAt(count - 1).ID;
            return EntryIDTracker;
        }

        [JsonIgnore]
        private string _name;
        [JsonProperty]
        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
            }
        }

        [JsonProperty]
        public ObservableImmutableList<ZDFEntry.IZDFEntry> EntryList
        {
            get { return _entryList; }
            set { SetProperty(ref _entryList, value); }
        }

        [JsonIgnore]
        private int _id;
        [JsonProperty]
        public int ID
        {
            get { return _id; }
            set
            {
                SetProperty(ref _id, value);
            }
        }

        //public SelectionStateList toSelectionStateList()
        //{
        //    SelectionStateList selStateList = SelectionStateList.Instance;

        //    selStateList.Clear();

        //    foreach (var item in EntryList)
        //    {
        //        selStateList.Add(item.toSelectionState());
        //    }

        //    return selStateList;


        //}

        public IEnumerable<IZDFEntry> ListEntries()
        {
            return EntryList.ToList<ZDFEntry.IZDFEntry>();
        }

        public void Add(ZDFEntry.IZDFEntry zEntry)
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

        public static void Add(object obj)
        {
            Instance.Add(obj as ZDFEntry.IZDFEntry);
        }

        public void Clear()
        {
            IEventAggregator ea = this._eventAggregator;
            instance = null;
            GetInstance(ea);
        }

        
    }

    class ZDFConverter : JsonConverter
    {

        
        
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(ZDFSingleton));
        }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        //public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        //{
        //    //var jsonObject = JObject.Load(reader);
        //    var zdf = ZDFSingleton.GetInstance();
        //    //System.Windows.Forms.MessageBox.Show(existingValue.ToString());
        //    if (reader.TokenType == JsonToken.StartObject)
        //    {

        //        T instance = (T)serializer.Deserialize<T>(reader);

        //        //Comments = (List<EntryComment>) jsonObject.Se
        //    }
     



        //    //serializer.Populate(jsonObject.CreateReader(), commentList);
        //    //return commentList;
        //    return new object();
        //}
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            ZDFSingleton activeZdf = ZDFSingleton.GetInstance();
            activeZdf.EntryList.Clear();

            //activeZdf = ZDFSingleton.GetInstance(eventAggregator);
            JArray ja = (JArray)jObject["EntryList"]["_items"];

            //activeZdf.EntryList = new ObservableImmutableList<IZDFEntry>(ja.ToObject<List<ZDFEntry>>());

            foreach (var item in (ja.ToObject<List<ZDFEntry.ZDFEntry>>()))
            {
                activeZdf.EntryList.Add(item);
            }
            ZDFSingleton.RefreshEntryIDCounter();

            return activeZdf;

        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

}
