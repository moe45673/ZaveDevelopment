using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
using System.Threading;

namespace ZaveViewModel.ViewModels
{
    public class ControlBarViewModel : BindableBase
    {

        private IEventAggregator _eventAggregator;

        private delegate Task<ObservableImmutableList<ColorItem>> returnListDel();
        

        public ControlBarViewModel(IEventAggregator eventAggregator)
        {
            _activeColor = new Color();
            if (_eventAggregator == null && eventAggregator != null)
                _eventAggregator = eventAggregator;

            ColorItemList = new ObservableImmutableList<ColorItem>();
            
            returnListDel beginColorSet = async () => await setColorsAsync();
            beginColorSet.Invoke();
            ActiveColor = Color.FromArgb(255, 255, 255, 0);




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

       

        async private Task<ObservableImmutableList<ColorItem>> setColorsAsync()
        {
            var items = new ObservableImmutableList<ColorItem>();
            //var converter = new System.Windows.Media.ColorConverter();
            foreach (string color in Enum.GetNames(typeof(AvailableColors)))
            {
                items.Add(new ColorItem((Color)ColorConverter.ConvertFromString(color), color));

            }
            await Task.Delay(2000);
            //ActiveColor = Color.FromArgb(255, 255, 255, 0);
            System.Drawing.Color col = new System.Drawing.Color();
            col = ColorCategory.FromWPFColor(ActiveColor).Color;
            _eventAggregator.GetEvent<MainControlsUpdateEvent>().Publish(col);
            ColorItemList = new ObservableImmutableList<ColorItem>(items);
            OnPropertyChanged("ColorItemList");
            return items;
        }



    }
}
