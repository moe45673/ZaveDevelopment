using System;
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
using ZaveGlobalSettings.Data_Structures;
using System.IO;
using Prism.Commands;
using System.Threading.Tasks;

namespace Zave
{
    /// <summary>
    /// Sets up the Unity DI using the Prism 6 library
    /// </summary>
    public class BootStrapper : UnityBootstrapper
    {

        StartupEventArgs startup;

        /// <summary>
        /// Sets Prism Shell to MainWindow class
        /// 
        ///
        /// </summary>
        /// <returns></returns>
        //Called Before Modules are initialized
        protected override DependencyObject CreateShell()
        {
            var window = Container.Resolve<MainWindow>();
            Container.RegisterInstance(InstanceNames.MainWindowView, window);


            return window;
        }
        
        /// <summary>
        /// Shows the MainWindow shell with custom settings
        /// </summary>
        protected override void InitializeShell()
        {
            //ZaveApp.Current.MainWindow = Shell as Window;

            var window = Shell as MainWindow;
            //using (var memStream = new MemoryStream(Properties.Resources.marker_cursor2))
            //{
            //window.Cursor = new System.Windows.Input.Cursor(@"C:\Users\Moshe\Documents\Visual Studio 2015\Projects\Zave\ZaveGlobalSettings\Resources\marker-cursor2.cur");
            //}
            window.Show();
            
        }

        public void Run(StartupEventArgs e)
        {
            base.Run();
            if (e.Args.Length > 0)
            {
                var baseVM = Container.Resolve<MainWindow>(InstanceNames.MainWindowView).DataContext as MainWindowViewModel;
                var command = baseVM.OpenZDFFromFileDelegateCommand;
                //Task.Factory.StartNew(async () => await command.Execute());
                //var commAction = new Prism.Interactivity.InvokeCommandAction();
                //commAction.Command = command;
                //commAction.CommandParameter = e.Args[0];
                command.Execute(e.Args[0]);
            }
            
        }



        /// <summary>
        /// 4
        /// </summary>
        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            
            ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
            moduleCatalog.AddModule(typeof(IOModule));
            moduleCatalog.AddModule(typeof(ApplicationSettingsModule));
            moduleCatalog.AddModule(typeof(JsonModule));
            moduleCatalog.AddModule(typeof(DataServiceModule));
            moduleCatalog.AddModule(typeof(MainWindowModule));
            moduleCatalog.AddModule(typeof(ZDFModule));
            moduleCatalog.AddModule(typeof(MainContainerModule));
            moduleCatalog.AddModule(typeof(ZDFEntryModule));
            moduleCatalog.AddModule(typeof(WidgetModule));
            moduleCatalog.AddModule(typeof(ColorPickerModule));
            
            
            //moduleCatalog.AddModule(typeof(ZDFList));





        }

        /// <summary>
        /// Resolves the classes that Unity recognizes
        /// </summary>
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            
            UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(ZDFList), "ZDFList");

            

            UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(ModalInputDialog), "ModalInputDialog");
            //UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(ModalInputDialogViewModel), "ModalInputDialogViewModel");


        }

        /// <summary>
        /// Child class of ModuleCatalog to dynamically load Modules
        /// </summary>
        /// <returns></returns>
        //protected override IModuleCatalog CreateModuleCatalog()
        //{
        //    DynamicDirectoryModuleCatalog catalog = new DynamicDirectoryModuleCatalog(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Module"));
        //    return catalog;
        //}

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
