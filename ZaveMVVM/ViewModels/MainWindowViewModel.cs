﻿using System;
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
using Prism.Interactivity.InteractionRequest;
using Word = Microsoft.Office.Tools.Word;
using Task = System.Threading.Tasks.Task;
using ZaveGlobalSettings.Data_Structures.CustomAttributes;


//using GalaSoft.MvvmLight.CommandWpf;

namespace ZaveViewModel.ViewModels
{
    /// <summary>
    /// ViewModel for the Shell
    /// </summary>
    /// <remarks>
    /// Many commands and their implementations are ZDF-centric and not 
    /// Application-centric and should be moved to the ZDFAppContainerViewModel
    /// </remarks>
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;
        private readonly IEventAggregator _eventAggregator;
        private string _filename;
        public string SaveLocation;
        private WindowMode _winMode;
        private readonly IIOService _ioService;
        private readonly IJsonService _jsonService;
        public static List<ZdfUndoComments> ZdfUndoComments = new List<ZdfUndoComments>();
        public static List<ZdfUndoComments> RemoveZdundoComments = new List<ZdfUndoComments>();
        public static List<IZDFEntry> activeZdfUndo = new List<IZDFEntry>();
        private readonly TaskScheduler currsync;

        private TaskCompletionSource<bool> _WindowModeChangeResult = null;
        public InteractionRequest<IConfirmation> ConfirmationRequest { get; private set; }

        #region Delegate Properties

        /// <summary>
        /// Command to be called when switching Window Mode using Default switch method(eg fullsize to Widget)
        /// </summary>
        public DelegateCommand SwitchWindowModeCommand { get; set; }

        /// <summary>
        /// Command to be called when deleting a ZDFEntry
        /// </summary>
        /// <remarks>Should be moved to ZDFAppContainerViewModel</remarks>
        public DelegateCommand<string> DeleteZDFEntryCommand { get; set; }

        /// <summary>
        /// Command to be called when switching to a specific Window Mode
        /// </summary>
        /// <remarks>Should be moved to ZDFAppContainerViewModel</remarks>
        public DelegateCommand<WindowMode?> SwitchSpecificWindowModeCommand { get; set; }

        /// <summary>
        /// Command to be called when saving a ZDF
        /// </summary>
        public DelegateCommand SaveZDFDelegateCommand { get; set; }
        /// <summary>
        /// Command to be called when opening a previously saved ZDF
        /// </summary>
        /// <remarks>Identical to OpenZDFDelegateCommand</remarks>
        public DelegateCommand OpenZDFDelegateCommand { get; set; }

        /// <summary>
        /// Command to be called when opening a previously saved ZDF
        /// </summary>
        /// <remarks>Identical to OpenZDFDelegateCommand</remarks>
        public DelegateCommand<string> OpenZDFFromFileDelegateCommand { get; set; }


        /// <summary>
        /// Command to be called when opening a new ZDF Document
        /// </summary>
        public DelegateCommand NewZDFDelegateCommand { get; set; }

        /// <summary>
        /// Command to be called when Adding a New ZDFEntry to the list
        /// </summary>
        public DelegateCommand NewZDFEntryDelegateCommand { get; set; }

        /// <summary>
        /// Command to be called when "Undo" is clicked or "Ctrl-Z" is pressed
        /// </summary>
        /// <remarks>Should be moved to ZDFAppContainerViewModel</remarks>
        public DelegateCommand UndoZDFDelegateCommand { get; set; }
        /// <summary>
        /// Command to be called when "Redo" is clicked or "Ctrl-Y" is pressed
        /// </summary>
        public DelegateCommand RedoZDFDelegateCommand { get; set; }
        /// <summary>
        /// Command to be called when the "Screenshot" button is clicked
        /// </summary>
        public DelegateCommand ScreenshotZDFDelegateCommand { get; set; }
        /// <summary>
        /// Command to be called when the Export function is chosen
        /// </summary>
        /// <remarks>Should be moved to ZDFAppContainerViewModel</remarks>
        public DelegateCommand<String> ExportZDFDelegateCommand { get; set; }
        /// <summary>
        /// Command to be called when the "Save As..." option is chosen
        /// </summary>
        public DelegateCommand SaveASZDFDelegateCommand { get; set; }

