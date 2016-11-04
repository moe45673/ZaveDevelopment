using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Regions;
using Prism.Commands;

namespace ZaveViewModel.ViewModels
{
    public class MenuViewModel : BindableBase
    {
        private IEventAggregator _eventAggregator;

        private IRegionManager _regionManager;

        private IUnityContainer _container;

        public DelegateCommand SaveZDFDelegateCommand { get; set; }
        public DelegateCommand OpenZDFDelegateCommand { get; set; }

        public DelegateCommand NewZDFDelegateCommand { get; set; }

        public DelegateCommand<String> ExportZDFDelegateCommand { get; set; }
        public DelegateCommand SaveASZDFDelegateCommand { get; set; }
        public MenuViewModel(IEventAggregator eventAgg, IRegionManager reg, IUnityContainer cont)
        {

            if (cont == null) throw new ArgumentNullException("container");
            if (reg == null) throw new ArgumentNullException("regionManager");
            if (eventAgg == null) throw new ArgumentNullException("eventAggregator");

            _eventAggregator = eventAgg;
            _regionManager = reg;
            _container = cont;

            var vm = _container.Resolve(typeof(MainWindowViewModel)) as MainWindowViewModel;
            SaveZDFDelegateCommand = vm.SaveZDFDelegateCommand;
            OpenZDFDelegateCommand = vm.OpenZDFDelegateCommand;
            NewZDFDelegateCommand = vm.NewZDFDelegateCommand;
            ExportZDFDelegateCommand = vm.ExportZDFDelegateCommand;
            SaveASZDFDelegateCommand = vm.SaveASZDFDelegateCommand;


        }

    }
}
