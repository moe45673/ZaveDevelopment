using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
//using System.Text.RegularExpressions;
using System.ComponentModel;
using WordInterop = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using WordTools = Microsoft.Office.Tools.Word;
using FirstWordAddIn.DataStructures;
using ZaveGlobalSettings.Data_Structures;
using ZaveGlobalSettings.ZaveFile;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using ZaveGlobalSettings.ZaveResources;
using ZaveGlobalSettings.Data_Structures.AddInInterface;



//using ZaveSrc = ZaveGlobalSettings.Data_Structures.SelectionState;

 
namespace FirstWordAddIn
{
    
    

    


    
    public partial class ThisAddIn : IZaveAddIn
    {
       
        RichTextBox rtb;


        private IZaveAddInState _enabled;

        private IZaveAddInState _disabled;

        public IZaveAddInState CurrentState { get; set; }

        private IZaveLowLevelHook llHook;

        





        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="fileName"></param>
        ///// <returns></returns>
       

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            llHook = new ZaveCursorHook();
            _enabled = new ZaveEnabled(llHook);
            _disabled = new ZaveDisabled(llHook);
            CreateFileWatcher(Path.GetTempPath());
            if (File.Exists(watcher.Path + watcher.Filter))
                CurrentState = _enabled;
            else
                CurrentState = _disabled;


            //TODO Build a factory of some kind


            //try
            //{


            //    llHook.Init();
            //    llHook.Start();
            //}
            //catch (Exception ex)
            //{
            //    llHook.Dispose();
            //    throw ex;
            //}

            //MouseHook.MouseAction += new EventHandler(Captured);
            //only start listening for the event when a document is opened or created
            this.Application.DocumentOpen -=
            new WordInterop.ApplicationEvents4_DocumentOpenEventHandler(DocumentSelectionChange);
            this.Application.DocumentOpen +=
            new WordInterop.ApplicationEvents4_DocumentOpenEventHandler(DocumentSelectionChange);

            ((WordInterop.ApplicationEvents4_Event)this.Application).NewDocument -=
                new WordInterop.ApplicationEvents4_NewDocumentEventHandler(DocumentSelectionChange);
            ((WordInterop.ApplicationEvents4_Event)this.Application).NewDocument +=
                new WordInterop.ApplicationEvents4_NewDocumentEventHandler(DocumentSelectionChange);

            this.Application.WindowDeactivate -= new WordInterop.ApplicationEvents4_WindowDeactivateEventHandler(Deactivated);
            this.Application.WindowDeactivate += new WordInterop.ApplicationEvents4_WindowDeactivateEventHandler(Deactivated);
            this.Application.WindowActivate -= new WordInterop.ApplicationEvents4_WindowActivateEventHandler(Activated);
            this.Application.WindowActivate += new WordInterop.ApplicationEvents4_WindowActivateEventHandler(Activated);


            rtb = new RichTextBox();

            

        }


        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {

            CurrentState.Dispose();
        }
        
        /// <summary>
        /// Get the current doc and pass it to the event handler
        /// </summary>
        /// <param name="Doc"></param>
        private void DocumentSelectionChange(Microsoft.Office.Interop.Word.Document Doc)
        {

            WordTools.Document vstoDoc = Globals.Factory.GetVstoObject(this.Application.ActiveDocument);
            vstoDoc.SelectionChange += new Microsoft.Office.Tools.Word.SelectionEventHandler(ThisDocument_SelectionChange);

        }

        

        private void Activated(WordInterop.Document Doc, WordInterop.Window Wn) {
            try {
                CurrentState.Start();
            }catch(Exception ex)
            {
                CurrentState.Dispose();
                throw ex;
            }
        }
        private void Deactivated(WordInterop.Document Doc, WordInterop.Window Wn) { CurrentState.Stop(); }


        private void ThisDocument_SelectionChange(object sender, Microsoft.Office.Tools.Word.SelectionEventArgs e)
        {

            CurrentState.SelectionChanged<Microsoft.Office.Tools.Word.SelectionEventArgs>(this, e);
           
            
        }


        //private async Task WriteToJsonFileAsync(string filename, List<SelectionState> selStateList)
        //{

        //    await Task.Run(() =>
        //    {
        //        string json = Newtonsoft.Json.JsonConvert.SerializeObject(selStateList.ToArray());




        //        using (StreamWriter sw = StreamWriterFactory.createStreamWriter(filename))
        //        {
        //            try
        //            {

        //                sw.Write(json);

        //                sw.Close();
        //            }
        //            catch (IOException ex)
        //            {
        //                throw ex;
        //            }
        //        }
        //    });

