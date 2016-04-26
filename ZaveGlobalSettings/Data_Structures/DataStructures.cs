using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Source = ZaveGlobalSettings.Data_Structures.SelectionState;

namespace ZaveGlobalSettings.Data_Structures
{

    /// <summary>
    /// holds all allowable platforms for Zave to integrate with
    /// </summary>
    public enum SrcType { GENERIC = 0, WORD = 1, EXCEL = 2 }

     
    /// <summary>
    /// High Level class that holds all data/metadata from an Entry abstractly
    /// </summary>
    public class SelectionState
    {
    
        public SelectionState(string name = "", string page = "", string text = "", SrcType src = SrcType.WORD)
        {
            SelectionDocName = name;
            SelectionPage = page;
            SelectionText = text;
            srcType = src;
            IsValid = true;
        }
        public String SelectionPage { get; set; }
        public String SelectionDocName { get; set; }
        public String SelectionText { get; set; }
        // String SelectionType { get; set; }
        public SrcType srcType { get; set; }  


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

    public class ModelEventArgs : EventArgs
    {
        public ModelEventArgs(string description, SelectionState selState)
        {
            _selState = selState;
            _description = description;
        }
        private SelectionState _selState;

        private string _description;

        public SelectionState SelState
        {
            get { return _selState; }
        }

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


        public Source produceSource(ZaveGlobalSettings.Data_Structures.SelectionState selDat)
        {
            Source Src = createSrc(selDat.SelectionDocName, selDat.SelectionPage, selDat.SelectionText);

            return Src;
        }

        protected abstract Source createSrc(string name, string page, string text);



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

    
}
