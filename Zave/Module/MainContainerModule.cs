using System;
using System.Windows;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Zave.Views;
using ZaveViewModel.ViewModels;

namespace Zave.Module
{
    public class MainContainerModule : ModuleBaseClass
    {

        public MainContainerModule(IUnityContainer cont, IRegionViewRegistry registry) : base(cont, registry) { }
        

        public override void Initialize()
        {
            _regionViewRegistry.RegisterViewWithRegion("ZDFEntry", typeof(Views.MainContainer));
            UnityContainerExtensions.RegisterType(_unityContainer, typeof(object), typeof(MainContainer), "MainContainer");
        }
    }
}