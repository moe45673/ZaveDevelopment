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
    /// <summary>
    /// 
    /// </summary>
    [Module(ModuleName = "ZDFEntryModule")]
    public class ZDFEntryModule : ModuleBaseClass
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cont"></param>
        /// <param name="_reg"></param>
        public ZDFEntryModule(IUnityContainer cont, IRegionManager _reg) : base(cont, _reg) { }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {

            UnityContainerExtensions.RegisterType(_unityContainer, typeof(object), typeof(ZDFEntryView), "ZDFEntryView");
            //_regionManager.RegisterViewWithRegion(RegionNames.ZDFEntryDetailRegion, () => _unityContainer.Resolve<ZDFEntryView>());
            //_unityContainer.RegisterType<IZDFEntryService, ZDFEntryService>();
            this._unityContainer.Resolve<ZDFEntryController>();


        }
    }
}