using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Unity;
using Zave.Views;
using Prism.Mvvm;
using System.Globalization;

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

            Microsoft.Practices.Unity.UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(ControlBar), "ControlBar");
            Microsoft.Practices.Unity.UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(ZDFEntryDetails), "ZDFEntryDetails");
            Microsoft.Practices.Unity.UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(ZDFEntryList), "ZDFEntryList");
            Microsoft.Practices.Unity.UnityContainerExtensions.RegisterType(Container, typeof(object), typeof(ZDFList), "ZDFList");
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
                viewModelName = viewModelName.Replace(viewAssemblyName, viewAssemblyName + ".Process");
                var viewModelAssemblyName = viewAssemblyName + ".Process";
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
