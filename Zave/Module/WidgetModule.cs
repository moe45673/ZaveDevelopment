using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Regions;
using Zave.Views;
using ZaveModel.ZDF;
using ZaveViewModel.ViewModels;
using ZaveModel.ZDFEntry;
using Prism.Modularity;

namespace Zave.Module
{
    [Module(ModuleName = "WidgetModule", OnDemand = true)]
    [ModuleDependency("DataServiceModule")]

    public class WidgetModule : ModuleBaseClass
    {
        public WidgetModule(IUnityContainer cont, IRegionManager registry) : base(cont, registry)
        {
        }

        public override void Initialize()
        {

            UnityContainerExtensions.RegisterType(_unityContainer, typeof(object), typeof(WidgetView), "WidgetView");
            //var window = _unityContainer.Resolve<WidgetControls>();
            //var viewmodel = _unityContainer.Resolve<ControlBarViewModel>();
            //window.DataContext = viewmodel;
            _regionManager.RegisterViewWithRegion(RegionNames.WidgetMainRegion, () => _unityContainer.Resolve<ControlBar>());
            //_regionManager.RegisterViewWithRegion(RegionNames.ZDFEntryListRegion, () => _unityContainer.Resolve<ZDFView>());

            //UnityContainerExtensions.RegisterType(_unityContainer, typeof(object), typeof(ZdfViewModel), "ZDFViewModel");
            //UnityContainerExtensions.RegisterInstance(_unityContainer, typeof(ZaveModel.ZDF.IZDF), "ZDFSingleton", ZDFSingleton.GetInstance(_unityContainer.Resolve(typeof(IEventAggregator)) as EventAggregator));

        }
    }
}
