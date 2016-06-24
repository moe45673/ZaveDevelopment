using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZaveModel.ZDFEntry;
using Prism.Mvvm;
using ZaveGlobalSettings.Data_Structures;
using ZaveGlobalSettings.Data_Structures.Observable;
using WPFColor = System.Windows.Media.Color;
using Color = System.Drawing.Color;


namespace ZaveViewModel.Data_Structures
{

    using CommentList = ObservableImmutableList<ZDFCommentItem>;
    using selStateCommentList = List<Object<string, string>>;

    public abstract class ZDFEntryItem : BindableBase
    {

        protected IZDFEntry _zdfEntry;

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



        }

      

        protected void setProperties(SelectionState selState)
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
            var tempList = new CommentList();

            foreach (var comment in list)
            {
                var tempComment = ZDFCommentItem.fromObject(comment);
                tempList.Add(tempComment);
            }
            return tempList;
        }

        public static CommentList fromZDFCommentList(IList<ZaveModel.ZDFEntry.Comment.IEntryComment> zComments)
        {

            var tempList = new CommentList();
            foreach (var comment in zComments)
            {
                var tempComment = new ZDFCommentItem(comment.CommentText, comment.Author);
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
                _txtDocName = value;
                _zdfEntry.Name = _txtDocName;
                OnPropertyChanged("TxtDocName");
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

                _txtDocID = value;
                OnPropertyChanged("TxtDocID");
                //_zdfEntry.ID = int.Parse(_txtDocID);
            }

        }

        protected String _txtDocPage;
        public String TxtDocPage
        {
            get { return _zdfEntry.Page; }
            set
            {
                _txtDocPage = value;
                _zdfEntry.Page = _txtDocPage;
                OnPropertyChanged("TxtDocPage");

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
                _txtDocText = value;
                _zdfEntry.Text = value;
                OnPropertyChanged("TxtDocText");

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
                _txtDocLastModified = value;
                _zdfEntry.DateModified = DateTime.Parse(_txtDocLastModified);
                OnPropertyChanged("TxtDocLastModified");

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
                _txtDocColor = value;
                _zdfEntry.HColor = ZaveModel.Colors.ColorCategory.FromWPFColor(value);
                OnPropertyChanged("TxtDocColor");

            }
        }

        public ZDFEntry ZDFEntry
        {
            get;
            protected set;
        }


        protected CommentList _txtDocComments;

        public CommentList TxtDocComments
        {
            get { return this._txtDocComments; }
            set
            {
                SetProperty(ref _txtDocComments, value);
            }
        }


        #endregion 
    }

    public class ZDFCommentItem : BindableBase
    {
        public ZDFCommentItem(string text = default(string), string author = default(string))
        {
            _comment = new ZaveModel.ZDFEntry.Comment.EntryComment();
            _commentText = text;
            _commentAuthor = author;
        }

        public static ZDFCommentItem fromObject(Object<string, string> obj)
        {
            return new ZDFCommentItem(obj.FirstProp, obj.SecondProp);
        }



        protected ZaveModel.ZDFEntry.Comment.IEntryComment _comment;



        private String _commentText;

        public String CommentText
        {
            get { return _comment.CommentText; }
            set
            {
                _comment.CommentText = value;
                OnPropertyChanged("CommentText");
            }
        }

        private String _commentAuthor;

        public String CommentAuthor
        {
            get { return _comment.Author; }
            set
            {
                _comment.Author = value;
                OnPropertyChanged("CommentAuthor");
            }
        }
    }

        public class HighlightCommand : ICommand
    {
        private Action<object> execute;

        private Predicate<object> canExecute;
        private event EventHandler CanExecuteChangedInternal;
        public HighlightCommand(Action<object> execute) : this(execute, DefaultCanExecute) { }

        public HighlightCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            if (canExecute == null)
                throw new ArgumentNullException("canExecute");

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                this.CanExecuteChangedInternal += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
                this.CanExecuteChangedInternal -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute != null && this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            if (handler != null)
            {
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        public void Destroy()
        {
            this.canExecute = _ => false;
            this.execute = _ => { return; };
        }

        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }

    }

}
