using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using ZaveGlobalSettings.Data_Structures;
using ZaveViewModel.ViewModels;


namespace Zave.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        /// <summary>
        /// initializes MainWindow and creates a property reference to itself
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();



        }



        public static readonly new DependencyProperty HeightProperty =
            DependencyProperty.Register
            (
                "DynamicHeight", typeof(int), typeof(MainWindow)
            );

        public int DynamicHeight
        {
            get
            {
                return (int)this.GetValue(HeightProperty);
            }
            set
            {
                SetValue(HeightProperty, value);
            }
        }

        

        private void Window_Deactivated(object sender, EventArgs args)
        {
            var window = (Window)sender;
            try
            {
                if (((MainWindowViewModel)((MainWindow)sender).DataContext).WinMode == WindowMode.WIDGET)
                {

                    //todo Need to put the following logic into the viewmodel so that it can be used with User Settings
                    //WindowMode? assignable = WindowMode.WIDGET;
                    //var vm = this.DataContext as MainWindowViewModel;
                    //if ((vm != null) && (vm.WinMode != WindowMode.WIDGET) && (vm.SwitchSpecificWindowModeCommand.CanExecute(assignable)))
                    //    vm.SwitchSpecificWindowModeCommand.Execute(assignable);

                    window.Topmost = true;
                    window.Opacity = 0.5;
                    window.BeginAnimation(Window.OpacityProperty, null);
                    window.Activate();
                }
                else
                {
                    window.Topmost = false;
                }
            }
            catch (NullReferenceException nre)
            {
                Debug.WriteLine(nre.Message);
            }

        }
        
        private void Window_Activated(object sender, EventArgs e)
        {
            var window = (Window)sender;
            window.Topmost = true;
            window.Opacity = 1;
        }

        //private void Window_LocationChanged(object sender, EventArgs e)
        //{
        //    MoveIntoView();
        //}

        //private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    MoveIntoView();
        //}

        //private void Window_SourceUpdated(object sender, DataTransferEventArgs e)
        //{
        //    MoveIntoView();
        //}

        private void Window_LayoutUpdated(object sender, EventArgs e)
        {
            Zave.Controllers.ShiftWindowOntoScreenHelper.ShiftWindowOntoScreen(this);
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void Window_LostFocus(object sender, MouseEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //e.Cancel = true;
            Dispatcher.Invoke(() => ((MainWindowViewModel)DataContext).ConfirmUnsavedChangesCommand.Execute());
            

        }

        //private void MainViewRegion_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    
        //}

        //private void Window_LocationChanged(object sender, EventArgs e)
        //{
        //    Zave.Controllers.ShiftWindowOntoScreenHelper.ShiftWindowOntoScreen(this);
        //}
    }


}
