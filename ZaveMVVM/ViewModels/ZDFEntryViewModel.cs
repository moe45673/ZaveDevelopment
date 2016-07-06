using System;
//using GalaSoft.MvvmLight;
//using GalaSoft.MvvmLight.CommandWpf;
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
using JetBrains.Util;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using ZaveViewModel.Commands;
using Prism.Mvvm;
using Prism.Events;
using Prism.Commands;
using Prism.Regions;
using Prism.Unity;
using ZaveGlobalSettings.Events;
using ZaveGlobalSettings.Data_Structures.Observable;
using ZaveViewModel.Data_Structures;
using ModelComment = ZaveModel.ZDFEntry.Comment;
//using Zave


namespace ZaveViewModel.ViewModels
{
    //using activeZDF = ZaveModel.ZDF.ZDFSingleton;

    public class ZDFEntryViewModel : ZDFEntryItem
    {
       
        private IEventAggregator _eventAggregator;

        private IRegionManager _regionManager;

        private IUnityContainer _container;

        public ZDFEntryViewModel(IEventAggregator eventAgg, IRegionManager regionManager, IUnityContainer container) : base(new ZDFEntry())
        {
            
            if (_eventAggregator == null && eventAgg != null)
            {
                _eventAggregator = eventAgg;
                _eventAggregator.GetEvent<EntryReadEvent>().Subscribe(eventSetProperties);
                _regionManager = regionManager;
                _container = container;
            }
            AddCommentDelegateCommand = new DelegateCommand<System.Collections.IList>(AddComment).ObservesCanExecute(p => CanAdd);
            try
            {
                setProperties(_zdfEntry.ID, _zdfEntry.Name, _zdfEntry.Page, _zdfEntry.Text, _zdfEntry.DateModified, _zdfEntry.HColor.Color, fromZDFCommentList(_zdfEntry.Comments));

                

            }
            catch (NullReferenceException nre)
            {
                throw nre;
            }
            
        }


        protected void eventSetProperties(object obj)
        {
            ZDFEntryItem item = obj as ZDFEntryItem;

            _zdfEntry = item.ZDFEntry;
            setProperties(_zdfEntry.toSelectionState());
        }
        


        public static ZDFEntryViewModel entryVMFactory(IEventAggregator eventAgg, IRegionManager regionManager, IUnityContainer container, IZDFEntry entry)
        {
            var entryVM = new ZDFEntryViewModel(eventAgg, regionManager, container);
            entryVM._zdfEntry = entry;            

            try
            {
                entryVM.setProperties(entry.ID, entry.Name, entry.Page, entry.Text, entry.DateModified, entry.HColor.Color, fromZDFCommentList(entry.Comments));
            }
            catch (NullReferenceException nre)
            {
                throw nre;
            }
            return entryVM;
        }

        public override string TxtDocID
        {
            get
            {
                return _txtDocID;
            }
            protected set
            {
                
                SetProperty(ref _txtDocID, value);
                //_zdfEntry.ID = int.Parse(_txtDocID);
            }

        }

        protected void AddComment(System.Collections.IList items)
        {

            ObservableCollection<IDialogViewModel> vm = _container.Resolve(typeof(ObservableCollection<IDialogViewModel>), "DialogVMList") as ObservableCollection<IDialogViewModel>;

            new ModalInputDialogViewModel { CommentText = "Testing!"}.Show(vm);

            //System.Windows.MessageBox.Show(("From addComment: " +  vm.GetHashCode()));


        }

        






    }
}
