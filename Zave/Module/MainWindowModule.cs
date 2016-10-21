using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Regions;
using Zave.Views;
using Prism.Events;

namespace Zave.Module
{
    public class MainWindowModule : ModuleBaseClass
    {
        private ZaveController.EventInitSingleton eventInit;

        public MainWindowModule(IUnityContainer cont, IRegionManager reg) : base(cont, reg) { }

        public override void Initialize()
        {
            _regionManager.RegisterViewWithRegion(RegionNames.MainContainerRegion, () => _unityContainer.Resolve<MainContainer>());
            UnityContainerExtensions.RegisterType(_unityContainer, typeof(object), typeof(Views.MainWindow), "MainWindow");
            eventInit = ZaveController.EventInitSingleton.GetInstance(_unityContainer.Resolve<IEventAggregator>(), _unityContainer);
        }
    }

    public static class RegionNames
    {
        public const string RecentZDFListRegion = "RecentZDFListRegion";
        public const string ZDFEntryListRegion = "ZDFEntryListRegion";
        public const string ZDFEntryDetailRegion = "ZDFEntryDetailRegion";
        public const string ControlBarRegion = "ControlBarRegion";
        public const string MainContainerRegion = "MainContainerRegion";
    }
}
