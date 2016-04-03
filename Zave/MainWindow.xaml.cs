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

namespace Zave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainWin = (MainWindow)App.Current.Windows[0];
            
        }

        

        //private ZaveSourceAdapter.Global_Settings.EventInitSingleton eventInit;
        public MainWindow MainWin { get; set; }
        

        


        public void setTextBoxes(string docname, string page, string text)
        {
            //Zave.MVVM.View.MainWindow.MainWindowViewModel.se
        }
        
    }
}
