using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Prism.Events;
using ZaveGlobalSettings.Data_Structures;
using ZaveGlobalSettings.ZaveFile;
using ZaveModel.ZDFColors;
//using ZaveService.ZDFEntry;
using WPFColor = System.Windows.Media.Color;

//using ZaveMo

namespace ZaveController
{



    public sealed class EventInitSingleton : PubSubEvent<SelectionState>, IDisposable
    {

        bool disposed = false;
        public ZaveModel.ZDF.ZDFSingleton activeZDF;
        private static DateTime lastRead;

        private static readonly EventInitSingleton instance = new EventInitSingleton();
        //private static EventInitSingleton instance;
        private FileSystemWatcher watcher;
        private IEventAggregator _eventAggregator;
        
        //public ZDFEntryHandler zdfEntryHandler { get; set; }

        //SafeHandle handle = new SafeFileHandle(, true);
        

        private EventInitSingleton()
        {

            
            CreateFileWatcher(Path.GetTempPath());
            lastRead = DateTime.MinValue;
            System.Drawing.Color startupColor = new System.Drawing.Color();
            //activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance();

            //ZaveControlsViewModel.Instance.ActiveColor = setStartupColor();
            //System.Windows.Forms.MessageBox.Show("EventInit Started!");
            


        }

        public static EventInitSingleton GetInstance(IEventAggregator eventAgg = null)
        {

            if (instance._eventAggregator == null && eventAgg != null)
            {
                instance._eventAggregator = eventAgg;
                instance._eventAggregator.GetEvent<MainControlsUpdateEvent>().Subscribe(instance.SetActiveColor);
                //instance.activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance(eventAgg);
            }
            
            return Instance;
        }

        private WPFColor setStartupColor()
        {
            return System.Windows.Media.Colors.Yellow;
        }

        private WPFColor activeColor;
        public void SetActiveColor(System.Drawing.Color color)
        {
            string colorName = "Unknown Color";
            foreach (var item in GetColors())
            {
                if (color.ToString().Equals(item.Value.ToString())){
                    colorName = item.Key;
                }
            }
            activeColor = new ColorCategory(color, colorName).toWPFColor();
            
            
        }

        private IEnumerable<KeyValuePair<String, System.Drawing.Color>> GetColors()
        {
            return typeof(System.Drawing.SystemColors)
                .GetProperties()
                .Where(prop =>
                    typeof(System.Drawing.Color).IsAssignableFrom(prop.PropertyType))
                .Select(prop =>
                    new KeyValuePair<String, System.Drawing.Color>(prop.Name, (System.Drawing.Color)prop.GetValue(null)));
        }

        //private ~EventInitSingleton()
        //{
        //    Dispose();
        //}

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                watcher.Dispose();
            }

            disposed = true;
        }

        private static EventInitSingleton Instance
        {
            get
            {                
                return instance;
            }
        }

        public void CreateFileWatcher(string path)
        {
            // Create a new FileSystemWatcher and set its properties.
            watcher = new FileSystemWatcher();
            watcher.Path = path;
            /* Watch for changes in LastAccess and LastWrite times, and 
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.

            watcher.Filter = GuidGenerator.getGuid();

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnFileChanged);
            //watcher.Created += new FileSystemEventHandler(OnChanged);
            //watcher.Deleted += new FileSystemEventHandler(OnChanged);
            //watcher.Renamed += new RenamedEventHandler(OnRenamed);

            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }

        // Define the event handlers.
        private void OnFileChanged(object source, FileSystemEventArgs e)
        {

            //onchanged called multiple times, this ensures
            watcher.EnableRaisingEvents = false;
            System.Threading.Thread.Sleep(250);
            watcher.EnableRaisingEvents = true;
            //activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance();

            // Specify what is done when a file is changed, created, or deleted.


            //List<SelectionState> _selState = new List<SelectionState>();
            //SelectionState selState = new SelectionState();

            //_selState.Add(JsonConvert.DeserializeObject<SelectionState>(File.ReadAllText(e.FullPath)));
            //SelectionState temp = new SelectionState>();

            using (StreamReader sr = StreamReaderFactory.createStreamReader(e.FullPath))
                {
                    try
                    {
                        string json = sr.ReadToEnd();

                        var temp = JsonConvert.DeserializeObject<SelectionState[]>(json).ToList<SelectionState>();

                        if (temp.Any<SelectionState>())
                        {
                            temp[0].ID = ZaveModel.ZDF.ZDFSingleton.setID();
                            temp[0].Comments = new List<object>();
                            ZaveModel.ZDFEntry.ZDFEntry entry = new ZaveModel.ZDFEntry.ZDFEntry(temp[0]);
                            
                            entry.HColor = ColorCategory.FromWPFColor(activeColor);

                            //_eventAggregator.GetEvent<EntryCreatedEvent>().Publish(entry)
                            

                            activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance();
                            //MessageBox.Show(Thread.CurrentThread.ManagedThreadId.ConvertToString());
                            activeZDF.Add(entry);
                        }
                        sr.Close();
                    }
                    catch (IOException ex)
                    {
                        throw ex;
                    }
                   
            

            }



        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        }


        //public void SrcHighlightEventHandler(Object o, SrcEventArgs e)
        //{



        //    ZaveModel.ZDFEntry.ZDFEntry entry = new ZaveModel.ZDFEntry.ZDFEntry();

        //    entry.Source = e.zSrc;
        //    activeZDF.Add(entry);
            //zdfEntryHandler = new ZaveService.ZDFEntry.DefaultZDFEntryHandler(e.zSrc, activeZDF);
            //zdfEntryHandler.CreateZDFEntry(new ZaveModel.ZDFEntry.ZDFEntry(e.zSrc));
//            entry.Source = e.zSrc;

//            _saveZDFEntryCommand = new RelayCommand(param => SaveZDFEntry(entry)
//, param => (entry != null));

           
           // activeZDF.Add(entry);

            
            //zevm.TxtDocName = activeZDF.EntryList[activeZDF.EntryList.Count - 1].Source.SelectionDocName;
            
            
            
        //}

        


    }

    

    
}
