using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Prism.Events;
using System.Windows.Forms;
using ZaveGlobalSettings.ZaveFile;
using Prism.Interactivity.InteractionRequest;
using WPFColor = System.Windows.Media.Color;
using System.Runtime.Serialization;

namespace ZaveGlobalSettings.Data_Structures
{
    /// <summary>
    /// holds all allowable platforms for Zave to integrate with. Value must be a power of 2
    /// </summary>
    [Flags]
    public enum SrcType { NONE = 0, WORD = 1, EXCEL = 2, OUTLOOK = 4 }


    /// <summary>
    /// A small signifier meant to be used for ID-ing what state the Window is in
    /// </summary>
    public enum WindowMode { NONE = 0, MAIN = 1, WIDGET = 2 }


    /// <summary>
    /// An idea but ultimately unused
    /// </summary>
    public sealed class SelectionStateList : List<SelectionState>
    {
        public List<SelectionState> SelStateList;

        private static readonly Lazy<SelectionStateList> lazy = new Lazy<SelectionStateList>(() => new SelectionStateList());

        public static SelectionStateList Instance { get { return lazy.Value; } }

        private readonly object _selStateLock = new object();

        private SelectionStateList() : base()
        {
            SelStateList = new List<SelectionState>();
        }



        public new void Add(SelectionState selstate)
        {
            lock (_selStateLock)
            {
                SelStateList.Add(selstate);
            }
        }

        public SelectionState Find(int id)
        {
            return SelStateList.SingleOrDefault(x => x.ID == id);
        }




    }

    /// <summary>
    /// 
    /// The 16 standard colors of highlighters. Should be revised to be dynamic.
    /// </summary>
    [Flags]
    public enum AvailableColors
    {
        None, YELLOW, LIGHTBLUE, LIGHTGREEN = 4, FUCHSIA = 8, BLACK = 16, AQUA = 32, LIME = 64, WHITE = 128, NAVY = 256, TEAL = 512, PURPLE = 1024, MAROON = 2048, OLIVE = 4096, GRAY = 8192, SILVER = 16384, RED = 32768
    }

    //public struct AvailableColors
    //{
    //    public static Dictionary<string, string> getColors()
    //    {
    //        Dictionary<string, string> temp = new Dictionary<string, string>();

    //    }

    //}

    /// <summary>
    /// A generic struct for comments to be communicated, usually embedded inside a SelectionState
    /// </summary>
    public struct SelectionComment
    {
        public int ID;
        public string Text;
        public string Author;
    }

    
    /// <summary>
    /// High Level class that holds all data/metadata from a Zave Entry abstractly
    /// </summary>
    public class SelectionState
    {
        //TODO change so that the properties aren't hardcoded for Documents (eg Title, Page #), but also allow metadata for formats such as emails or webpages

        public SelectionState(int id = -1, string name = "", string page = "", string text = "", DateTime date = default(DateTime), Color col = default(Color), SrcType src = SrcType.WORD, List<SelectionComment> comments = null)
        {
            ID = id;
            SelectionDocName = name;
            SelectionPage = page;
            SelectionText = text;

            if (date == default(DateTime))
                SelectionDateModified = DateTime.Now;
            else
                SelectionDateModified = date;
            this.Color = col;
            srcType = src;
            if (comments != null)
                Comments = comments;
            else
                Comments = new List<SelectionComment>();

            IsValid = true;
        }
        public int ID { get; set; }
        public String SelectionPage { get; set; }
        public Color Color { get; set; }
        public String SelectionDocName { get; set; }
        public String SelectionText { get; set; }
        public DateTime SelectionDateModified { get; set; }
        // String SelectionType { get; set; }
        public SrcType srcType { get; set; }
        public List<SelectionComment> Comments { get; set; }


        public bool IsValid { get; set; }

        public List<SelectionStateError> ErrorCollection { get; private set; }

        public void AddError(string property, string message)
        {

            SelectionStateError err = new SelectionStateError(new Exception(), message);
            IsValid = false;
            AddError(property, err);
        }

        protected void AddError(string property, SelectionStateError error)
        {
            ErrorCollection.Add(error);
        }


    }

    //Unused
    public class Object<T1, T2, T3>
    {

        public Object(T1 first = default(T1), T2 second = default(T2), T3 third = default(T3))
        {
            FirstProp = first;
            SecondProp = second;
        }
        public Object()
        {
            FirstProp = default(T1);
            SecondProp = default(T2);
            ThirdProp = default(T3);
        }

        public T1 FirstProp { get; set; }
        public T2 SecondProp { get; set; }
        public T3 ThirdProp { get; set; }
    }

