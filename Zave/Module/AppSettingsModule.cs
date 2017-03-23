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
    [Module(ModuleName = "AppSettingsModule")]
    public class ApplicationSettingsModule : ModuleBaseClass
    {
        Controllers.AppSettingsController settingsController;
        Prism.Events.IEventAggregator _eventAgg;
        public ApplicationSettingsModule(IUnityContainer cont, IRegionManager registry, IEventAggregator eventAgg) : base(cont, registry)
        {
            _eventAgg = eventAgg;
            settingsController = new Controllers.AppSettingsController(_eventAgg);
        }

        //private ZaveController.EventInitSingleton eventInit;
        public override void Initialize()
        {
            _unityContainer.RegisterType<IConfigProvider, Controllers.AppSettingsController>();
            UnityContainerExtensions.RegisterInstance<IConfigProvider>(_unityContainer, InstanceNames.AppSettings, settingsController);




        }
    }
}

