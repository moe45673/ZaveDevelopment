using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using ZaveModel;
using ZaveModel.ZDFEntry;
using System.Windows.Media;
using ZaveViewModel.Commands;

//using ZaveModel.Factories.ZDFEntry;
using ZaveGlobalSettings.Data_Structures;
//using Zave


namespace ZaveViewModel.ZDFViewModel
{
    //using activeZDF = ZaveModel.ZDF.ZDFSingleton;
    public class ZDFViewModel : ZaveGlobalSettings.Data_Structures.ObservableObject
    {
        private ZDFEntryViewModel _activeZdfEntry;

        private ZaveModel.ZDF.ZDFSingleton activeZDF = ZaveModel.ZDF.ZDFSingleton.Instance;

        //private ICommand _getZDFEntryCommand;
        //private ICommand _saveZDFEntryCommand;



        public ZDFEntryViewModel ActiveZDFEntry
        {
            get { return _activeZdfEntry; }
            set
            {
                _activeZdfEntry = value;

                OnPropertyChanged("ActiveZDFEntry");
            }
        }

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

        protected ObservableCollection<ZDFEntryViewModel> createEntryList(ZaveModel.ZDF.IZDF zdf)
        {
            if (ZDFEntries.Count == 0)
            {
                foreach (var entry in zdf.EntryList)
                {
                    ZDFEntries.Add(new ZDFEntryViewModel(entry));
                }
            }
            return ZDFEntries;


        }

        private readonly object _zdfEntriesLock;
        private ObservableCollection<ZDFEntryViewModel> _zdfEntries;
        public ObservableCollection<ZDFEntryViewModel> ZDFEntries
        {
            get { return _zdfEntries; }
            private set
            {
                _zdfEntries = value;
                BindingOperations.EnableCollectionSynchronization(_zdfEntries, _zdfEntriesLock);
            }
            
        }

