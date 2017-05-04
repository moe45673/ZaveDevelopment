using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Events;
using Microsoft.Practices.Unity;
using ZaveGlobalSettings.Data_Structures;
using Prism.Commands;

namespace ZaveViewModel.ViewModels
{

    

    public class TitleBarViewModel: BindableBase
    {
        private IEventAggregator _eventAgg;
        private IRegionManager _regionMan;
        private IUnityContainer _container;
        public DelegateCommand SwitchWindowModeDelegateCommand { get; set; }
        public DelegateCommand ConfirmUnsavedChangesCommand { get; set; }

        public TitleBarViewModel(IEventAggregator eventAgg, IRegionManager regionMan, IUnityContainer container)
        {
            _eventAgg = eventAgg;
            _regionMan = regionMan;
            _container = container;

            //_eventAgg.GetEvent<MainWindowInstantiatedEvent>().Subscribe(SetFileName);

            _eventAgg.GetEvent<FilenameChangedEvent>().Subscribe(SetFileName);

            var vm = _container.Resolve<MainWindowViewModel>(InstanceNames.MainWindowViewModel) as MainWindowViewModel;

            Filename = vm.Filename;
            
            SwitchWindowModeDelegateCommand = vm.SwitchWindowModeCommand;
           ConfirmUnsavedChangesCommand =  vm.ConfirmUnsavedChangesCommand;
        }

        private MainWindowViewModel mainWinVM;


        private string _filename;

        public string Filename
        {
            get { return this._filename; }

            set {
                
                if (mainWinVM != null)
                    SetProperty(ref _filename, mainWinVM.Filename);
                else
                    SetProperty(ref _filename, value);

            }
        }


        private void SetFileName(string newName)
        {
            Filename = newName;
        }

        private void SetFileName(object instantiate)
        {
            mainWinVM = _container.Resolve<MainWindowViewModel>(InstanceNames.MainWindowViewModel);
            SetFileName(mainWinVM.Filename);
        }
    }
}
