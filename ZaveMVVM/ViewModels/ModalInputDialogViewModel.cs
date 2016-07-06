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

using ModelComment = ZaveModel.ZDFEntry.Comment;


namespace ZaveViewModel.ViewModels
{
    public class ModalInputDialogViewModel : BindableBase, IUserDialogViewModel
    {

        //private Object _sender;

        public ModalInputDialogViewModel()
        {
            SaveCommentDelegateCommand = new DelegateCommand(SaveComment);
            CancelCommentDelegateCommand = new DelegateCommand(CancelComment);
            
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
                System.Windows.MessageBox.Show(("Save Command Executed!"));
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

        public DelegateCommand CancelCommentDelegateCommand { get; private set; }
        protected void CancelComment()
        {

        }

        public event EventHandler DialogClosing;

        public void RequestClose()
        {
            try
            {
                DialogClosing(this, null);
            }
            catch (NullReferenceException nre)
            {
                System.Windows.MessageBox.Show("I can't do that, Tim");
            }
            
        }

        public void Show(IList<IDialogViewModel> collection)
        {
            collection.Add(this);
        }


    }
}
