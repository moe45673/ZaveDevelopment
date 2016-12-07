using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism;
using Prism.Common;
using Prism.Regions;
using Microsoft.Practices.Unity;
using Prism.Events;
using ZaveGlobalSettings.Data_Structures.ZaveObservableCollection;
using Xceed.Wpf.Toolkit;
using ZaveGlobalSettings.Data_Structures;
using ZaveModel.ZDFColors;
using System.Windows.Media;

namespace ZaveViewModel.ViewModels
{
    public class ColorPickerViewModel : BindableBase
    {

        private IEventAggregator _eventAggregator;
        private IUnityContainer _container;
        public ColorPickerViewModel(IUnityContainer cont, IRegionManager rmanage, IEventAggregator agg)
        {
            _container = cont;
            _eventAggregator = agg;
            ColorItemList = new ObservableImmutableList<ColorItem>();

            _container.RegisterInstance<ColorPickerViewModel>(this);

            //ReturnListDel beginColorSet = async () => await SetColorsAsync();
            ColorItemList = SetColorsAsync().Result;
            //beginColorSet.Invoke();
            ActiveColor = Color.FromArgb(255, 255, 255, 0);
            _eventAggregator.GetEvent<ActiveColorUpdatedEvent>().Publish(ColorCategory.FromWPFColor(ActiveColor).Color);
        }

        private void SetActiveColor(System.Drawing.Color col)
        {
            ActiveColor = Color.FromArgb(col.A, col.R, col.G, col.B);
        }

        private Color _activeColor;
        public Color ActiveColor
        {

            get { return _activeColor; }
            set
            {
                if (_activeColor != value)
                {
                    SetProperty(ref _activeColor, value);
                    ColorCategory colCat = ColorCategory.FromWPFColor(ActiveColor);
                    _eventAggregator.GetEvent<ActiveColorUpdatedEvent>().Publish(colCat.Color);
                }
            }

        }

        public ObservableImmutableList<ColorItem> ColorItemList
        {
            //get { return this._colorItemList; }
            //private set { SetProperty(ref _colorItemList, value); }
            get;
            set;
        }




        async private Task<ObservableImmutableList<ColorItem>> SetColorsAsync()
        {
            var items = new ObservableImmutableList<ColorItem>();
            //var converter = new System.Windows.Media.ColorConverter();


            var query = Enum.GetValues(typeof(AvailableColors))
                .Cast<AvailableColors>()
                .Except(new AvailableColors[] { AvailableColors.None }); //remove "None" from equation

            foreach (AvailableColors color in query)
            {
                var name = color.ToString();
                items.Add(new ColorItem((Color)ColorConverter.ConvertFromString(name), name));
            }

            //await Task.Delay(2000);
            //ActiveColor = Color.FromArgb(255, 255, 255, 0);
            System.Drawing.Color col = new System.Drawing.Color();
            col = ColorCategory.FromWPFColor(ActiveColor).Color;
            _eventAggregator.GetEvent<ActiveColorUpdatedEvent>().Publish(col);
            ColorItemList = new ObservableImmutableList<ColorItem>(items);
            OnPropertyChanged("ColorItemList");

            return items;
        }



    }
}
