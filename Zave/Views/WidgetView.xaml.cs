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
using Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Zave.Module;

namespace Zave.Views
{
    /// <summary>
    /// Interaction logic for WidgetView.xaml
    /// </summary>
    public partial class WidgetView : UserControl
    {
        public WidgetView()
        {
            InitializeComponent();
            
        }

        protected override void OnInitialized(System.EventArgs e)
        {
            base.OnInitialized(e);

            //var regionManager = ServiceLocator.Current.GetService(typeof(IRegionManager)) as IRegionManager;
            //regionManager.Regions.Remove("WidgetMainRegion");
            //RegionManager.SetRegionManager(WidgetMainRegion, regionManager);
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            var mainWin = Window.GetWindow(this);
            if(e.LeftButton == MouseButtonState.Pressed)
                mainWin.DragMove();

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
