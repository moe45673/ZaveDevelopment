using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaveSourceAdapter.ZDFSource;
using ZaveSourceAdapter.Data_Structures;
using ZaveModel.ZDFSource;

namespace ZaveSourceAdapter.Global_Settings
{
    public sealed class EventInitSingleton
    {
        

        private static readonly EventInitSingleton instance = new EventInitSingleton();

     

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

        public void SrcHighlightEventHandler(Object o, ZaveSourceAdapter.Data_Structures.SrcEventArgs e)
        {

            Zave.MVVM.View.MainWindow.MainWindowViewModel.setTextBoxes(e.zSrc);
            



        }


    }
}
