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
    using System.Windows.Data;
    using System.Windows.Threading;
    using ZaveGlobalSettings.Data_Structures.CustomAttributes;
    using CommentList = ObservableImmutableList<IEntryComment>;

    using selStateCommentList = List<SelectionComment>;


    #region Form Editing States
    /// <summary>
    /// Interface for all FormEditingState objects to inherit from
    /// </summary>
    /// <remarks>Implementation of the State design pattern</remarks>
    public interface IEditingItemState : IConfirmNavigationRequest
    {
        InteractionRequest<IConfirmation> ConfirmExitInteractionRequest { get; }
    }

    /// <summary>
    /// State to instantiate when a user has not finished editing their form
    /// </summary>
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

    /// <summary>
    /// State to instantiate after a user clicks "Finish" but before any processing of the form is done
    /// </summary>
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

    /// <summary>
    /// State to instantiate after all form processing has been done
    /// </summary>
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

    [PlaceHolder(
        Name = "Default Sorter",
        Description = "Provides sorting by color. Needs update to allow " +
        "sorting to be set by user preference"
        )
    ]
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

    /// <summary>
    /// Class that all Comments inherit from
    /// </summary>
    /// <remarks>Not a good design. Each ViewModel should be a separate class</remarks>
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

            //If the Model changes in any way, this should reflect that change
            ((EntryComment)_modelComment).PropertyChanged += ZDFCommentItem_PropertyChanged;

        }

        /// <summary>
        /// Update the ViewModel if the corresponding Model changes
        /// </summary>
        /// <param name="sender">The Model that has changed</param>
        /// <param name="e">The Properties that were changed</param>
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

        /// <summary>
        /// Corresponding IEntryComment to expose to the View
        /// </summary>
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
                SetProperty(ref _isActive, value, () =>
                {
                    OnIsActiveChanged(); 
                });
                
            }
        }
    }

    /// <summary>
    /// Class that ZDFEntry ViewModels inherit from in the List -> Detail view pattern
    /// </summary>
    /// <remarks>Should be better implemented, all Viewmodels should be separate</remarks>
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class ZDFEntryItem : BindableBase, INavigationAware
    {

        protected IZDFEntry _zdfEntry;



        public static string SelectedZDFByUser = null;

        /// <summary>
        /// Called when the corresponding IZDFEntry model's comments are changed to update the viewmodel
        /// </summary>
        /// <param name="sender">The corresponding model's collection</param>
        /// <param name="e">details about how the collection was changed</param>
        protected void ModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                if (!(TxtDocComments.ToList()).AreEqual(((CommentList)sender).ToList(), new CommentEqualityComparer())) //ensure the VM list and the Model list are not equal before propagating changes to the VM
                {
                    switch (e.Action)
                    {


                        case NotifyCollectionChangedAction.Add:


                            var listSend = ((IEnumerable<IEntryComment>)sender).ToList<IEntryComment>();
                            var tempComment = listSend.LastOrDefault();

                            var newComment = new EntryComment(tempComment);

                            //var tempList = new ObservableImmutableList<IEntryComment>();
                            //tempList.Add(newComment);

                            TxtDocComments.Add(newComment);




                            break;

                        case NotifyCollectionChangedAction.Reset:

                            TxtDocComments.Clear();
                            TxtDocComments.AddRange(sender as IEnumerable<IEntryComment>);

                            break;

                        case NotifyCollectionChangedAction.Remove:


                            foreach (IEntryComment comment in e.OldItems)
                            {
                                TxtDocComments.Remove(comment, new CommentEqualityComparer());
                            }

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
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Unable to Add New Comment!");
            }

        }


        //private string _txtDocId;

            /// <summary>
            /// Method to ensure object integrity whenever created or modified
            /// </summary>
            /// <param name="id"></param>
            /// <param name="name"></param>
            /// <param name="page"></param>
            /// <param name="txt"></param>
            /// <param name="dateModded"></param>
            /// <param name="col">The Color associated with the Entry</param>
            /// <param name="comments"></param>
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
            EditCommentDelegateCommand = new DelegateCommand<Object>(EditComment).ObservesCanExecute(() => CanEdit);


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


        /// <summary>
        /// Method to ensure object integrity whenever created or modified
        /// </summary>
        /// <param name="selState">The SelectionState object to get the data from</param>
        /// <exception cref="NullReferenceException">Thrown if SelectionState is null</exception>
        
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

        /// <summary>
        /// Method to ensure object integrity whenever created or modified
        /// </summary>
        /// <param name="zEntry">The model with which to get the data from</param>
        /// <exception cref="NullReferenceException"/>
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

        /// <summary>
        /// The Commands that the view sends to the corresponding ViewModel
        /// </summary>
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
                //var comment = items.Cast<IEntryComment>().FirstOrDefault();
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
        /// <summary>
        /// Flag to declare if the Entry is being edited
        /// </summary>
        /// <remarks>Should be replaced by Prism's built-in enabling/disabling (using commands) and/or the State design pattern</remarks>
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

        public virtual DelegateCommand<Object> EditCommentDelegateCommand
        {
            get;
            protected set;
        }

        protected abstract void EditComment(Object commentList);




        #endregion




        /// <summary>
        /// Sets this ViewModel's Model
        /// </summary>
        /// <param name="zdfEntry">The ZDFEntry with the relevant data</param>
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

        /// <summary>
        /// Turns a List of SelectionState Comments into ZaveEntry comments
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns an ObservableImmutableList{IEntryComment} from an IList{IEntryComment}
        /// </summary>
        /// <param name="zComments"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Name of the Source Document
        /// </summary>
        /// <remarks>Should be redesigned to allow for more diverse source materials (EG emails)</remarks>
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

        /// <summary>
        /// Identifier of the ZDFEntry
        /// </summary>
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
        /// <summary>
        /// Page in the source doc that this Entry came from
        /// </summary>
        /// <remarks>Should be redesigned to allow for more diverse source materials (EG emails)</remarks>
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
        /// <summary>
        /// Highlighted Text from the original document
        /// </summary>
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

        /// <summary>
        /// The corresponding Model of the ViewModel
        /// </summary>
        public IZDFEntry ZDFEntry
        {
            get { return _zdfEntry; }
            protected set
            {
                SetProperty(ref _zdfEntry, value);
            }
        }
        
        protected CommentList _txtDocComments;
        public CommentList TxtDocComments
        {
            get { return ZDFEntry.Comments; }

            protected set
            {
                //lock (_docCommentsLock)
                //{
                if (_txtDocComments == null)
                {
                    _txtDocComments = new CommentList(ZDFEntry.Comments);
                }
                if (SetProperty(ref _txtDocComments, value))
                    ZDFEntry.Comments = value;



                //}
            }
        }

        private bool _canEdit;

        /// <summary>
        /// Flag to determine if able to edit a Comment
        /// </summary>
        /// <remarks>
        /// Should be true if one Comment is selected
        /// Should be redesigned, likely using the State Pattern
        /// </remarks>
        public bool CanEdit
        {
            get { return this._canEdit; }
            set { SetProperty(ref _canEdit, value); }
        }


        private bool _canDelete;
        /// <summary>
        /// Flag to determine if able to Delete a Comment
        /// </summary>
        /// <remarks>
        /// Should be true if one Comment is selected
        /// Should be redesigned, likely using the State Pattern
        /// </remarks>
        public bool CanDelete
        {
            get { return this._canDelete; }
            set { SetProperty(ref _canDelete, value); }
        }


        private bool _isEditing;
        /// <summary>
        /// Flag to determine if a comment is being edited
        /// </summary>
        /// <remarks>
        /// Horrible code. Permission should be set by dialog window, not by calling window
        /// </remarks>
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

        /// <summary>
        /// Flag to determine if able to add Comment to ZDFEntry
        /// </summary>
        /// <remarks>
        /// Should be true if a ZDFEntry has been selected from the list and a Comment is not currently being edited
        /// </remarks>
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
