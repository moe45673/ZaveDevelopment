using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Events;
using ZaveViewModel.ViewModels;
using ZaveModel.ZDFEntry;
using System.Collections.ObjectModel;
using ZaveGlobalSettings.Data_Structures;
using ZaveGlobalSettings.ZaveFile;
using ZaveModel.ZDF;
using System.ComponentModel;
using System.Windows.Data;
using Microsoft.Practices.Unity;
//using Novacode;
using System.Windows.Forms;
using ZaveGlobalSettings.Data_Structures.ZaveObservableCollection;
using System.Drawing;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZaveService.IOService;
using ZaveService.JSONService;
using System.Collections.Specialized;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
//using DevExpress.Utils.Drawing.Helpers;
using System.Windows.Media;
using System.Windows.Controls;
using System.Diagnostics;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Tools.Word;
using Word = Microsoft.Office.Tools.Word;
using Task = System.Threading.Tasks.Task;


//using GalaSoft.MvvmLight.CommandWpf;

namespace ZaveViewModel.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;
        private readonly IEventAggregator _eventAggregator;
        private string _filename;
        public static string SaveLocation = null;
        private WindowMode _winMode;
        private readonly IIOService _ioService;
        private readonly IJsonService _jsonService;
        public static List<ZdfUndoComments> ZdfUndoComments = new List<ZdfUndoComments>();
        public static List<ZdfUndoComments> RemoveZdundoComments = new List<ZdfUndoComments>();
        public static List<IZDFEntry> activeZdfUndo = new List<IZDFEntry>();

        private TaskCompletionSource<bool> _WindowModeChangeResult = null;

        #region Delegate Properties
        public DelegateCommand SwitchWindowModeCommand { get; set; }

        public DelegateCommand<WindowMode?> SwitchSpecificWindowModeCommand { get; set; }

        public DelegateCommand SaveZDFDelegateCommand { get; set; }
        public DelegateCommand OpenZDFDelegateCommand { get; set; }
        public DelegateCommand<string> OpenZDFFromFileDelegateCommand { get; set; }



        public DelegateCommand NewZDFDelegateCommand { get; set; }

        public DelegateCommand NewZDFEntryDelegateCommand { get; set; }

        public DelegateCommand UndoZDFDelegateCommand { get; set; }

        public DelegateCommand RedoZDFDelegateCommand { get; set; }

        public DelegateCommand ScreenshotZDFDelegateCommand { get; set; }

        public DelegateCommand<String> ExportZDFDelegateCommand { get; set; }

        public DelegateCommand SaveASZDFDelegateCommand { get; set; }

        #endregion

        //async Task<string> GetDefaultSaveDirectory()
        //{
        //    for(int i = 0; i<20; i++)
        //    {
        //        try
        //        {
        //            var mcvm = await Task<MainContainerViewModel>.Factory.StartNew(() =>
        //            {
        //                return _container.Resolve<MainContainerViewModel>() as MainContainerViewModel;
        //            });                   

        //            return mcvm.getSaveDirectory() + "ZDF_" + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString() + ".zdf"; ;


        //        }
        //        catch(NullReferenceException nre)
        //        {
        //            Thread.Sleep(100);
        //        }



        //    }

        //    return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //}

        public MainWindowViewModel(IRegionManager regionManager, IUnityContainer cont, IEventAggregator agg, IIOService ioservice, IJsonService jsonService)
        {

            if (cont == null) throw new ArgumentNullException("container");
            if (regionManager == null) throw new ArgumentNullException("regionManager");
            if (agg == null) throw new ArgumentNullException("eventAggregator");

            _container = cont;
            _regionManager = regionManager;
            SwitchWindowModeCommand = new DelegateCommand(SwitchWindowMode);
            SwitchSpecificWindowModeCommand = new DelegateCommand<WindowMode?>(SwitchWindowMode);
            //Dialogs.Add(new ModalInputDialogViewModel());
            cont.RegisterInstance(typeof(ObservableCollection<IDialogViewModel>), "DialogVMList", Dialogs);
            cont.RegisterInstance<MainWindowViewModel>(this);
            _eventAggregator = agg;
            _eventAggregator.GetEvent<ZDFSavedEvent>().Subscribe(setFileName);
            _eventAggregator.GetEvent<WindowModeChangedEvent>().Subscribe(SwitchWindowMode);
            //var getDirectory = GetDefaultSaveDirectory();
            SaveLocation = String.Empty;
            Filename = GuidGenerator.UNSAVEDFILENAME;
            _eventAggregator.GetEvent<ZDFOpenedEvent>().Subscribe(setFileName);
            _WindowModeChangeResult = new TaskCompletionSource<bool>();
            _ioService = ioservice;
            _jsonService = jsonService;
            SaveZDFDelegateCommand = DelegateCommand.FromAsyncHandler(SaveZDF);
            OpenZDFDelegateCommand = DelegateCommand.FromAsyncHandler(OpenZDF);
            OpenZDFFromFileDelegateCommand = DelegateCommand<string>.FromAsyncHandler(x => OpenZDF(x));
            NewZDFDelegateCommand = new DelegateCommand(NewZDF);
            NewZDFEntryDelegateCommand = new DelegateCommand(NewZDFEntry);
            UndoZDFDelegateCommand = new DelegateCommand(UndoZDF);
            RedoZDFDelegateCommand = new DelegateCommand(RedoZDF);
            ScreenshotZDFDelegateCommand = new DelegateCommand(ScreenshotZDF);
            //ExportZDFDelegateCommand = DelegateCommand<string>.FromAsyncHandler(x => ExportZDF(x));
            ExportZDFDelegateCommand = DelegateCommand<string>.FromAsyncHandler(x => ExportZDF(x));
            SaveASZDFDelegateCommand = new DelegateCommand(SaveAsZdfFile);
            SetWindowMode(WindowMode.WIDGET);



            //_eventAggregator.GetEvent<MainWindowInstantiatedEvent>().Publish(true);

        }

        private async Task OpenZDF(string filename)
        {
            ZDFSingleton activeZdf = _container.Resolve<ZDFSingleton>(InstanceNames.ActiveZDF);
            using (var sr = _ioService.OpenFileService(filename))
            {
                try
                {
                    using (JsonReader wr = new JsonTextReader(sr))
                    {
                        try
                        {
                            JObject jObject = JObject.Load(wr);


                            //activeZdf = JsonConvert.DeserializeObject<ZaveModel.ZDF.ZDFSingleton>(jObject.ToString());
                            JArray ja = (JArray)jObject["EntryList"]["_items"];

                            await Task.Run(() => activeZdf.EntryList = new ObservableImmutableList<IZDFEntry>(ja.ToObject<List<ZDFEntry>>()));
                            activeZdf = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<ZaveModel.ZDF.ZDFSingleton>(jObject.ToString()));

                            activeZdf = ZDFSingleton.GetInstance(_eventAggregator);


                            //activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance(eventAgg);
                            //foreach (var item in activeZDF.EntryList)
                            //{
                            //    activeZDF.Add(item);
                            //}



                            if (activeZdf.EntryList.Count > 0)
                            {
                                ObservableImmutableList<ZdfEntryItemViewModel> ZdfEntries = new ObservableImmutableList<ZdfEntryItemViewModel>();
                                ////activeZDF.EntryList.Clear();
                                ZaveModel.ZDF.ZDFSingleton activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance();
                                await Task.Run(() =>
                                {

                                    foreach (var item in activeZdf.EntryList)
                                    {
                                        //activeZdf.Add(item);

                                        ZdfEntries.Add(new ZdfEntryItemViewModel(item as ZDFEntry));
                                    }
                                });

                                ////ZdfEntries.FirstOrDefault().TxtDocName = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                                ////ZdfEntries.Select(w => w.TxtDocName == System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName));

                                ////List<SelectionState> selState = activeZDF.toSelectionStateList();

                                // = Path.GetFileName(filename);
                                SaveLocation = filename;
                                activeZdf.Name = filename;
                                _eventAggregator.GetEvent<ZDFOpenedEvent>().Publish(activeZdf.Name);

                            }



                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        finally
                        {
                            wr.Close();
                        }
                    }
                }

                catch (Exception ex)
                {
                }
                finally
                {
                    sr.Close();
                }
            }
        }

        [Conditional("DEBUG")]
        private void setIndented(JsonSerializer ser)
        {
            ser.Formatting = Newtonsoft.Json.Formatting.Indented;
        }

        private void setFileName(string activeZDF)
        {
            Filename = (String)activeZDF;//((ZaveModel.ZDF.ZDFSingleton)activeZDF).Name;
        }

        private void SetWindowMode(WindowMode setting)
        {
            WinMode = setting;
        }

        private void SwitchWindowMode()
        {


            int intMode = (int)WinMode;
            intMode++;
            var tempwinmode = ((WindowMode?)intMode);
            SwitchWindowMode(tempwinmode);

        }

        private void SwitchWindowMode(WindowMode? wm)
        {
            if (wm != null)
            {
                var assignable = ((WindowMode)wm);
                VerifyWindowMode(ref assignable);


                _eventAggregator.GetEvent<WindowModeChangeEvent>().Publish(assignable);

                Task.WhenAny(_WindowModeChangeResult.Task, Task.Delay(15000));
                if (_WindowModeChangeResult.Task.IsCompleted)
                {
                    WinMode = assignable;
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Terribly Sorry About That.", "Switch Window Operation Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void VerifyWindowMode(ref WindowMode wm)
        {
            if (!(Enum.IsDefined(typeof(WindowMode), wm)))
                wm = WindowMode.MAIN;


        }



        private void SwitchWindowMode(bool result)
        {
            _WindowModeChangeResult.TrySetResult(result);
        }

        #region Properties
        private ObservableCollection<IDialogViewModel> _dialogs = new ObservableCollection<IDialogViewModel>();
        public ObservableCollection<IDialogViewModel> Dialogs { get { return _dialogs; } }
        public String Filename
        {
            get
            {
                return _filename;
            }
            set
            {
                SaveLocation = value;
                var name = Path.GetFileName(SaveLocation);
                SetProperty(ref _filename, name);
                _eventAggregator.GetEvent<FilenameChangedEvent>().Publish(_filename);

                //SetProperty(ref _filename, name);
            }
        }


        public WindowMode WinMode
        {
            get { return _winMode; }
            set { SetProperty(ref _winMode, value); }
        }
        #endregion

        #region Delegate Implementation

        //private async Task ExportZDF(string source)
        private async Task ExportZDF(string source)
        {

            var activeZDF = ZDFSingleton.GetInstance();
            var entries = activeZDF.EntryList.ToList();

            //TODO make the sorting method up to the User when exporting a doc
            var comp = new ColorComparer();
            entries.Sort(comp);

            IZDFEntry lastEntry = default(ZDFEntry);
            var WordApp = new Microsoft.Office.Interop.Word.Application();
            if (WordApp.Documents.Count == 0)
            {
                WordApp.Visible = false;
            }
            //WordApp.Activate();

            switch (source)
            {

                case "WORD":
                    try
                    {
                        IOService.DeleteFile(Path.GetTempPath() + APIFileNames.ZaveToSource);
                        await Task.Factory.StartNew(() =>
                        {
                            var exportfilename = createExportFileName("docx");
                            try
                            {



                                //TODO Better way to delete the file?
                                if (File.Exists(exportfilename))
                                {
                                    for (int i = 0; i < 20; i++)
                                    {
                                        try
                                        {
                                            File.Delete(exportfilename);
                                            System.Threading.Thread.Sleep(50);
                                            break;
                                        }
                                        catch (IOException ioex)
                                        {
                                            Console.WriteLine(ioex.Message);
                                        }
                                    }

                                }
                                //try
                                //{
                                //    using (DocX newDoc = DocX.Create(exportfilename))
                                //        newDoc.Save();
                                //}
                                //catch (IOException ioex)
                                //{

                                //    throw ioex;
                                //}

                                object exportfilenameObject = exportfilename;
                                var wordDoc = WordApp.Documents.Add();

                                //wordDoc.Activate();
                                wordDoc.SaveAs2(ref exportfilenameObject);
                                wordDoc.Activate();


                                foreach (var entry in entries)

                                {
                                    object start = wordDoc.Content.Start;
                                    object end = wordDoc.Content.End - 1;
                                    object end2 = wordDoc.Content.End;
                                    var rng = wordDoc.Range(ref end, ref end2);
                                    string bmName = "ZDFEntry" + entry.ID.ToString();
                                    var para = wordDoc.Content.Paragraphs.Add();

                                    //using (DocX doc = DocX.Load(exportfilename))
                                    //{

                                    //var thisColor = entry.HColor.Color;

                                    if (entry.Equals(entries.ElementAt(0)) || (lastEntry != null && (!(comp.Compare(entry, lastEntry) == 0))))
                                    {
                                        addColorHeadingToWordDoc(wordDoc, entry, para.Range.Start as object, para.Range.End as object, bmName + "_color");
                                    }

                                    //else if ()
                                    //    {
                                    //        addColorHeadingToWordDoc(wordDoc, entry, para.Range.Start as object, para.Range.End as object, bmName + "_color");




                                    //}

                                    //var p = doc.InsertBookmark(bmName);
                                    //doc.Save();
                                    //}


                                    //WordApp.Activate();
                                    //var bm = wordDoc.Bookmarks.Add(bmName);

                                    //var bmList = wordDoc.Bookmarks as List<Microsoft.Office.Interop.Word.Bookmark>;

                                    // = bmList.Find(x => x.Name == bmName);

                                    //var rng = bm.Range;
                                    System.Windows.Forms.RichTextBox rb = new System.Windows.Forms.RichTextBox();
                                    //var toolDoc = wordDoc as Word.Document;

                                    wordDoc.Content.SetRange(0, 0);


                                    //para.Range.FormattedText.Paste();

                                    //var rtfControl = toolDoc.Controls.AddRichTextContentControl(bmName + "rtf" + entry.ID.ToString());
                                    rb.Rtf = entry.Text;
                                    //cc.BuildingBlockType = WdBuildingBlockTypes.wdTypeAutoText;
                                    //cc.set_DefaultTextStyle()
                                    if (rb.Rtf.Last() == Environment.NewLine.ToCharArray().First())
                                    {
                                        rb.Rtf.Remove(rb.Rtf.Count() - 1);
                                    }
                                    System.Windows.Forms.Clipboard.SetText(rb.Rtf, System.Windows.Forms.TextDataFormat.Rtf);

                                    para.Range.FormattedText.Paste();

                                    para.Range.Text += Environment.NewLine;

                                    //rtfControl.Text = rb.Rtf;
                                    //
                                    //rng.Paste();

                                    //if ((entries.IndexOf(entry) == (entries.Count - 1)))
                                    //{
                                    //    addColorHeadingToWordDoc(wordDoc, entry, end, end2, bmName + "_color");
                                    //}

                                    //wordDoc.Save();




                                    //var p = doc.InsertParagraph();
                                    //var rtb = new System.Windows.Forms.RichTextBox();                                    
                                    //rtb.Rtf = entry.Text;

                                    //p.




                                    lastEntry = entry;
                                }
                                wordDoc.Close(WdSaveOptions.wdSaveChanges);

                                if (wordDoc != null)
                                {
                                    Marshal.ReleaseComObject(wordDoc);
                                }
                                wordDoc = null;

                                _eventAggregator.GetEvent<ZDFExportedEvent>().Publish(exportfilename);
                            }




                            catch (IOException ioex)
                            {
                                System.Windows.Forms.MessageBox.Show("Unable to create export file. If file is currently open, please close and try again.", "FILE NOT EXPORTED", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            }

                            finally
                            {

                                IOService.CreateFileAsync(Path.GetTempPath() + APIFileNames.ZaveToSource);
                                //WordApp.Documents.Close(WdSaveOptions.wdPromptToSaveChanges);
                                if (WordApp.Visible.Equals(false))
                                {
                                    WordApp.Quit();

                                    //if (WordApp.Documents != null)
                                    //{
                                    //    Marshal.ReleaseComObject(WordApp.Documents);
                                    //}
                                    if (WordApp != null)
                                    {
                                        Marshal.ReleaseComObject(WordApp);
                                    }



                                    WordApp = null;

                                }
                                GC.Collect();
                            }

                        }
                        , CancellationToken.None
                        , TaskCreationOptions.None
                        , TaskScheduler.FromCurrentSynchronizationContext()
                        );

                        //t.Wait();
                    }
                    catch (AggregateException aggex)
                    {
                        string ExceptionMessage = "";
                        foreach (var ex in aggex.InnerExceptions)
                        {
                            ExceptionMessage += ex.Message + Environment.NewLine;
                        }
                        System.Windows.Forms.MessageBox.Show(ExceptionMessage);
                    }

                    break;
            }


        }

        private void addColorHeadingToWordDoc(Microsoft.Office.Interop.Word.Document wordDoc, IZDFEntry entry, object rngStart, object rngEnd, string bmName)
        {
            var colorRng = wordDoc.Range(ref rngStart, ref rngEnd);
            //var colorRng = colorBM.Range;
            //colorRng.InsertAlignmentTab((int)WdAlignmentTabAlignment.wdCenter);
            Table tab = wordDoc.Tables.Add(colorRng, 2, 3);

            foreach (Row row in tab.Rows)
            {

                foreach (Cell c in row.Cells)
                {
                    if (c.ColumnIndex == 1 && c.RowIndex == 1)
                    {
                        c.Range.Shading.BackgroundPatternColor = ConvertColortoWdColor(entry.HColor.Color);
                        c.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                        c.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    }
                }

            }

        }



        /// <summary>
        /// Creates a filename for the ZDF export
        /// </summary>
        /// <param name="extension">extension of the desired format to export to</param>
        /// <returns>exportfilename as a string</returns>
        private string createExportFileName(String extension = "")
        {
            var activeZDF = ZDFSingleton.GetInstance();
            var str1 = getSaveDirectory() + "\\ExportedFiles";
            if (!Directory.Exists(str1))
            {
                Directory.CreateDirectory(str1);
            }

            var exportFileName = "Export";

            if (!extension.Equals(""))
            {
                exportFileName += "." + extension;
            }

            var str2 = Path.Combine(str1, Path.GetFileNameWithoutExtension(activeZDF.Name) + exportFileName);



            return str2;
        }

        private void UndoZDF()
        {

            #region MyCode2
            ZDFSingleton activeZdf = ZDFSingleton.GetInstance();

            ObservableImmutableList<ZdfEntryItemViewModel> ZdfEntries = new ObservableImmutableList<ZdfEntryItemViewModel>();
            var zdfEntryList = new List<ZDFEntry>();
            if (activeZdf.EntryList.Count > 0)
            {
                int id = activeZdf.EntryList.LastOrDefault().ID;
                var withoutfilter = activeZdf.EntryList.Where(t => t.ID == id).ToList();

                foreach (var undoitem in withoutfilter)
                {
                    if (((ZDFEntry)undoitem).Comments != null && ((ZDFEntry)undoitem).Comments.Count > 0)
                    {
                        ZdfEntries.Add(new ZdfEntryItemViewModel(undoitem as ZDFEntry));
                        var commentID = ((ZDFEntry)undoitem).Comments.LastOrDefault().CommentID;
                        var commentObject = ((ZDFEntry)undoitem).Comments.FirstOrDefault(a => a.CommentID == commentID);
                        ((ZDFEntry)undoitem).Comments.Remove(commentObject);
                        zdfEntryList.Add(((ZDFEntry)undoitem));
                        if (RemoveZdundoComments.Any(a => a.ID == id && a.Comments.CommentID == commentID))
                        {
                            RemoveZdundoComments.Remove(RemoveZdundoComments.FirstOrDefault(a => a.ID == id && a.Comments.CommentID == commentID));
                        }
                        if (!ZdfUndoComments.Any(a => a.Comments.CommentID == commentID && a.ID == id))
                        {
                            ZdfUndoComments.Add(new ZdfUndoComments
                            {
                                Comments = commentObject,
                                ID = id
                            });
                        }
                    }
                    else
                    {
                        activeZdfUndo.Add(undoitem);
                        ZdfEntries.Add(new ZdfEntryItemViewModel(undoitem as ZDFEntry));
                    }


                }

                var filter = activeZdf.EntryList.Where(t => t.ID != id).ToList();

                activeZdf.EntryList.Clear();

                foreach (var item in zdfEntryList)
                {
                    activeZdf.Add(item);
                }
                foreach (var item in filter.ToList())
                {
                    activeZdf.Add(item);
                    ZdfEntries.Add(new ZdfEntryItemViewModel(item as ZDFEntry));
                }
            }
            #endregion
        }

        private void RedoZDF()
        {
            #region MyCode2
            ZDFSingleton activeZdf = ZDFSingleton.GetInstance();
            ObservableImmutableList<ZdfEntryItemViewModel> ZdfEntries = new ObservableImmutableList<ZdfEntryItemViewModel>();

            //if (activeZdfUndo.Count > 0 && activeZdfUndo.Count != 1)
            //if (activeZdfUndo.Count > 0)
            //{
            //    activeZdf.EntryList.Add(activeZdfUndo.LastOrDefault());
            //    ZdfEntries.Add(new ZdfEntryItemViewModel(activeZdfUndo.LastOrDefault() as ZDFEntry));
            //    activeZdfUndo.RemoveAt(activeZdfUndo.Count - 1);
            //    //return;
            //}
            if (ZdfUndoComments != null && ZdfUndoComments.Count > 0)
            {
                if (activeZdf.EntryList.Count > 0)
                {
                    var tempObject = new ZdfUndoComments();
                    if (RemoveZdundoComments == null || !RemoveZdundoComments.Any())
                    {
                        tempObject = ZdfUndoComments.LastOrDefault();
                    }
                    else
                    {
                        tempObject = ZdfUndoComments.Where(a => !RemoveZdundoComments.Any(b => b.Comments.CommentID == a.Comments.CommentID && b.ID == a.ID)).LastOrDefault();
                    }
                    if (tempObject != null)
                    {
                        var zdfEntry = activeZdf.EntryList.FirstOrDefault(a => a.ID == tempObject.ID);
                        if (zdfEntry != null)
                        {
                            if (!zdfEntry.Comments.Any(a => a.CommentID == tempObject.Comments.CommentID))
                            {
                                zdfEntry.Comments.Add(tempObject.Comments);
                                RemoveZdundoComments.Add(tempObject);
                                if (ZdfUndoComments.Any(a => a.ID == zdfEntry.ID && a.Comments.CommentID == tempObject.Comments.CommentID))
                                {
                                    ZdfUndoComments.Remove(ZdfUndoComments.FirstOrDefault(a => a.ID == zdfEntry.ID && a.Comments.CommentID == tempObject.Comments.CommentID));
                                }
                                //ZdfUndoComments.Remove(item);
                            }
                        }
                    }
                }
                else
                {
                    activeZdf.EntryList.Add(activeZdfUndo.LastOrDefault());
                    ZdfEntries.Add(new ZdfEntryItemViewModel(activeZdfUndo.LastOrDefault() as ZDFEntry));
                    activeZdfUndo.RemoveAt(activeZdfUndo.Count - 1);
                    //return;
                }
                //ZdfUndoComments = null;
            }
            else
            {
                if (activeZdfUndo.Count > 0)
                {
                    activeZdf.EntryList.Add(activeZdfUndo.LastOrDefault());
                    ZdfEntries.Add(new ZdfEntryItemViewModel(activeZdfUndo.LastOrDefault() as ZDFEntry));
                    activeZdfUndo.RemoveAt(activeZdfUndo.Count - 1);
                    //return;
                }
            }
            //if(removeZdundoComments!=null && removeZdundoComments.Any())
            //{
            //    foreach (var item in removeZdundoComments)
            //    {
            //        ZdfUndoComments.Remove(item);
            //    }
            //}
            //else if (activeZdfUndo.Count == 1)
            //{
            //    if (activeZdf.EntryList.Count > 0)
            //    {
            //        activeZdf.EntryList.Add(activeZdf.EntryList.LastOrDefault());
            //        ZdfEntries.Add(new ZdfEntryItemViewModel(activeZdf.EntryList.LastOrDefault() as ZDFEntry));
            //    }
            //    else{
            //        activeZdf.EntryList.Add(activeZdfUndo.LastOrDefault());
            //        ZdfEntries.Add(new ZdfEntryItemViewModel(activeZdfUndo.LastOrDefault() as ZDFEntry));
            //    }

            //}
            //else if (activeZdfUndo.Count == 0)
            //{
            //    if (activeZdf.EntryList.Count > 0)
            //    {
            //        activeZdf.EntryList.Add(activeZdf.EntryList.LastOrDefault());
            //        ZdfEntries.Add(new ZdfEntryItemViewModel(activeZdf.EntryList.LastOrDefault() as ZDFEntry));
            //    }
            //}

            #endregion
        }

        private void ScreenshotZDF()
        {
            double screenLeft = SystemParameters.VirtualScreenLeft;
            double screenTop = SystemParameters.VirtualScreenTop;
            double screenWidth = SystemParameters.VirtualScreenWidth;
            double screenHeight = SystemParameters.VirtualScreenHeight;

            //double x =0, y=0, width=0, height=0;
            Bitmap sb;
            sb = new System.Drawing.Bitmap((int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(sb))
            {

                g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, sb.Size);
            }

            //IntPtr handle = IntPtr.Zero;
            try
            {
                //    handle = sb.GetHbitmap();

                //SaveFileDialog dlg = new SaveFileDialog();
                //dlg.DefaultExt = "png";
                //dlg.Filter = "Png Files|*.png";
                //DialogResult res = dlg.ShowDialog();
                //if (res == System.Windows.Forms.DialogResult.OK)
                ////sb.Save(dlg.FileName, ImageFormat.Png);
                //{

                var image = NativeMethods.CaptureActiveWindow();
                image.Save("F:\\Zave Backup\\a.png", ImageFormat.Png);
                //}
                //String filename1 = "ScreenCapture-" + DateTime.Now.ToString("ddMMyyyy-hhmmss") + ".png";
                //sb.Save("F:\\Zave Backup\\" + filename1);

                //using (Bitmap bmp = new Bitmap((int)screenWidth,
                //    (int)screenHeight))
                //{
                //    using (Graphics g = Graphics.FromImage(bmp))
                //    {
                //        String filename = "ScreenCapture-" + DateTime.Now.ToString("ddMMyyyy-hhmmss") + ".png";

                //        g.CopyFromScreen((int)screenLeft, (int)screenTop, 0, 0, bmp.Size);
                //        bmp.Save("C:\\Screenshots\\" + filename);

                //    }

                //}
            }
            catch { }
            #region MyCode2
            //int ix, iy, iw, ih;
            //ix = Convert.ToInt32(x);
            //iy = Convert.ToInt32(y);
            //iw = Convert.ToInt32(width);
            //ih = Convert.ToInt32(height);
            //Bitmap image = new Bitmap(iw, ih,
            //       System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            //Graphics g = Graphics.FromImage(image);
            //g.CopyFromScreen(ix, iy, ix, iy,
            //         new System.Drawing.Size(iw, ih),
            //         CopyPixelOperation.SourceCopy);
            //SaveFileDialog dlg = new SaveFileDialog();
            //dlg.DefaultExt = "png";
            //dlg.Filter = "Png Files|*.png";
            //DialogResult res = dlg.ShowDialog();
            //if (res == System.Windows.Forms.DialogResult.OK)
            //    image.Save(dlg.FileName, ImageFormat.Png);
            #endregion
        }

        //public void SaveScreen(double x, double y, double width, double height)
        //{
        //    int ix, iy, iw, ih;
        //    ix = Convert.ToInt32(x);
        //    iy = Convert.ToInt32(y);
        //    iw = Convert.ToInt32(width);
        //    ih = Convert.ToInt32(height);
        //    try
        //    {
        //        Bitmap myImage = new Bitmap(iw, ih);

        //        Graphics gr1 = Graphics.FromImage(myImage);
        //        IntPtr dc1 = gr1.GetHdc();
        //        IntPtr dc2 = NativeMethods.GetWindowDC(NativeMethods.GetForegroundWindow());
        //        NativeMethods.BitBlt(dc1, ix, iy, iw, ih, dc2, ix, iy, 13369376);
        //        gr1.ReleaseHdc(dc1);
        //        SaveFileDialog dlg = new SaveFileDialog();
        //        dlg.DefaultExt = "png";
        //        dlg.Filter = "Png Files|*.png";
        //        DialogResult res = dlg.ShowDialog();
        //        if (res == System.Windows.Forms.DialogResult.OK)
        //            myImage.Save(dlg.FileName, ImageFormat.Png);
        //    }
        //    catch { }
        //}
        //public void CaptureScreen(double x, double y, double width, double height)
        //{
        //    int ix, iy, iw, ih;
        //    ix = Convert.ToInt32(x);
        //    iy = Convert.ToInt32(y);
        //    iw = Convert.ToInt32(width);
        //    ih = Convert.ToInt32(height);
        //    Bitmap image = new Bitmap(iw, ih, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        //    Graphics g = Graphics.FromImage(image);
        //    g.CopyFromScreen(ix, iy, ix, iy, new System.Drawing.Size(iw, ih), CopyPixelOperation.SourceCopy);
        //    SaveFileDialog dlg = new SaveFileDialog();
        //    dlg.DefaultExt = "png";
        //    dlg.Filter = "Png Files|*.png";
        //    DialogResult res = dlg.ShowDialog();
        //    if (res == System.Windows.Forms.DialogResult.OK)
        //        image.Save(dlg.FileName, ImageFormat.Png);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task SaveZDF()
        {
            if (SaveLocation == null || SaveLocation == String.Empty || SaveLocation == GuidGenerator.UNSAVEDFILENAME)
            {
                var filename = _ioService.SaveFileDialogService(getSaveDirectory());
                await SaveLogic(Convert.ToString(filename));
            }
            else
            {
                var filename = SaveLocation;
                await SaveLogic(Convert.ToString(filename));
            }
        }

        #region Save Common Logic
        private async Task SaveLogic(string filename)
        {
            var activeZDFVM = _container.Resolve(typeof(ZDFViewModel), "ZDFView") as ZDFViewModel;

            JsonSerializer serializer = new JsonSerializer();

            if (filename != String.Empty)
            {

                using (var sw = _ioService.SaveFileService(filename))
                {
                    try
                    {
                        using (JsonWriter wr = new JsonTextWriter(sw))
                        {
                            try
                            {

                                setIndented(serializer);
                                await Task.Factory.StartNew(() => serializer.Serialize(wr, activeZDFVM.GetModel()));
                                SaveLocation = filename;
                                var activeZDF = ZDFSingleton.GetInstance();
                                activeZDF.Name = filename;
                                _eventAggregator.GetEvent<ZDFSavedEvent>().Publish(activeZDF.Name);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            finally
                            {
                                wr.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("Save File Error!");
                    }
                    finally
                    {
                        sw.Close();
                    }
                }
            }
        }
        #endregion

        #region New OPen
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task OpenZDF()
        {
            //var activeZDFVM = _container.Resolve(typeof(ZDFViewModel), "ZDFView") as ZDFViewModel;

            //JsonSerializer serializer = new JsonSerializer();
            ZDFSingleton activeZdf = ZDFSingleton.GetInstance();
            var filename = _ioService.OpenFileDialogService(getSaveDirectory());


            if (filename != String.Empty) //If the user did not press cancel
            {

                await OpenZDF(filename);

            }

        }
        #endregion

        #region Old Open
        //private void OpenZDF()
        //{
        //    //var activeZDFVM = _container.Resolve(typeof(ZDFViewModel), "ZDFView") as ZDFViewModel;

        //    JsonSerializer serializer = new JsonSerializer();
        //    ZDFSingleton activeZdf = ZDFSingleton.GetInstance();
        //    var filename = _ioService.OpenFileDialogService(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        //    if (filename != null)
        //    {
        //        using (var sr = _ioService.OpenFileService(filename))
        //        {
        //            try
        //            {
        //                using (JsonReader wr = new JsonTextReader(sr))
        //                {
        //                    try
        //                    {
        //                        JObject jObject = JObject.Load(wr);
        //                        //var output = "";
        //                        //foreach(JProperty prop in jObject.Properties())
        //                        //{
        //                        //    output += "PROPERTY 1 EQUALS " + prop.Name + "-" + prop.Value + '\r' + '\n';
        //                        //}

        //                        //System.Windows.Forms.MessageBox.Show(output);

        //                        //ZaveModel.ZDF.ZDFSingleton activeZDF = ZaveModel.ZDF.ZDFSingleton.Instance;

        //                        //activeZdf = JsonConvert.DeserializeObject<ZaveModel.ZDF.ZDFSingleton>(jObject.ToString());
        //                        activeZdf = JsonConvert.DeserializeObject<ZaveModel.ZDF.ZDFSingleton>(jObject.ToString());
        //                        activeZdf = ZDFSingleton.GetInstance(_eventAggregator);
        //                        JArray ja = (JArray)jObject["EntryList"]["_items"];

        //                        activeZdf.EntryList = new ObservableImmutableList<IZDFEntry>(ja.ToObject<List<ZDFEntry>>());

        //                        if (activeZdf.EntryList.Count > 0)
        //                        {
        //                            activeZdf.EntryList.FirstOrDefault().Name = Path.GetFileName(filename);
        //                            _eventAggregator.GetEvent<ZDFOpenedEvent>().Publish(activeZdf);
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        throw ex;
        //                    }
        //                    finally
        //                    {
        //                        wr.Close();
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //            finally
        //            {
        //                sr.Close();
        //            }
        //        }
        //    }
        //}
        #endregion



        private WdColor ConvertColortoWdColor(System.Drawing.Color c)
        {
            return (WdColor)(c.R + 0x100 * c.G + 0x10000 * c.B);
        }


        private void NewZDFEntry()
        {
            #region checkin
            ObservableImmutableList<ZdfEntryItemViewModel> ZdfEntries = new ObservableImmutableList<ZdfEntryItemViewModel>();
            ZaveModel.ZDFEntry.ZDFEntry entry = new ZaveModel.ZDFEntry.ZDFEntry();
            ZDFSingleton activeZDF = ZDFSingleton.GetInstance();
            activeZDF.Add(new ZDFEntry());
            ZdfEntries.Add(new ZdfEntryItemViewModel(entry as ZDFEntry));
            #endregion

        }


        private void NewZDF()
        {
            ZDFSingleton activeZDF = ZDFSingleton.GetInstance();
            activeZDF.EntryList.Clear();
            MainContainerViewModel.activeZdfUndo.Clear();
            SaveLocation = null;
            Filename = GuidGenerator.UNSAVEDFILENAME;
            new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            _eventAggregator.GetEvent<NewZDFCreatedEvent>().Publish(activeZDF.ID.ToString());

        }

        public static string getSaveDirectory()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ZDFs";
        }

        private string getSaveFileName()
        {
            return @"\SaveDoc";
        }

        void SaveAsZdfFile()
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.DefaultExt = ".ZDF"; // Default file extension
            saveFileDialog.Filter = "ZDF documents (.ZDF)|*.ZDF"; // Filter files by extension
            saveFileDialog.FileName = "";

            if (saveFileDialog.ShowDialog() == true)
            {
                ZaveModel.ZDF.ZDFSingleton activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance();
                SaveLogic(Convert.ToString(saveFileDialog.FileName));
            }
        }
    }

    internal class NativeMethods
    {

        [DllImport("user32.dll")]
        public extern static IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hwnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("gdi32.dll")]
        public static extern UInt64 BitBlt(IntPtr hDestDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, System.Int32 dwRop);
        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);
        public static Bitmap CaptureActiveWindow()
        {
            return CaptureWindow(GetForegroundWindow());
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        public static Bitmap CaptureWindow(IntPtr handle)
        {
            var rect = new Rect();
            GetWindowRect(handle, ref rect);
            var bounds = new System.Drawing.Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            var result = new Bitmap(bounds.Width, bounds.Height);

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CopyFromScreen(new System.Drawing.Point(bounds.Left, bounds.Top), System.Drawing.Point.Empty, bounds.Size);
            }

            return result;
        }
    }
    public class ZdfUndoComments
    {
        public int ID { get; set; }
        public IEntryComment Comments { get; set; }
    }

}
#endregion





