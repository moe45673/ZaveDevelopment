using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zave.Views;
using ZaveGlobalSettings.Data_Structures;

namespace Zave.Module
{
    [Module(ModuleName = "ZDFAppContainerModule", OnDemand = true)]
    [ModuleDependency("MainWindowModule")]
    public class ZDFAppContainerModule : ModuleBaseClass
    {

        //private Zave.Controllers.ZDFEntryController _mainContainerController;

        public ZDFAppContainerModule(IUnityContainer cont, IRegionManager registry) : base(cont, registry) { }

        //private ZaveController.EventInitSingleton eventInit;
        public override void Initialize()
        {
            //var instance = _unityContainer.Resolve<ZDFAppContainer>();
            //_unityContainer.RegisterInstance<ZDFAppContainer>(InstanceNames.ZDFAppContainer, instance);
            //UnityContainerExtensions.RegisterType(_unityContainer, typeof(Object), typeof(ZDFAppContainer), InstanceNames.ZDFAppContainer);
            _unityContainer.RegisterType<Object, ZDFAppContainer>(InstanceNames.ZDFAppContainer);
            //var controlbar = _unityContainer.Resolve<ControlBar>();
            IRegion mviewRegion = _regionManager.Regions[RegionNames.ContainerRegion];

            if (mviewRegion == null) return;


            _regionManager.RequestNavigate(RegionNames.ContainerRegion, new Uri(InstanceNames.ZDFAppContainer, UriKind.Relative));
            //    , result =>
            //{
            //string strResult = (result.Result == true ? "View Successfully Loaded" : "View Failed to Load");
            //    System.Windows.MessageBox.Show(strResult);

            //});


        }
    }
}
