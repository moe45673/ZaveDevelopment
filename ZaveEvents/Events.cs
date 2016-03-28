using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaveEvents
{
    public class SrcEventArgs : EventArgs
    {
        public Data_Structures.SelectionData selDat {get; set;}

        public SrcEventArgs(Data_Structures.SelectionData sd) : base()
        {
            selDat = sd;
        }
    }
}
