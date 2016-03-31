using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstWordAddIn;
using FirstWordAddIn.DataStructures;
using ZaveSourceAdapter.ZDFSource;
using ZaveSourceAdapter;

namespace ZaveSourceAdapter.Global_Settings
{
    public sealed class EventInitSingleton
    {
        private MainWindow mainwin;

        private static readonly EventInitSingleton instance = new EventInitSingleton();

        private EventInitSingleton()
        {
            ThisAddIn.WordFired += new EventHandler<WordEventArgs>(SrcHighlightEventHandler);
        }

        public static EventInitSingleton Instance
        {
            get
            {               
                return instance;
            }
        }

        private void SrcHighlightEventHandler(Object o, SrcEventArgs e)
        {
            if (e.selDat.st.Equals(ZaveSourceAdapter.Data_Structures.SrcType.WORD))
            {
                SourceFactory sf = new WordSourceFactory();
                Source ws = sf.produceSource(e.selDat);

                mainwin = (MainWindow)App.Current.MainWindow;
                mainwin.setTextBoxes(e.selDat.SelectionDocName, e.selDat.SelectionPage, e.selDat.SelectionText);
                
                
            }
        }


    }
}
