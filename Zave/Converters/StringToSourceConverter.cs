using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;

namespace Zave.Converters
{
    /// <summary>
    /// Takes two strings, concatenates them, and returns an ImageSource
    /// </summary>
    public class StringToSourceConverter : IValueConverter
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            
            ImageSourceConverter conv = new ImageSourceConverter();


            if (parameter is string)
            {
                var returnValue = string.Concat(value, parameter.ToString());
                ImageSource imgsrc = conv.ConvertFromString(returnValue) as ImageSource;
                return imgsrc;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }




        /// <summary>
        /// Takes two strings, concatenates them, and returns an ImageSource
        /// </summary>
        public class StringsToSourceConverter : IMultiValueConverter
        {

            /// <summary>
            /// 
            /// </summary>
            /// <param name="values"></param>
            /// <param name="targetType"></param>
            /// <param name="parameter"></param>
            /// <param name="culture"></param>
            /// <returns></returns>
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {


                ImageSourceConverter conv = new ImageSourceConverter();
                int suffixPos = ((String)parameter).Length - 4;
                var returnValue = ((String)parameter);
                //var returnValue = ((String)parameter).Insert(suffixPos, values[1].ToString());
                returnValue = Path.Combine(values[0].ToString(), returnValue);                
                ImageSource imgsrc = conv.ConvertFromString(returnValue) as ImageSource;
                return imgsrc;
                
               
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <param name="targetTypes"></param>
            /// <param name="parameter"></param>
            /// <param name="culture"></param>
            /// <returns></returns>
            

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
}