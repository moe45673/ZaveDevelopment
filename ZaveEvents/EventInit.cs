using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZaveController.ZDFSource;
using ZaveGlobalSettings.Data_Structures;
using ZaveService.ZDFEntry;
using ZaveModel;
using System.IO;
//using ZaveMo

namespace ZaveController.Global_Settings
{

    
    
    public sealed class EventInitSingleton
    {

        public ZaveModel.ZDF.ZDFSingleton activeZDF;
        private static readonly EventInitSingleton instance = new EventInitSingleton();
        public ZDFEntryHandler zdfEntryHandler { get; set; }
        

        private EventInitSingleton()
        {
            activeZDF = ZaveModel.ZDF.ZDFSingleton.Instance;
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



            ZaveModel.ZDFEntry.ZDFEntry entry = new ZaveModel.ZDFEntry.ZDFEntry();
            //zdfEntryHandler = new ZaveService.ZDFEntry.DefaultZDFEntryHandler(e.zSrc, activeZDF);
            //zdfEntryHandler.CreateZDFEntry(new ZaveModel.ZDFEntry.ZDFEntry(e.zSrc));
            entry.Source = e.zSrc;
           
            activeZDF.Add(entry);

            
            //zevm.TxtDocName = activeZDF.EntryList[activeZDF.EntryList.Count - 1].Source.SelectionDocName;
            
            
            
        }


    }

    

    
}
