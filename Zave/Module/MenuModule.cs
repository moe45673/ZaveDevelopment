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
    [Module(ModuleName = "MenuModule")]
    [ModuleDependency("IOModule")]
    public class MenuModule : ModuleBaseClass
    {
        public MenuModule(IUnityContainer cont, IRegionManager registry) : base(cont, registry)
        {
        }

        public override void Initialize()
        {

            UnityContainerExtensions.RegisterType(_unityContainer, typeof(object), typeof(Menu), "MenuView");
            //_regionManager.RegisterViewWithRegion(RegionNames.ZDFEntryListRegion, () => _unityContainer.Resolve<ZDFView>());
            //_unityContainer.RegisterType<Controllers.ZDFEntryController>();

            //UnityContainerExtensions.RegisterType(_unityContainer, typeof(object), typeof(ZdfViewModel), "ZDFViewModel");
            //UnityContainerExtensions.RegisterInstance(_unityContainer, typeof(ZaveModel.ZDF.IZDF), "ZDFSingleton", ZDFSingleton.GetInstance(_unityContainer.Resolve(typeof(IEventAggregator)) as EventAggregator));

        }
    }
}
