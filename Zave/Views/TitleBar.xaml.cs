using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Prism.Mvvm;
using System.ComponentModel;
using ZaveViewModel.ViewModels;

namespace Zave.Views
{
    /// <summary>
    /// Interaction logic for TitleBar.xaml
    /// </summary>
    public partial class TitleBar : UserControl
    {
        
        /// <summary>
        /// 
        /// </summary>
        public TitleBar()
        {

            
            
            InitializeComponent();
           
                

        }

       

       
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            var mainWin = Window.GetWindow(this);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                mainWin.DragMove();
                ((TitleBarViewModel)DataContext).SnapToCorner = false;
            }

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            //((TitleBarViewModel)DataContext).ConfirmUnsavedChangesCommand.Execute();
            ((MainWindow)Application.Current.MainWindow).Close();
        }

        

        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
    }
}
