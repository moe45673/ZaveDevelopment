using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Events;
using ZaveViewModel.ViewModels;
using ZaveModel.ZDFEntry;
using System.Collections.ObjectModel;
using ZaveGlobalSettings.Events;
using ZaveGlobalSettings.Data_Structures;
using System.ComponentModel;
using System.Windows.Data;
//using GalaSoft.MvvmLight.CommandWpf;

namespace ZaveViewModel.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public DelegateCommand<string> NavigateCommand { get; set; }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            this.Dialogs.Add(new TestDialogViewModel());
        }

        private void Navigate(string uri)
        {
            _regionManager.RequestNavigate("ContentRegion", uri);
        }

        private ObservableCollection<IDialogViewModel> _Dialogs = new ObservableCollection<IDialogViewModel>();
        public ObservableCollection<IDialogViewModel> Dialogs { get { return _Dialogs; } }

        