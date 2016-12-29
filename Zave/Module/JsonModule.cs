using System;
using System.Windows;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using ZaveService.JSONService;

namespace Zave.Module
{
    [Module(ModuleName = "JsonModule")]
    public class JsonModule : ModuleBaseClass
    {
        public JsonModule(IUnityContainer cont) : base(cont) { }


        public override void Initialize()
        {

            _unityContainer.RegisterType<IJsonService, JsonService>();
        }
    }
}