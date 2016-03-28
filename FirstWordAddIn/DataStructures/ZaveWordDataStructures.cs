using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;

namespace FirstWordAddIn.DataStructures
{
    struct SelectionData
    {
         public String SelectionPage { get; set; }
         public String SelectionDocName { get; set; }
        public String SelectionText { get; set; }
         
    }
}
