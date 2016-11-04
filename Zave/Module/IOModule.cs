using System;
using System.Windows;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using ZaveService.IOService;

namespace Zave.Module
{
    [Module(ModuleName = "IOModule")]
    public class IOModule : ModuleBaseClass
    {
        public IOModule(IUnityContainer cont) : base(cont) { }


        public override void Initialize()
        {

            _unityContainer.RegisterType<IIOService, IOService>();
        }
    }
}
