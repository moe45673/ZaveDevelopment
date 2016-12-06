using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Regions;
using Zave.Views;
using Prism.Events;
using Prism.Modularity;
using ZaveGlobalSettings.Data_Structures;
using ZaveViewModel.ViewModels;

namespace Zave.Module
{
    [Module(ModuleName = "MainWindowModule", OnDemand = true)]
    [ModuleDependency("IOModule")]
    public class MainWindowModule : ModuleBaseClass
    {
        private ZaveController.EventInitSingleton eventInit;
        private Zave.Controllers.MainWindowController mainWinController;

        public MainWindowModule(IUnityContainer cont, IRegionManager reg) : base(cont, reg) { }

        public override void Initialize()
        {
            //_unityContainer.RegisterInstance<MainWindow>(_unityContainer.Resolve<MainWindow>());
            //var viewmodel = _unityContainer.Resolve<ZaveViewModel.ViewModels.MainWindowViewModel>();
            //window.DataContext = viewmodel;
            var window = _unityContainer.Resolve<MainWindow>(InstanceNames.MainWindowView);
            
            _regionManager.RegisterViewWithRegion(RegionNames.MainViewRegion, () => _unityContainer.Resolve<MainContainer>());
            //UnityContainerExtensions.RegisterType(_unityContainer, typeof(object), typeof(Views.MainWindow), "MainWindow");
            eventInit = ZaveController.EventInitSingleton.GetInstance(_unityContainer.Resolve<IEventAggregator>(), _unityContainer);
            mainWinController = _unityContainer.Resolve<Controllers.MainWindowController>();
            
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class RegionNames
    {
        public const string RecentZDFListRegion = "RecentZDFListRegion";
        public const string ZDFEntryListRegion = "ZDFEntryListRegion";
        public const string ZDFEntryDetailRegion = "ZDFEntryDetailRegion";
        public const string ControlBarRegion = "ControlBarRegion";
        public const string MainViewRegion = "MainViewRegion";
        public const string WidgetMainRegion = "WidgetMainRegion";
        public static string MenuRegion = "MenuRegion";
        public static string ZaveMainColorPicker = "ZaveMainColorPicker";
        public static string ZaveWidgetColorPicker = "ZaveWidgetColorPicker";
        public static string MainTitleBarRegion = "MainTitleBarRegion";
        public static string WidgetTitleBarRegion = "WidgetTitleBarRegion";

        
    }

   
}
