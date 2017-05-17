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
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;

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
                if (!(string.IsNullOrEmpty((string)value)))
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
            catch (Exception ex)
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

    /// <summary>
    /// 
    /// </summary>
    public class RtfToPlainTextConverter : IValueConverter
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
            string plainText = "";

            string rtfText = (string)value;
            if (RichTextBoxExtensions.IsRtf(rtfText))
            {
                var richTxtBx = new RichTextBox();
                richTxtBx.Rtf = rtfText;
                var indices = richTxtBx.Rtf.AllIndexesOf(@"\pict");
                //foreach(int i in indices)
                //{
                //    MessageBox.Show(System.Convert.ToString(i) +  "\n");
                //}

                plainText = richTxtBx.Text;
            }

            else
            {
                plainText = (string)value;
            }

            return plainText;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

namespace System
{
    public static class ListExtension
    {
        public static IEnumerable<int> AllIndexesOf(this string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    break;
                yield return index;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class RichTextBoxExtensions
    {
        //public static void SetTextOrRtf(this RichTextBox richTextBox, IEnumerable<string> texts)
        //{
        //    if (richTextBox == null || texts == null)
        //        throw new ArgumentNullException();
        //    using (richTextBox.BeginUpdate())
        //    {
        //        bool first = true;
        //        richTextBox.Clear();
        //        foreach (var text in texts)
        //        {
        //            if (text == null)
        //                continue;
        //            if (first)
        //                first = false;
        //            else
        //                richTextBox.AppendText("\n");
        //            richTextBox.Select(richTextBox.TextLength, 0);
        //            if (text.IsRtf())
        //            {
        //                richTextBox.SelectedRtf = text;
        //            }
        //            else
        //            {
        //                richTextBox.SelectedText = text;
        //            }
        //        }
        //        richTextBox.SelectionStart = richTextBox.Text.Length;
        //        richTextBox.ScrollToCaret();
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsRtf(this string text)
        {
            // Adapted from http://stackoverflow.com/questions/22502924/how-to-determine-text-format-in-c-sharp
            if (text == null)
                return false;
            const string rtfPrefix = @"{\rtf1";
            int start = 0;
            for (; start < text.Length && Char.IsWhiteSpace(text[start]); start++)
                ;
            return string.Compare(text, start, rtfPrefix, 0, rtfPrefix.Length, StringComparison.Ordinal) == 0;
        }

        //private const int WM_USER = 0x0400;
        //private const int EM_SETEVENTMASK = (WM_USER + 69);
        //private const int WM_SETREDRAW = 0x0b;

        //class ResetEventMask : IDisposable
        //{
        //    readonly IntPtr oldEventMask;
        //    RichTextBox richTextBox;

        //    public ResetEventMask(RichTextBox richTextBox, IntPtr oldEventMask)
        //    {
        //        this.richTextBox = richTextBox;
        //        this.oldEventMask = oldEventMask;
        //    }

        //    #region IDisposable Members

        //    public void Dispose()
        //    {
        //        var richTextBox = Interlocked.Exchange(ref this.richTextBox, null);
        //        if (richTextBox != null && !richTextBox.IsDisposed)
        //        {
        //            SendMessage(richTextBox.Handle, WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);
        //            SendMessage(richTextBox.Handle, EM_SETEVENTMASK, IntPtr.Zero, oldEventMask);
        //        }
        //    }

        //    #endregion
        //}

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        //public static IDisposable BeginUpdate(this RichTextBox richTextBox)
        //{
        //    if (richTextBox == null)
        //        throw new ArgumentNullException();
        //    // Adapted from http://stackoverflow.com/questions/192413/how-do-you-prevent-a-richtextbox-from-refreshing-its-display
        //    SendMessage(richTextBox.Handle, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
        //    var oldEventMask = (IntPtr)SendMessage(richTextBox.Handle, EM_SETEVENTMASK, IntPtr.Zero, IntPtr.Zero);
        //    return new ResetEventMask(richTextBox, oldEventMask);
        //}
    }
}


