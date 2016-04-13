using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZaveGlobalSettings.Data_Structures
{


    public enum SrcType { GENERIC = 0, WORD = 1, EXCEL = 2 }

    public Dictionary<int, SelectionData> SelectionDataDictionary; 

    public struct SelectionData
    {
    
        public SelectionData(string name = "", string page = "", string text = "", SrcType src = 0) : this()
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

        

        
    }
}
