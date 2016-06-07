using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.Windows;
using Prism.Unity;
using Zave.Views;
using Prism.Mvvm;
using Prism.Events;
using System.Globalization;
using Microsoft.Practices.Unity;

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
            return Microsoft.Practices.Unity.UnityContainerExtensions.Resolve<MainWindow>(Container);
        }
        
        /// <summary>
        /// Shows the MainWindow shell with custom settings
        /// </summary>
        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        /// <summary>
        /// Resolves the classes that Unity recognizes
        /// </summary>
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(ControlBar), "ControlBar");
            UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(ZDFEntryView), "ZDFEntryView");
            UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(ZDFView), "ZDFView");
            UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(ZDFList), "ZDFList");
            

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
