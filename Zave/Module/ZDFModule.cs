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
    /// <summary>
    /// 
    /// </summary>
    [Module(ModuleName="ZDFModule")]
    [ModuleDependency("DataServiceModule")]
    public class ZDFModule : ModuleBaseClass
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cont"></param>
        /// <param name="registry"></param>
        public ZDFModule(IUnityContainer cont, IRegionManager registry) : base(cont, registry)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            
            UnityContainerExtensions.RegisterType(_unityContainer, typeof(object), typeof(ZDFView), "ZDFView");
            _regionManager.RegisterViewWithRegion(RegionNames.ZDFEntryListRegion, () => _unityContainer.Resolve<ZDFView>());
            _unityContainer.RegisterType<Controllers.ZDFEntryController>();

            //UnityContainerExtensions.RegisterType(_unityContainer, typeof(object), typeof(ZdfViewModel), "ZDFViewModel");
            //UnityContainerExtensions.RegisterInstance(_unityContainer, typeof(ZaveModel.ZDF.IZDF), "ZDFSingleton", ZDFSingleton.GetInstance(_unityContainer.Resolve(typeof(IEventAggregator)) as EventAggregator));

        }
    }
}
