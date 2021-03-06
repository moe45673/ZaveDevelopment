﻿using System;
using System.Windows;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Events;
using Zave.Views;
using ZaveViewModel.ViewModels;
using ZaveService.ZDFEntry;
using ZaveGlobalSettings.Data_Structures;

namespace Zave.Module
{
    [Module(ModuleName="ExpandedViewModule")]
    [ModuleDependency("DataServiceModule")]
    [ModuleDependency("ZDFAppContainerModule")]
    [ModuleDependency("ControlBarModule")]
    [ModuleDependency("AppSettingsModule")]
    public class ExpandedViewModule : ModuleBaseClass
    {

        //private Zave.Controllers.ZDFEntryController _mainContainerController;

        public ExpandedViewModule(IUnityContainer cont, IRegionManager registry) : base(cont, registry) { }

        //private ZaveController.EventInitSingleton eventInit;
        public override void Initialize()
        {
            //var controlbar = _unityContainer.Resolve<ControlBar>();
            UnityContainerExtensions.RegisterType(_unityContainer, typeof(object), typeof(ExpandedView), InstanceNames.ExpandedView);
            //IRegion controlbarRegion = _regionManager.Regions[RegionNames.ControlBarRegion];
            //controlbarRegion.Add(controlbar);
            _regionManager.RegisterViewWithRegion(RegionNames.ControlBarRegion, () => _unityContainer.Resolve<ControlBar>(InstanceNames.ControlBarView));
            _regionManager.RegisterViewWithRegion(RegionNames.ZaveMainColorPicker, () => _unityContainer.Resolve<ColorPickerView>(InstanceNames.ColorPickerView));
            _regionManager.RegisterViewWithRegion(RegionNames.MainTitleBarRegion, () => _unityContainer.Resolve<TitleBar>());
            _regionManager.RegisterViewWithRegion(RegionNames.RecentZDFListRegion, () => _unityContainer.Resolve<ZDFList>());
            

            //_regionManager.RegisterViewWithRegion(RegionNames.ControlBarRegion, () => controlbar);
            //_regionManager.RegisterViewWithRegion(RegionNames.WidgetMainRegion, () => controlbar);
            _regionManager.RegisterViewWithRegion(RegionNames.MenuRegion, () => _unityContainer.Resolve<Menu>());
            //_unityContainer.RegisterType<IZDFEntryService, ZDFEntryService>();



            //_mainContainerController = _unityContainer.Resolve<Controllers.MainContainerController>();


        }
    }
}