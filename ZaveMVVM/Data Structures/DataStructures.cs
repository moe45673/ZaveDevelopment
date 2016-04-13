using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaveSrc = ZaveModel.ZDFSource.Source;


namespace ZaveViewModel.Data_Structures
{
    public enum SrcType { WORD, EXCEL }

    public struct SelectionData
    {
        public SelectionData(string name = "", string page = "", string text = "", SrcType src = 0)
        {
            SelectionPage = page;
            SelectionDocName = name;
            SelectionText = text;
            srcType = src;
        }
        public String SelectionPage { get; set; }
        public String SelectionDocName { get; set; }
        public String SelectionText { get; set; }
        public SrcType srcType { get; set; }

    }
    
}
