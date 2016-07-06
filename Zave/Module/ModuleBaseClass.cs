using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;

namespace Zave.Module
{
    public abstract class ModuleBaseClass : IModule
    {
        protected readonly IRegionViewRegistry _regionViewRegistry;
        protected readonly IUnityContainer _unityContainer;


        public ModuleBaseClass(IUnityContainer cont, IRegionViewRegistry registry)
        {
            this._regionViewRegistry = registry;
            _unityContainer = cont.CreateChildContainer();
        }

        public abstract void Initialize();
    }
}
