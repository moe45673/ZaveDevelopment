using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using ZaveModel;
using ZaveModel.ZDFEntry;
using System.Windows.Media;
using ZaveViewModel.Commands;
using Prism.Mvvm;
using Prism.Events;
using ZaveGlobalSettings.Events;
using ZaveGlobalSettings.Data_Structures;
using ZaveGlobalSettings.Data_Structures.Observable;

namespace ZaveViewModel.ViewModels
{
    public class ZDFViewModel : BindableBase
    {


        private ZaveModel.ZDF.ZDFSingleton activeZDF;
        private IEventAggregator _eventAggregator;

        //private ZDFEntryItemViewModel _activeZdfEntry;
        //public ZDFEntryItemViewModel ActiveZDFEntry
        //{
        //    get { return _activeZdfEntry; }
        //    set
        //    {
        //        SetProperty<ZDFEntryItemViewModel>(ref _activeZdfEntry, value);
        //    }
        //}



        protected ObservableImmutableList<ZDFEntryItemViewModel> createEntryList(ZaveModel.ZDF.IZDF zdf)
        {
            if (ZDFEntries.Count == 0)
            {
                foreach (var entry in zdf.EntryList)
                {
                    ZDFEntries.Add(new ZDFEntryItemViewModel(entry as ZDFEntry));
                }
            }
            //context = SynchronizationContext.Current;
            return ZDFEntries;


        }

        private readonly object _zdfEntriesLock;
        private ObservableImmutableList<ZDFEntryItemViewModel> _zdfEntries;
        public ObservableImmutableList<ZDFEntryItemViewModel> ZDFEntries
        {
            get { return _zdfEntries; }
            private set
            {
                lock (_zdfEntriesLock)
                {
                    SetProperty(ref _zdfEntries, value);
                }
            }

        }

        private void ModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                switch (e.Action)
                {

                    case NotifyCollectionChangedAction.Add:


                        //foreach (var item in e.NewItems.SyncRoot as List<Object>)
                        //{
                        int index = (e.NewItems.SyncRoot as Array).Length - 1;
                        var tempEntry = (e.NewItems.SyncRoot as Array).GetValue(index);
                        //}
                        //var tempEntry = ((ICollection<ZDFEntry>)e.NewItems.SyncRoot).ToList<ZDFEntry>().FirstOrDefault(x => x!=null);

                        ZDFEntries.Add(new ZDFEntryItemViewModel(tempEntry as ZDFEntry));
                        break;

                    default:
                        System.Windows.Forms.MessageBox.Show("Nothing Done!");
                        break;
                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }



            //ActiveZDFEntry = new ZDFEntryItemViewModel(activeZDF.EntryList[index]);
            //System.Windows.Forms.MessageBox.Show(zdfEntry.Source.SelectionText);
            //ZDFEntries.Add(new ZDFEntryItemViewModel(activeZDF.EntryList[index], _eventAggregator));
            //UpdateGui(zdfEntry.Source);

        }

        //private void ViewPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "TxtDocID")
        //    {
        //        _activeZdfEntry = ZDFEntries.SingleOrDefault(x => x.TxtDocID == sender.ID as ZDFEntryItemViewModel);
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
            var id = items.Cast<ZDFEntryItemViewModel>();
            //var selStateList = SelectionStateList.Instance;
            //selStateList.Add(ZDFEntries.FirstOrDefault(x => x.TxtDocID == id.First<ZDFEntryItemViewModel>().TxtDocID).toSelectionState());
            _eventAggregator.GetEvent<EntryUpdateEvent>().Publish(ZDFEntries.FirstOrDefault(x => x.TxtDocID == id.First<ZDFEntryItemViewModel>().TxtDocID).toSelectionState());

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

