using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Regions;
using Zave.Views;
using Prism.Events;
using Prism.Modularity;
using ZaveGlobalSettings.Data_Structures;
using ZaveViewModel.ViewModels;
using Zave.Controllers;

namespace Zave.Module
{
    [Module(ModuleName = "MainWindowModule", OnDemand = true)]
    [ModuleDependency("IOModule")]
    [ModuleDependency("JsonModule")]
    [ModuleDependency("ColorPickerModule")]
    [ModuleDependency("AppSettingsModule")]
    public class MainWindowModule : ModuleBaseClass
    {
        //private ZaveController.EventInitSingleton eventInit;
        private IEventAggregator _agg;
        //private Zave.Controllers.MainWindowController mainWinController;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cont"></param>
        /// <param name="reg"></param>
        /// <param name="agg"></param>
        public MainWindowModule(IUnityContainer cont, IRegionManager reg, IEventAggregator agg) : base(cont, reg) {

            _agg = agg;
        }

        public override void Initialize()
        {
            //_unityContainer.RegisterInstance<MainWindow>(_unityContainer.Resolve<MainWindow>());
            //var viewmodel = _unityContainer.Resolve<ZaveViewModel.ViewModels.MainWindowViewModel>();
            //window.DataContext = viewmodel;
           
            var window = _unityContainer.Resolve<MainWindow>(InstanceNames.MainWindowView);
            //System.Windows.Forms.MessageBox.Show("Hooray!");
            window.DataContext = _unityContainer.Resolve<MainWindowViewModel>();

            //_unityContainer.RegisterInstance<MainWindow>(InstanceNames.MainWindowView, window);
            _unityContainer.RegisterInstance<MainWindowViewModel>(InstanceNames.MainWindowViewModel, ((MainWindowViewModel)window.DataContext));
            
            //var startingView = _unityContainer.Resolve<WidgetView>();
            //var altView = _unityContainer.Resolve<MainContainer>();
            //UnityContainerExtensions.RegisterType(_unityContainer, typeof(object), typeof(Views.MainWindow), "MainWindow");
            ZaveController.EventInitSingleton.GetInstance(_unityContainer, _unityContainer.Resolve<IEventAggregator>());
            var controller = _unityContainer.Resolve<Controllers.MainWindowController>();
            //if (((MainWindowViewModel)window.DataContext).WinMode == WindowMode.WIDGET) {
            //    ShiftWindowOntoScreenHelper.ShiftWindowOntoScreen(window);
            //    ShiftWindowOntoScreenHelper.ShiftWindowOntoDesiredCorner(window, DesiredCorner.BOTTOM_RIGHT);
            //}

            

        }
    }

    /// <summary>
    /// 
    /// </summary>
    



   
}
