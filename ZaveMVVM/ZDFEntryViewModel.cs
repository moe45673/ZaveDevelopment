using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using ZaveModel;
using ZaveModel.ZDFEntry;
using ZaveViewModel.Commands;

//using ZaveModel.Factories.ZDFEntry;
using ZaveGlobalSettings.Data_Structures;
//using Zave

namespace ZaveViewModel.ZDFEntryViewModel
{
    //using activeZDF = ZaveModel.ZDF.ZDFSingleton;
    public class ZDFEntryViewModel : ObservableObject
    {
        //public static ZDFEntry.ZDFEntry ZdfEntry { get; set; }




        private ZaveModel.ZDF.ZDFSingleton activeZDF = ZaveModel.ZDF.ZDFSingleton.Instance;

        //private ICommand _getZDFEntryCommand;
        private ICommand _saveZDFEntryCommand;

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

        public ZaveModel.ZDF.ZDFSingleton ActiveZDF
        {
            get
            {
                return activeZDF;
            }
            set
            {
                activeZDF = value;
                OnPropertyChanged("ActiveZDF");
            }
        }

        private void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "EntryList") {
                int index = activeZDF.EntryList.Count - 1;
                zdfEntry = activeZDF.EntryList[index];
                System.Windows.Forms.MessageBox.Show(zdfEntry.Source.SelectionText);
                UpdateGui(zdfEntry.Source);
            }
        }

        public void UpdateGui(SelectionState selState)
        {
            TxtDocName = selState.SelectionDocName;
            TxtDocPage = selState.SelectionPage;
            TxtDocText = selState.SelectionText;
            System.Windows.Forms.MessageBox.Show(TxtDocText);
        }

        public ICommand SaveZDFEntryCommand
        {
            get
            {
                if (_saveZDFEntryCommand == null)
                {
                    _saveZDFEntryCommand = new RelayCommand(
                        param => SaveZDFEntry(),
                        param => (zdfEntry != null)
                    );
                }
                return _saveZDFEntryCommand;
            }
        }

        private void SaveZDFEntry()
        {
            
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

            //activeZDF = ZaveModel.ZDF.ZDFSingleton.Instance;

            
            activeZDF.PropertyChanged += new PropertyChangedEventHandler(ModelPropertyChanged);

#if DEBUG
            System.Windows.Forms.MessageBox.Show("ViewModelOpened!");
#endif

            //activeZDF.Add(zdfEntry);


        }

        ~ZDFEntryViewModel()
        {
            activeZDF.PropertyChanged -= new PropertyChangedEventHandler(this.ModelPropertyChanged);
#if DEBUG
            System.Windows.Forms.MessageBox.Show("ViewModel Closed!");
#endif
        }



        //public void ShowMessage(object obj)
        //{


        //}



        public String TxtDocName{
            get{return zdfEntry.Source.SelectionDocName; }
            set
            {

                
                zdfEntry.Source.SelectionDocName = value;

                OnPropertyChanged("TxtDocName");
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

        
    }
}
