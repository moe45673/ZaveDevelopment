using System;
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
    //[ModuleDependency("MainContainerModule")]
    //[ModuleDependency("WidgetModule")]
    [Module(ModuleName = "ColorPickerModule")]
    [ModuleDependency("AppSettingsModule")]
    public class ColorPickerModule : ModuleBaseClass
    {
        public ColorPickerModule(IUnityContainer cont, IRegionManager registry) : base(cont, registry) { }

        //private ZaveController.EventInitSingleton eventInit;
        public override void Initialize()
        {
            var instance = _unityContainer.Resolve<ColorPickerView>();
            UnityContainerExtensions.RegisterInstance<ColorPickerView>(_unityContainer, InstanceNames.ColorPickerView, instance);
            


        }
    }
}

