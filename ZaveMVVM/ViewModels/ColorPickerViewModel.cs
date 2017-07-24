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
using System.Diagnostics;
using Prism.Commands;

namespace ZaveViewModel.ViewModels
{
    public class ColorPickerViewModel : BindableBase
    {

        private IEventAggregator _eventAggregator;
        private IUnityContainer _container;


        public DelegateCommand<string> SetActiveColorCommand { get; set; }

        public ColorPickerViewModel(IUnityContainer cont, IRegionManager rmanage, IEventAggregator agg)
        {
            _container = cont;
            _eventAggregator = agg;
            ColorItemList = new ObservableImmutableList<ColorItem>();

            _container.RegisterInstance<ColorPickerViewModel>(this);
            SetActiveColorCommand = new DelegateCommand<string>(SetActiveColor);
           
            SetColorsAsync();
            var settings = _container.Resolve<IConfigProvider>();

            //TODO Make this possible to change in settings
            settings.ActiveColor = System.Drawing.Color.FromArgb(255, 255, 255, 0);

            SetActiveColor(settings.ActiveColor);
            _eventAggregator.GetEvent<ActiveColorUpdatedEvent>().Publish(ColorHelper.FromWPFColor(ActiveColor));
        }

        private void SetActiveColor(System.Drawing.Color col)
        {
            ActiveColor = Color.FromArgb(col.A, col.R, col.G, col.B);
        }

        private void SetActiveColor(string hexValue)
        {
            System.Drawing.Color col = System.Drawing.ColorTranslator.FromHtml(hexValue);
            SetActiveColor(col);
            
        }

        private Color _activeColor;
        public Color ActiveColor
        {

            get { return _activeColor; }
            set
            {
                if (_activeColor != value)
                {
                    SetProperty(ref _activeColor, value, () =>
                    {
                        _eventAggregator.GetEvent<ActiveColorUpdatedEvent>().Publish(ColorHelper.FromWPFColor(_activeColor));
                    });
                   
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




        async private Task SetColorsAsync()
        {
            //var items = new ObservableImmutableList<ColorItem>();
            //var converter = new System.Windows.Media.ColorConverter();

            var items = await SetColorListAsync();

            System.Drawing.Color col = new System.Drawing.Color();
            col = ColorCategory.FromWPFColor(ActiveColor).Color;
            _eventAggregator.GetEvent<ActiveColorUpdatedEvent>().Publish(col);
            ColorItemList = new ObservableImmutableList<ColorItem>(items);
            OnPropertyChanged("ColorItemList");

            
        }

        private async Task<ObservableImmutableList<ColorItem>> SetColorListAsync()
        {
            return await Task.Run(() =>
            {
                ObservableImmutableList<ColorItem> innerItems = new ObservableImmutableList<ColorItem>();
                Debug.WriteLine("Started Lambda");
                var query = Enum.GetValues(typeof(AvailableColors))
                    .Cast<AvailableColors>()
                    .Except(new AvailableColors[] { AvailableColors.None }); //remove "None" from equation

                Debug.WriteLine("Got Lambda Query");
                foreach (AvailableColors color in query)
                {

                    var name = color.ToString();
                    innerItems.Add(new ColorItem((Color)ColorConverter.ConvertFromString(name), name));
                    Debug.Write(name + ", ");

                }
                Debug.WriteLine("Added");
                Debug.WriteLine("Finished Lambda ForEach");
                return innerItems;
            });

            //Debug.WriteLine("Finished Entire Lambda");

            ////await Task.Delay(2000);
            ////ActiveColor = Color.FromArgb(255, 255, 255, 0);
            //System.Drawing.Color col = new System.Drawing.Color();
            //col = ColorCategory.FromWPFColor(ActiveColor).Color;
        }



    }
}
