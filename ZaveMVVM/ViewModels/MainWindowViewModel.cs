using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Events;
using ZaveViewModel.ZDFViewModel;
using ZaveModel.ZDFEntry;
using System.Collections.ObjectModel;
using ZaveGlobalSettings.Events;
using ZaveGlobalSettings.Data_Structures;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Data;
using GalaSoft.MvvmLight.CommandWpf;

namespace ZaveViewModel.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        //private readonly IRegionManager _regionManager;

        //public DelegateCommand<string> NavigateCommand { get; set; }

        //public ZaveMainWindowViewModel(IRegionManager regionManager)
        //{
        //    _regionManager = regionManager;
        //    NavigateCommand = new DelegateCommand<string>(Navigate);

        //}

        //private void Navigate(string uri)
        //{
        //    _regionManager.RequestNavigate("ContentRegion", uri);
        //}

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



        protected ObservableCollection<ZDFEntryViewModel> createEntryList(ZaveModel.ZDF.IZDF zdf)
        {
            if (ZDFEntries.Count == 0)
            {
                foreach (var entry in zdf.EntryList)
                {
                    ZDFEntries.Add(new ZDFEntryViewModel(entry));
                }
            }
            context = SynchronizationContext.Current;
            return ZDFEntries;


        }

        private readonly object _zdfEntriesLock;
        private ObservableCollection<ZDFEntryViewModel> _zdfEntries;
        public ObservableCollection<ZDFEntryViewModel> ZDFEntries
        {
            get { return _zdfEntries; }
            private set
            {
                SetProperty(ref _zdfEntries, value);
            }

        }

        private void ModelPropertyChanged(SelectionStateList selStateList)
        {
            context.Send(x =>
            {
                ZDFEntries.Clear();
                foreach (var item in selStateList)
                {
                    ZDFEntries.Add(new ZDFEntryViewModel(new ZDFEntry(item)));
                }
            }, null);
      
                
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
            //var selStateList = SelectionStateList.Instance;
            //selStateList.Add(ZDFEntries.FirstOrDefault(x => x.TxtDocID == id.First<ZDFEntryViewModel>().TxtDocID).toSelectionState());
            _eventAggregator.GetEvent<EntryUpdateEvent>().Publish(ZDFEntries.FirstOrDefault(x => x.TxtDocID == id.First<ZDFEntryViewModel>().TxtDocID).toSelectionState());

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

        private SynchronizationContext context;

        public MainWindowViewModel(IEventAggregator eventAggregator)
        {

            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ZDFUpdateEvent>().Subscribe(ModelPropertyChanged);
            activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance(eventAggregator);
           

            if (activeZDF.EntryList.Count != 0)
                //_activeZdfEntry = new ZDFEntryViewModel(activeZDF.EntryList[0]);
                _zdfEntriesLock = new Object();
            createEntryList();
           


            //activeZDF.PropertyChanged += new PropertyChangedEventHandler(ModelPropertyChanged);
            //_activeZdfEntry.PropertyChanged += new PropertyChangedEventHandler(ViewPropertyChanged);

            //zdfEntry.Source.SelectionDateModified = null;
            //System.Windows.Forms.MessageBox.Show("ViewModelOpened!");


            //activeZDF.Add(zdfEntry);


        }

        ~MainWindowViewModel()
        {
            //activeZDF.PropertyChanged -= new PropertyChangedEventHandler(this.ModelPropertyChanged);
            //#if DEBUG
            //            System.Windows.Forms.MessageBox.Show("ViewModel Closed!");
            //#endif
        }
    }
}
