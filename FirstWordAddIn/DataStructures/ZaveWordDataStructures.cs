using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;

namespace FirstWordAddIn.DataStructures
{
    

    public class WordEventArgs : ZaveEvents.SrcEventArgs{

        
        public WordEventArgs(ZaveEvents.Data_Structures.SelectionData sd) : base(sd) {
            
            
        }

    }
}
