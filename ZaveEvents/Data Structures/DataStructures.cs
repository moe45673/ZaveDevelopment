using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ZaveSourceAdapter.Data_Structures
{
    public enum SrcType { WORD, EXCEL }

    public struct SelectionData
    {
        public String SelectionPage { get; set; }
        public String SelectionDocName { get; set; }
        public String SelectionText { get; set; }
        public String SelectionType { get; set; }
        public SrcType st {get; set; }

        


}

   


}
