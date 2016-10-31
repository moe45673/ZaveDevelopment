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
using System.Threading;
using ZaveModel;
using ZaveModel.ZDFEntry;
using System.Windows.Media;
using Prism.Mvvm;
using Prism.Events;
using Prism;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZaveGlobalSettings.Data_Structures;
using Prism.Commands;
using ZaveGlobalSettings.Data_Structures.ZaveObservableCollection;
using Microsoft.Practices.Unity;
using ZaveModel.ZDF;
using ZaveViewModel.Data_Structures;



namespace ZaveViewModel.ViewModels
{
    

    
    
    public class ZDFViewModel : BindableBase
    {
        private string activeSort;
        private void getActiveSort()
        {
            activeSort = MainContainerViewModel.ACTIVESORT;
        }
        private ZaveModel.ZDF.ZDFSingleton _activeZdf;
        private IEventAggregator _eventAggregator;
        private IUnityContainer _container;

        //private ZDFEntryViewModel _activeZdfEntry;
        //public ZDFEntryViewModel ActiveZDFEntry
        //{
        //    get { return _activeZdfEntry; }
        //    set
        //    {
        //        SetProperty<ZDFEntryViewModel>(ref _activeZdfEntry, value);
        //    }
        //}

        

        protected ObservableImmutableList<ZdfEntryItemViewModel> CreateEntryList(ZaveModel.ZDF.IZDF zdf)
        {
            if (ZdfEntries.Count == 0)
            {
                foreach (var entry in zdf.EntryList)
                {
                    var item = new ZdfEntryItemViewModel(entry as ZDFEntry);
                    
                    ZdfEntries.Add(item);
                }
            }
            //context = SynchronizationContext.Current;
            return ZdfEntries;


        }

        private readonly object _zdfEntriesLock;
        private ObservableImmutableList<ZdfEntryItemViewModel> _zdfEntries;
        public ObservableImmutableList<ZdfEntryItemViewModel> ZdfEntries
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
                        var tempEntry = (e.NewItems.SyncRoot as Array).GetValue(index); //get new entry

                        var list = ZdfEntries.ToList(); //create temp list to add item to and then sort


                        list.Add(new ZdfEntryItemViewModel(tempEntry as ZDFEntry));

                        list = ZDFSorting.EntrySort(list, activeSort);                        
                        ZdfEntries.Clear();
                        ZdfEntries.AddRange(list);
                        //MessageBox.Show(Thread.CurrentThread.ManagedThreadId.ToString());
                        break;

                    case NotifyCollectionChangedAction.Move:
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        break;

                    case NotifyCollectionChangedAction.Replace:
                        break;

                    case NotifyCollectionChangedAction.Reset:
                        ZdfEntries.Clear();
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

