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
using System.Collections;
using ZaveModel.ZDF;
using ZaveService.ZDFEntry;

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

        private IZDFEntryService _entryService;

        protected ObservableCollection<IDialogViewModel> CommentDialog;
        protected ModalInputDialogViewModel _commentDlg;

        public ZDFEntryViewModel(IEventAggregator eventAgg, IRegionManager regionManager, IUnityContainer container, IZDFEntryService entryService) : base(new ZaveModel.ZDFEntry.ZDFEntry())
        {

            if (container == null) throw new ArgumentNullException("container");
            if (regionManager == null) throw new ArgumentNullException("regionManager");
            if (eventAgg == null) throw new ArgumentNullException("eventAggregator");

            if (_eventAggregator == null && eventAgg != null)
            {
                _eventAggregator = eventAgg;
                //_eventAggregator.GetEvent<EntrySelectedEvent>().Subscribe(EventSetProperties);
                _regionManager = regionManager;
                _container = container;
                _commentDlg = new ModalInputDialogViewModel();
                _entryService = entryService;
                _zdfEntry = _entryService.getZDFEntry(entryService.ActiveZDFEntryId);
                //_zdfEntry.Comments.CollectionChanged += base.ModelCollectionChanged;
            }
           
            try
            {
                setProperties(_zdfEntry.ID, _zdfEntry.Name, _zdfEntry.Page, _zdfEntry.Text, _zdfEntry.DateModified, _zdfEntry.HColor.Color, fromZDFCommentList(_zdfEntry.Comments));

                

            }
            catch (NullReferenceException nre)
            {
                throw nre;
            }
            
        }


        

        public ModalInputDialogViewModel CommentDlg
        {
            get { return this._commentDlg; }
            set { SetProperty(ref _commentDlg, value); }
        }


        protected virtual void EventSetProperties(string id)
        {
            

            if (id != null && id != "")
            {
                _zdfEntry = ZDFSingleton.GetInstance().EntryList.FirstOrDefault(x => x.ID == Convert.ToInt64(id)) as ZDFEntry;
                setProperties(_zdfEntry);
            }
            else
            {
                _zdfEntry = new ZDFEntry();
                setProperties(_zdfEntry);
            }


        }
        


        public static ZDFEntryViewModel EntryVmFactory(IEventAggregator eventAgg, IRegionManager regionManager, IUnityContainer container, ZaveService.ZDFEntry.IZDFEntryService entryService, IZDFEntry entry)
        {
            var entryVm = new ZDFEntryViewModel(eventAgg, regionManager, container, entryService);
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

        private void AddCommentDlgBoxReturn(object sender, EventArgs args)
        {
            if (((ModalInputDialogViewModel)sender).CommentText != null && ((ModalInputDialogViewModel)sender).CommentText.Any())
            {
                var ec = new ZDFCommentItem();
                ec.CommentText = ((ModalInputDialogViewModel)sender).CommentText;

                _zdfEntry.Comments.Add(new EntryComment(ec.CommentText, "User"));
                
            }
            
            
            IsEditing = false;
        }
       

        protected override void AddComment()
        {


            //var dlgBoxCollection = _container.Resolve(typeof(ObservableCollection<IDialogViewModel>), "DialogVMList") as ObservableCollection<IDialogViewModel>;
            //dlgBoxCollection.Clear();
            CommentDlg.DialogClosing -= new EventHandler(AddCommentDlgBoxReturn);
            CommentDlg.DialogClosing += new EventHandler(AddCommentDlgBoxReturn);
            CommentDlg.CommentText = "";
            
            IsEditing = true;

            //dlg.Show(dlgBoxCollection);
            //System.Windows.MessageBox.Show(("From addComment: " +  vm.GetHashCode()));

            
        }

        private void EditDlgBoxReturn(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "CommentText")
            {
                var commentIndex = _zdfEntry.Comments.IndexOf(_zdfEntry.Comments.FirstOrDefault(x => x.CommentText == EditedComment.CommentText));
                
                _zdfEntry.Comments[commentIndex].CommentText = ((ModalInputDialogViewModel)sender).CommentText;

                
            }
        }

        protected override void EditComment(IList commentList)
        {
            try
            {
                var vm = _container.Resolve(typeof(ObservableCollection<IDialogViewModel>), "DialogVMList") as ObservableCollection<IDialogViewModel>;
                var dlg = new ModalInputDialogViewModel();
                vm.Clear();

                EditedComment = TxtDocComments.FirstOrDefault(x => x.CommentText == (commentList[0] as ZDFCommentItem).CommentText);
                dlg.CommentText = EditedComment.CommentText;
                dlg.PropertyChanged += new PropertyChangedEventHandler(EditDlgBoxReturn);
                //dlg.Show(vm);
                dlg.PropertyChanged -= new PropertyChangedEventHandler(EditDlgBoxReturn);
                    
            }
            catch (NullReferenceException nre)
            {
                System.Windows.Forms.MessageBox.Show(nre.Message + "\nMust Select a comment!");
            }
            catch (ArgumentOutOfRangeException arg)
            {
                System.Windows.Forms.MessageBox.Show(arg.Message + "\nMust Select a comment!");
            }
        }

        private void Dlg_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
