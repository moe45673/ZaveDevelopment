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
    public class BootStrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Microsoft.Practices.Unity.UnityContainerExtensions.Resolve<MainWindow>(Container);
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(ControlBar), "ControlBar");
            UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(ZDFEntryDetails), "ZDFEntryDetails");
            UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(ZDFEntryList), "ZDFEntryList");
            UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(ZDFList), "ZDFList");

            var container = this.Container;

            //ServiceLocator.SetLocatorProvider(() => container);

        }

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
