using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Prism.Events;
using Prism.Unity;
using ZaveGlobalSettings.Data_Structures;
using ZaveGlobalSettings.ZaveFile;
using ZaveModel.ZDFColors;
//using ZaveService.ZDFEntry;
using WPFColor = System.Windows.Media.Color;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using ZaveGlobalSettings.Data_Structures.ZaveObservableCollection;
using ZaveViewModel.ViewModels;
using ZaveModel.ZDFEntry;
using Microsoft.Office.Interop.Word;
using ZaveModel.ZDF;
using Microsoft.Practices.Unity;
using ZaveViewModel.Data_Structures;
using System.Collections.Specialized;
using System.ComponentModel;



//using ZaveMo

namespace ZaveController
{


    /// <summary>
    /// 
    /// </summary>
    public sealed class EventInitSingleton : PubSubEvent<SelectionState>, IDisposable
    {

        bool disposed = false;
        public ZaveModel.ZDF.ZDFSingleton activeZDF;
        private static DateTime lastRead;
        private IUnityContainer _container;

        private static readonly EventInitSingleton instance = new EventInitSingleton();
        //private static EventInitSingleton instance;
        private FileSystemWatcher watcher;
        private IEventAggregator _eventAggregator;


        //public ZDFEntryHandler zdfEntryHandler { get; set; }

        //SafeHandle handle = new SafeFileHandle(, true);

