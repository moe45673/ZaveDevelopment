using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZaveController.ZDFSource;
using ZaveGlobalSettings.Data_Structures;
//using ZaveService.ZDFEntry;
using WordInterop = Microsoft.Office.Interop.Word;
using Microsoft.Win32.SafeHandles;
using Office = Microsoft.Office.Core;
using WordTools = Microsoft.Office.Tools.Word;
using System.Runtime.InteropServices;

using ZaveGlobalSettings.ZaveFile;
using ZaveViewModel.ZaveControlsViewModel;
using System.IO;
using Newtonsoft.Json;
//using ZaveMo

namespace ZaveController.Global_Settings
{

    
    
    public sealed class EventInitSingleton : IDisposable
    {

        bool disposed = false;
        public ZaveModel.ZDF.ZDFSingleton activeZDF;
        private static DateTime lastRead;

        private static readonly EventInitSingleton instance = new EventInitSingleton();
        private FileSystemWatcher watcher;
        
        //public ZDFEntryHandler zdfEntryHandler { get; set; }

        //SafeHandle handle = new SafeFileHandle(, true);
        

        private EventInitSingleton()
        {
            
            activeZDF = ZaveModel.ZDF.ZDFSingleton.Instance;
            CreateFileWatcher(Path.GetTempPath());
            lastRead = DateTime.MinValue;
            ZaveControlsViewModel.Instance.ActiveColor = setStartupColor();

            DateTime date = DateTime.Now;
            SelectionState selState1 = new SelectionState("ExampleDoc1.doc", "32", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur pharetra dapibus dolor quis tincidunt. Curabitur leo dui, blandit in consequat eget, luctus ac magna. Quisque leo neque, tincidunt eu ultricies fringilla, convallis eu odio. Vestibulum fringilla mauris id ipsum lobortis, ac accumsan nisi tristique. Sed cursus varius neque eu bibendum. Nam fringilla diam eget turpis pharetra, ac congue urna auctor. Phasellus feugiat, purus ac venenatis varius, risus nisl porta lectus, nec pharetra ipsum velit congue massa. Pellentesque tempus vehicula elit, dictum venenatis mi hendrerit sed. Etiam et diam elementum, tristique est eget, aliquam massa. In id auctor augue. Integer accumsan ante ut ligula pellentesque dictum.\nSed augue dui, faucibus ac neque eget, euismod dignissim mi.Nullam nec varius nulla.In ut enim elit.Sed in leo non nisi ultrices lacinia.Mauris eleifend lectus purus, eget blandit ante suscipit vel.Nunc hendrerit nisl et nunc sodales volutpat.Proin quis metus quam.Proin eget felis tortor.Fusce eget imperdiet velit.\nDuis porta molestie dui, eget facilisis massa venenatis ac.Integer in condimentum est, at iaculis enim.Duis tempus efficitur est, eget sollicitudin turpis.Suspendisse leo velit, aliquet tristique quam id, vulputate tempus purus.Phasellus aliquam aliquet neque at tincidunt.Nam vulputate consequat nulla eu bibendum.Suspendisse auctor, sapien mollis laoreet lacinia, eros velit fermentum purus, non dictum odio tellus vitae diam.Sed enim risus, aliquam sit amet tristique in, interdum in augue.Nunc viverra pulvinar elit eget venenatis.Sed laoreet neque sed nibh fringilla scelerisque.Proin vestibulum rhoncus elit, vel convallis ligula pellen", date.AddMinutes(360), System.Drawing.Color.Yellow);
            SelectionState selState2 = new SelectionState();
            SelectionState selState3 = new SelectionState();

            activeZDF.Add(new ZaveModel.ZDFEntry.ZDFEntry(selState1));
            activeZDF.Add(new ZaveModel.ZDFEntry.ZDFEntry(selState2));
            activeZDF.Add(new ZaveModel.ZDFEntry.ZDFEntry(selState3));

            //System.Windows.Forms.MessageBox.Show("EventInit Started!");
            //ThisAddIn.WordFired += new EventHandler<SrcEventArgs>(SrcHighlightEventHandler);
        }

        private System.Windows.Media.Color setStartupColor()
        {
            return System.Windows.Media.Colors.Yellow;
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

        public static EventInitSingleton Instance
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
                        ZaveModel.ZDFEntry.ZDFEntry entry = new ZaveModel.ZDFEntry.ZDFEntry(temp[0]);
                        entry.HColor.FromWPFColor(ZaveControlsViewModel.Instance.ActiveColor);

                        ZaveModel.ZDF.ZDFSingleton activeZDF = ZaveModel.ZDF.ZDFSingleton.Instance;

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
