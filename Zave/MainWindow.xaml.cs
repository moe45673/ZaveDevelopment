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
using ZaveViewModel.ZDFEntryViewModel;

namespace Zave
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

        private void displayUI(ZDFEntryViewModel zaveEntry)
        {
            docNameTxtBx.Text = zaveEntry.TxtDocname;
            docPgNmbrTxtBx.Text = zaveEntry.TxtDocPage;
            docTextTxtBx1.Text = zaveEntry.TxtDocText;
        }

        

        

        

        


        
        
    }
}
