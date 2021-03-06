﻿using System;
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
using Prism.Interactivity.InteractionRequest;
using ZaveGlobalSettings.Data_Structures.ZaveObservableCollection;
using System.Globalization;
using System.Collections.Specialized;

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

        public InteractionRequest<CommentInputDialogViewModel> CommentDialogRequest { get; set; }
        public InteractionRequest<IConfirmation> ConfirmDialog { get; set; }
        public DelegateCommand AddCommentDelegateCommand { get; set; }
        public DelegateCommand<Object> EditCommentDelegateCommand { get; set; }
        public DelegateCommand<Object> DeleteCommentDelegateCommand { get; set; }


        



        protected CommentInputDialogViewModel _commentDlg;



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
                _commentDlg = new CommentInputDialogViewModel(_container);
                CommentDialogRequest = new InteractionRequest<CommentInputDialogViewModel>();
                ConfirmDialog = new InteractionRequest<IConfirmation>();
                AddCommentDelegateCommand = new DelegateCommand(AddComment);
                EditCommentDelegateCommand = new DelegateCommand<Object>(EditComment);
                DeleteCommentDelegateCommand = new DelegateCommand<Object>(DeleteComment);
                DeleteEntryDelegateCommand = _container.Resolve<MainWindowViewModel>(InstanceNames.MainWindowViewModel).DeleteZDFEntryCommand;
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

       

        protected override void DeleteEntry(string id)
        {
            //System.Windows.MessageBox.Show("Child!");
            //var mainWinVM = _container.Resolve(typeof(MainWindowViewModel), InstanceNames.MainConViewModel);

        }

        public CommentInputDialogViewModel CommentDlg
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
                return String.Format("{0}", _zdfEntry.ID);
            }
            protected set
            {

                SetProperty(ref _txtDocID, value);
                //_zdfEntry.ID = int.Parse(_txtDocID);
            }

        }

        private void AddCommentDlgBoxReturn(object sender, EventArgs args)
        {
            if (((CommentInputDialogViewModel)sender).CommentText != null && ((CommentInputDialogViewModel)sender).CommentText.Any())
            {
                var ec = new EntryComment();
                ec.CommentText = ((CommentInputDialogViewModel)sender).CommentText;

                _zdfEntry.Comments.Add(new EntryComment(ec.CommentText, "User"));

            }


            IsEditing = false;
        }


        protected override void AddComment()
        {


            //var dlgBoxCollection = _container.Resolve(typeof(ObservableCollection<IDialogViewModel>), "DialogVMList") as ObservableCollection<IDialogViewModel>;
            //dlgBoxCollection.Clear();
            var comment = new EntryComment();
            IsEditing = true;
            CommentInputDialogViewModel vm = new CommentInputDialogViewModel(_container);
            vm.Title = "Add New Comment";

            this.CommentDialogRequest.Raise(
                vm,
                addComment =>
                {
                    if (addComment.Confirmed)
                    {

                        comment.CommentText = vm.CommentText;
                        comment.Author = (User)"User";
                        TxtDocComments.Add(comment);
                        //ModelCollectionChanged(new List<IEntryComment> { comment }, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, comment));
                        _eventAggregator.GetEvent<CommentUpdateEvent>().Publish(comment);
                    }
                }
                );

            IsEditing = false;



            //dlg.Show(dlgBoxCollection);
            //System.Windows.MessageBox.Show(("From addComment: " +  vm.GetHashCode()));


        }

        private void DeleteComment(object commentObj)
        {
            var comment = commentObj as IEntryComment;
            IsEditing = true;
            //CommentInputDialogViewModel vm = new CommentInputDialogViewModel(_container);
            //vm.Title = "Add New Comment";

            this.ConfirmDialog.Raise(
                new Confirmation { Title="Confirm Deletion", Content="Are you sure you wish to delete this comment?"},
                commentToDelete =>
                {
                    if (commentToDelete.Confirmed)
                    {

                        
                        TxtDocComments.Remove(comment, new CommentEqualityComparer());
                        //ModelCollectionChanged(new List<IEntryComment> { comment }, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, comment));
                        _eventAggregator.GetEvent<CommentDeletedEvent>().Publish(comment);
                    }
                }
                );

            IsEditing = false;
        }

        //private void EditDlgBoxReturn(object sender, PropertyChangedEventArgs args)
        //{
        //    if (args.PropertyName == "CommentText")
        //    {
        //        var commentIndex = _zdfEntry.Comments.IndexOf(_zdfEntry.Comments.FirstOrDefault(x => x.CommentText == EditedComment.CommentText));

        //        _zdfEntry.Comments[commentIndex].CommentText = ((CommentInputDialogViewModel)sender).CommentText;


        //    }
        //}

        //protected void EditComment(ICollection<ZaveCommentItem> commentList)
        //{
        //    try
        //    {
        //        //var comment = commentList.
        //        var vm = _container.Resolve(typeof(ObservableCollection<IDialogViewModel>), "DialogVMList") as ObservableCollection<IDialogViewModel>;
        //        var dlg = new CommentInputDialogViewModel(_container);
        //        vm.Clear();

        //        //EditedComment = TxtDocComments.FirstOrDefault(x => x.CommentText == (commentList[0] as ZaveCommentViewModel).CommentText);
        //        //dlg.CommentText = EditedComment.CommentText;
        //        //dlg.PropertyChanged += new PropertyChangedEventHandler(EditDlgBoxReturn);
        //        ////dlg.Show(vm);
        //        //dlg.PropertyChanged -= new PropertyChangedEventHandler(EditDlgBoxReturn);

        //    }
        //    catch (NullReferenceException nre)
        //    {
        //        System.Windows.Forms.MessageBox.Show(nre.Message + "\nMust Select a comment!");
        //    }
        //    catch (ArgumentOutOfRangeException arg)
        //    {
        //        System.Windows.Forms.MessageBox.Show(arg.Message + "\nMust Select a comment!");
        //    }
        //}

        private void Dlg_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void EditComment(Object comment)
        {
            var commentToEdit = comment as IEntryComment;
            //IsEditing = true;
            CommentInputDialogViewModel vm = new CommentInputDialogViewModel(_container);

            vm.Title = "Edit Comment";
            vm.Content = commentToEdit.CommentText;
            //vm.

            this.CommentDialogRequest.Raise(
                vm,
                dialogEditedComment =>
                {
                    
                    if (dialogEditedComment != null & dialogEditedComment.Confirmed)
                    {


                        //comment.Author = (User)"User";
                        var newComment = this.TxtDocComments.First(x => x.CommentID == commentToEdit.CommentID);
                        newComment.CommentText = dialogEditedComment.CommentText;
                        
                        var newList = TxtDocComments.ToList();
                        var index = TxtDocComments.IndexOf(commentToEdit);
                        newList.Remove(commentToEdit);
                        newList.Insert(index, newComment);
                        
                        
                        TxtDocComments.Clear();
                        TxtDocComments.AddRange(newList);
                        //TxtDocComments.Replace(commentToEdit, newComment, new CommentEqualityComparer());
                        _eventAggregator.GetEvent<CommentUpdateEvent>().Publish(newComment);
                        
                    }
                }
                );
            //vm.
        }

        //TODO Add Authorization
        private bool CheckIfAuthorizedUser()
        {
            return true;
        }

    }
}
