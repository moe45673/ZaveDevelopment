using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.IO;

namespace Zave.Converters
{
    [ValueConversion(typeof(string), typeof(FlowDocument))]
    public class FlowDocumentToXamlConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Converts from XAML markup to a WPF FlowDocument.
        /// </summary>
        public object Convert(object value, System.Type targetType,
        object parameter, System.Globalization.CultureInfo culture)
        {
            /* See http://stackoverflow.com/questions/897505/
		getting-a-flowdocument-from-a-xaml-template-file */

            try
            {
                var flowDocument = new FlowDocument();
                if (value != null)
                {
                    var xamlText = (string)value;
                    TextRange content = new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd);
                    if (content.CanLoad(DataFormats.Rtf) && string.IsNullOrEmpty(xamlText) == false)
                    {
                        byte[] valueArray = Encoding.ASCII.GetBytes(xamlText);
                        using (MemoryStream stream = new MemoryStream(valueArray))
                        {
                            content.Load(stream, DataFormats.Rtf);
                        }
                    }
                }

                // Set return value
                return flowDocument;
            }
            catch(Exception ex)
            {

            }
            return null;
        }

        /// <summary>
        /// Converts from a WPF FlowDocument to a XAML markup string.
        /// </summary>
        public object ConvertBack(object value, System.Type targetType,
        object parameter, System.Globalization.CultureInfo culture)
        {
            /* This converter does not insert returns or indentation into the XAML. 
             * If you need to indent the XAML in a text box, 
             * see http://www.knowdotnet.com/articles/indentxml.html */

            // Exit if FlowDocument is null
            if (value == null) return string.Empty;

            // Get flow document from value passed in
            var flowDocument = (FlowDocument)value;

            // Convert to XAML and return
            return XamlWriter.Save(flowDocument);
        }

        #endregion
    }
}
