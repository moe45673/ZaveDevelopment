using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Regions;
using ZaveGlobalSettings.Data_Structures;
using ZaveService.ZDFEntry;
using Zave.Module;

namespace Zave.Controllers
{
    public class MainContainerController
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IZDFEntryService entryService;
        
        public MainContainerController(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator, IZDFEntryService entryService)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (regionManager == null) throw new ArgumentNullException("regionManager");
            if (eventAggregator == null) throw new ArgumentNullException("eventAggregator");
            if (entryService == null) throw new ArgumentNullException("zdfEntryService");


            this.container = container;
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            this.entryService = entryService;

            eventAggregator.GetEvent<EntrySelectedEvent>().Subscribe(this.ZDFEntrySelected, true);
        }

        private void ZDFEntrySelected(string id)
        {
            if (string.IsNullOrEmpty(id)) return;

            var selectedEntry = this.entryService.getZDFEntry(id);



            IRegion mainContRegion = regionManager.Regions[RegionNames.MainContainerRegion];

            if (mainContRegion == null) return;

            var view = mainContRegion.GetView("ZDFEntryView") as Zave.Views.ZDFEntryView;

            if(view == null)
            {
                view = container.Resolve<Zave.Views.ZDFEntryView>();
                mainContRegion.Add(view, "ZDFEntryView");
            }
            else
            {
                mainContRegion.Activate(view);
            }

            var viewModel = view.DataContext as ZaveViewModel.ViewModels.ZDFEntryViewModel;

            if(viewModel != null)
            {
                viewModel.setProperties(selectedEntry);
            }

        }


    }
}
