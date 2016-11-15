﻿using System;
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
using ZaveViewModel.ViewModels;


namespace Zave.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //public ZaveViewModel.ZDFEntryViewModel.ZDFEntryViewModel ZdfObj;
        /// <summary>
        /// initializes MainWindow and creates a property reference to itself
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            


        }
        public static readonly DependencyProperty HeightProperty = 
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
            window.Topmost = true;
            window.Opacity = 0.5;
            //window.BeginAnimation(Window.OpacityProperty, null);
                      //window.Activate();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            var window = (Window)sender;
            window.Opacity = 1;
        }
    }

    
}
