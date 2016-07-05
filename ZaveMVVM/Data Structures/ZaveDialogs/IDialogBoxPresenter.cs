using ZaveViewModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaveDialogs.Presenters
{
	public interface IDialogBoxPresenter<T> where T : IDialogViewModel
	{
		void Show(T viewModel);
	}
}
