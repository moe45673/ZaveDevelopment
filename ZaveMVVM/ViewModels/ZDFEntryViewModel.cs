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
using ZaveViewModel.Commands;
using Prism.Mvvm;
using Prism.Events;
using Prism.Commands;
using ZaveGlobalSettings.Events;
using ZaveGlobalSettings.Data_Structures.Observable;
using ZaveViewModel.Data_Structures;
//using Zave


namespace ZaveViewModel.ViewModels
{
    //using activeZDF = ZaveModel.ZDF.ZDFSingleton;

    public class ZDFEntryViewModel : ZDFEntryItem
    {
       
        private IEventAggregator _eventAggregator;

        private ObservableImmutableList<ZDFCommentItem> SelectedItems { get; set; }


        public ZDFEntryViewModel(IEventAggregator eventAgg) : base(new ZDFEntry())
        {
            
            if (_eventAggregator == null && eventAgg != null)
            {
                _eventAggregator = eventAgg;
                _eventAggregator.GetEvent<EntryUpdateEvent>().Subscribe(setProperties);                
            }

            SelectCommentDelegateCommand = new DelegateCommand<System.Collections.IList>(selectEntry);


            try
            {
                setProperties(_zdfEntry.ID, _zdfEntry.Name, _zdfEntry.Page, _zdfEntry.Text, _zdfEntry.DateModified, _zdfEntry.HColor.Color, fromZDFCommentList(_zdfEntry.Comments));
            }
            catch (NullReferenceException nre)
            {
                throw nre;
            }
            
        }

        #region Commands
        public DelegateCommand<System.Collections.IList> SelectCommentDelegateCommand
        {
            get; private set;
        }


        private bool _canEdit;

        public bool CanEdit
        {
            get { return this._canEdit; }
            set { SetProperty(ref _canEdit, value); }
        }


        private bool _canDelete;

        public bool CanDelete
        {
            get { return this._canDelete; }
            set { SetProperty(ref _canDelete, value); }
        }


        private bool _isEditing;

        public bool IsEditing
        {
            get { return this._isEditing; }
            set { SetProperty(ref _isEditing, value); }
        }

        private void selectEntry(System.Collections.IList items)
        {
            if (items != null)
            {
                SelectedItems = new ObservableImmutableList<ZDFCommentItem>(items.Cast<ZDFCommentItem>());
                CanDelete = true;
                CanEdit = true;    
            }
            else
            {
                CanDelete = false;
                CanEdit = false;
            }
             
           
        }



        public DelegateCommand AddCommentDelegatCommand
        {
            get;
            private set;
        }

        public void AddComment()
        {
            SelectedItems.Clear();
            SelectedItems.Add(new ZDFCommentItem("Enter Comment Here"));
            IsEditing = true;
            
        }

        #endregion


        public static ZDFEntryViewModel entryVMFactory(IEventAggregator eventAgg, IZDFEntry entry)
        {
            var entryVM = new ZDFEntryViewModel(eventAgg);
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

        






    }
}
