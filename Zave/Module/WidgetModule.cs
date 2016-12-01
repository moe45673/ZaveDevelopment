using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Regions;
using Zave.Views;
using ZaveModel.ZDF;
using ZaveViewModel.ViewModels;
using ZaveModel.ZDFEntry;
using Prism.Modularity;

namespace Zave.Module
{
    [Module(ModuleName = "WidgetModule")]
    [ModuleDependency("DataServiceModule")]
    [ModuleDependency("ControlBarModule")]
    [ModuleDependency("MainWindowModule")]

    public class WidgetModule : ModuleBaseClass
    {
        public WidgetModule(IUnityContainer cont, IRegionManager registry) : base(cont, registry)
        {
        }

        public override void Initialize()
        {

            UnityContainerExtensions.RegisterType(_unityContainer, typeof(object), typeof(WidgetView), "WidgetView");

            //var view = _unityContainer.Resolve<WidgetView>();
            //_regionManager.Regions.Add(RegionNames.WidgetMainRegion);
            //var window = _unityContainer.Resolve<WidgetControls>();
            //var viewmodel = _unityContainer.Resolve<ControlBarViewModel>();
            //window.DataContext = viewmodel;
            //IRegionManager regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();
            //_regionManager.Regions.Remove("WidgetMainRegion");
            //RegionManager.SetRegionManager(view.WidgetMainRegion, _regionManager);
            //var controlbar = _unityContainer.Resolve<ControlBar>();
            //var widgetView = _unityContainer.Resolve<WidgetView>();
            //for (int i = 0; i < 200; i++)
            //{
            //    try
            //    {
            //        IRegion widgetmainRegion = _regionManager.Regions[RegionNames.WidgetMainRegion];
            //        widgetmainRegion.Add(controlbar);
            //        i = 500;
            //        System.Windows.Forms.MessageBox.Show("widgetRegionInitialized!");
            //    }
            //    catch(Exception ex)
            //    {
            //        i++;
            //    }
            //}
            _regionManager.RegisterViewWithRegion(RegionNames.WidgetMainRegion, () => _unityContainer.Resolve<ControlBar>("ControlBarView"));
            _regionManager.RegisterViewWithRegion(RegionNames.ZaveWidgetColorPicker, () => _unityContainer.Resolve<ColorPickerView>("ColorPickerView"));

            //var controlbar = _unityContainer.Resolve<ControlBar>();
            //_regionManager.RegisterViewWithRegion(RegionNames.WidgetMainRegion, () => _unityContainer.Resolve<ControlBar>());


            //_regionManager.RegisterViewWithRegion(RegionNames.ZDFEntryListRegion, () => _unityContainer.Resolve<ZDFView>());

            //UnityContainerExtensions.RegisterType(_unityContainer, typeof(object), typeof(ZdfViewModel), "ZDFViewModel");
            //UnityContainerExtensions.RegisterInstance(_unityContainer, typeof(ZaveModel.ZDF.IZDF), "ZDFSingleton", ZDFSingleton.GetInstance(_unityContainer.Resolve(typeof(IEventAggregator)) as EventAggregator));

        }
    }
}
