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
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using Microsoft.Practices.Unity;
using ZaveViewModel.Data_Structures;
using ZaveGlobalSettings.Data_Structures;

namespace ZaveViewModel.ViewModels
{
    public class CommentInputDialogViewModel : BindableBase, IConfirmation, IInteractionRequestAware, IConfirmNavigationRequest
    {
        string originalValue;
        IEditingItemState editingState;
        IUnityContainer _container;

        //private Object _sender;

        public CommentInputDialogViewModel(IUnityContainer cont)
        {
            //toReturn = fromSender;
            SaveCommentDelegateCommand = new DelegateCommand(SaveComment);
            CancelCommentDelegateCommand = new DelegateCommand(CancelComment);
            
            editingState = new EditingItemState();
            _container = cont;

            //OnCloseRequest = (sender) =>
            //{
            //    sender.result = CommentText;
            //    sender.Close();
            //};
        }

        public bool Confirmed { get; set; }

        public string Title { get; set; }

        private object _content;
        public object Content
        {
            get
            {
                return _content;
            }
            set
            {
                if (value is string || value is String)
                {
                    CommentText = (string)value;
                }

                SetProperty(ref _content, value);
            }
        }

        public INotification Notification { get; set; }

        public Action FinishInteraction { get; set; }


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
                editingState = new FinishedEditingItemState();
                this.Confirmed = true;
                this.FinishInteraction();

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
            get { return _commentText; }
            set
            {
                SetProperty(ref _commentText, value);
                //Content = value;
            }
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

            this.Confirmed = false;
            this.FinishInteraction();
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            editingState.ConfirmNavigationRequest(navigationContext, continuationCallback);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var paramaters = navigationContext.Parameters;
            if (paramaters.Any())
            {
                CommentText = paramaters[ZaveNavigationParameters.CommentText] as string;

            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        //public event EventHandler DialogClosing;

        //public virtual void RequestClose()
        //{
        //    if (this.OnCloseRequest != null)
        //        this.OnCloseRequest(this);
        //    else
        //        Close();
        //}

        //public Action<ModalInputDialogViewModel> OnCloseRequest { get; set; }

        //public void Close()
        //{
        //    if (this.DialogClosing != null)
        //    {                
        //        this.DialogClosing(this, new EventArgs());
        //    }
        //}

        //public void Show()
        //{
        //    originalValue = CommentText;
        //}


    }
}
