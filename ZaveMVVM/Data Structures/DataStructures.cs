using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZaveSrc = ZaveModel.ZDFSource.Source;


namespace ZaveViewModel.Data_Structures
{
    public enum SrcType { WORD, EXCEL }

    public struct SelectionState
    {
        public SelectionState(string name = "", string page = "", string text = "", SrcType src = 0)
        {
            SelectionPage = page;
            SelectionDocName = name;
            SelectionText = text;
            srcType = src;
        }
        public String SelectionPage { get; set; }
        public String SelectionDocName { get; set; }
        public String SelectionText { get; set; }
        public SrcType srcType { get; set; }

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
