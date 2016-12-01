using System;
using System.Windows;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Events;
using Zave.Views;
using ZaveViewModel.ViewModels;
using ZaveService.ZDFEntry;

namespace Zave.Module
{
    [ModuleDependency("DataServiceModule")]
    [ModuleDependency("MainWindowModule")]
    public class MainContainerModule : ModuleBaseClass
    {

        //private Zave.Controllers.ZDFEntryController _mainContainerController;

        public MainContainerModule(IUnityContainer cont, IRegionManager registry) : base(cont, registry) { }

        //private ZaveController.EventInitSingleton eventInit;
        public override void Initialize()
        {
            var controlbar = _unityContainer.Resolve<ControlBar>();
            UnityContainerExtensions.RegisterType(_unityContainer, typeof(object), typeof(MainContainer), "MainContainerView");
            //IRegion controlbarRegion = _regionManager.Regions[RegionNames.ControlBarRegion];
            //controlbarRegion.Add(controlbar);
            _regionManager.RegisterViewWithRegion(RegionNames.ControlBarRegion, () => _unityContainer.Resolve<ControlBar>("ControlBarView"));
            _regionManager.RegisterViewWithRegion(RegionNames.ZaveMainColorPicker, () => _unityContainer.Resolve<ColorPickerView>("ColorPickerView"));


            //_regionManager.RegisterViewWithRegion(RegionNames.ControlBarRegion, () => controlbar);
            //_regionManager.RegisterViewWithRegion(RegionNames.WidgetMainRegion, () => controlbar);
            _regionManager.RegisterViewWithRegion(RegionNames.MenuRegion, () => _unityContainer.Resolve<Menu>());
            //_unityContainer.RegisterType<IZDFEntryService, ZDFEntryService>();

            //_mainContainerController = _unityContainer.Resolve<Controllers.MainContainerController>();


        }
    }
}