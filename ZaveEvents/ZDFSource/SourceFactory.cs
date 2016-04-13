using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Source = ZaveGlobalSettings.Data_Structures.SelectionState;
using System.ComponentModel;

namespace ZaveController.ZDFSource
{
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
            if(!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if(disposing)
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
}
