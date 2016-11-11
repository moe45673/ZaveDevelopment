using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Data;

namespace Zave.Converters
{

    public class WindowModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var winMode = Enum.GetName(targetType, value);

            return winMode;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("cannot convert to Window Mode");
        }

    }
}
