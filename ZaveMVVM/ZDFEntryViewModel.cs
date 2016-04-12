using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaveModel;
using ZaveModel.ZDF;
using ZaveModel.ZDFSource;
using ZaveModel.Factories.ZDFEntry;
using ZaveGlobalSettings.Data_Structures;

namespace ZaveMVVM.ZDFEntryViewModel
{
    public class ZDFEntryViewModel
    {
        //public static ZDFEntry.ZDFEntry ZdfEntry { get; set; }

        private ZaveModel.IZDFEntry zdfEntry;

        public ZDFEntryViewModel()
        {
            using (DefaultZDFEntryFactory dzdff = new DefaultZDFEntryFactory())
            {
                try
                {
                    zdfEntry = dzdff.produceZDFEntry("default");
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
        }

        public String TxtDocname{
            get{return zdfEntry.Source.DocName;}
            set
            {
                zdfEntry.Source.DocName = value;
            }
        
        }

        public String TxtDocPage
        {
            get { return zdfEntry.Source.DocPage; }
            set
            {
                zdfEntry.Source.DocPage = value;
            }

        }

        public String TxtDocText
        {
            get { return zdfEntry.Source.DocText; }
            set
            {
                zdfEntry.Source.DocText = value;
            }
        }
        
        public void setTextBoxes(ZaveModel.ZDFSource.Source zsrc)
        {
            
            //zdfEntry.
            //zsrc.DocName;
            //zsrc.DocPage;
            //zsrc.DocText;
        }
    }
}
