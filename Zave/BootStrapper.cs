﻿using System;
using System.Windows;
using Prism.Unity;
using Zave.Views;
using Prism.Mvvm;
using System.Globalization;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Zave.Module;
using ZaveViewModel.ViewModels;
using Prism.Events;
using Zave.Controllers;
using ZaveService.ZDFEntry;

namespace Zave
{
    /// <summary>
    /// Sets up the Unity DI using the Prism 6 library
    /// </summary>
    public class BootStrapper : UnityBootstrapper
    {

        /// <summary>
        /// Sets Prism Shell to MainWindow class
        /// </summary>
        /// <returns></returns>
        protected override DependencyObject CreateShell()
        {
            
            return UnityContainerExtensions.Resolve<MainWindow>(Container);
        }
        
        /// <summary>
        /// Shows the MainWindow shell with custom settings
        /// </summary>
        protected override void InitializeShell()
        {
            ZaveApp.Current.MainWindow = Shell as Window;
            Application.Current.MainWindow.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
            moduleCatalog.AddModule(typeof(MainWindowModule));
            moduleCatalog.AddModule(typeof(IOModule));
            moduleCatalog.AddModule(typeof(DataServiceModule));
            //moduleCatalog.AddModule(typeof(MainContainerModule));
            moduleCatalog.AddModule(typeof(ZDFEntryModule));
            //moduleCatalog.AddModule(typeof(ZDFModule));





        }

        /// <summary>
        /// Resolves the classes that Unity recognizes
        /// </summary>
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            //UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(MainWindow), "MainWindow");
            UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(MainContainer), "MainContainer");
            UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(Menu), "Menu");
            UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(ControlBar), "ControlBar");
            UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(ZDFList), "ZDFList");
            UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(ZDFView), "ZDFView");
            //UnityContainerExtensions.RegisterType(Container, typeof(IZDFEntryService), typeof(ZDFEntryService), "ZDFEntryService");
            //UnityContainerExtensions.RegisterType(Container, typeof(object), typeof())
            //UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(ZDFEntryView), "ZDFEntryView");
            //UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(MainContainerController), "ZDFEntryView");
            //UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(ZaveModel.ZDF.ZDFSingleton), "ZDFModel");
            //UnityContainerExtensions.RegisterInstance(Container, typeof(ZaveModel.ZDF.ZDFSingleton), ZaveModel.ZDF.ZDFSingleton.GetInstance(Container.Resolve(typeof(EventAggregator)) as EventAggregator));



            UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(ModalInputDialog), "ModalInputDialog");
            //UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(ModalInputDialogViewModel), "ModalInputDialogViewModel");


        }

        /// <summary>
        /// Overrides the default VMLocator to wire to ViewModel assembly
        /// </summary>
        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(
            viewType =>
            {
                var viewName = viewType.FullName;
                var viewAssemblyName = viewType.Assembly.GetName().Name;

                var viewModelNameSuffix = viewName.EndsWith("View") ? "Model" : "ViewModel";

                var viewModelName = viewName.Replace("Views", "ViewModels") + viewModelNameSuffix;
                viewModelName = viewModelName.Replace(viewAssemblyName, viewAssemblyName + "ViewModel");
                var viewModelAssemblyName = viewAssemblyName + "ViewModel";
                var viewModelTypeName = string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}, {1}",
                    viewModelName,
                    viewModelAssemblyName);

                return Type.GetType(viewModelTypeName);
            });

        }

    }
}
