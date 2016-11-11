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

namespace Zave.Views
{
    /// <summary>
    /// Interaction logic for ControlBar.xaml
    /// </summary>
    public partial class ControlBar : UserControl
    {
        public ControlBar(IEventAggregator eventAgg)
        {
            InitializeComponent();
            eventAgg.GetEvent<WindowModeChangeEvent>().Subscribe(setSuffix);
            
        }

        private void setSuffix(WindowMode wm)
        {
            var suffix = FindResource("BtnImgSuffix");
            switch (wm)
            {
                case (WindowMode.MAIN):
                    suffix = "";
                    break;
                case (WindowMode.WIDGET):
                    suffix = "_w";
                    break;
                default:
                    suffix = "";
                    break;
            }
        }
    }
}
