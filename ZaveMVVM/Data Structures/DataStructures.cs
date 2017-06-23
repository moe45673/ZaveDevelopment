using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZaveModel.ZDFEntry;
using Prism.Mvvm;
using Prism.Commands;
using Prism;
using Prism.Regions;
using Prism.Interactivity.InteractionRequest;
using System.Collections.Specialized;
using Microsoft.Practices.Unity;
using ZaveGlobalSettings.Data_Structures;
using ZaveGlobalSettings.Data_Structures.ZaveObservableCollection;
using ZaveModel.ZDFColors;
using ZaveViewModel.ViewModels;
using WPFColor = System.Windows.Media.Color;
using Color = System.Drawing.Color;
using Newtonsoft.Json;


namespace ZaveViewModel.Data_Structures
{
    using Prism;
    using Prism.Regions;
    using System.Windows.Threading;
    using CommentList = ObservableImmutableList<IEntryComment>;

    using selStateCommentList = List<SelectionComment>;

    #region Form Editing States
    public interface IEditingItemState: IConfirmNavigationRequest
    {
       InteractionRequest<IConfirmation> ConfirmExitInteractionRequest { get; }
    }

    public class EditingItemState : IEditingItemState
    {
        protected InteractionRequest<IConfirmation> _confirmExitInteractionRequest;

        public EditingItemState()
        {
            _confirmExitInteractionRequest = new InteractionRequest<IConfirmation>();
        }

        public InteractionRequest<IConfirmation> ConfirmExitInteractionRequest
        {
            get
            {
                return _confirmExitInteractionRequest;
            }

            private set
            {
                throw new NotImplementedException();
            }
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            this._confirmExitInteractionRequest.Raise(
                    new Confirmation { Content = ZaveMessageBoxes.ConfirmNavigateAwayFromFormCommand.Content, Title = ZaveMessageBoxes.ConfirmNavigateAwayFromFormCommand.Title },
                    c => { continuationCallback(c.Confirmed); });
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }
    }

    public class FinishingEditingItemState : IEditingItemState
    {

        protected InteractionRequest<IConfirmation> _confirmExitInteractionRequest;

