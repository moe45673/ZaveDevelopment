using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaveViewModel.ViewModels
{
    public class ZDFAppContainerViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;
        private readonly IEventAggregator _eventAggregator;

        public ZDFAppContainerViewModel(IUnityContainer cont, IRegionManager reg, IEventAggregator agg)
        {
            if (cont == null) throw new ArgumentNullException("container");
            if (reg == null) throw new ArgumentNullException("regionManager");
            if (agg == null) throw new ArgumentNullException("eventAggregator");
           

            _container = cont;
            _regionManager = reg;
            _eventAggregator = agg;
        }
    }
}
