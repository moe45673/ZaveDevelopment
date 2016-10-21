using System;
using System.Windows;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Zave.Views;
using ZaveViewModel.Data_Structures;
using ZaveViewModel.ViewModels;

namespace Zave.Module
{
    public class ZDFEntryModule : ModuleBaseClass
    {

        public ZDFEntryModule(IUnityContainer cont, IRegionManager _reg) : base(cont, _reg) { }

        public override void Initialize()
        {
            _regionManager.RegisterViewWithRegion("ZDFEntry", typeof(ZDFEntryView));
            UnityContainerExtensions.RegisterType(_unityContainer, typeof(object), typeof(ZDFEntryView), "ZDFEntry");

        }
    }
}