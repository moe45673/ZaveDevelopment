using ZaveViewModel.ViewModels;

namespace ZaveViewModel.Data_Structures.ZaveDialogs
{
	public interface IDialogBoxPresenter<T> where T : IDialogViewModel
	{
		void Show(T viewModel);
	}
}
