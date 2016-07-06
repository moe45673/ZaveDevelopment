using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Regions;

namespace Zave.Module
{
    public class MainWindowModule : ModuleBaseClass
    {
        public MainWindowModule(IUnityContainer cont, IRegionViewRegistry reg) : base(cont, reg) { }

        public override void Initialize()
        {
            _regionViewRegistry.RegisterViewWithRegion("MainWindow", typeof(Views.MainWindow));
            UnityContainerExtensions.RegisterType(_unityContainer, typeof(object), typeof(Views.MainWindow), "MainWindow");
        }
    }
}
