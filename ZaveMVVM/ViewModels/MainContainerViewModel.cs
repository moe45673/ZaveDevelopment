using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Regions;

namespace ZaveViewModel.ViewModels
{
    public class MainContainerViewModel
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;



        public MainContainerViewModel(IRegionManager regionManager, IUnityContainer cont)
        {
            _container = cont;
            _regionManager = regionManager;
            
            
        }

        public MainContainerViewModel()
        {
            
        }
        
    }
}