        private void modelOpened(object zdf)
        {
            //var jObject = zdf as JObject;
            //_activeZdf = JsonConvert.DeserializeObject<ZaveModel.ZDF.ZDFSingleton>(jObject.ToString());
            //_activeZdf = ZDFSingleton.GetInstance(_eventAggregator);
            //JArray ja = (JArray)jObject["EntryList"]["_items"];

            ////activeZdf.EntryList = new ObservableImmutableList<IZDFEntry>(ja.ToObject<List<ZDFEntry>>());

            //foreach (var item in (ja.ToObject<List<ZDFEntry>>()))
            //{
            //    _activeZdf.EntryList.Add(item);
            //}
            CreateEntryList();

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
        ///
        /// </summary>
        /// <returns></returns>
        private void SelectItem(System.Collections.IList items)
        {
            string id = "";
            if (items.Count > 0)
            {
                var entryItems = items.Cast<ZdfEntryItemViewModel>();

                id = ZdfEntries.FirstOrDefault(x => x.TxtDocID == entryItems.First<ZdfEntryItemViewModel>().TxtDocID).TxtDocID;
                
                //SelectionState selstate = ZDFEntries.FirstOrDefault(x => x.TxtDocID == id.First<ZDFEntryViewModel>().TxtDocID).toSelectionState();
                
            }

            _eventAggregator.GetEvent<EntrySelectedEvent>().Publish(id);

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

        private void SaveZdfEntry()
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

        private void CreateEntryList()
        {

            _activeZdf.EntryList.CollectionChanged -= new NotifyCollectionChangedEventHandler(ModelCollectionChanged);
            _activeZdf.EntryList.CollectionChanged += new NotifyCollectionChangedEventHandler(ModelCollectionChanged);

            if (_zdfEntries == null)
            {
                _zdfEntries = new ObservableImmutableList<ZdfEntryItemViewModel>();
            }
            else
            {
                _zdfEntries.Clear();
                
            }
            //DateTime date = DateTime.Now;
            //SelectionState selState1 = new SelectionState(0, "ExampleDoc1.doc", "32", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur pharetra dapibus dolor quis tincidunt. Curabitur leo dui, blandit in consequat eget, luctus ac magna. Quisque leo neque, tincidunt eu ultricies fringilla, convallis eu odio. Vestibulum fringilla mauris id ipsum lobortis, ac accumsan nisi tristique. Sed cursus varius neque eu bibendum. Nam fringilla diam eget turpis pharetra, ac congue urna auctor. Phasellus feugiat, purus ac venenatis varius, risus nisl porta lectus, nec pharetra ipsum velit congue massa. Pellentesque tempus vehicula elit, dictum venenatis mi hendrerit sed. Etiam et diam elementum, tristique est eget, aliquam massa. In id auctor augue. Integer accumsan ante ut ligula pellentesque dictum.\nSed augue dui, faucibus ac neque eget, euismod dignissim mi.Nullam nec varius nulla.In ut enim elit.Sed in leo non nisi ultrices lacinia.Mauris eleifend lectus purus, eget blandit ante suscipit vel.Nunc hendrerit nisl et nunc sodales volutpat.Proin quis metus quam.Proin eget felis tortor.Fusce eget imperdiet velit.\nDuis porta molestie dui, eget facilisis massa venenatis ac.Integer in condimentum est, at iaculis enim.Duis tempus efficitur est, eget sollicitudin turpis.Suspendisse leo velit, aliquet tristique quam id, vulputate tempus purus.Phasellus aliquam aliquet neque at tincidunt.Nam vulputate consequat nulla eu bibendum.Suspendisse auctor, sapien mollis laoreet lacinia, eros velit fermentum purus, non dictum odio tellus vitae diam.Sed enim risus, aliquam sit amet tristique in, interdum in augue.Nunc viverra pulvinar elit eget venenatis.Sed laoreet neque sed nibh fringilla scelerisque.Proin vestibulum rhoncus elit, vel convallis ligula pellen", date.AddMinutes(360), System.Drawing.Color.Red, SrcType.WORD, new List<object>());
            //SelectionState selState2 = new SelectionState(1, "ExampleDoc2.doc", "33", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur pharetra dapibus dolor quis tincidunt. Curabitur leo dui, blandit in consequat eget, luctus ac magna. Quisque leo neque, tincidunt eu ultricies fringilla, convallis eu odio. Vestibulum fringilla mauris id ipsum lobortis, ac accumsan nisi tristique. Sed cursus varius neque eu bibendum. Nam fringilla diam eget turpis pharetra, ac congue urna auctor. Phasellus feugiat, purus ac venenatis varius, risus nisl porta lectus, nec pharetra ipsum velit congue massa. Pellentesque tempus vehicula elit, dictum venenatis mi hendrerit sed. Etiam et diam elementum, tristique est eget, aliquam massa. In id auctor augue. Integer accumsan ante ut ligula pellentesque dictum.\nSed augue dui, faucibus ac neque eget, euismod dignissim mi.Nullam nec varius nulla.In ut enim elit.Sed in leo non nisi ultrices lacinia.Mauris eleifend lectus purus, eget blandit ante suscipit vel.Nunc hendrerit nisl et nunc sodales volutpat.Proin quis metus quam.Proin eget felis tortor.Fusce eget imperdiet velit.\nDuis porta molestie dui, eget facilisis massa venenatis ac.Integer in condimentum est, at iaculis enim.Duis tempus efficitur est, eget sollicitudin turpis.Suspendisse leo velit, aliquet tristique quam id, vulputate tempus purus.Phasellus aliquam aliquet neque at tincidunt.Nam vulputate consequat nulla eu bibendum.Suspendisse auctor, sapien mollis laoreet lacinia, eros velit fermentum purus, non dictum odio tellus vitae diam.Sed enim risus, aliquam sit amet tristique in, interdum in augue.Nunc viverra pulvinar elit eget venenatis.Sed laoreet neque sed nibh fringilla scelerisque.Proin vestibulum rhoncus elit, vel convallis ligula pellen", date.AddMinutes(360), System.Drawing.Color.Yellow, SrcType.WORD, new List<object>());
            
            
            if (_activeZdf.EntryList.Any<IZDFEntry>())
            {
                foreach (var item in _activeZdf.EntryList)
                    ZdfEntries.Add(new ZdfEntryItemViewModel(item as ZDFEntry));

            }

            //ZdfEntries.Add(new ZdfEntryItemViewModel(new ZDFEntry(selState1)));
            //ZdfEntries.Add(new ZdfEntryItemViewModel(new ZDFEntry(selState2)));
            

            //System.Windows.Forms.MessageBox.Show(" Done1");
        }

        //private SynchronizationContext context;

        public ZDFViewModel(IEventAggregator eventAggregator, IUnityContainer container)
        {
            
            _eventAggregator = eventAggregator;
            SelectedItem = true;
            SelectItemDelegateCommand = new DelegateCommand<IList>(SelectItem, CanSelectItem);

            _container = container;

            _activeZdf = ZDFSingleton.GetInstance(_eventAggregator);

            //MessageBox.Show(Thread.CurrentThread.ManagedThreadId.ToString());
            //if (activeZDF.EntryList.Count != 0)
            //_activeZdfEntry = new ZDFEntryViewModel(activeZDF.EntryList[0]);
            _zdfEntriesLock = new Object();
            CreateEntryList();
            getActiveSort();

            //_activeZdf.EntryList.CollectionChanged += new NotifyCollectionChangedEventHandler(ModelCollectionChanged);
            _eventAggregator.GetEvent<ZDFOpenedEvent>().Subscribe(modelOpened);
            //_activeZdfEntry.PropertyChanged += new PropertyChangedEventHandler(ViewPropertyChanged);

            //zdfEntry.Source.SelectionDateModified = null;
            //System.Windows.Forms.MessageBox.Show("ViewModelOpened!");


            //activeZDF.Add(zdfEntry);


        }

        public ZDFSingleton GetModel()
        {
            return _activeZdf;
        }

        public void SetModel(Object obj)
        {
            _activeZdf = obj as ZaveModel.ZDF.ZDFSingleton;
            CreateEntryList();

        }

        public bool SelectedItem { get; set; }
        private bool CanSelectItem(IList arg)
        {
            return SelectedItem;
        }


        private string _name;

        public string Name
        {
            get { return _activeZdf.Name; }
            set {
                _activeZdf.Name = value;
                SetProperty(ref _name, value); }
        }
    }

    public class ZdfEntryItemViewModel : ZaveViewModel.Data_Structures.ZDFEntryItem
    {

       
       
        

        public ZdfEntryItemViewModel(ZDFEntry zdfEntry) : base(zdfEntry)
        {
            

        }

        protected override void AddComment()
        {
            throw new NotImplementedException();
        }

        protected override void EditComment(IList commentList)
        {
            throw new NotImplementedException();
        }
    }

    
}
