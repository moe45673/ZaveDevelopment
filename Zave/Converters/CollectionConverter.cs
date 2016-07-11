using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;
using ZaveGlobalSettings.Data_Structures.ZaveObservableCollection;

namespace Zave.Converters
{
    public class AvailableColorCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<ColorItem> obsColl = null;
            try
            {
                obsColl = new ObservableCollection<ColorItem>(value as ObservableImmutableList<ColorItem>);
                
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return obsColl;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableImmutableList<ColorItem> coll = new ObservableImmutableList<ColorItem>(value as ObservableCollection<ColorItem>);
            return coll;
        }
    }
}
