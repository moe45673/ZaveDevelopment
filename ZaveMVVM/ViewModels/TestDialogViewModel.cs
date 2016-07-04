using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaveViewModel.ViewModels
{
    public class TestDialogViewModel : IUserDialogViewModel
    {


        public virtual bool IsModal { get { return true; } }

        public virtual void RequestClose() { this.DialogClosing(this, null); }

        public virtual event EventHandler DialogClosing;



    }
}
