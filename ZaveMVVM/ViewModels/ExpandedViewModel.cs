﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ZaveGlobalSettings.Data_Structures.ZaveObservableCollection;
using ZaveGlobalSettings.Data_Structures;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Practices.Unity;
using Prism.Regions;
using Prism.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using ZaveModel.ZDF;
using ZaveModel.ZDFEntry;
using ZaveGlobalSettings.ZaveFile;
using ZaveService.IOService;
using Prism.Events;
using Prism.Mvvm;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Interop;
//using DevExpress.Utils.Drawing.Helpers;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Controls;
using Novacode;

namespace ZaveViewModel.ViewModels
{
    abstract class ZDFAbstractComparer : IComparer<IZDFEntry>
    {
        private IZDFEntry A;
        private IZDFEntry B;

        public ZDFAbstractComparer()
        {
            A = new ZDFEntry();
            B = new ZDFEntry();
        }

        public abstract int Compare(IZDFEntry x, IZDFEntry y);
        
        
    }
    class ColorComparer : IComparer<IZDFEntry>
    {

        public int Compare(IZDFEntry x, IZDFEntry y)
        {
            return x.HColor.CompareTo(y.HColor);
        }


    }

    public class ExpandedViewModel : BindableBase
    {

        

        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;
        private readonly IIOService _ioService;
        public static string ACTIVESORT = "TxtDocColor";
        public static List<IZDFEntry> activeZdfUndo = new List<IZDFEntry>();
        //public static string SaveLocation = null;
        //public static List<ZdfUndoComments> ZdfUndoComments = new List<ZdfUndoComments>();
        //public static List<ZdfUndoComments> RemoveZdundoComments = new List<ZdfUndoComments>();
        //public event PropertyChangedEventHandler PropertyChanged = null;


        //public DelegateCommand SaveZDFDelegateCommand { get; set; }
        //public DelegateCommand OpenZDFDelegateCommand { get; set; }

        //public DelegateCommand NewZDFDelegateCommand { get; set; }

        //public DelegateCommand NewZDFEntryDelegateCommand { get; set; }

        //public DelegateCommand UndoZDFDelegateCommand { get; set; }

        //public DelegateCommand RedoZDFDelegateCommand { get; set; }

        //public DelegateCommand ScreenshotZDFDelegateCommand { get; set; }

        //public DelegateCommand<String> ExportZDFDelegateCommand { get; set; }

        //public DelegateCommand SaveASZDFDelegateCommand { get; set; }

        //public string SaveLocation
        //{
        //    get
        //    {
        //        return MainWindowViewModel.SaveLocation;
        //    }
        //    set
        //    {
        //        MainWindowViewModel.SaveLocation = value;
        //    }
        //}

        public ExpandedViewModel(IRegionManager regionManager, IUnityContainer cont, IEventAggregator eventAgg, IOService ioService)
        {

            if (cont == null) throw new ArgumentNullException("container");
            if (regionManager == null) throw new ArgumentNullException("regionManager");
            if (eventAgg == null) throw new ArgumentNullException("eventAggregator");

            _container = cont;
            _regionManager = regionManager;
            _eventAggregator = eventAgg;
            //SaveZDFDelegateCommand = new DelegateCommand(SaveZDF);
            //OpenZDFDelegateCommand = new DelegateCommand(OpenZDF);
            //NewZDFDelegateCommand = new DelegateCommand(NewZDF);
            //NewZDFEntryDelegateCommand = new DelegateCommand(NewZDFEntry);
            //UndoZDFDelegateCommand = new DelegateCommand(UndoZDF);
            //RedoZDFDelegateCommand = new DelegateCommand(RedoZDF);
            //ScreenshotZDFDelegateCommand = new DelegateCommand(ScreenshotZDF);            
            //ExportZDFDelegateCommand = DelegateCommand<string>.FromAsyncHandler(x => ExportZDF(x));
            //SaveASZDFDelegateCommand = new DelegateCommand(SaveAsZdfFile);
            //ExportZDFDelegateCommand = new DelegateCommand<string>(ExportZDF);
            _ioService = ioService;

            _eventAggregator.GetEvent<WindowModeChangeEvent>().Subscribe(ChangeIsActive);

        }


        private bool _isActive;

        public bool IsActive
        {
            get { return this._isActive; }
            set { SetProperty(ref _isActive, value); }
        }

        private void ChangeIsActive(WindowMode wm)
        {
            if (wm == WindowMode.MAIN)
            {
                IsActive = true;

            }
            else
                IsActive = false;
        }


        
        [Conditional("DEBUG")]
        private void setIndented(JsonSerializer ser)
        {
            ser.Formatting = Newtonsoft.Json.Formatting.Indented;
        }

