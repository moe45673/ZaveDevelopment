using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaveGlobalSettings.Data_Structures;
using Xceed.Wpf.Toolkit;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Prism.Mvvm;
using Prism.Events;

namespace ZaveViewModel.ViewModels
{
    public class ControlBarViewModel : BindableBase
    {

        private IEventAggregator _eventAggregator;

        public ControlBarViewModel(IEventAggregator eventAggregator)
        {
            ColorItemList = new ObservableCollection<ColorItem>(setColors());
            _eventAggregator = eventAggregator;

        }

       

        private Color _activeColor;
        public Color ActiveColor
        {

            get { return _activeColor; }
            set
            {
                SetProperty(ref _activeColor, value);
            }

        }


        private ObservableCollection<ColorItem> _colorItemList;

        public ObservableCollection<ColorItem> ColorItemList
        {
            get { return this._colorItemList; }
            private set { SetProperty(ref _colorItemList, value); }
        }

      

        private List<ColorItem> setColors()
        {
            var items = new List<ColorItem>();
            var converter = new System.Windows.Media.ColorConverter();
            foreach (string color in Enum.GetNames(typeof(AvailableColors)))
            {
                items.Add(new ColorItem((Color)ColorConverter.ConvertFromString(color), color));

            }

            return items;
        }
    }
}
