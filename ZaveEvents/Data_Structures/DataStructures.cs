using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaveGlobalSettings.Data_Structures;

namespace ZaveController.Data_Structures
{
    public class SrcEventArgs : EventArgs
    {
        public SelectionState zSrc { get; set; }

        public SrcEventArgs(SelectionState src)
            : base()
        {
            zSrc = src;
        }
    }
}