        #region Code moved to MainWindowViewModel
        //private async Task ExportZDF(string source)
        //{

        //    var activeZDF = ZDFSingleton.GetInstance();
        //    switch (source)
        //    {
        //        case "WORD":
        //            await Task.Run(() =>
        //            {
        //                try
        //                {

        //                    var exportfilename = createExportFileName();
        //                    using (DocX doc = DocX.Load(exportfilename))

        //                    {

        //                        var entries = activeZDF.EntryList.ToList();
        //                        var comp = new ColorComparer();
        //                        entries.Sort(comp);
        //                        var lastColor = default(System.Drawing.Color);

        //                        foreach (var entry in entries)

        //                        {

        //                            var thisColor = entry.HColor.Color;

        //                            if (!thisColor.Equals(lastColor))
        //                            {

        //                                Table t = doc.InsertTable(1, 1);
        //                                Row r = t.Rows.ElementAt(0);
        //                                Cell c = r.Cells.ElementAt(0);
        //                                c.Shading = thisColor;

        //                            }


        //                            Paragraph p = doc.InsertParagraph();

        //                            p.InsertText(entry.Text + Environment.NewLine);

        //                            lastColor = thisColor;
        //                        }

        //                        doc.Save();
        //                        _eventAggregator.GetEvent<ZDFExportedEvent>().Publish(exportfilename);
        //                    }
        //                }
        //                catch(IOException ioex)
        //                {
        //                System.Windows.Forms.MessageBox.Show("Unable to create export file. If file is currently open, please close and try again.", "FILE NOT EXPORTED", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        //                }

        //            });
        //            break;
        //    }
        //}


        //private string createExportFileName()
        //{
        //    var activeZDF = ZDFSingleton.GetInstance();
        //    var str1 = getSaveDirectory() + "\\ExportedFiles";
        //    if (!Directory.Exists(str1))
        //    {
        //        Directory.CreateDirectory(str1);
        //    }


        //    var str2 = Path.Combine(str1, Path.GetFileNameWithoutExtension(activeZDF.Name) + "Export.docx");

        //    //Better way to delete the file?
        //    if (File.Exists(str2))
        //    {
        //       for(int i = 0; i < 20; i++)
        //        {
        //            try
        //            {
        //                File.Delete(str2);
        //                System.Threading.Thread.Sleep(50);
        //                break;
        //            }
        //            catch(IOException ioex)
        //            {
        //                Console.WriteLine(ioex.Message);
        //            }
        //        }

        //    }
        //    try
        //    {
        //        using (DocX newDoc = DocX.Create(str2))
        //            newDoc.Save();

        //    }
        //    catch(IOException ioex)
        //    {

        //        throw ioex;
        //    }
        //    return str2;
        //}

        //private void UndoZDF()
        //{

        //    #region MyCode2
        //    ZDFSingleton activeZdf = ZDFSingleton.GetInstance();

        //    ObservableImmutableList<ZdfEntryItemViewModel> ZdfEntries = new ObservableImmutableList<ZdfEntryItemViewModel>();
        //    var zdfEntryList = new List<ZDFEntry>();
        //    if (activeZdf.EntryList.Count > 0)
        //    {
        //        int id = activeZdf.EntryList.LastOrDefault().ID;
        //        var withoutfilter = activeZdf.EntryList.Where(t => t.ID == id).ToList();

        //        foreach (var undoitem in withoutfilter)
        //        {
        //            if (((ZDFEntry)undoitem).Comments != null && ((ZDFEntry)undoitem).Comments.Count > 0)
        //            {
        //                ZdfEntries.Add(new ZdfEntryItemViewModel(undoitem as ZDFEntry));
        //                var commentID = ((ZDFEntry)undoitem).Comments.LastOrDefault().CommentID;
        //                var commentObject = ((ZDFEntry)undoitem).Comments.FirstOrDefault(a => a.CommentID == commentID);
        //                ((ZDFEntry)undoitem).Comments.Remove(commentObject);
        //                zdfEntryList.Add(((ZDFEntry)undoitem));
        //                if(MainWindowViewModel.RemoveZdundoComments.Any(a=>a.ID==id && a.Comments.CommentID == commentID))
        //                {
        //                    MainWindowViewModel.RemoveZdundoComments.Remove(MainWindowViewModel.RemoveZdundoComments.FirstOrDefault(a => a.ID == id && a.Comments.CommentID == commentID));
        //                }
        //                if (!ZdfUndoComments.Any(a => a.Comments.CommentID == commentID && a.ID == id))
        //                {
        //                    ZdfUndoComments.Add(new ZdfUndoComments
        //                    {
        //                        Comments = commentObject,
        //                        ID = id
        //                    });
        //                }
        //            }
        //            else
        //            {
        //            activeZdfUndo.Add(undoitem);
        //            ZdfEntries.Add(new ZdfEntryItemViewModel(undoitem as ZDFEntry));
        //        }


