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
using ZaveModel.ZDFEntry;

namespace Zave.Converters
{
    /// <summary>
    /// 
    /// </summary>
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

    public class CommentCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<IEntryComment> obsColl = null;
            try
            {
                obsColl = new ObservableCollection<IEntryComment>(value as ObservableImmutableList<IEntryComment>);


            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return obsColl;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableImmutableList<IEntryComment> coll = new ObservableImmutableList<IEntryComment>(value as ObservableCollection<IEntryComment>);
            return coll;
        }
    }
}