    //    public static class FileChecker
    //    {

    //        private const int NumberOfRetries = 20;
    //        private const int DelayOnRetry = 50;

    //        public delegate void StreamChecker(string filepath);
    //        private Object obj;




    //        public static void checkFile(Object stream, string filepath){


    //        for (int i=1; i <= NumberOfRetries; ++i) {
    //            try {
    //                // Do stuff with file
    //                if (stream is StreamWriter)
    //                    stream = new StreamWriter(filepath);
    //                else if (stream is StreamReader)
    //                {
    //                    stream = new StreamReader(filepath);
    //                }
    //                break; // When done we can break loop
    //            }
    //            catch (IOException e) {
    //                // You may check error code to filter some exceptions, not every error
    //                // can be recovered.
    //                if (i == NumberOfRetries) // Last one, (re)throw exception and exit
    //                    throw;

    //                Thread.Sleep(DelayOnRetry);
    //            }
    //        }
    //    }



        //Unused
    public class ModelEventArgs : EventArgs
    {
        public ModelEventArgs(string description)
        {
            //_selState = selState;
            _description = description;
        }
        //private SelectionState _selState;

        private string _description;

        //public SelectionState SelState
        //{
        //    get { return _selState; }
        //}

        public string Description
        {
            get;
            private set;
        }
    }

    //Unused
    public class MouseMessageFilter : IMessageFilter
    {
        public static event MouseEventHandler MouseMove = delegate { };
        const int WM_MOUSEMOVE = 0x0200;

        public bool PreFilterMessage(ref Message m)
        {

            if (m.Msg == WM_MOUSEMOVE)
            {

                Point mousePosition = Control.MousePosition;

                MouseMove(null, new MouseEventArgs(
                    MouseButtons.None, 0, mousePosition.X, mousePosition.Y, 0));
            }
            return false;
        }
    }

    //Unsure if unused? If so, barely
    public class SrcEventArgs : EventArgs
    {
        public SelectionState zSrc { get; set; }

        public SrcEventArgs(SelectionState src)
            : base()
        {
            zSrc = src;
        }
    }

    //Unused
    public abstract class SourceFactory : IDisposable
    {


        // Track whether Dispose has been called.
        private bool disposed = false;


        public SelectionState produceSource(ZaveGlobalSettings.Data_Structures.SelectionState selDat)
        {
            SelectionState Src = createSrc(selDat.SelectionDocName, selDat.SelectionPage, selDat.SelectionText);

            return Src;
        }

        protected abstract SelectionState createSrc(string name, string page, string text);



        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.

                }



