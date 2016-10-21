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

namespace Zave.Module
{
    class ZDFModule : ModuleBaseClass
    {
        public ZDFModule(IUnityContainer cont, IRegionManager registry) : base(cont, registry)
        {
        }

        public override void Initialize()
        {
            //var zdfSingleton =
            //    ZDFSingleton.GetInstance(_unityContainer.Resolve(typeof(IEventAggregator)) as EventAggregator);
            _regionManager.RegisterViewWithRegion("ZDFView", typeof(ZDFView));
            //UnityContainerExtensions.RegisterType(_unityContainer, typeof(object), typeof(ZDFView), "ZDFView");
            //UnityContainerExtensions.RegisterType(_unityContainer, typeof(object), typeof(ZdfViewModel), "ZDFViewModel");
            //UnityContainerExtensions.RegisterInstance(_unityContainer, typeof(ZaveModel.ZDF.IZDF), "ZDFSingleton", ZDFSingleton.GetInstance(_unityContainer.Resolve(typeof(IEventAggregator)) as EventAggregator));

        }
    }
}
