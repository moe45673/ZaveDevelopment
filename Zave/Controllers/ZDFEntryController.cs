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
    public class ZDFEntryController
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IZDFEntryService entryService;
        
        public ZDFEntryController(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator, IZDFEntryService entryService)
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
            //Get the view
            IRegion entryRegion = regionManager.Regions[RegionNames.ZDFEntryDetailRegion];

            if (entryRegion == null) return;

            var view = entryRegion.GetView("ZDFEntryView") as Zave.Views.ZDFEntryView;

            
            
            //If no entry was selected, clear the view                       
            if (string.IsNullOrEmpty(id))
            {
               if (view != null)
                {
                    entryRegion.Deactivate(view);
                }
                return;
            }

            //otherwise, populate view with selected Entry
            ZaveModel.ZDFEntry.IZDFEntry selectedEntry = this.entryService.getZDFEntry(id);


            

            if(view == null)
            {
                view = container.Resolve<Zave.Views.ZDFEntryView>();
                entryRegion.Add(view, "ZDFEntryView");
                //entryRegion.Activate(view);
            }
            
                entryRegion.Activate(view);
              
            

            var viewModel = view.DataContext as ZaveViewModel.ViewModels.ZDFEntryViewModel;

            if(viewModel != null)
            {
                viewModel.setProperties(selectedEntry);
            }

        }


    }
}
