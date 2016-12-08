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
using Prism.Mvvm;
using System.ComponentModel;
using ZaveViewModel.ViewModels;
using ZaveGlobalSettings.Data_Structures;
using System.Diagnostics;
using System.Windows.Threading;

namespace Zave.Views
{
    /// <summary>
    /// Interaction logic for WidgetView.xaml
    /// </summary>
    public partial class WidgetView : UserControl
    {

        private Window window;
        public WidgetView()
        {
            InitializeComponent();
            //Loaded += new DependencyPropertyChangedEventHandler()
        }


        //private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    base.OnMouseLeftButtonDown(e);
        //    var mainWin = Window.GetWindow(this);
        //    if(e.LeftButton == MouseButtonState.Pressed)
        //        mainWin.DragMove();

        //}

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }  



        

        //private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        //{
        //    if (window != null)
        //    {
        //        if (((WidgetViewModel)DataContext).IsActive == false)
        //        {
        //            window.Topmost = false;
        //        }
        //    }
            

        //}

        

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            window = Window.GetWindow(this);

            Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(
                () =>
                {
                    WidgetTitleBarRegion.Focus();
                }));
            

            

        }

        //private void UserControl_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    if (window != null)
        //    {
        //        //System.Windows.MessageBox.Show("Window Found!");
        //        if (((WidgetViewModel)DataContext).IsActive)
        //        {
        //            if (window.IsActive == true)
        //            {
        //                window.Opacity = 1;
        //            }
        //            else if (window.IsActive == false)
        //            {
        //                window.Topmost = true;
        //                window.Opacity = 0.5;
        //                window.BeginAnimation(Window.OpacityProperty, null);
        //                //window.Activate();
        //            }
        //        }
        //    }
        //}

       
    }
}
