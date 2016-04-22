using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using ZaveModel;
using ZaveModel.ZDFEntry;

//using ZaveModel.Factories.ZDFEntry;
using ZaveGlobalSettings.Data_Structures;
//using Zave

namespace ZaveViewModel.ZDFEntryViewModel
{
    //using activeZDF = ZaveModel.ZDF.ZDFSingleton;
    public class ZDFEntryViewModel : INotifyPropertyChanged
    {
        //public static ZDFEntry.ZDFEntry ZdfEntry { get; set; }
        

        public event PropertyChangedEventHandler PropertyChanged;

        private ZaveModel.ZDF.ZDFSingleton activeZDF;

        private IZDFEntry zdfEntry;

        //public ZaveModel.ZDF.IZDF ActiveZDF
        //{
        //    get { return activeZDF; }
        //    set
        //    {
        //        activeZDF = value;
                
        //        this.NotifyPropertyChanged("ActiveZDF");
        //    }
        //}

        //public IZDFEntry ZDFEntry
        //{
        //    get { return zdfEntry; }
        //    set
        //    {
        //        if (zdfEntry != null)
        //            zdfEntry.PropertyChanged -= ModelPropertyChanged;

        //        if (value != null)
        //        {
        //            value.PropertyChanged += ModelPropertyChanged;
        //        }

        //        zdfEntry = value;
        //        NotifyPropertyChanged("Model");
        //    }
        //}

        private void ModelPropertyChanged(object sender, ModelEventArgs e)
        {
            zdfEntry = activeZDF.EntryList.Find(x => x.Title == e.Description);
            System.Windows.Forms.MessageBox.Show(zdfEntry.Source.SelectionText);
            UpdateGui(e.SelState);
        }

        public void UpdateGui(SelectionState selState)
        {
            TxtDocName = selState.SelectionDocName;
            TxtDocPage = selState.SelectionPage;
            TxtDocText = selState.SelectionText;
            System.Windows.Forms.MessageBox.Show(TxtDocText);
        }

        //public ZDFEntryViewModel(ZaveModel.ZDF.IZDF zdf, IZDFEntry zdfEntry = null) : base()
        //{



        //   ActiveZDF = zdf;
        //    if (this.zdfEntry != null)
        //    {
        //        ActiveZDF.Add(zdfEntry);
        //        this.zdfEntry = zdfEntry;
        //    }

        //    else
        //        this.zdfEntry = activeZDF.EntryList[activeZDF.EntryList.Count - 1];


        //}

        public ZDFEntryViewModel()
        {
            this.zdfEntry = new ZDFEntry();

            activeZDF = ZaveModel.ZDF.ZDFSingleton.Instance;

            
            activeZDF.ModelPropertyChanged += new EventHandler<ZaveGlobalSettings.Data_Structures.ModelEventArgs>(this.ModelPropertyChanged);
            
            
            System.Windows.Forms.MessageBox.Show("ViewModelOpened!");
            
            //activeZDF.Add(zdfEntry);
            

        }

        ~ZDFEntryViewModel()
        {
            activeZDF.ModelPropertyChanged -= new EventHandler<ZaveGlobalSettings.Data_Structures.ModelEventArgs>(this.ModelPropertyChanged);
            System.Windows.Forms.MessageBox.Show("ViewModel Closed!");
        }

        

        //public void ShowMessage(object obj)
        //{
            

        //}
        
        

        public String TxtDocName{
            get{return zdfEntry.Source.SelectionDocName; }
            set
            {

                
                zdfEntry.Source.SelectionDocName = value;

                NotifyPropertyChanged("TxtDocName");
                //System.Windows.Forms.MessageBox.Show(value.ToString());
               
                
            }
        
        }

        public String TxtDocPage
        {
            get { return zdfEntry.Source.SelectionPage; }
            set
            {
                zdfEntry.Source.SelectionPage = value;
            }

        }

        public String TxtDocText
        {
            get { return zdfEntry.Source.SelectionText; }
            set
            {
                zdfEntry.Source.SelectionText = value;
            }
        }

        private void NotifyPropertyChanged(String info)
        {
            var handler = PropertyChanged;
            if(handler != null)
            handler(this, new PropertyChangedEventArgs(info));

        }
    }
}
