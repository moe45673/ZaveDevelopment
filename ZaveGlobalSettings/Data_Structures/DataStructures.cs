using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
