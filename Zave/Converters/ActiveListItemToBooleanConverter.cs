using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Data;
using ZaveGlobalSettings.Data_Structures;
using System.Windows.Controls;

namespace Zave.Converters
{
    class ActiveListItemToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currItem = (ListViewItem)value;
            if (currItem.IsSelected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