        //public ZDFEntryItemViewModel(ZaveModel.ZDF.IZDF zdf, IZDFEntry zdfEntry = null) : base()
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
            ZDFEntries = new ObservableImmutableList<ZDFEntryItemViewModel>();
            if (activeZDF.EntryList.Any<IZDFEntry>())
            {
                foreach (var item in activeZDF.EntryList)
                    ZDFEntries.Add(new ZDFEntryItemViewModel(item as ZDFEntry));
            }


        }

        //private SynchronizationContext context;

        public ZDFViewModel(IEventAggregator eventAggregator)
        {

            _eventAggregator = eventAggregator;

            activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance(_eventAggregator);


            //if (activeZDF.EntryList.Count != 0)
            //_activeZdfEntry = new ZDFEntryItemViewModel(activeZDF.EntryList[0]);
            _zdfEntriesLock = new Object();
            createEntryList();



            activeZDF.EntryList.CollectionChanged += new NotifyCollectionChangedEventHandler(ModelCollectionChanged);
            //_activeZdfEntry.PropertyChanged += new PropertyChangedEventHandler(ViewPropertyChanged);

            //zdfEntry.Source.SelectionDateModified = null;
            //System.Windows.Forms.MessageBox.Show("ViewModelOpened!");


            //activeZDF.Add(zdfEntry);


        }





    }

    public class ZDFEntryItemViewModel : BindableBase
    {

        private IZDFEntry _zdfEntry;
        
        //private string _txtDocId;

        private void setProperties(int id = default(int), string name = default(string), string page = default(string), string txt = default(string), DateTime dateModded = default(DateTime), Color col = default(Color))
        {
            if (_zdfEntry == null)
            {
                throw new NullReferenceException("No ZDFEntryItemViewModel referenced!");
            }

            TxtDocID = id.ToString();
            TxtDocName = name;
            TxtDocPage = page;
            TxtDocText = txt;
            TxtDocLastModified = dateModded.ToShortDateString() + " " + dateModded.ToShortTimeString();
            TxtDocColor = col;


        }

        public SelectionState toSelectionState()
        {
            return _zdfEntry.toSelectionState();
        }

        private void setProperties(SelectionState selState)
        {
            try
            {
                setProperties(selState.ID, selState.SelectionDocName, selState.SelectionPage, selState.SelectionText, selState.SelectionDateModified);
            }
            catch (NullReferenceException nre)
            {
                throw nre;
            }
        }
       
        

        public ZDFEntryItemViewModel(ZDFEntry zdfEntry)
        {
            _zdfEntry = zdfEntry;

            try
            {
                setProperties(zdfEntry.ID, zdfEntry.Name, zdfEntry.Page, zdfEntry.Text, zdfEntry.DateModified);
            }
            catch (NullReferenceException nre)
            {
                throw nre;
            }

        }
        #region Properties

        private String _txtDocName;
        public String TxtDocName
        {
            get { return _zdfEntry.Name; }
            set
            {
                SetProperty(ref _txtDocName, value);
                _zdfEntry.Name = _txtDocName;
                //System.Windows.Forms.MessageBox.Show(value.ToString());


            }

        }

        private String _txtDocID;
        public string TxtDocID
        {
            get { return _zdfEntry.ID.ToString(); }
            private set
            {

                SetProperty(ref _txtDocID, value);
                //_zdfEntry.ID = int.Parse(_txtDocID);
            }

        }

        private String _txtDocPage;
        public String TxtDocPage
        {
            get { return _zdfEntry.Page; }
            set
            {
                if (SetProperty(ref _txtDocPage, value))
                    _zdfEntry.Page = _txtDocPage;

            }

        }

        private String _txtDocText;
        public String TxtDocText
        {
            get { return _zdfEntry.Text; }
            set
            {
                if (SetProperty(ref _txtDocText, value))
                    _zdfEntry.Text = _txtDocText;

            }
        }

        private String _txtDocLastModified;
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
                SetProperty(ref _txtDocLastModified, value);
                _zdfEntry.DateModified = DateTime.Parse(_txtDocLastModified);

            }
        }

        private Color _txtDocColor;
        public Color TxtDocColor
        {
            get
            {
                return _zdfEntry.HColor.toWPFColor();
            }
            set
            {
                SetProperty(ref _txtDocColor, value);
                _zdfEntry.HColor = ZaveModel.Colors.ColorCategory.FromWPFColor(value);

            }
        }

        public ZDFEntry ZDFEntry
        {
            get;
        }

        #endregion 
    }
}
