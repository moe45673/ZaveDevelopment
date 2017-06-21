using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Data;
using ZaveGlobalSettings.Data_Structures;

namespace Zave.Converters
{
    public class TextColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Color col = ((SolidColorBrush)value).Color;
                var grayCol = ColorHelper.ToGrayscaleARGB(ColorHelper.FromWPFColor(col));
                if (isDarkBackground(grayCol))
                {
                    return new SolidColorBrush(Color.FromRgb(255, 255, 255));

                }
                else
                {
                    return new SolidColorBrush(Color.FromRgb(00, 00, 00));
                }
            }
            catch(NullReferenceException nre)
            {
                return new SolidColorBrush(Color.FromRgb(00, 00, 00));
            }
           
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color col = (value as SolidColorBrush).Color;
            return col;
        }

        private bool isDarkBackground(System.Drawing.Color grayCol)
        {
            bool lightText = false;
            if (grayCol.R < 128)
                lightText = true;

            return lightText;

        }
    }
}
