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
    [ModuleDependency("MainContainerModule")]
    [ModuleDependency("WidgetModule")]
    public class ColorPickerModule : ModuleBaseClass
    {
        public ColorPickerModule(IUnityContainer cont, IRegionManager registry) : base(cont, registry) { }

        //private ZaveController.EventInitSingleton eventInit;
        public override void Initialize()
        {
            
            UnityContainerExtensions.RegisterType(_unityContainer, typeof(object), typeof(ColorPickerView), "ColorPickerView");
            


        }
    }
}

