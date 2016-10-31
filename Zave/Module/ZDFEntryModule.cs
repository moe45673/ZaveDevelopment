using System;
using System.Windows;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Zave.Views;
using ZaveViewModel.Data_Structures;
using ZaveViewModel.ViewModels;
using Zave.Controllers;
using ZaveService.ZDFEntry;

namespace Zave.Module
{
    public class ZDFEntryModule : ModuleBaseClass
    {
        private ZDFEntryController _entryController;

        public ZDFEntryModule(IUnityContainer cont, IRegionManager _reg) : base(cont, _reg) { }

        public override void Initialize()
        {

            UnityContainerExtensions.RegisterType(_unityContainer, typeof(object), typeof(ZDFEntryView), "ZDFEntry");
            //_regionManager.RegisterViewWithRegion(RegionNames.ZDFEntryDetailRegion, () => _unityContainer.Resolve<ZDFEntryView>());
            //_unityContainer.RegisterType<IZDFEntryService, ZDFEntryService>();
            _entryController = this._unityContainer.Resolve<ZDFEntryController>();


        }
    }
}