                // Note disposing has been done.
                disposed = true;

            }
        }

        // Use interop to call the method necessary
        // to clean up the unmanaged resource.
        //[System.Runtime.InteropServices.DllImport("Kernel32")]
        //private extern static Boolean CloseHandle(IntPtr handle);

        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        ~SourceFactory()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }
    }


    public class SelectionStateError
    {
        public String ErrorMessage { get; private set; }
        public Exception Exception { get; private set; }

        public SelectionStateError(Exception ex = null, String msg = "") : base()
        {
            ErrorMessage = msg;
            this.Exception = ex;
        }
    }





    //Unused
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);
            //#if DEBUG
            //            MessageBox.Show("Time is " + propertyName);
            //#endif

            var handler = this.PropertyChanged;
            if (handler != null)
            {
                //#if DEBUG
                //                MessageBox.Show("Event Fired!");
                //#endif
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion // INotifyPropertyChanged Members

        #region Debugging Aides
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public virtual void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                if (this.ThrowOnInvalidPropertyName)
                    throw new Exception(msg);
                else
                    Debug.Fail(msg);
            }
        }

        /// <summary>
        /// Returns whether an exception is thrown, or if a Debug.Fail() is used
        /// when an invalid property name is passed to the VerifyPropertyName method.
        /// The default value is false, but subclasses used by unit tests might
        /// override this property's getter to return true.
        /// </summary>
        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

        #endregion // Debugging Aides


    }
    #region Global Events
    public class EntryUpdateEvent : PubSubEvent<Object>
    {

    }

    public class EntryReadEvent : PubSubEvent<Object>
    {

    }

    public class EntrySelectedEvent : PubSubEvent<string>
    {

    }

    public class EntryCreatedEvent : PubSubEvent<Object>
    {

    }

    public class ActiveEntryUpdatedEvent : PubSubEvent<string>
    {

    }

    public class EntryDeletedEvent : PubSubEvent<Object>
    {

    }

    public class CommentUpdateEvent : PubSubEvent<Object>
    {

    }

    public class CommentReadEvent : PubSubEvent<Object>
    {

    }

    public class CommentCreatedEvent : PubSubEvent<Object>
    {

    }

    public class CommentDeletedEvent : PubSubEvent<Object>
    {

    }

    public class MainControlsUpdateEvent : PubSubEvent<Object>
    {

    }

    public class ActiveColorUpdatedEvent : PubSubEvent<Color>
    {

    }

    public class ZDFOpenedEvent : PubSubEvent<string> { }

    public class ZDFSavedEvent : PubSubEvent<string> { }

    public class ZDFExportedEvent : PubSubEvent<object> { }

    public class NewZDFCreatedEvent : PubSubEvent<string>
    {

    }

    public class MRUChangedEvent : PubSubEvent<string>
    {

    }

    public class MainWindowInstantiatedEvent : PubSubEvent<object> { }

    public class FilenameChangedEvent : PubSubEvent<string> { }

    public class WindowModeChangeEvent : PubSubEvent<WindowMode> { }

    public class WindowModeChangedEvent : PubSubEvent<bool> { }

    #endregion

    /// <summary>
    /// Custom Zave Exception class
    /// </summary>
    public class ZaveOperationFailedException : Exception
    {
        public ZaveOperationFailedException() : base()
        {

        }

        public ZaveOperationFailedException(string message) : base(message)
        {

        }

        public ZaveOperationFailedException(String message, Exception innerException) : base(message, innerException)
        {

        }

        protected ZaveOperationFailedException(SerializationInfo info, StreamingContext context) : base(info, context)

        {

        }
    }

    /// <summary>
    /// Hardcoded instance names, mainly used with registered types and instances in the Unity container
    /// </summary>
    public static class InstanceNames
    {
        public const string MainWindowView = "MainWindowView";
        public const string MainWindowViewModel = "MainWindowViewModel";
        public const string ActiveZDF = "ActiveZDF";
        public const string ExpandedView = "ExpandedView";
        public const string WidgetView = "WidgetView";
        public const string AppSettings = "AppSettings";
        public const string ColorPickerView = "ColorPickerView";
        public const string ControlBarView = "ControlBarView";
        public const string ExportedToWord = "ZDF has been exported to your ZDFs Folder";
        public const string ZDFEntry = "ZDFEntry";
        public const string ZDFAppContainer = "ZDFAppContainer";
    }


    /// <summary>
    /// Hardcoded Region names in the RegionManager
    /// </summary>
    public class RegionNames
    {
        public static string RecentZDFListRegion
        {
            get
            {
                return "RecentZDFListRegion";
            }
        }
        public static string ZDFEntryListRegion
        {
            get
            {
                return "ZDFEntryListRegion";
            }
        }
        public static string ZDFEntryDetailRegion
        {
            get
            {
                return "ZDFEntryDetailRegion";
            }
        }
        public static string ControlBarRegion
        {
            get
            {
                return "ControlBarRegion";
            }
        }
        public static string MainViewRegion
        {
            get
            {
                return "MainViewRegion";
            }
        }

        public static string ContainerRegion
        {
            get
            {
                return "ContainerRegion";
            }
        }

        public static string WidgetMainRegion
        {
            get
            {
                return "WidgetMainRegion";
            }
        }
        public static string MenuRegion
        {
            get
            {
                return "MenuRegion";
            }
        }
        public static string ZaveMainColorPicker
        {
            get
            {
                return "ZaveMainColorPicker";
            }
        }

        public static string ZaveWidgetColorPicker
        {
            get
            {
                return "ZaveWidgetColorPicker";
            }
        }
        public static string MainTitleBarRegion
        {
            get
            {
                return "MainTitleBarRegion";
            }
        }
        public static string WidgetTitleBarRegion
        {
            get
            {
                return "WidgetTitleBarRegion";
            }
        }
    }

    /// <summary>
    /// Implementations of IConfirmation to be used as modal dialog boxes
    /// </summary>
    public struct ZaveMessageBoxes
    {
        public static IConfirmation ConfirmUnsavedChanges = new Confirmation { Content = "You have unsaved changes. Would you first like to save these?", Title = "Save Unsaved Changes?" };
        public static IConfirmation ConfirmDeleteEntryCommand = new Confirmation { Content = "Are you sure you want to delete this ZDFEntry?", Title = "Confirm Deletion Process" };
        public static IConfirmation ConfirmNavigateAwayFromFormCommand = new Confirmation { Content = "Are you sure you want to leave this window? Your changes will not be saved.", Title = "Confirm Exiting Popup" };

    }


    public struct ZaveNavigationParameters
    {
        public static readonly string CommentText = "CommentText";
    }

    /// <summary>
    /// Hardcoded Names of the files that are written to the Temp folder for Zave to communicate with other software
    /// </summary>
    public static class APIFileNames
    {
        public static readonly string SourceToZave = GuidGenerator.getGuid() + "1";
        public static readonly string ZaveToSource = GuidGenerator.getGuid() + "2";
    }

    public abstract class WatcherFactory
    {
        public Component Watcher { get; set; }
        protected abstract Component createWatcher();
    }

    public interface IConfigProvider
    {
        Color ActiveColor { get; set; }
    }

    /// <summary>
    /// Static class to perform operations on colors
    /// </summary>
    public static class ColorHelper
    {
        /// <summary>
        /// What to multiply the RGB values to convert a color to greyscale
        /// </summary>
        private static readonly float[] grayscaleValues = { 0.299F, 0.587F, 0.114F };

        /// <summary>
        /// Takes a color and returns it as greyscale value 
        /// </summary>
        /// <param name="col">The color to convert to greyscale</param>
        /// <returns>The color as it would be represented in greyscale</returns>
        public static Color ToGrayscaleARGB(Color col)
        {
            int grayscale = (int)((col.R * grayscaleValues[0]) + (col.G * grayscaleValues[1]) + (col.B * grayscaleValues[2]));
            return Color.FromArgb(col.A, grayscale, grayscale, grayscale);

        }

        /// <summary>
        /// Changes System.Drawing.Color to System.Windows.Media.Color (used with XAML)
        /// </summary>
        /// <param name="col">System Color to convert</param>
        /// <returns>The color as a System.Windows.Media.Color</returns>
        public static WPFColor toWPFColor(Color col)
        {
            return WPFColor.FromArgb(col.A, col.R, col.G, col.B);

        }

        /// <summary>
        /// Turns a System.Windows.Media.Color (used with XAML and WPF) into a System.Drawing.Color
        /// </summary>
        /// <param name="wCol">The System.Windows.Media.Color to convert</param>
        /// <returns>The color as a System.Drawing.Color</returns>
        public static Color FromWPFColor(WPFColor wCol)
        {

            Color col = new Color();
            string colorName = "";
            try
            {
                if (Convert.ToString(wCol) == "#00000000")
                    colorName = "White";
                else
                    colorName = GetWPFColorName(wCol);
            }
            catch (System.Data.ObjectNotFoundException onf)
            {
                System.Windows.Forms.MessageBox.Show("The Specified Color Could Not Be Found", "Color Not Found", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            finally
            {
                col = Color.FromArgb(wCol.A, wCol.R, wCol.G, wCol.B);

            }
            return col;

        }

        /// <summary>
        /// Gets a color from a name provided as a string
        /// </summary>
        /// <param name="str">Name of the color</param>
        /// <returns>The System.Drawing.Color type that represents the input</returns>
        public static Color ParseFromString(string str)
        {
            ColorConverter converter = new ColorConverter();
            var col = (Color)converter.ConvertFromString(str);

            return col;

        }

        /// <summary>
        /// Gets the name of a System.Windows.Media.Color
        /// </summary>
        /// <param name="color">The color from which to extract the name</param>
        /// <returns>A string representation of the color name</returns>
        private static string GetWPFColorName(WPFColor color)
        {
            Type colors = typeof(System.Windows.Media.Colors);
            foreach (var prop in colors.GetProperties())
            {
                if (((System.Windows.Media.Color)prop.GetValue(null, null)) == color)
                    return prop.Name;
            }

            throw new System.Data.ObjectNotFoundException("The provided Color is not named.");
        }


    }

    /// <summary>
    /// Extension methods for List<> types
    /// </summary>
    public static class ZaveListExtensions
    {

        /// <summary>
        /// Tests if two lists are exactly the same in terms of what they contain
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns>true if the lists are the </returns>
        public static bool AreEqual<T>(this List<T> list1, List<T> list2)
        {


            var firstNotSecond = list1.Except(list2).ToList();
            var secondNotFirst = list2.Except(list1).ToList();

            return !firstNotSecond.Any() && !secondNotFirst.Any();
        }


        /// <summary>
        /// Tests if two lists are exactly the same in terms of what they contain
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <param name="compEq">An instance of IEqualityComparer<> specifically made for T</param>
        /// <returns></returns>
        public static bool AreEqual<T>(this List<T> list1, List<T> list2, IEqualityComparer<T> compEq)
        {


            var firstNotSecond = list1.Except(list2, compEq).ToList();
            var secondNotFirst = list2.Except(list1, compEq).ToList();

            return !firstNotSecond.Any() && !secondNotFirst.Any();
        }
    }

}