        /// <summary>
        /// Command to be called when a ZDF is being unloaded with unsaved changes
        /// </summary> 
        /// <remarks>Should be moved to ZDFAppContainerViewModel</remarks>
        public DelegateCommand ConfirmUnsavedChangesCommand { get; set; }

        #endregion

    

        public MainWindowViewModel(IRegionManager regionManager, IUnityContainer cont, IEventAggregator agg, IIOService ioservice, IJsonService jsonService)
        {

            if (cont == null) throw new ArgumentNullException("container");
            if (regionManager == null) throw new ArgumentNullException("regionManager");
            if (agg == null) throw new ArgumentNullException("eventAggregator");
            if (ioservice == null) throw new ArgumentNullException("ioService");
            if (jsonService == null) throw new ArgumentNullException("jsonService");

            _container = cont;
            _regionManager = regionManager;
            _eventAggregator = agg;
            _ioService = ioservice;
            _jsonService = jsonService;


            SwitchWindowModeCommand = new DelegateCommand(SwitchWindowMode);
            SwitchSpecificWindowModeCommand = new DelegateCommand<WindowMode?>(SwitchWindowMode);
            //Dialogs.Add(new ModalInputDialogViewModel());
            cont.RegisterInstance(typeof(ObservableCollection<IDialogViewModel>), "DialogVMList", Dialogs);
            //cont.RegisterInstance<MainWindowViewModel>(this);

            _eventAggregator.GetEvent<ZDFSavedEvent>().Subscribe(setFileName);
            _eventAggregator.GetEvent<WindowModeChangedEvent>().Subscribe(SwitchWindowMode);
            //var getDirectory = GetDefaultSaveDirectory();

            //if (SaveLocation == null)
            SaveLocation = String.Empty;

            Filename = GuidGenerator.UNSAVEDFILENAME;
            _eventAggregator.GetEvent<ZDFOpenedEvent>().Subscribe(setFileName);
            _WindowModeChangeResult = new TaskCompletionSource<bool>();

            SaveZDFDelegateCommand = new DelegateCommand(SaveZDFAsync);
            OpenZDFDelegateCommand = new DelegateCommand(OpenZDFAsync);
            OpenZDFFromFileDelegateCommand = new DelegateCommand<string>(OpenZDFAsync);
            NewZDFDelegateCommand = new DelegateCommand(NewZDF);
            NewZDFEntryDelegateCommand = new DelegateCommand(NewZDFEntry);
            UndoZDFDelegateCommand = new DelegateCommand(UndoZDF);
            ConfirmUnsavedChangesCommand = new DelegateCommand(ConfirmUnsavedChanges);
            RedoZDFDelegateCommand = new DelegateCommand(RedoZDF);
            ScreenshotZDFDelegateCommand = new DelegateCommand(ScreenshotZDF);
            //ExportZDFDelegateCommand = DelegateCommand<string>.FromAsyncHandler(x => ExportZDF(x));
            ExportZDFDelegateCommand = new DelegateCommand<string>(ExportZDF);
            SaveASZDFDelegateCommand = new DelegateCommand(SaveAsZdfFile);
            ConfirmationRequest = new InteractionRequest<IConfirmation>();
            DeleteZDFEntryCommand = new DelegateCommand<string>(DeleteZDFEntry);
            //var startupWinMode = new Nullable<WindowMode>(GetStartupWinMode());
            //SwitchWindowMode(startupWinMode);
            WinMode = GetStartupWinMode();
            _snapToCorner = true;
            currsync = TaskScheduler.FromCurrentSynchronizationContext();





        }

