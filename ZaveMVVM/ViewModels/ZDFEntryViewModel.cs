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
using Prism.Mvvm;
using Prism.Events;
using Prism.Commands;
using Prism.Regions;
using Prism.Unity;
using ZaveGlobalSettings.Data_Structures;
using ZaveViewModel.Data_Structures;

//using Zave


namespace ZaveViewModel.ViewModels
{
    //using activeZDF = ZaveModel.ZDF.ZDFSingleton;

    // ReSharper disable once InconsistentNaming
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
                _eventAggregator.GetEvent<EntryReadEvent>().Subscribe(EventSetProperties);
                _regionManager = regionManager;
                _container = container;
            }
            // ReSharper disable once VirtualMemberCallInConstructor
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


        protected virtual void EventSetProperties(object obj)
        {
            ZDFEntryItem item = obj as ZDFEntryItem;

            if (item != null) _zdfEntry = item.ZDFEntry;
            setProperties(_zdfEntry.toSelectionState());
        }
        


        public static ZDFEntryViewModel EntryVmFactory(IEventAggregator eventAgg, IRegionManager regionManager, IUnityContainer container, IZDFEntry entry)
        {
            var entryVm = new ZDFEntryViewModel(eventAgg, regionManager, container);
            entryVm._zdfEntry = entry;            

            try
            {
                entryVm.setProperties(entry.ID, entry.Name, entry.Page, entry.Text, entry.DateModified, entry.HColor.Color, fromZDFCommentList(entry.Comments));
            }
            catch (NullReferenceException nre)
            {
                throw;
            }
            return entryVm;
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

        private void AddDlgBoxReturn(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "CommentText")
            {
                var ec = new ZDFCommentItem();
                ec.CommentText = ((ModalInputDialogViewModel)sender).CommentText;

                _zdfEntry.Comments.Add(new EntryComment(ec.CommentText, "User"));
            }
        }

        protected override void AddComment(System.Collections.IList items)
        {

            ObservableCollection<IDialogViewModel> vm = _container.Resolve(typeof(ObservableCollection<IDialogViewModel>), "DialogVMList") as ObservableCollection<IDialogViewModel>;
            
            
            var dlg = new ModalInputDialogViewModel();
            dlg.PropertyChanged += new PropertyChangedEventHandler(AddDlgBoxReturn);
            
            dlg.Show(vm);


            //System.Windows.MessageBox.Show(("From addComment: " +  vm.GetHashCode()));

            dlg.PropertyChanged -= new PropertyChangedEventHandler(AddDlgBoxReturn);
        }

        private void Dlg_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
