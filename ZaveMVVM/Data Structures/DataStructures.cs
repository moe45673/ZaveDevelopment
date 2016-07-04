using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZaveModel.ZDFEntry;
using Prism.Mvvm;
using Prism.Commands;
using ZaveGlobalSettings.Data_Structures;
using ZaveGlobalSettings.Data_Structures.Observable;
using ModelComment = ZaveModel.ZDFEntry.Comment;
using WPFColor = System.Windows.Media.Color;
using Color = System.Drawing.Color;


namespace ZaveViewModel.Data_Structures
{

    using CommentList = ObservableImmutableList<ZDFCommentItem>;

    using selStateCommentList = List<Object>;

    public abstract class ZDFEntryItem : BindableBase
    {

        protected IZDFEntry _zdfEntry;

        private ObservableImmutableList<ZDFCommentItem> SelectedItems { get; set; }

        

        //private string _txtDocId;

        protected void setProperties(int id = default(int), string name = default(string), string page = default(string), string txt = default(string), DateTime dateModded = default(DateTime), Color col = default(Color), CommentList comments = default(CommentList))
        {
            if (_zdfEntry == null)
            {
                throw new NullReferenceException("No ZDFEntryItemViewModel referenced!");
            }

            _txtDocID = id.ToString();
            TxtDocName = name;
            TxtDocPage = page;
            TxtDocText = txt;
            TxtDocLastModified = dateModded.ToShortDateString() + " " + dateModded.ToShortTimeString();
            _txtDocColor = new WPFColor();
            _txtDocColor.A = col.A;
            _txtDocColor.R = col.R;
            _txtDocColor.B = col.B;
            _txtDocColor.G = col.G;
            OnPropertyChanged("TxtDocColor");
            _txtDocComments = comments;            
            OnPropertyChanged("TxtDocComments");

            _editedComment = new ZDFCommentItem(null);

            SelectCommentDelegateCommand = new DelegateCommand<System.Collections.IList>(selectEntry);
            AddCommentDelegateCommand = new DelegateCommand<System.Collections.IList>(AddComment).ObservesCanExecute(p => CanAdd);
            SaveCommentDelegateCommand = new DelegateCommand<ZDFCommentItem>(SaveComment).ObservesCanExecute((p) => IsEditing);

            SelectedItems = new ObservableImmutableList<ZDFCommentItem>();

            if(_txtDocText != "")
            {
                CanAdd = true;
            }


        }

        protected virtual void setProperties(SelectionState selState)
        {
            try
            {
                setProperties(selState.ID, selState.SelectionDocName, selState.SelectionPage, selState.SelectionText, selState.SelectionDateModified, selState.Color, fromObjectList(selState.Comments));
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




        protected void selectEntry(System.Collections.IList items)
        {
            if (items != null)
            {
                EditedComment = items.Cast<ZDFCommentItem>().ToList<ZDFCommentItem>().FirstOrDefault();

                CanDelete = true;
                CanEdit = true;
            }
            else
            {
                CanDelete = false;
                CanEdit = false;
            }


        }



        public DelegateCommand<System.Collections.IList> AddCommentDelegateCommand
        {
            get;
            private set;
        }


        private ZDFCommentItem _editedComment;

        public ZDFCommentItem EditedComment
        {
            get { return this._editedComment; }
            set { SetProperty(ref _editedComment, value); }
        }

        private void AddComment(System.Collections.IList commentList)
        {
            if (SelectedItems != null)
                SelectedItems.Clear();

            IsEditing = true;

            EditedComment = new ZDFCommentItem(null);
            TxtDocComments.Add(EditedComment);
            commentList.Add(EditedComment);


            


        }

        public DelegateCommand<ZDFCommentItem> SaveCommentDelegateCommand
        {
            get;
            set;
        }

        private void SaveComment(ZDFCommentItem commItem)
        {
            var modelComm = new ModelComment.EntryComment(EditedComment.CommentText, EditedComment.CommentAuthor);

            _zdfEntry.Comments.Add(modelComm);
            


            IsEditing = false;
        }

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
                var tempComment = new ZDFCommentItem(comment as ModelComment.IEntryComment);
                tempList.Add(tempComment);
            }
            return tempList;
        }

        public static CommentList fromZDFCommentList(IList<ModelComment.IEntryComment> zComments)
        {

            var tempList = new CommentList();
            foreach (var comment in zComments)
            {
                var tempComment = new ZDFCommentItem(comment);
                tempList.Add(tempComment);
            }

            return tempList;
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
                
                _zdfEntry.HColor = ZaveModel.Colors.ColorCategory.FromWPFColor(value);
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

        protected CommentList _txtDocComments;

        public CommentList TxtDocComments
        {
            get { return fromZDFCommentList(_zdfEntry.Comments); }
            set
            {
                _zdfEntry.Comments = new ObservableImmutableList<ModelComment.IEntryComment>(value.ToList<ZDFCommentItem>().ConvertAll(x => new ModelComment.EntryComment(x.CommentText)));
                SetProperty(ref _txtDocComments, value);
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
            set { SetProperty(ref _isEditing, value); }
        }

        private bool _canAdd;

        public bool CanAdd
        {
            get { return this._canAdd; }
            set { SetProperty(ref _canAdd, value); }
        }


        #endregion 
    }

    public class ZDFCommentItem : BindableBase
    {
        
        private ZDFCommentItem(ModelComment.IEntryComment modelComment = default(ModelComment.EntryComment), string text = default(string), string author = default(string), int id = default(int))
        {
            _modelComment = modelComment;
            _commentText = text;
            _commentAuthor = author;
        }

        public static ZDFCommentItem ItemFactory(ref ModelComment.IEntryComment modelComment, string text = default(string), string author = default(string))
        {
            
            
            var item = new ZDFCommentItem(modelComment, text, author, modelComment.CommentID);
            
            return item;
        }

        public ZDFCommentItem(ModelComment.IEntryComment comment = default(ModelComment.EntryComment)) 
        {
            _commentAuthor = "";
            _commentText = "Testing";
            if (comment != null)
                _commentID = comment.CommentID;
            else
                _commentID = -1;
            _modelComment = comment;
            
                

        }

        public static ZDFCommentItem fromObject(Object<Object, String, String> obj = default(Object<Object, String, String>))
        {
            
            return new ZDFCommentItem(obj.FirstProp as ModelComment.EntryComment, obj.SecondProp, obj.ThirdProp);
        }

        public static explicit operator ZDFCommentItem(ModelComment.EntryComment comment)
        {
            return new ZDFCommentItem(comment);
        }


        protected ModelComment.IEntryComment _modelComment;


        private int _commentID;

        public int CommentID
        {
            get { return _modelComment.CommentID; }
            private set { SetProperty(ref _commentID, value); }
        }

        private String _commentText;

        public String CommentText
        {
            get { return _modelComment.CommentText; }
            set
            {
                _modelComment.CommentText = value;
                SetProperty(ref _commentText, value);
            }
        }

        public ModelComment.IEntryComment ModelComment {
            get { return _modelComment; }
            private set { SetProperty(ref _modelComment, value); }
        }

        private String _commentAuthor;

        public String CommentAuthor
        {
            get { return (string)_modelComment.Author; }
            set
            {
                _modelComment.Author.Name = value;
                SetProperty(ref _commentAuthor, value);
            }
        }
    }

   

}
