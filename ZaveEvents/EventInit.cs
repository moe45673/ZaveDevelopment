using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaveController.ZDFSource;
using ZaveGlobalSettings.Data_Structures;
using ZaveModel.ZDFSource;
using ZaveModel;

namespace ZaveController.Global_Settings
{
    public sealed class EventInitSingleton
    {
        

        private static readonly EventInitSingleton instance = new EventInitSingleton();
        public ZaveService.ZDFEntry.IZDFEntryHandler zdfEntryHandler { get; set; }
     

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

            zdfEntryHandler = new ZaveService.ZDFEntry.ZDFEntryHandler(e.zSrc);
        
        }


    }
}
