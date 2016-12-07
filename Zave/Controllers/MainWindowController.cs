using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Regions;
using Prism.Mvvm;
using ZaveGlobalSettings.Data_Structures;
using Zave.Module;
using Prism.Modularity;
using ZaveViewModel.ViewModels;

namespace Zave.Controllers
{
    public class MainWindowController
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        

        public MainWindowController(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (regionManager == null) throw new ArgumentNullException("regionManager");
            if (eventAggregator == null) throw new ArgumentNullException("eventAggregator");
            


            this.container = container;
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            

            eventAggregator.GetEvent<WindowModeChangeEvent>().Subscribe(WindowModeChangeAbstract, true);
        }

        private void WindowModeChangeAbstract(WindowMode winMode) {

            //var main = container.Resolve<MainWindowViewModel>();

            switch (winMode)
            {
                
            
                case (WindowMode.MAIN):
                    WindowModeChange<Views.MainContainer, MainContainerViewModel>("MainContainer");
                    
                    break;
                case (WindowMode.WIDGET):
                    //var modManager = container.Resolve<IModuleManager>();
                    //modManager.LoadModule("WidgetModule");
                    WindowModeChange<Views.WidgetView, WidgetViewModel>("WidgetView");  
                            
                    break;
                default:                    
                    break;
            }
        }

        private void WindowModeChange<ViewType, ViewModelType>(string uri) 
            where ViewType : System.Windows.Controls.UserControl 
            where ViewModelType : BindableBase
        { 

        IRegion mviewRegion = regionManager.Regions[RegionNames.MainViewRegion];

            if (mviewRegion == null) return;


            regionManager.RequestNavigate(mviewRegion.Name, new Uri(uri, UriKind.Relative));         

            
            
           

        }

        

    }
}
