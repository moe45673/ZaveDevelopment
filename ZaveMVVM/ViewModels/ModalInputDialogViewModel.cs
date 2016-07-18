using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Properties;
using Prism.Common;


namespace ZaveViewModel.ViewModels
{
    public class ModalInputDialogViewModel : BindableBase, IUserDialogViewModel
    {
        string originalValue;
        
        //private Object _sender;

        public ModalInputDialogViewModel()
        {
            //toReturn = fromSender;
            SaveCommentDelegateCommand = new DelegateCommand(SaveComment);
            CancelCommentDelegateCommand = new DelegateCommand(CancelComment);
            _commentText = "";

            //OnCloseRequest = (sender) =>
            //{
            //    sender.result = CommentText;
            //    sender.Close();
            //};
        }


        private Object result;

        public Object Result
        {
            get { return this.result; }
            private set { result = value; }
        }

        public bool IsModal
        {
            get
            {
                return true;
            }
        }

        public DelegateCommand SaveCommentDelegateCommand { get; private set; }

        private void SaveComment()
        {
            
            try
            {

                Close();
                
            }
            catch (NullReferenceException nre)
            {
                System.Windows.MessageBox.Show("Item must be selected!");

            }
            finally
            {
                
            }
        }

        private string _commentText;
        public string CommentText
        {
            get { return _commentText;}
            set { SetProperty(ref _commentText, value); }
        }

        private string _caption;
        public string Caption
        {
            get { return _caption; }
            set { SetProperty(ref _caption, value); }
        }

        public DelegateCommand CancelCommentDelegateCommand { get; private set; }
        protected void CancelComment()
        {
            
            CommentText = originalValue;
            Close();
        }

        public event EventHandler DialogClosing;

        public virtual void RequestClose()
        {
            if (this.OnCloseRequest != null)
                this.OnCloseRequest(this);
            else
                Close();
        }

        public Action<ModalInputDialogViewModel> OnCloseRequest { get; set; }

        public void Close()
        {
            if (this.DialogClosing != null)
            {                
                this.DialogClosing(this, new EventArgs());
            }
        }

        public void Show()
        {
            originalValue = CommentText;
        }


    }
}
