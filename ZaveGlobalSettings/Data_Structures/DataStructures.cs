using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;
using Prism.Events;

namespace ZaveGlobalSettings.Data_Structures
{
    /// <summary>
    /// holds all allowable platforms for Zave to integrate with. Value must be a power of 2
    /// </summary>
    [Flags]
    public enum SrcType { NONE = 0, WORD = 1, EXCEL = 2 }

    public enum WindowMode { NONE = 0, MAIN = 1, WIDGET = 2 }



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

        

        public void Add(SelectionState selstate)
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
    /// 
    /// </summary>
    [Flags]
    public enum AvailableColors
    {
        None, YELLOW, LIGHTBLUE, LIGHTGREEN=4, FUCHSIA=8, BLACK=16, AQUA=32, LIME=64,  WHITE=128, NAVY=256, TEAL=512, PURPLE=1024, MAROON=2048, OLIVE=4096, GRAY=8192, SILVER=16384, RED=32768
    }

//public struct AvailableColors
//{
//    public static Dictionary<string, string> getColors()
//    {
//        Dictionary<string, string> temp = new Dictionary<string, string>();

//    }

//}
    public struct SelectionComment
    {
        public int ID;
        public string Text;
        public string Author;
    }

    /// <summary>
    /// High Level class that holds all data/metadata from an Entry abstractly
    /// </summary>
    public class SelectionState
    {
        
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

    public class SrcEventArgs : EventArgs
    {
        public SelectionState zSrc { get; set; }

        public SrcEventArgs(SelectionState src)
            : base()
        {
            zSrc = src;
        }
    }

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

    

    public static class InstanceNames
    {
        public const string MainWindowView = "MainWindowView";
        public const string MainWindowViewModel = "MainWindowViewModel";
    }

}