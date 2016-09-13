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


        private EventInitSingleton()
        {

            //CreateFileWatcher(Path.GetTempPath());
            CreateFileWatcher(Path.GetTempPath());
            lastRead = DateTime.MinValue;
            //System.Drawing.Color startupColor = ColorCategory.FromWPFColor(setStartupColor()).Color;
            //activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance();

            //ZaveControlsViewModel.Instance.ActiveColor = setStartupColor();
            //System.Windows.Forms.MessageBox.Show("EventInit Started!");
            //DateTime date = DateTime.Now;
            //SelectionState selState1 = new SelectionState(default(int), "ExampleDoc1.doc", "32", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur pharetra dapibus dolor quis tincidunt. Curabitur leo dui, blandit in consequat eget, luctus ac magna. Quisque leo neque, tincidunt eu ultricies fringilla, convallis eu odio. Vestibulum fringilla mauris id ipsum lobortis, ac accumsan nisi tristique. Sed cursus varius neque eu bibendum. Nam fringilla diam eget turpis pharetra, ac congue urna auctor. Phasellus feugiat, purus ac venenatis varius, risus nisl porta lectus, nec pharetra ipsum velit congue massa. Pellentesque tempus vehicula elit, dictum venenatis mi hendrerit sed. Etiam et diam elementum, tristique est eget, aliquam massa. In id auctor augue. Integer accumsan ante ut ligula pellentesque dictum.\nSed augue dui, faucibus ac neque eget, euismod dignissim mi.Nullam nec varius nulla.In ut enim elit.Sed in leo non nisi ultrices lacinia.Mauris eleifend lectus purus, eget blandit ante suscipit vel.Nunc hendrerit nisl et nunc sodales volutpat.Proin quis metus quam.Proin eget felis tortor.Fusce eget imperdiet velit.\nDuis porta molestie dui, eget facilisis massa venenatis ac.Integer in condimentum est, at iaculis enim.Duis tempus efficitur est, eget sollicitudin turpis.Suspendisse leo velit, aliquet tristique quam id, vulputate tempus purus.Phasellus aliquam aliquet neque at tincidunt.Nam vulputate consequat nulla eu bibendum.Suspendisse auctor, sapien mollis laoreet lacinia, eros velit fermentum purus, non dictum odio tellus vitae diam.Sed enim risus, aliquam sit amet tristique in, interdum in augue.Nunc viverra pulvinar elit eget venenatis.Sed laoreet neque sed nibh fringilla scelerisque.Proin vestibulum rhoncus elit, vel convallis ligula pellen", date.AddMinutes(360), System.Drawing.Color.Yellow);
            //SelectionState selState2 = new SelectionState();
            //SelectionState selState3 = new SelectionState();

            //activeZDF.Add(new ZaveModel.ZDFEntry.ZDFEntry(selState1));
            //activeZDF.Add(new ZaveModel.ZDFEntry.ZDFEntry(selState2));
            //activeZDF.Add(new ZaveModel.ZDFEntry.ZDFEntry(selState3));


        }

        public static EventInitSingleton GetInstance(IEventAggregator eventAgg = null, IUnityContainer cont = null)
        {
            if (instance._eventAggregator == null && eventAgg != null)
            {
                instance._eventAggregator = eventAgg;
                instance._eventAggregator.GetEvent<MainControlsUpdateEvent>().Subscribe(instance.SetActiveColor);
                instance.activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance(eventAgg);
                instance._container = cont;
                instance.activeColor = instance._container.Resolve<ControlBarViewModel>().ActiveColor;
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

            watcher.Filter = GuidGenerator.getGuid();


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

            //onchanged called multiple times, this ensures
            watcher.EnableRaisingEvents = false;
            System.Threading.Thread.Sleep(250);
            watcher.EnableRaisingEvents = true;


            // Specify what is done when a file is changed, created, or deleted.


            //List<SelectionState> _selState = new List<SelectionState>();
            //SelectionState selState = new SelectionState();

            //_selState.Add(JsonConvert.DeserializeObject<SelectionState>(File.ReadAllText(e.FullPath)));
            //SelectionState temp = new SelectionState>();

            using (StreamReader sr = StreamReaderFactory.createStreamReader(e.FullPath))
            {
                try
                {
                    //ObservableImmutableList<ZdfEntryItemViewModel> ZdfEntries = new ObservableImmutableList<ZdfEntryItemViewModel>();
                    string json = sr.ReadToEnd();

                    var temp = JsonConvert.DeserializeObject<SelectionState[]>(json).ToList<SelectionState>();

                    if (temp.Any<SelectionState>())
                    {
                        temp[0].ID = ZaveModel.ZDF.ZDFSingleton.setID();
                        temp[0].Comments = new List<SelectionComment>();

                        ZaveModel.ZDFEntry.ZDFEntry entry = new ZaveModel.ZDFEntry.ZDFEntry(temp[0]);

                        entry.HColor = ColorCategory.FromWPFColor(activeColor);

                        //_eventAggregator.GetEvent<EntryCreatedEvent>().Publish(entry)

                        activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance();

                        //MessageBox.Show(Thread.CurrentThread.ManagedThreadId.ConvertToString());
                        //if (activeZDF.EntryList.Count == 0)
                        //{
                        activeZDF.Add(entry);
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


        //public event NotifyCollectionChangedEventHandler CollectionChanged;
        //protected void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        //{

        //}

        //private void DocumentSelectionChange(Microsoft.Office.Interop.Word.Document Doc)
        //{


        //}

        //private static void ThisDocument_SelectionChange(object sender, Microsoft.Office.Tools.Word.SelectionEventArgs e)
        //{

        //    string SelectionDocName = e.Selection.Application.ActiveDocument.Name;

        //}

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
