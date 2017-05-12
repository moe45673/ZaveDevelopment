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
using ZaveViewModel.ViewModels;

namespace ZaveViewModel.ViewModels
{
    public class ControlBarViewModel : BindableBase
    {

        private IEventAggregator _eventAggregator;

        private delegate Task<ObservableImmutableList<ColorItem>> ReturnListDel();

        private IUnityContainer _container;

        public DelegateCommand SaveZDFDelegateCommand { get; set; }
        public DelegateCommand OpenZDFDelegateCommand { get; set; }
        public DelegateCommand NewZDFDelegateCommand { get; set; }
        public DelegateCommand UndoZDFDelegateCommand { get; set; }
        public DelegateCommand RedoZDFDelegateCommand { get; set; }
        public DelegateCommand ScreenshotZDFDelegateCommand { get; set; }
        public DelegateCommand SaveASZDFDelegateCommand { get; set; }

        public DelegateCommand SwitchWindowModeDelegateCommand { get; set; }

        public ControlBarViewModel(IEventAggregator eventAggregator, IUnityContainer cont)
        {

            if (cont == null) throw new ArgumentNullException("container");            
            if (eventAggregator == null) throw new ArgumentNullException("eventAggregator");

            _activeColor = new Color();
            if (_eventAggregator == null && eventAggregator != null)
            {
                _eventAggregator = eventAggregator;
                _container = cont;
            }
            //ColorItemList = new ObservableImmutableList<ColorItem>();
            
            //ReturnListDel beginColorSet = async () => await SetColorsAsync();
            //ColorItemList = SetColorsAsync().Result;
            //beginColorSet.Invoke();
            //ActiveColor = Color.FromArgb(255, 255, 255, 0);
            //eventAggregator.GetEvent<ActiveColorUpdatedEvent>().Publish(ColorCategory.FromWPFColor(ActiveColor).Color);
            eventAggregator.GetEvent<WindowModeChangeEvent>().Subscribe(setSuffix);
            //eventAggregator.GetEvent<ActiveColorUpdatedEvent>().Subscribe(SetActiveColor);

            var vm = _container.Resolve<MainWindowViewModel>(InstanceNames.MainWindowViewModel) as MainWindowViewModel;
            
            setSuffix(vm.WinMode);
            SaveZDFDelegateCommand = vm.SaveZDFDelegateCommand;
            OpenZDFDelegateCommand = vm.OpenZDFDelegateCommand;
            NewZDFDelegateCommand = vm.NewZDFDelegateCommand;
            UndoZDFDelegateCommand = vm.UndoZDFDelegateCommand;
            RedoZDFDelegateCommand = vm.RedoZDFDelegateCommand;
            ScreenshotZDFDelegateCommand = vm.ScreenshotZDFDelegateCommand;
            SaveASZDFDelegateCommand = vm.SaveASZDFDelegateCommand;
            SwitchWindowModeDelegateCommand = vm.SwitchWindowModeCommand;
        }

        private string _suffix;
        public string Suffix
        {
            get
            {
                return _suffix;
            }
            set
            {
                SetProperty(ref _suffix, value);
            }
        }

        private void setSuffix(WindowMode wm)
        {
            switch (wm)
            {
                case (WindowMode.MAIN):
                    Suffix = String.Empty;
                    break;
                case (WindowMode.WIDGET):
                    Suffix = "_w";
                    break;
                default:
                    Suffix = String.Empty;
                    break;
            }
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


        //private ObservableImmutableList<ColorItem> _colorItemList;

        public ObservableImmutableList<ColorItem> ColorItemList
        {
            //get { return this._colorItemList; }
            //private set { SetProperty(ref _colorItemList, value); }
            get;
            set;
        }

        


        //async private Task<ObservableImmutableList<ColorItem>> SetColorsAsync()
        //{
        //    var items = new ObservableImmutableList<ColorItem>();
        //    //var converter = new System.Windows.Media.ColorConverter();

            
        //    var query = Enum.GetValues(typeof(AvailableColors))
        //        .Cast<AvailableColors>()
        //        .Except(new AvailableColors[] { AvailableColors.None }); //remove "None" from equation

        //    foreach (AvailableColors color in query)
        //    {
        //        var name = color.ToString();
        //        items.Add(new ColorItem((Color)ColorConverter.ConvertFromString(name), name));
        //    }

        //    //await Task.Delay(2000);
        //    //ActiveColor = Color.FromArgb(255, 255, 255, 0);
        //    System.Drawing.Color col = new System.Drawing.Color();
        //    col = ColorCategory.FromWPFColor(ActiveColor).Color;
        //    _eventAggregator.GetEvent<ActiveColorUpdatedEvent>().Publish(col);
        //    ColorItemList = new ObservableImmutableList<ColorItem>(items);
        //    OnPropertyChanged("ColorItemList");

        //    return items;
        //}



    }
}