        /// <summary>
        /// 
        /// </summary>
        private EventInitSingleton()
        {
            CreateFileWatcher(Path.GetTempPath());
            lastRead = DateTime.MinValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cont">The Unity DI Container</param>
        /// <param name="eventAgg">The Prism Event Aggregator</param>
        /// <returns></returns>
        public static EventInitSingleton GetInstance(IUnityContainer cont, IEventAggregator eventAgg = null)
        {
            if (instance._eventAggregator == null && eventAgg != null)
            {
                instance._eventAggregator = eventAgg;
                instance._eventAggregator.GetEvent<ActiveColorUpdatedEvent>().Subscribe(instance.SetActiveColor);
                instance.activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance(eventAgg);
                instance._container = cont;
                cont.RegisterInstance<ZDFSingleton>(InstanceNames.ActiveZDF, instance.activeZDF);
                instance.activeColor = instance._container.Resolve<ColorPickerViewModel>().ActiveColor;
            }



            return Instance;
        }

        //private WPFColor setStartupColor()
        //{
        //    return new WPFColor();
        //}

        private WPFColor activeColor;
        public void SetActiveColor(System.Drawing.Color color)
        {
            string colorName = "Unknown Color";
            foreach (var item in GetColors())
            {
                if (color.ToString().Equals(item.Value.ToString()))
                {
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

            watcher.Filter = APIFileNames.SourceToZave;


            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnFileChanged);
            //watcher.Created += new FileSystemEventHandler(OnFileCreated);
            //watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);

            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }

        private void OnFileChanged(object source, FileSystemEventArgs e)
        {

            //onFilechanged called multiple times per logical file change, this ensures that only the first file change is caught
            watcher.EnableRaisingEvents = false;
            System.Threading.Thread.Sleep(250);
            watcher.EnableRaisingEvents = true;




            using (StreamReader sr = StreamReaderFactory.createStreamReader(e.FullPath))
            {
                try
                {
                    //ObservableImmutableList<ZdfEntryItemViewModel> ZdfEntries = new ObservableImmutableList<ZdfEntryItemViewModel>();
                    string json = sr.ReadToEnd();

                    var temp = JsonConvert.DeserializeObject<SelectionState[]>(json).ToList<SelectionState>();

                    if (temp.Any<SelectionState>())
                    {
                        temp[0].ID = ZaveModel.ZDF.ZDFSingleton.setEntryID();
                        temp[0].Comments = new List<SelectionComment>();

                        ZaveModel.ZDFEntry.ZDFEntry entry = ZaveModel.ZDFEntry.ZDFEntry.CreateZDFEntry(temp[0]);

                        entry.HColor = ColorCategory.FromWPFColor(activeColor);

                        //_eventAggregator.GetEvent<EntryCreatedEvent>().Publish(entry)

                        activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance();
                        MainWindowViewModel.ZdfUndoComments.Clear();
                        MainWindowViewModel.RemoveZdundoComments.Clear();
                        //MessageBox.Show(Thread.CurrentThread.ManagedThreadId.ConvertToString());
                        //if (activeZDF.EntryList.Count == 0)
                        //{
                        activeZDF.Add(entry);
                        #region oldstuff
                        //}
                        //if (activeZDF.EntryList.Count > 0)
                        //{
                        //    var selected = ZDFEntryItem.SelectedZDFByUser;
                        //    if (selected != null)
                        //    {
                        //        var selectZDF = activeZDF.EntryList.Where(t => Convert.ToString(t.ID) == selected);
                        //        selectZDF.FirstOrDefault().Text = selectZDF.FirstOrDefault().Text + entry.Text;
                        //        ZdfEntries.Add(new ZdfEntryItemViewModel(entry as ZDFEntry));

                        //        //SelectionState selState = new SelectionState(temp[0].ID, temp[0].SelectionDocName, temp[0].SelectionPage, temp[0].SelectionText, DateTime.Now.AddMinutes(360), System.Drawing.Color.Yellow, SrcType.WORD, new List<object>());
                        //        //ZDFEntryViewModel OBJ = ZDFEntryViewModel.EntryVmFactory(null, null, null, selectZDF.FirstOrDefault());

                        //        ////SelectionState selState = new SelectionState(temp[0].ID, temp[0].SelectionDocName, temp[0].SelectionPage, temp[0].SelectionText, DateTime.Now.AddMinutes(360), System.Drawing.Color.Yellow, SrcType.WORD, new List<object>());
                        //        //ZaveViewModel.Data_Structures.ZDFEntryItem obj = ZdfEntries[0];
                        //        //EventSetProperties(obj, selState);
                        //    }

                        //    #region Later Code
                        //    //SelectionState selState = new SelectionState(temp[0].ID, temp[0].SelectionDocName, temp[0].SelectionPage, temp[0].SelectionText, DateTime.Now.AddMinutes(360), System.Drawing.Color.Yellow, SrcType.WORD, new List<object>());
                        //    //selected zdf required
                        //    //string selection = selState.SelectionDocName;
                        //    //var r = activeZDF.toSelectionStateList().Where(t => t.srcType == SrcType.WORD);
                        //    //ZdfEntries.Add(new ZdfEntryItemViewModel(entry as ZDFEntry));
                        //    //List<SelectionState> selState1 = activeZDF.toSelectionStateList();

                        //    //bool isactive = ZdfEntries.FirstOrDefault().AddCommentDelegateCommand.IsActive;
                        //    //foreach (ZDFEntry en in activeZDF.EntryList.ToList())
                        //    //{
                        //    //foreach (ZdfEntryItemViewModel zeivm in ZdfEntries)
                        //    //{
                        //    //    if (zeivm.AddCommentDelegateCommand.IsActive)
                        //    //    {
                        //    //        activeZDF.EntryList[1].Text = "Active-AddCommentDelegateCommand" + temp[0].SelectionText;
                        //    //    }
                        //    //    else {
                        //    //        activeZDF.EntryList[1].Text = "In Active" + temp[0].SelectionText;
                        //    //    }
                        //    //}
                        //    //if (en.toSelectionState().SelectionDocName != null)
                        //    //{
                        //    //    string sam = en.toSelectionState().SelectionDocName.ToString();
                        //    //}
                        //    //}

                        //    #endregion
                        //}
                        #endregion

                    }
                    sr.Close();

                }
                catch (IOException ex)
                {
                    throw ex;
                }

            }
        }
        private IZDFEntry _zdfEntry;
        void EventSetProperties(object obj, SelectionState sc)
        {
            ZDFEntryItem item = obj as ZDFEntryItem;

            if (item != null) _zdfEntry = item.ZDFEntry;

        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        }







    }




}
