using System;
//using GalaSoft.MvvmLight;
//using GalaSoft.MvvmLight.CommandWpf;
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
using Prism;
using ZaveGlobalSettings.Events;
using ZaveGlobalSettings.Data_Structures;
using ZaveGlobalSettings.Data_Structures.Observable;
using Prism.Commands;

namespace ZaveViewModel.ViewModels
{
    public class ZDFViewModel : BindableBase
    {


        private ZaveModel.ZDF.ZDFSingleton activeZDF;
        private IEventAggregator _eventAggregator;

        //private ZDFEntryViewModel _activeZdfEntry;
        //public ZDFEntryViewModel ActiveZDFEntry
        //{
        //    get { return _activeZdfEntry; }
        //    set
        //    {
        //        SetProperty<ZDFEntryViewModel>(ref _activeZdfEntry, value);
        //    }
        //}



        protected ObservableImmutableList<ZDFEntryItemViewModel> createEntryList(ZaveModel.ZDF.IZDF zdf)
        {
            if (ZDFEntries.Count == 0)
            {
                foreach (var entry in zdf.EntryList)
                {
                    var item = new ZDFEntryItemViewModel(entry as ZDFEntry);
                    
                    ZDFEntries.Add(item);
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



            //ActiveZDFEntry = new ZDFEntryViewModel(activeZDF.EntryList[index]);
            //System.Windows.Forms.MessageBox.Show(zdfEntry.Source.SelectionText);
            //ZDFEntries.Add(new ZDFEntryViewModel(activeZDF.EntryList[index], _eventAggregator));
            //UpdateGui(zdfEntry.Source);

        }

        //private void ViewPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "TxtDocID")
        //    {
        //        _activeZdfEntry = ZDFEntries.SingleOrDefault(x => x.TxtDocID == sender.ID as ZDFEntryViewModel);
        //    }
        //}

        //private DelegateCommand<System.Collections.IList> _selectItemDelegateCommand;

        /// <summary>
        /// Relay command associated with the selection of an item in the observablecollection
        /// </summary>
        public DelegateCommand<System.Collections.IList> SelectItemDelegateCommand
        {
            get; private set;
        }

        

        /// <summary>
        /// I went with async in case you sub is a long task, and you don't want to lock you UI
        /// </summary>
        /// <returns></returns>
        private void selectItem(System.Collections.IList items)
        {
            var id = items.Cast<ZDFEntryItemViewModel>();

            _eventAggregator.GetEvent<EntryUpdateEvent>().Publish(ZDFEntries.FirstOrDefault(x => x.TxtDocID == id.First<ZDFEntryItemViewModel>().TxtDocID).toSelectionState());
            //SelectionState selstate = ZDFEntries.FirstOrDefault(x => x.TxtDocID == id.First<ZDFEntryViewModel>().TxtDocID).toSelectionState();


           

            //return await Task.FromResult(1);
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
            SelectedItem = true;
            SelectItemDelegateCommand = new DelegateCommand<IList>(selectItem, canSelectItem);

            activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance(_eventAggregator);


            //if (activeZDF.EntryList.Count != 0)
            //_activeZdfEntry = new ZDFEntryViewModel(activeZDF.EntryList[0]);
            _zdfEntriesLock = new Object();
            createEntryList();



            activeZDF.EntryList.CollectionChanged += new NotifyCollectionChangedEventHandler(ModelCollectionChanged);
            //_activeZdfEntry.PropertyChanged += new PropertyChangedEventHandler(ViewPropertyChanged);

            //zdfEntry.Source.SelectionDateModified = null;
            //System.Windows.Forms.MessageBox.Show("ViewModelOpened!");


            //activeZDF.Add(zdfEntry);


        }

        public bool SelectedItem { get; set; }
        private bool canSelectItem(IList arg)
        {
            return SelectedItem;
        }
    }

    public class ZDFEntryItemViewModel : ZaveViewModel.Data_Structures.ZDFEntryItem
    {

       
       
        

        public ZDFEntryItemViewModel(ZDFEntry zdfEntry) : base(zdfEntry)
        {
            

        }
        
    }
}