        //        }

        //        var filter = activeZdf.EntryList.Where(t => t.ID != id).ToList();

        //        activeZdf.EntryList.Clear();

        //        foreach (var item in zdfEntryList)
        //        {
        //            activeZdf.Add(item);
        //        }
        //        foreach (var item in filter.ToList())
        //        {
        //            activeZdf.Add(item);
        //            ZdfEntries.Add(new ZdfEntryItemViewModel(item as ZDFEntry));
        //        }
        //    }
        //#endregion
        //}

        //private void RedoZDF()
        //{
        //    #region MyCode2
        //    ZDFSingleton activeZdf = ZDFSingleton.GetInstance();
        //    ObservableImmutableList<ZdfEntryItemViewModel> ZdfEntries = new ObservableImmutableList<ZdfEntryItemViewModel>();

        //    //if (activeZdfUndo.Count > 0 && activeZdfUndo.Count != 1)
        //    //if (activeZdfUndo.Count > 0)
        //    //{
        //    //    activeZdf.EntryList.Add(activeZdfUndo.LastOrDefault());
        //    //    ZdfEntries.Add(new ZdfEntryItemViewModel(activeZdfUndo.LastOrDefault() as ZDFEntry));
        //    //    activeZdfUndo.RemoveAt(activeZdfUndo.Count - 1);
        //    //    //return;
        //    //}
        //    if (ZdfUndoComments != null && ZdfUndoComments.Count > 0)
        //    {
        //        if (activeZdf.EntryList.Count > 0)
        //        {
        //            var tempObject = new ZdfUndoComments();
        //            if (RemoveZdundoComments == null || !RemoveZdundoComments.Any())
        //            {
        //                tempObject = ZdfUndoComments.LastOrDefault();
        //            }
        //            else
        //            {
        //                tempObject = ZdfUndoComments.Where(a => !RemoveZdundoComments.Any(b => b.Comments.CommentID == a.Comments.CommentID && b.ID == a.ID)).LastOrDefault();
        //            }
        //            if (tempObject != null)
        //            {
        //                var zdfEntry = activeZdf.EntryList.FirstOrDefault(a => a.ID == tempObject.ID);
        //                if (zdfEntry != null)
        //                {
        //                    if (!zdfEntry.Comments.Any(a => a.CommentID == tempObject.Comments.CommentID))
        //                    {
        //                        zdfEntry.Comments.Add(tempObject.Comments);
        //                        RemoveZdundoComments.Add(tempObject);
        //                        if (ZdfUndoComments.Any(a=>a.ID== zdfEntry.ID &&  a.Comments.CommentID== tempObject.Comments.CommentID))
        //                        {
        //                            ZdfUndoComments.Remove(ZdfUndoComments.FirstOrDefault(a => a.ID == zdfEntry.ID && a.Comments.CommentID == tempObject.Comments.CommentID));
        //                        }
        //                        //ZdfUndoComments.Remove(item);
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            activeZdf.EntryList.Add(activeZdfUndo.LastOrDefault());
        //            ZdfEntries.Add(new ZdfEntryItemViewModel(activeZdfUndo.LastOrDefault() as ZDFEntry));
        //            activeZdfUndo.RemoveAt(activeZdfUndo.Count - 1);
        //            //return;
        //        }
        //        //ZdfUndoComments = null;
        //    }
        //    else
        //    {
        //    if (activeZdfUndo.Count > 0)
        //    {
        //        activeZdf.EntryList.Add(activeZdfUndo.LastOrDefault());
        //        ZdfEntries.Add(new ZdfEntryItemViewModel(activeZdfUndo.LastOrDefault() as ZDFEntry));
        //        activeZdfUndo.RemoveAt(activeZdfUndo.Count - 1);
        //            //return;
        //        }
        //    }
        //    //if(removeZdundoComments!=null && removeZdundoComments.Any())
        //    //{
        //    //    foreach (var item in removeZdundoComments)
        //    //    {
        //    //        ZdfUndoComments.Remove(item);
        //    //    }
        //    //}
        //    //else if (activeZdfUndo.Count == 1)
        //    //{
        //    //    if (activeZdf.EntryList.Count > 0)
        //    //    {
        //    //        activeZdf.EntryList.Add(activeZdf.EntryList.LastOrDefault());
        //    //        ZdfEntries.Add(new ZdfEntryItemViewModel(activeZdf.EntryList.LastOrDefault() as ZDFEntry));
        //    //    }
        //    //    else{
        //    //        activeZdf.EntryList.Add(activeZdfUndo.LastOrDefault());
        //    //        ZdfEntries.Add(new ZdfEntryItemViewModel(activeZdfUndo.LastOrDefault() as ZDFEntry));
        //    //    }