        private void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "EntryList") {
                int index = activeZDF.EntryList.Count - 1;
                
                //ActiveZDFEntry = new ZDFEntryViewModel(activeZDF.EntryList[index]);
                //System.Windows.Forms.MessageBox.Show(zdfEntry.Source.SelectionText);
                ZDFEntries.Add(new ZDFEntryViewModel(activeZDF.EntryList[index]));
                //UpdateGui(zdfEntry.Source);
            }
        }

        //private void ViewPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "TxtDocID")
        //    {
        //        _activeZdfEntry = ZDFEntries.SingleOrDefault(x => x.TxtDocID == sender.ID as ZDFEntryViewModel);
        //    }
        //}

        private RelayCommand<System.Collections.IList> _selectItemRelayCommand;

        /// <summary>
        /// Relay command associated with the selection of an item in the observablecollection
        /// </summary>
        public RelayCommand<System.Collections.IList> SelectItemRelayCommand
        {
            get
            {
                if (_selectItemRelayCommand == null)
                {
                    _selectItemRelayCommand = new RelayCommand<System.Collections.IList>(async (id) =>
                    {
                        await selectItem(id);
                    });
                }

                return _selectItemRelayCommand;
            }
            set { _selectItemRelayCommand = value; }
        }

        /// <summary>
        /// I went with async in case you sub is a long task, and you don't want to lock you UI
        /// </summary>
        /// <returns></returns>
        private async Task<int> selectItem(System.Collections.IList items)
        {
            var id = items.Cast<ZDFEntryViewModel>();
            this.ActiveZDFEntry = ZDFEntries.FirstOrDefault(x => x.TxtDocID == id.First<ZDFEntryViewModel>().TxtDocID);
            
            //Do async work

            return await Task.FromResult(1);
        }

        //public void UpdateGui(SelectionState selState)
        //{
        //    TxtDocName = selState.SelectionDocName;
        //    TxtDocPage = selState.SelectionPage;
        //    TxtDocText = selState.SelectionText;
        //    TxtDocLastModified = selState.SelectionDateModified.ToShortDateString() + " " + selState.SelectionDateModified.ToShortTimeString();
        //    //System.Windows.Forms.MessageBox.Show(TxtDocText);
        //}

        //public ICommand SaveZDFEntryCommand
        //{
        //    get
        //    {
        //        if (_saveZDFEntryCommand == null)
        //        {
        //            _saveZDFEntryCommand = new RelayCommand(
        //                param => SaveZDFEntry(),
        //                param => (zdfEntry != null)
        //            );
        //        }
        //        return _saveZDFEntryCommand;
        //    }
        //}

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

        private void createEntryList()
        {
            ZDFEntries = new ObservableCollection<ZDFEntryViewModel>();
            ZDFEntries = createEntryList(activeZDF);
            
        }

        public ZDFViewModel()
        {
           

            activeZDF = ZaveModel.ZDF.ZDFSingleton.Instance;

            if (activeZDF.EntryList.Count != 0)
                _activeZdfEntry = new ZDFEntryViewModel(activeZDF.EntryList[0]);
            _zdfEntriesLock = new Object();
            createEntryList();

            

            activeZDF.PropertyChanged += new PropertyChangedEventHandler(ModelPropertyChanged);
            //_activeZdfEntry.PropertyChanged += new PropertyChangedEventHandler(ViewPropertyChanged);

            //zdfEntry.Source.SelectionDateModified = null;
            //System.Windows.Forms.MessageBox.Show("ViewModelOpened!");


            //activeZDF.Add(zdfEntry);


        }

        ~ZDFViewModel()
        {
            activeZDF.PropertyChanged -= new PropertyChangedEventHandler(this.ModelPropertyChanged);
//#if DEBUG
//            System.Windows.Forms.MessageBox.Show("ViewModel Closed!");
//#endif
        }



       
    }





    public class ZDFEntryViewModel : ZaveGlobalSettings.Data_Structures.ObservableObject
    {

        private IZDFEntry _zdfEntry;
        //private string _txtDocId;

        private void setProperties(int id, string name, string page, string txt, DateTime dateModded)
        {
            if (_zdfEntry == null) { 
                throw new NullReferenceException("No ZDFEntryViewModel referenced!");
            }

            TxtDocID = id.ToString();
            TxtDocName = name;
            TxtDocPage = page;
            TxtDocText = txt;
            TxtDocLastModified = dateModded.ToShortDateString() + " " + dateModded.ToShortTimeString();
            
        }

        

        private void setProperties(SelectionState selState)
        {
            try
            {
                setProperties(selState.ID, selState.SelectionDocName, selState.SelectionPage, selState.SelectionText, selState.SelectionDateModified);
            }
            catch(NullReferenceException nre)
            {
                throw nre;
            }
        }

        public ZDFEntryViewModel(IZDFEntry zdfEntry)
        {
            _zdfEntry = zdfEntry;

            try
            {
                setProperties(zdfEntry.ID, zdfEntry.Name, zdfEntry.Page, zdfEntry.Text, zdfEntry.DateModified);
            }
            catch(NullReferenceException nre)
            {
                throw nre;
            }
            
        }
        #region Properties


        public String TxtDocName
        {
            get { return _zdfEntry.Name; }
            set
            {


                _zdfEntry.Name = value;

                OnPropertyChanged("TxtDocName");
                //System.Windows.Forms.MessageBox.Show(value.ToString());


            }

        }

        public string TxtDocID
        {
            get { return _zdfEntry.ID.ToString(); }
            private set
            {
                
                OnPropertyChanged("TxtDocID");
            }
            
        }

        public String TxtDocPage
        {
            get { return _zdfEntry.Page; }
            set
            {
                _zdfEntry.Page = value;
                OnPropertyChanged("TxtDocPage");
            }

        }

        public String TxtDocText
        {
            get { return _zdfEntry.Text; }
            set
            {
                _zdfEntry.Text = value;
                OnPropertyChanged("TxtDocText");
            }
        }

        public String TxtDocLastModified
        {
            get
            {
                string date;
                if (!_zdfEntry.DateModified.Equals(default(DateTime)))
                    date = _zdfEntry.DateModified.ToShortDateString() + " " + _zdfEntry.DateModified.ToShortTimeString();
                else
                    date = "";
                return date;

            }
            set
            {
                _zdfEntry.DateModified = DateTime.Parse(value);
                OnPropertyChanged("TxtDocLastModified");
            }
        }

        public String TxtDocColor
        {
            get
            {
                return _zdfEntry.HColor.getColor().ToString();
            }
            set
            {
                _zdfEntry.HColor.setColor((Color)ColorConverter.ConvertFromString(value));
                OnPropertyChanged("TxtDocColor");
            }
        }

        #endregion 
    }
}