        public InteractionRequest<IConfirmation> ConfirmExitInteractionRequest
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            throw new NotImplementedException();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }
    }

    public class FinishedEditingItemState : IEditingItemState
    {

        protected InteractionRequest<IConfirmation> _confirmExitInteractionRequest;

        public FinishedEditingItemState()
        {
            _confirmExitInteractionRequest = new InteractionRequest<IConfirmation>();
        }

        public InteractionRequest<IConfirmation> ConfirmExitInteractionRequest
        {
            get
            {
                return _confirmExitInteractionRequest;
            }
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            continuationCallback(true);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    public class ZDFSorting
    {
        public static List<T> EntrySort<T>(List<T> listToSort, string propName)
        {
            System.Reflection.PropertyInfo propInfo = typeof(T).GetProperty(propName);
            object property = propInfo.GetValue(listToSort.FirstOrDefault(), null);
            List<T> list = default(List<T>);
            if (property is IComparable)
            {

                list = listToSort.OrderBy(o => propInfo.GetValue(o, null)).ToList();
            }
            else
            {
                list = listToSort.OrderBy(o => propInfo.GetValue(o, null).ToString()).ToList();
            }
            return list;
        }
    }

    public abstract class ZaveCommentItem : BindableBase, INavigationAware, IActiveAware
    {
        protected ZaveCommentItem(IEntryComment modelComment = null, string text = default(string), string author = default(string), int id = 0)
        {
            _modelComment = modelComment;
            _commentText = text;
            _commentAuthor = author;
        }

        public ZaveCommentItem(IEntryComment comment = default(EntryComment))
        {
            _commentAuthor = "";
            _commentText = "Testing";
            _modelComment = comment;
            if (comment != null)
            {
                _commentID = comment.CommentID;
                _modelComment = comment;
            }
            else
            {

                _modelComment = new EntryComment();
                _commentID = -1;
            }
            ((EntryComment)_modelComment).PropertyChanged += ZDFCommentItem_PropertyChanged;

        }

        protected void ZDFCommentItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            IEntryComment temp = sender as IEntryComment;
            if (e.PropertyName == "CommentText")
            {
                this.CommentText = temp.CommentText;
            }
            if (e.PropertyName == "Author")
            {
                CommentAuthor = (string)temp.Author;
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            IsActive = true;
            
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            IsActive = false;
        }

        protected IEntryComment _modelComment;


        protected int _commentID;

        public int CommentID
        {
            get { return _modelComment.CommentID; }
            private set { SetProperty(ref _commentID, value); }
        }

        protected String _commentText;

        public String CommentText
        {
            get { return _modelComment.CommentText; }
            set
            {
                _modelComment.CommentText = value;
                SetProperty(ref _commentText, value);
            }
        }

        public IEntryComment ModelComment
        {
            get { return _modelComment; }
            private set { SetProperty(ref _modelComment, value); }
        }

        protected String _commentAuthor;

        public event EventHandler IsActiveChanged;

        protected virtual void OnIsActiveChanged()
        {
            var handler = IsActiveChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public String CommentAuthor
        {
            get { return (string)_modelComment.Author; }
            set
            {
                _modelComment.Author.Name = value;
                SetProperty(ref _commentAuthor, value);
            }
        }

        private bool _isActive;
        public bool IsActive
        {
            get
            {
                return _isActive;
            }

            set
            {
                if(SetProperty(ref _isActive, value))
                {
                    OnIsActiveChanged();
                }
            }
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public abstract class ZDFEntryItem : BindableBase, INavigationAware
    {

        protected IZDFEntry _zdfEntry;

        public static string SelectedZDFByUser = null;


        protected void ModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                switch (e.Action)
                {


                    case NotifyCollectionChangedAction.Add:


                        var listSend = ((IEnumerable<IEntryComment>)sender).ToList<IEntryComment>();
                        var tempComment = listSend.LastOrDefault();

                        var newComment = new EntryComment(tempComment);

                        var tempList = new ObservableImmutableList<ZaveCommentViewModel>();
                        tempList.Add(newComment);

                        TxtDocComments.DoAdd(x => newComment);



                        break;


                        //            case NotifyCollectionChangedAction.Replace:

                        //                System.Windows.Forms.MessageBox.Show("Replace Action Called!");

                        //                break;

                        //            default:
                        //                System.Windows.Forms.MessageBox.Show("Nothing Done!");
                        //                break;
                        //        }

                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        System.Windows.Forms.MessageBox.Show(ex.Message);
                        //    }



                        //    //ActiveZDFEntry = new ZDFEntryViewModel(activeZDF.EntryList[index]);
                        //    //System.Windows.Forms.MessageBox.Show(zdfEntry.Source.SelectionText);
                        //    //ZDFEntries.Add(new ZDFEntryViewModel(activeZDF.EntryList[index], _eventAggregator));
                        //    //UpdateGui(zdfEntry.Source);

                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Unable to Add New Comment!");
            }
        }


        //private string _txtDocId;

        protected void setProperties(int id = 0, string name = default(string), string page = default(string), string txt = default(string), DateTime dateModded = default(DateTime), Color col = default(Color), CommentList comments = null)
        {
            if (_zdfEntry == null)
            {
                throw new NullReferenceException("No ZDFEntryItemViewModel referenced!");
            }

            TxtDocID = id.ToString();
            TxtDocName = name;
            TxtDocPage = page;
            TxtDocText = txt;
            //OnPropertyChanged("TxtDocText");
            TxtDocLastModified = dateModded.ToShortDateString() + " " + dateModded.ToShortTimeString();
            _txtDocColor = new WPFColor();
            _txtDocColor.A = col.A;
            _txtDocColor.R = col.R;
            _txtDocColor.B = col.B;
            _txtDocColor.G = col.G;
            OnPropertyChanged("TxtDocColor");
            _txtDocComments = comments;
            //if (comments != null)
            //{
            //    _txtDocComments.Add(new ZDFCommentItem(new ModelComment.EntryComment("Test 1", "Moe")));
            //}
            OnPropertyChanged("TxtDocComments");


            //_editedComment = new ZaveCommentViewModel(null);
            //DeleteEntryDelegateCommand = new DelegateCommand<string>(DeleteEntry);
            SelectCommentDelegateCommand = new DelegateCommand<System.Collections.IList>(SelectComment);
            //AddCommentDelegateCommand = new DelegateCommand(AddComment).ObservesCanExecute(p => CanAdd);
            EditCommentDelegateCommand = new DelegateCommand<System.Collections.IList>(EditComment).ObservesCanExecute(p => CanEdit);


            _zdfEntry.Comments.CollectionChanged -= new NotifyCollectionChangedEventHandler(ModelCollectionChanged);
            _zdfEntry.Comments.CollectionChanged += new NotifyCollectionChangedEventHandler(ModelCollectionChanged);





            if (!IsEditing)
            {
                IsEditing = true;
                IsEditing = false;
            }
            if (IsNotEditing)
            {
                CanAdd = true;

            }
            CanEdit = false;






        }

        //~ZDFEntryItem()
        //{
        //    _zdfEntry.Comments.CollectionChanged -= ModelCollectionChanged;
        //}



        protected virtual void setProperties(SelectionState selState)
        {
            try
            {
                //if (selState.SelectionDocName != null)
                //{
                //    //SelectedZDFByUser = selState.SelectionDocName.ToString();
                //    SelectedZDFByUser = Convert.ToString(selState.ID);
                //}
                setProperties(selState.ID, selState.SelectionDocName, selState.SelectionPage, selState.SelectionText, selState.SelectionDateModified, selState.Color, fromObjectList(selState.Comments));
            }
            catch (NullReferenceException nre)
            {
                throw nre;
            }
        }

        public virtual void setProperties(IZDFEntry zEntry)
        {
            try
            {
                if (zEntry.ID != 0)
                {
                    SelectedZDFByUser = Convert.ToString(zEntry.ID);
                    _zdfEntry = zEntry;
                }
                setProperties(zEntry.ID, zEntry.Name, zEntry.Page, zEntry.Text, zEntry.DateModified, zEntry.HColor.Color, fromZDFCommentList(zEntry.Comments));
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

        public virtual DelegateCommand<string> DeleteEntryDelegateCommand
        {
            get; set;
        }
        //public DelegateCommand<string> DeleteEntryDelegateCommand { virtual get; virtual set; }

        protected virtual void DeleteEntry(string id)
        {
            System.Windows.MessageBox.Show("Parent!");
        }


        protected void SelectComment(System.Collections.IList items)
        {
            if (items != null)
            {
                //EditedComment = items.Cast<ZaveCommentViewModel>().ToList<ZaveCommentViewModel>().FirstOrDefault();

                CanDelete = true;
                CanEdit = true;
            }
            else
            {
                CanDelete = false;
                CanEdit = false;
            }


        }

        //private ZaveCommentViewModel _editedComment;

        //public ZaveCommentViewModel EditedComment
        //{
        //    get { return this._editedComment; }
        //    set { SetProperty(ref _editedComment, value); }
        //}

        private bool _isNotEditing;
        protected bool IsNotEditing
        {
            get { return _isNotEditing; }
            private set { SetProperty(ref _isNotEditing, value); }
        }

        //public virtual DelegateCommand AddCommentDelegateCommand
        //{
        //    get;
        //    protected set;
        //}

        protected virtual void AddComment()
        {
            ;
        }

        public virtual DelegateCommand<System.Collections.IList> EditCommentDelegateCommand
        {
            get;
            protected set;
        }

        protected abstract void EditComment(System.Collections.IList commentList);




        #endregion





        protected ZDFEntryItem(ZDFEntry zdfEntry)
        {
            _zdfEntry = zdfEntry;

            try
            {
                setProperties(zdfEntry.ID, zdfEntry.Name, zdfEntry.Page, zdfEntry.Text, zdfEntry.DateModified, zdfEntry.HColor.Color, fromZDFCommentList(zdfEntry.Comments));
            }
            catch (NullReferenceException nre)
            {
                throw nre;
            }

        }

        #region Converters
        public SelectionState toSelectionState()
        {
            return _zdfEntry.toSelectionState();
        }

        public static CommentList fromObjectList(selStateCommentList list)
        {
            if (list == null)
            {
                list = new selStateCommentList();
            }
            var tempList = new CommentList();

            foreach (var comment in list)
            {
                var tempComment = new EntryComment(comment.Text, comment.Author);
                tempList.Add(tempComment);
            }
            return tempList;
        }

        public static CommentList fromZDFCommentList(IList<IEntryComment> zComments)
        {

            var tempList = new CommentList();
            foreach (var comment in zComments)
            {
                var tempComment = new EntryComment(comment);
                tempList.Add(tempComment);
            }

            return tempList;
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            _zdfEntry = (ZDFEntry)navigationContext.Parameters[InstanceNames.ZDFEntry];
            try
            {
                setProperties(_zdfEntry);
            }
            catch (NullReferenceException nre)
            {
                throw nre;
            }
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        #endregion

        #region Properties

        protected String _txtDocName;
        public String TxtDocName
        {
            get { return _zdfEntry.Name; }
            set
            {
                _zdfEntry.Name = value;
                SetProperty(ref _txtDocName, value);
                //System.Windows.Forms.MessageBox.Show(value.ToString());


            }

        }

        protected String _txtDocID;
        public virtual string TxtDocID
        {
            get
            {
                string tempID = "";
                if (_txtDocID != _zdfEntry.ID.ToString())
                {
                    _txtDocID = _zdfEntry.ID.ToString();
                    OnPropertyChanged("TxtDocID");
                }

                tempID = _txtDocID;

                return tempID;
            }
            protected set
            {

                SetProperty(ref _txtDocID, value);
                //_zdfEntry.ID = int.Parse(_txtDocID);
            }

        }

        protected String _txtDocPage;
        public String TxtDocPage
        {
            get { return _zdfEntry.Page; }
            set
            {

                _zdfEntry.Page = value;
                SetProperty(ref _txtDocPage, value);

            }

        }

        protected String _txtDocText;
        public String TxtDocText
        {
            get { return _zdfEntry.Text; }
            set
            {
                //if (SetProperty(ref _txtDocText, value))
                //    _zdfEntry.Text = _txtDocText;

                _zdfEntry.Text = value;
                SetProperty(ref _txtDocText, value);

            }
        }

        protected String _txtDocLastModified;
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

                _zdfEntry.DateModified = DateTime.Parse(value);
                SetProperty(ref _txtDocLastModified, value);

            }
        }

        protected WPFColor _txtDocColor;
        public WPFColor TxtDocColor
        {
            get
            {
                return _zdfEntry.HColor.toWPFColor();
            }
            set
            {

                _zdfEntry.HColor = ColorCategory.FromWPFColor(value);
                SetProperty(ref _txtDocColor, value);

            }
        }

        public IZDFEntry ZDFEntry
        {
            get { return _zdfEntry; }
            protected set
            {
                SetProperty(ref _zdfEntry, value);
            }
        }

        //protected IEnumerable<>

        //private readonly object _docCommentsLock;
        protected CommentList _txtDocComments;
        public CommentList TxtDocComments
        {
            get { return _txtDocComments; }

            protected set
            {
                //lock (_docCommentsLock)
                //{
                _txtDocComments = value;


                //}
            }
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
            set
            {
                _isEditing = value;
                IsNotEditing = !_isEditing;
                CanAdd = !_isEditing;
                OnPropertyChanged("IsEditing");
                //System.Windows.Forms.MessageBox.Show("IsEditing is set to " + _isEditing);
            }
        }

        private bool _canAdd;

        public bool CanAdd
        {
            get { return this._canAdd; }
            set
            {
                try
                {
                    lock (this)
                    {
                        SetProperty(ref _canAdd, value);
                        if (_txtDocText == "")
                        {

                            SetProperty(ref _canAdd, false);
                        }
                    }
                    //if (_canAdd == true)
                    //{
                    //    Dispatcher.CurrentDispatcher.Invoke(() =>
                    //    {
                    //        SetProperty(ref _canAdd, value);
                    //        if (_txtDocText == "")
                    //        {

                    //            SetProperty(ref _canAdd, false);
                    //        }
                    //    });
                    //}

                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message.ToString());
                    //System.Threading.Thread th = System.Threading.Thread.CurrentThread;
                    //th.IsBackground = true;
                    //System.Threading.Thread.Sleep(50);
                }

            }
        }


        #endregion 
    }

    



}
