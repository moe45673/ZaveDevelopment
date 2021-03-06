﻿using System;
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
    /// <summary>
    /// 
    /// </summary>
    public class ZDFEntryController
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private readonly IZDFEntryService entryService;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="regionManager"></param>
        /// <param name="eventAggregator"></param>
        /// <param name="entryService"></param>
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

            eventAggregator.GetEvent<ActiveEntryUpdatedEvent>().Subscribe(this.ZDFActiveEntryUpdated, true);
        }

        private void ZDFActiveEntryUpdated(string id)
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