        //    //}
        //    //else if (activeZdfUndo.Count == 0)
        //    //{
        //    //    if (activeZdf.EntryList.Count > 0)
        //    //    {
        //    //        activeZdf.EntryList.Add(activeZdf.EntryList.LastOrDefault());
        //    //        ZdfEntries.Add(new ZdfEntryItemViewModel(activeZdf.EntryList.LastOrDefault() as ZDFEntry));
        //    //    }
        //    //}

        //    #endregion
        //}

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
        //private void SaveZDF()
        //{
        //    if (SaveLocation == null || SaveLocation == "" || SaveLocation == GuidGenerator.UNSAVEDFILENAME)
        //    {
        //        var filename = _ioService.SaveFileDialogService(getSaveDirectory());
        //        SaveLogic(Convert.ToString(filename));
        //    }
        //    else
        //    {
        //        var filename = SaveLocation;
        //        SaveLogic(Convert.ToString(filename));
        //    }
        //}

        #region Save Common Logic
        //private void SaveLogic(string filename)
        //{
        //    var activeZDFVM = _container.Resolve(typeof(ZDFViewModel), "ZDFView") as ZDFViewModel;

        //    JsonSerializer serializer = new JsonSerializer();

        //    if (filename != String.Empty)
        //    {

        //        using (var sw = _ioService.SaveFileService(filename))
        //        {
        //            try
        //            {
        //                using (JsonWriter wr = new JsonTextWriter(sw))
        //                {
        //                    try
        //                    {

        //                        setIndented(serializer);
        //                        serializer.Serialize(wr, activeZDFVM.GetModel());
        //                        SaveLocation = filename;
        //                        var activeZDF = ZDFSingleton.GetInstance();
        //                        activeZDF.Name = filename;
        //                        _eventAggregator.GetEvent<ZDFSavedEvent>().Publish(activeZDF);
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
        //                System.Windows.Forms.MessageBox.Show("Save File Error!");
        //            }
        //            finally
        //            {
        //                sw.Close();
        //            }
        //        }
        //    }
        //}
        #endregion

       

        //public static string getSaveDirectory()
        //{
        //    return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ZDFs";
        //}

        //private string getSaveFileName()
        //{
        //    return "\\SaveDoc";
        //}

       
    }

    //internal class NativeMethods
    //{

    //    [DllImport("user32.dll")]
    //    public extern static IntPtr GetDesktopWindow();
    //    [DllImport("user32.dll")]
    //    public static extern IntPtr GetWindowDC(IntPtr hwnd);
    //    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    //    public static extern IntPtr GetForegroundWindow();
    //    [DllImport("gdi32.dll")]
    //    public static extern UInt64 BitBlt(IntPtr hDestDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, System.Int32 dwRop);
    //    [DllImport("user32.dll")]
    //    private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);
    //    public static Bitmap CaptureActiveWindow()
    //    {
    //        return CaptureWindow(GetForegroundWindow());
    //    }
    //    [StructLayout(LayoutKind.Sequential)]
    //    private struct Rect
    //    {
    //        public int Left;
    //        public int Top;
    //        public int Right;
    //        public int Bottom;
    //    }
    //    public static Bitmap CaptureWindow(IntPtr handle)
    //    {
    //        var rect = new Rect();
    //        GetWindowRect(handle, ref rect);
    //        var bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
    //        var result = new Bitmap(bounds.Width, bounds.Height);

    //        using (var graphics = Graphics.FromImage(result))
    //        {
    //            graphics.CopyFromScreen(new System.Drawing.Point(bounds.Left, bounds.Top), System.Drawing.Point.Empty, bounds.Size);
    //        }

    //        return result;
    //    }
    //}
    //public class ZdfUndoComments
    //{
    //    public int ID { get; set; }
    //    public IEntryComment Comments { get; set; }
    //}
    #endregion

        

}

