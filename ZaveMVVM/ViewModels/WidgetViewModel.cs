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
            _eventAgg.GetEvent<WindowModeChangeEvent>().Subscribe(ChangeIsActive);
            ActiveColor = _container.Resolve<ColorPickerViewModel>().ActiveColor;


            var win = _container.Resolve<MainWindowViewModel>(InstanceNames.MainWindowViewModel);
            
            ChangeIsActive(win.WinMode);


        }

        #region Properties
        private bool _isActive;

        public bool IsActive
        {
            get { return this._isActive; }
            set { SetProperty(ref _isActive, value); }
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
#endregion


        #region Event Delegates

        private void ChangeIsActive(WindowMode wm)
        {
            if(wm == WindowMode.WIDGET)
            {
                IsActive = true;
            }
            else
            {
                IsActive = false;
            }
        }

        private void UpdateColor(System.Drawing.Color color)
        {
            ActiveColor = Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        #endregion

    }
}
