using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Events;
using ZaveViewModel.ViewModels;
using ZaveModel.ZDFEntry;
using System.Collections.ObjectModel;
using ZaveGlobalSettings.Data_Structures;
using System.ComponentModel;
using System.Windows.Data;
using Microsoft.Practices.Unity;

//using GalaSoft.MvvmLight.CommandWpf;

namespace ZaveViewModel.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;
        private readonly IEventAggregator _eventAggregator;
        private string _filename;
        public static string SaveLocation = null;
        

        public DelegateCommand<string> NavigateCommand { get; set; }

        public MainWindowViewModel(IRegionManager regionManager, IUnityContainer cont, IEventAggregator agg)
        {
            _container = cont;
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            //Dialogs.Add(new ModalInputDialogViewModel());
            cont.RegisterInstance(typeof(ObservableCollection<IDialogViewModel>), "DialogVMList", Dialogs);
            _eventAggregator = agg;
            _eventAggregator.GetEvent<ZDFSavedEvent>().Subscribe(setFileName);
            SaveLocation = "";
            _filename = SaveLocation;
            

        }

        private void setFileName(string name)
        {
            Filename = name;
        }

        private void Navigate(string uri)
        {
            _regionManager.RequestNavigate("ContentRegion", uri);
        }

        #region Properties
        private ObservableCollection<IDialogViewModel> _dialogs = new ObservableCollection<IDialogViewModel>();
        public ObservableCollection<IDialogViewModel> Dialogs { get { return _dialogs; } }
        public string Filename
        {
            get
            {
                return _filename;
            }
            set
            {
                SaveLocation = value;
                var name = Path.GetFileName(SaveLocation);
                SetProperty(ref _filename, value);
            }
        }
        #endregion



    }
}

        