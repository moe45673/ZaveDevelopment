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
using Microsoft.Practices.Unity;

namespace Zave.Views
{
    /// <summary>
    /// Interaction logic for WidgetControls.xaml
    /// </summary>
    public partial class WidgetControls : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cont"></param>
        public WidgetControls(IUnityContainer cont)
        {

            InitializeComponent();
            //DataContext = cont.Resolve<ControlBarViewModel>();
        }
    }
}
