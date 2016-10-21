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
    public class MainContainerModule : ModuleBaseClass
    {

        private Zave.Controllers.MainContainerController _mainContainerController;

        public MainContainerModule(IUnityContainer cont, IRegionManager registry) : base(cont, registry) { }

        //private ZaveController.EventInitSingleton eventInit;
        public override void Initialize()
        {
            
            UnityContainerExtensions.RegisterType(_unityContainer, typeof(object), typeof(MainContainer), "MainContainer");
            _regionManager.RegisterViewWithRegion(RegionNames.ControlBarRegion, () => _unityContainer.Resolve<ControlBar>());
            this._unityContainer.RegisterType<IZDFEntryService, ZDFEntryService>();
            _mainContainerController = _unityContainer.Resolve<Controllers.MainContainerController>();

        }
    }
}