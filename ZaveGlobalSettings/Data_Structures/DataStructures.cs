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
    
        public SelectionState(string name = "", string page = "", string text = "", SrcType src = 0)
        {
            SelectionDocName = name;
            SelectionPage = page;
            SelectionText = text;
            srcType = src;
        }
        public String SelectionPage { get; set; }
        public String SelectionDocName { get; set; }
        public String SelectionText { get; set; }
        // String SelectionType { get; set; }
        public SrcType srcType { get; set; }  


        public bool IsValid { get; set; }

        public List<SelectionStateError> ErrorCollection { get; }

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

  


    public class SelectionStateError
    {
        public String ErrorMessage { get; }
        public Exception Exception { get; }

        public SelectionStateError(Exception ex = null, String msg = "") : base()
        {
            ErrorMessage = msg;
            this.Exception = ex;
        }
    }

    
}
