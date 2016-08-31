using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZaveGlobalSettings.Data_Structures;
using Xceed.Wpf.Toolkit;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Prism.Mvvm;
using Prism.Events;
using Prism.Commands;
using System.Threading;
using ZaveGlobalSettings.Data_Structures.ZaveObservableCollection;
using ZaveModel.ZDFColors;
using Microsoft.Practices.Unity;

namespace ZaveViewModel.ViewModels
{
    public class ControlBarViewModel : BindableBase
    {

        private IEventAggregator _eventAggregator;

        private delegate Task<ObservableImmutableList<ColorItem>> ReturnListDel();

        private IUnityContainer _container;

        public DelegateCommand SaveZDFDelegateCommand { get; set; }
        public DelegateCommand OpenZDFDelegateCommand { get; set; }
        public DelegateCommand NewZDFEntryDelegateCommand { get; set; }
        

        public ControlBarViewModel(IEventAggregator eventAggregator, IUnityContainer cont)
        {
            _activeColor = new Color();
            if (_eventAggregator == null && eventAggregator != null)
            {
                _eventAggregator = eventAggregator;
                _container = cont;
            }
            ColorItemList = new ObservableImmutableList<ColorItem>();

            //ReturnListDel beginColorSet = async () => await SetColorsAsync();
            ColorItemList = SetColorsAsync().Result;
            //beginColorSet.Invoke();
            ActiveColor = Color.FromArgb(255, 255, 255, 0);
            eventAggregator.GetEvent<MainControlsUpdateEvent>().Publish(ColorCategory.FromWPFColor(ActiveColor).Color);

            var vm = _container.Resolve(typeof(MainContainerViewModel)) as MainContainerViewModel;
            
            SaveZDFDelegateCommand = vm.SaveZDFDelegateCommand;
            OpenZDFDelegateCommand = vm.OpenZDFDelegateCommand;
            NewZDFEntryDelegateCommand = vm.NewZDFEntryDelegateCommand;
            


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

       

        async private Task<ObservableImmutableList<ColorItem>> SetColorsAsync()
        {
            var items = new ObservableImmutableList<ColorItem>();
            //var converter = new System.Windows.Media.ColorConverter();
            foreach (string color in Enum.GetNames(typeof(AvailableColors)))
            {
                items.Add(new ColorItem((Color)ColorConverter.ConvertFromString(color), color));

            }
            //await Task.Delay(2000);
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
