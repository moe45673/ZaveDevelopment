using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaveGlobalSettings.Data_Structures;
using ZaveGlobalSettings.Data_Structures.Observable;
using ZaveGlobalSettings.Events;
using ZaveModel.Colors;
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
            _activeColor = new Color();
            if (_eventAggregator == null && eventAggregator != null)
                _eventAggregator = eventAggregator;

            ColorItemList = new ObservableImmutableList<ColorItem>(setColors());
            
            

        }

       

        private Color _activeColor;
        public Color ActiveColor
        {

            get { return _activeColor; }
            set
            {
                SetProperty(ref _activeColor, value);
                ColorCategory colCat = ColorCategory.FromWPFColor(ActiveColor);
                _eventAggregator.GetEvent<MainControlsUpdateEvent>().Publish(colCat.Color);
            }

        }


        //private ObservableImmutableList<ColorItem> _colorItemList;

        public ObservableImmutableList<ColorItem> ColorItemList
        {
            //get { return this._colorItemList; }
            //private set { SetProperty(ref _colorItemList, value); }
            get;
            set;
        }

      

        private List<ColorItem> setColors()
        {
            var items = new List<ColorItem>();
            //var converter = new System.Windows.Media.ColorConverter();
            foreach (string color in Enum.GetNames(typeof(AvailableColors)))
            {
                items.Add(new ColorItem((Color)ColorConverter.ConvertFromString(color), color));

            }

            ActiveColor = Color.FromRgb(255, 255, 0);

            return items;
        }
    }
}
