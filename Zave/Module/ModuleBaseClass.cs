using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using ZaveController;

namespace Zave.Module
{
    public abstract class ModuleBaseClass : IModule
    {
        protected readonly IRegionManager _regionManager;
        protected readonly IUnityContainer _unityContainer;

        

        public ModuleBaseClass(IUnityContainer cont, IRegionManager registry)
        {
            this._regionManager = registry;
            _unityContainer = cont;
        }

        public abstract void Initialize();
    }
}