        private void DeleteZDFEntry(string entryIdAsStr)
        {
            IZDF activeZDF = ZDFSingleton.GetInstance();
            ConfirmationRequest.Raise(ZaveGlobalSettings.Data_Structures.ZaveMessageBoxes.ConfirmDeleteEntryCommand,
                        c =>
                        {

                            if (c.Confirmed)
                            {
                                int entryId = int.Parse(entryIdAsStr);
                                var entry = activeZDF.EntryList.First(x => { return entryId == x.ID; });
                                if (activeZDF.EntryList.Remove(entry))
                                {
                                    _eventAggregator.GetEvent<EntryDeletedEvent>().Publish(entry);
                                }
                                else
                                {
                                    throw new ZaveOperationFailedException("Unable to Delete the Desired Entry");
                                }
                            }



                        }
                    );

            
            
        }
        //TODO Create method to make it user's choice as to how Zave starts up
        private WindowMode GetStartupWinMode()
        {
            //TODO Make this configurable by the user settings
            return WindowMode.WIDGET;
        }


        private void OpenZDFAsync(string filename)
        {
            ZDFSingleton activeZdf = _container.Resolve<ZDFSingleton>(InstanceNames.ActiveZDF);
            OpenZDF(filename, activeZdf);
            
        }

        private async Task OpenZDF(string filename, ZDFSingleton activeZdf)
        {
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
                            activeZdf = await Task.Run(() => JsonConvert.DeserializeObject<ZaveModel.ZDF.ZDFSingleton>(jObject.ToString()));

                            activeZdf = ZDFSingleton.GetInstance(_eventAggregator);


                            //activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance(eventAgg);
                            //foreach (var item in activeZDF.EntryList)
                            //{
                            //    activeZDF.Add(item);
                            //}



                            if (activeZdf.EntryList.Count > 0)
                            {
                                ObservableImmutableList<IZDFEntry> ZdfEntries = new ObservableImmutableList<IZDFEntry>();
                                ////activeZDF.EntryList.Clear();
                                ZaveModel.ZDF.ZDFSingleton activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance();
                                await Task.Run(() =>
                                {

                                    foreach (var item in activeZdf.EntryList)
                                    {
                                        //activeZdf.Add(item);

                                        ZdfEntries.Add(item as IZDFEntry);
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



        private void VerifyWindowMode(ref WindowMode wm)
        {
            if (!(Enum.IsDefined(typeof(WindowMode), wm)))
                wm = WindowMode.MAIN;


        }



        private void SwitchWindowMode(bool result)
        {
            _WindowModeChangeResult.TrySetResult(result);
        }

        //TODO Needs to be improved by an isDirty() flag
        private bool CheckForUnsavedChanges()
        {
            bool result = false;
            //TODO Make a more accurate check for unsaved changes, using isDirty flags and the like
            if (Filename == GuidGenerator.UNSAVEDFILENAME || String.IsNullOrEmpty(Filename))
                result = true;

            return result;
        }

        //private void CheckForUns

        #region Debug Region
        [Conditional("DEBUG")]
        void WriteToDebugConsole(string msg)
        {
            Console.WriteLine(msg);
        }

        #endregion

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

        private bool AllowedToChangeSnappingCornerValue()
        {
            bool verified = false;

            if (_snapToCorner == true && WinMode == WindowMode.WIDGET)
            {
                verified = true;
            }

            if (_snapToCorner == false && WinMode == WindowMode.WIDGET)
            {
                verified = true;
            }

            return verified;

        }

        private bool _snapToCorner;
        public bool SnapToCorner
        {
            get
            { return _snapToCorner; }
            set
            {
                if (AllowedToChangeSnappingCornerValue())
                    SetProperty(ref _snapToCorner, value);
            }
        }


        public WindowMode WinMode
        {
            get { return _winMode; }
            set { SetProperty(ref _winMode, value); }
        }
        #endregion

        #region Delegate Implementation

        private void ConfirmUnsavedChanges()
        {
            WriteToDebugConsole("Before Raising ConfirmUnsaved");
            try
            {
                if (CheckForUnsavedChanges())
                {
                    ConfirmationRequest.Raise(ZaveGlobalSettings.Data_Structures.ZaveMessageBoxes.ConfirmUnsavedChanges,
                        c =>
                        {

                            if (c.Confirmed)
                            {
                                SaveZDF();
                                WriteToDebugConsole("Confirming Unsaved Changes!!!!!!!!");
                            }



                        }
                    );
                }
                else
                {
                    SaveZDF();
                }
            }
            catch (Exception ex)
            {
                WriteToDebugConsole(ex.Message);
            }
            WriteToDebugConsole("After Raising ConfirmUnsaved");
        }

        //private async Task ExportZDF(string source)
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

        private async Task ExportZDFAsync(string source)
        {
            var activeZDF = ZDFSingleton.GetInstance();
            var entries = activeZDF.EntryList.ToList();

            //TODO make the sorting method up to the User when exporting a doc
            var comp = new ColorComparer();
            entries.Sort(comp);

            IZDFEntry lastEntry = default(ZDFEntry);
            await Task.Factory.StartNew(() =>
            {

                //WordApp.Activate();

                switch (source)
                {

                    case "WORD":
                        try
                        {
                            Type wordType = Type.GetTypeFromProgID("Word.Application");

                            var WordApp = Activator.CreateInstance(wordType) as Microsoft.Office.Interop.Word.Application;

                            //var fileconvs = ((_Application)WordApp).FileConverters;


                            if (WordApp.Documents.Count == 0)
                            {
                                WordApp.Visible = false;
                                WordApp.ScreenUpdating = false;
                            }
                            IOService.DeleteFile(Path.GetTempPath() + APIFileNames.ZaveToSource);

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

                                // wordDoc.Activate();
                                var wdform = new WdSaveFormat();
                                wdform = WdSaveFormat.wdFormatDocument;
                                Object wdformObj = wdform;
                                //Object fileconvsinit = WordApp.FileConverters;
                                //var fileconvsobj = Marshal.CreateWrapperOfType(fileconvsinit, typeof(FileConverters));
                                //FileConverters fileconvs = fileconvsobj as FileConverters;
                                //if (WordApp != null)
                                //{
                                //    fileconvs = Marshal.CreateWrapperOfType<FileConverters, FileConverters>(WordApp.FileConverters);
                                //}
                                //foreach (FileConverter fc in fileconvs)
                                //{
                                //    try
                                //    {
                                //        //Console.WriteLine("FileConverter SaveFormat Int: " + fc.SaveFormat);
                                //        Console.WriteLine("FileConverter Name: " + fc.Name);
                                //        Console.WriteLine("FileConverter FormatName: " + fc.FormatName + '\n');
                                //    }
                                //    catch
                                //    {
                                //        ;
                                //    }



                                //}


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
                                    if (entry.Text.TrimStart().StartsWith(@"{\rtf1", StringComparison.Ordinal))
                                        rb.Rtf = entry.Text;
                                    else
                                    {
                                        rb.Text = entry.Text;

                                    }
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
                            catch (COMException cex)
                            {
                                Console.WriteLine(cex.Message);
                            }
                            catch (Exception ex)
                            {
                                ;
                            }
                            finally
                            {
                                IOService.CreateFileAsync(Path.GetTempPath() + APIFileNames.ZaveToSource);

                                if (WordApp.Visible.Equals(false))
                                {
                                    try
                                    {
                                        if (WordApp.Documents.Count > 0)
                                            WordApp.Documents.Close(WdSaveOptions.wdSaveChanges);
                                    }
                                    catch (Exception ex)
                                    {
                                        ;
                                    }
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
                        catch (Exception ex)
                        {
                            ;
                        }
                        finally
                        {

                        }

                        break;
                }
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }


        private void ExportZDF(string source)
        {



            ExportZDFAsync(source);



        }
        /// <summary>
        /// Used when exporting ZDF to WordDoc
        /// </summary>
        /// <param name="wordDoc">The word Document that is the target of the export</param>
        /// <param name="entry">The ZDF Entry</param>
        /// <param name="rngStart">The location in the Doc where to start the ColorHeading</param>
        /// <param name="rngEnd">The location in the Doc where to end the Color Heading</param>
        /// <param name="bmName">Unused</param>
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

            ObservableImmutableList<IZDFEntry> ZdfEntries = new ObservableImmutableList<IZDFEntry>();
            var zdfEntryList = new List<ZDFEntry>();
            if (activeZdf.EntryList.Count > 0)
            {
                int id = activeZdf.EntryList.LastOrDefault().ID;
                var withoutfilter = activeZdf.EntryList.Where(t => t.ID == id).ToList();

                foreach (var undoitem in withoutfilter)
                {
                    if (((ZDFEntry)undoitem).Comments != null && ((ZDFEntry)undoitem).Comments.Count > 0)
                    {
                        ZdfEntries.Add(undoitem as ZDFEntry);
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
                        ZdfEntries.Add(undoitem as ZDFEntry);
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
                    ZdfEntries.Add(item as ZDFEntry);
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
                    ZdfEntries.Add(new ZdfEntryItemViewModel(_container, activeZdfUndo.LastOrDefault() as ZDFEntry));
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
                    ZdfEntries.Add(new ZdfEntryItemViewModel(_container, activeZdfUndo.LastOrDefault() as ZDFEntry));
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

        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void SaveZDFAsync()
        {
            if (SaveLocation == null || SaveLocation == String.Empty || SaveLocation == GuidGenerator.UNSAVEDFILENAME)
            {
                var filename = _ioService.SaveFileDialogService(getSaveDirectory());
                SaveLogicAsync(Convert.ToString(filename));
            }
            else
            {
                var filename = SaveLocation;
                SaveLogicAsync(Convert.ToString(filename));
            }
        }

        private void SaveZDF()
        {
            if (SaveLocation == null || SaveLocation == String.Empty || SaveLocation == GuidGenerator.UNSAVEDFILENAME)
            {
                var filename = _ioService.SaveFileDialogService(getSaveDirectory());
                SaveLogic(Convert.ToString(filename));
            }
            else
            {
                var filename = SaveLocation;
                SaveLogic(Convert.ToString(filename));
            }
        }

        #region Save Common Logic
        private void SaveLogic(string filename)
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
                                serializer.Serialize(wr, activeZDFVM.GetModel());
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

        private async Task SaveLogicAsync(string filename)
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
        private void OpenZDFAsync()
        {
            //var activeZDFVM = _container.Resolve(typeof(ZDFViewModel), "ZDFView") as ZDFViewModel;

            //JsonSerializer serializer = new JsonSerializer();
            ZDFSingleton activeZdf = ZDFSingleton.GetInstance();
            var filename = _ioService.OpenFileDialogService(getSaveDirectory());


            if (filename != String.Empty) //If the user did not press cancel
            {

                OpenZDFAsync(filename);

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


        /// <summary>
        /// Used when exporting a ZDF to Word
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
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
            ZdfEntries.Add(new ZdfEntryItemViewModel(_container, entry as ZDFEntry));
            #endregion

        }


        private void NewZDF()
        {
            ZDFSingleton activeZDF = ZDFSingleton.GetInstance();
            activeZDF.EntryList.Clear();
            ExpandedViewModel.activeZdfUndo.Clear();
            SaveLocation = getSaveDirectory();
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
                SaveLogicAsync(Convert.ToString(saveFileDialog.FileName));
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





