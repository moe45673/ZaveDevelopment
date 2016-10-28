using System;
using System.Windows;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using ZaveService.ZDFEntry;

namespace Zave.Module
{
    public class DataServiceModule : ModuleBaseClass
    {
        public DataServiceModule(IUnityContainer cont) : base(cont) { }


        public override void Initialize()
        {
            _unityContainer.RegisterType<IZDFEntryService, ZDFEntryService>();
        }
    }
}
