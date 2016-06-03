using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaveGlobalSettings.Data_Structures;
using Xceed.Wpf.Toolkit;
using System.Collections.ObjectModel;
using System.Windows.Media;


namespace ZaveViewModel.ViewModels
{
    public sealed class ZaveControlsViewModel : ObservableObject
    {
        private static readonly ZaveControlsViewModel instance = new ZaveControlsViewModel();

        private ZaveControlsViewModel()
        {
            ColorItemList = new ObservableCollection<ColorItem>(setColors());

        }

        public static ZaveControlsViewModel Instance
        {
            get
            {
                return instance;
               
            }
        }

        private static Color _activeColor;
        public Color ActiveColor
        {
            
            get { return _activeColor; }
            set
            {
                _activeColor = value;
                OnPropertyChanged("ActiveColor");
            }
                
        }

        public ObservableCollection<ColorItem> ColorItemList
        {
            get;
            private set;
        }

        private List<ColorItem> setColors()
        {
            var items = new List<ColorItem>();
            var converter = new System.Windows.Media.ColorConverter();
            foreach(string color in Enum.GetNames(typeof(AvailableColors)))
            {
                items.Add(new ColorItem((Color)ColorConverter.ConvertFromString(color), color));
                
            }                     

            return items;
        }

        
    }
}
