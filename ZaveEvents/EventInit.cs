using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaveController.ZDFSource;
using ZaveGlobalSettings.Data_Structures;
using ZaveService.ZDFEntry;

namespace ZaveController.Global_Settings
{
    public sealed class EventInitSingleton
    {
        

        private static readonly EventInitSingleton instance = new EventInitSingleton();
        public ZDFEntryHandler zdfEntryHandler { get; set; }
     

        private EventInitSingleton()
        {
            
        }

        public static EventInitSingleton Instance
        {
            get
            {               
                return instance;
            }
        }

        public void SrcHighlightEventHandler(Object o, Data_Structures.SrcEventArgs e)
        {

            zdfEntryHandler = new ZaveService.ZDFEntry.DefaultZDFEntryHandler(e.zSrc);
        
        }


    }
}
