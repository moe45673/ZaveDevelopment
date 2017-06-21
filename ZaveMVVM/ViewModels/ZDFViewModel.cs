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
using ZaveService.ZDFEntry;


namespace ZaveViewModel.ViewModels
{
    

    
    
    public class ZDFViewModel : BindableBase
    {
        private string activeSort;
        private void getActiveSort()
        {
            activeSort = MainContainerViewModel.ACTIVESORT;
        }
        private ZDFSingleton _activeZdf;
        private IEventAggregator _eventAggregator;
        private IUnityContainer _container;
        private IZDFEntryService _entryService;

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
                    var item = new ZdfEntryItemViewModel(_container, entry as ZDFEntry);
                    
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

                        var addlist = ZdfEntries.ToList(); //create temp list to add item to and then sort


                        addlist.Add(new ZdfEntryItemViewModel(_container, tempEntry as ZDFEntry));

                        addlist = ZDFSorting.EntrySort(addlist, activeSort);                        
                        ZdfEntries.Clear();
                        ZdfEntries.AddRange(addlist);
                        //MessageBox.Show(Thread.CurrentThread.ManagedThreadId.ToString());
                        break;

                    case NotifyCollectionChangedAction.Move:
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        //var removelist = ZdfEntries.ToList();
                        //var itemRemoved = new ZdfEntryItemViewModel(e.OldItems[0] as ZDFEntry);
                        //removelist.Remove(itemRemoved);
                        //removelist = ZDFSorting.EntrySort(removelist, activeSort);
                        //ZdfEntries.Clear();
                        //ZdfEntries.AddRange(removelist);

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
        
            CreateEntryList();

        }

        private void modelEntryRemoved(object entry)
        {
            ModelCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, entry));
            CreateEntryList();
        }

       
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
                

               
            }

            _eventAggregator.GetEvent<EntrySelectedEvent>().Publish(id);

            
        }

       

        private void SaveZdfEntry()
        {

        }

       

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
           
            
            if (_activeZdf.EntryList.Any<IZDFEntry>())
            {
                foreach (var item in _activeZdf.EntryList)
                    ZdfEntries.Add(new ZdfEntryItemViewModel(_container, item as ZDFEntry));

            }

           
        }

   
        public ZDFViewModel(IEventAggregator eventAggregator, IUnityContainer container, IZDFEntryService entryService)
        {

            if (container == null) throw new ArgumentNullException("container");            
            if (eventAggregator == null) throw new ArgumentNullException("eventAggregator");

            _eventAggregator = eventAggregator;
            SelectedItem = true;
            SelectItemDelegateCommand = new DelegateCommand<IList>(SelectItem, CanSelectItem);

            _entryService = entryService;

            _container = container;

            _activeZdf = ZDFSingleton.GetInstance(_eventAggregator);

         
            _zdfEntriesLock = new Object();
            CreateEntryList();
            getActiveSort();

      
            _eventAggregator.GetEvent<ZDFOpenedEvent>().Subscribe(modelOpened);
            _eventAggregator.GetEvent<EntryDeletedEvent>().Subscribe(modelEntryRemoved);
         

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

       
       
        

        public ZdfEntryItemViewModel(IUnityContainer cont, ZDFEntry zdfEntry) : base(zdfEntry)
        {
            var mainWinVM = cont.Resolve<MainWindowViewModel>(InstanceNames.MainWindowViewModel);
            DeleteEntryDelegateCommand = mainWinVM.DeleteZDFEntryCommand;

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