        //}

        //TODO Create FileWatcher factory!
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

            watcher.Filter = APIFileNames.ZaveToSource;


            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnFileChanged);
            watcher.Created += new FileSystemEventHandler(OnFileCreated);
            watcher.Deleted += new FileSystemEventHandler(OnFileDeleted);
            //watcher.Renamed += new RenamedEventHandler(OnRenamed);

            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }

        private void OnFileCreated(object source, FileSystemEventArgs e)
        {

            CurrentState = _enabled;
            

            
        }

        private void OnFileChanged(object source, FileSystemEventArgs e)
        {

            CurrentState = _enabled;



        }

        private void OnFileDeleted(object source, FileSystemEventArgs e)
        {
            CurrentState = _disabled;
        }
        
        private FileSystemWatcher watcher;

        //void EventSetProperties(object obj, SelectionState sc)
        //{
        //    ZDFEntryItem item = obj as ZDFEntryItem;

        //    if (item != null) _zdfEntry = item.ZDFEntry;

        //}


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




    



    #region VSTO generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }

    class ZaveEnabled : IZaveAddInState
    {
        private IZaveLowLevelHook _llHook;
        private RichTextBox rtb;

        public ZaveEnabled(IZaveLowLevelHook llhook)
        {
            _llHook = llhook;
            rtb = new RichTextBox();

            try
            {


                _llHook.Init();
                _llHook.Start();
            }
            catch (Exception ex)
            {
                _llHook.Dispose();
                throw ex;
            }
        }
        public void Dispose()
        {
            _llHook.Dispose();
        }

        //TODO encapsulate WordAddIn's SelectionChanged method further
        public void SelectionChanged<T>(object sender, T e)
        {
            try
            {
                //WordTools.Document vstoDoc = Globals.Factory.GetVstoObject(Application.ActiveDocument);
                //_selStateList = new List<SelectionState>();
                var wordEvents = e as Microsoft.Office.Tools.Word.SelectionEventArgs;

                if (wordEvents.Selection.Text.Length >= 2)
                {
                    List<SelectionState> _selStateList = new List<SelectionState>();

                    rtb.Clear();
                    wordEvents.Selection.Copy();
                    rtb.Paste();

                    _selStateList.Add(new SelectionState()
                    {
                        SelectionDocName = wordEvents.Selection.Application.ActiveDocument.Name,
                        SelectionPage = wordEvents.Selection.Information[WordInterop.WdInformation.wdActiveEndAdjustedPageNumber].ToString(),
                        SelectionText = rtb.Rtf,
                        SelectionDateModified = DateTime.Now,
                        srcType = SrcType.WORD
                    });




                    string projFile = System.IO.Path.GetTempPath() + APIFileNames.SourceToZave;

                    WriteToJsonFileAsync(projFile, _selStateList);





                }

            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Unable to Write to Zave" + '\n' + ex.Message);
                Dispose();
            }
        }

        public void Start()
        {
            try
            {
                _llHook.Start();
            }
            catch
            {
                _llHook.Dispose();
            }
        }

        public void Stop()
        {
            _llHook.Stop();
        }

        private async Task WriteToJsonFileAsync(string filename, List<SelectionState> selStateList)
        {

            await Task.Run(() =>
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(selStateList.ToArray());

                //#if DEBUG
                //                    System.Windows.Forms.MessageBox.Show("After Serialization");
                //#endif
                //OnWordFired(selState);         

                //string projFile = System.IO.Path.GetTempPath() + "ZavePrototype";
                //System.Windows.Forms.MessageBox.Show(projDir);


                using (StreamWriter sw = StreamWriterFactory.createStreamWriter(filename))
                {
                    try
                    {
                        //sw.BaseStream.Seek(0, SeekOrigin.End);
                        sw.Write(json);

                        sw.Close();
                    }
                    catch (IOException ex)
                    {
                        throw ex;
                    }
                }
            });

        }

        
}

    class ZaveDisabled : IZaveAddInState
    {
        private IZaveLowLevelHook _llHook;

        public ZaveDisabled(IZaveLowLevelHook llhook)
        {
            _llHook = llhook;

            try
            {


                _llHook.Dispose();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("unable to dispose ZaveCursorHook!");
            }
        }
        public void Dispose()
        {
            _llHook.Dispose();
        }

        public void SelectionChanged<Args>(object sender, Args e)
        {
            return;
        }

        public void Start()
        {
            return;
        }

        public void Stop()
        {
            _llHook.Stop();
        }
    }


}
