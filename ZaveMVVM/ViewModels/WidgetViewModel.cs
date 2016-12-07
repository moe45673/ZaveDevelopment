using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Events;
using Prism.Common;
using Prism.Unity;
using Prism.Regions;
using Microsoft.Practices.Unity;
using System.Windows.Media;
using ZaveGlobalSettings.Data_Structures;

namespace ZaveViewModel.ViewModels
{
    public class WidgetViewModel : BindableBase
    {
        private IEventAggregator _eventAgg;
        private IUnityContainer _container;
        private IRegionManager _regionManager;

        public WidgetViewModel(IUnityContainer cont, IEventAggregator eventAgg, IRegionManager manager)
        {
            if (cont == null) throw new ArgumentNullException("container");
            if (manager == null) throw new ArgumentNullException("regionManager");
            if (eventAgg == null) throw new ArgumentNullException("eventAggregator");

            _eventAgg = eventAgg;
            _container = cont;
            _regionManager = manager;

            _eventAgg.GetEvent<ActiveColorUpdatedEvent>().Subscribe(UpdateColor);
            ActiveColor = _container.Resolve<ColorPickerViewModel>().ActiveColor;

        }

        private Color _activeColor;
        public Color ActiveColor {
            get
            {
                return _activeColor;
            }
            set
            {
                
                SetProperty<Color>(ref _activeColor, value);
            }
        }

        private void UpdateColor(System.Drawing.Color color)
        {
            ActiveColor = Color.FromArgb(color.A, color.R, color.G, color.B);
        }

    }
}
