using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Prism.Events;
using ZaveGlobalSettings.Data_Structures;
using System.Threading;

namespace Zave.Views
{
    /// <summary>
    /// Interaction logic for ControlBar.xaml
    /// </summary>
    public partial class ControlBar : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        public static int counter = 0;
        public ControlBar()
        {
            InitializeComponent();
            Interlocked.Increment(ref counter);
        }

        ~ControlBar()
        {
            Interlocked.Decrement(ref counter);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("There are " + counter + " instances");
        }
    }
        
}
