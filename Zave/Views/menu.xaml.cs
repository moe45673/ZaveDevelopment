using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
using ZaveGlobalSettings.Data_Structures.ZaveObservableCollection;
using ZaveModel.ZDFEntry;
using ZaveViewModel.ViewModels;

namespace Zave.Views
{
    /// <summary>
    /// Interaction logic for menu.xaml
    /// </summary>
    public partial class Menu : UserControl
    {
        public Menu()
        {
            InitializeComponent();
        }
        private void btnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.DefaultExt = ".ZDF"; // Default file extension
            saveFileDialog.Filter = "ZDF documents (.ZDF)|*.ZDF"; // Filter files by extension
            saveFileDialog.FileName = "";

            if (saveFileDialog.ShowDialog() == true)
            {
                ZaveModel.ZDF.ZDFSingleton activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance();
                //ZaveModel.ZDFEntry.ZDFEntry entry = new ZaveModel.ZDFEntry.ZDFEntry();
                //ObservableImmutableList<IZDFEntry> EntryList = activeZDF.EntryList;


                string selectedJson = JsonConvert.SerializeObject(activeZDF.EntryList.ToList());
                File.WriteAllText(saveFileDialog.FileName, selectedJson);
            }
        }
        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".ZDF"; // Default file extension
            openFileDialog.Filter = "ZDF documents (.ZDF)|*.ZDF"; // Filter files by extension
            if (openFileDialog.ShowDialog() == true)
            {

                string fileContent = File.ReadAllText(openFileDialog.FileName);
                
                List<ZDFEntry> selectedJson = JsonConvert.DeserializeObject<List<ZDFEntry>>(fileContent);
                //Create Instance of one zdf for all open zdf
                //ZDFEntry ultimateZDFEntry = new ZDFEntry();
                ZDFEntryView ZEV = new ZDFEntryView();

                if (selectedJson.Count > 0)
                {
                    ObservableImmutableList<ZdfEntryItemViewModel> ZdfEntries = new ObservableImmutableList<ZdfEntryItemViewModel>();
                    ZaveModel.ZDF.ZDFSingleton activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance();
                    activeZDF.Add(selectedJson[0]);
                    ZdfEntries.Add(new ZdfEntryItemViewModel(selectedJson[0] as ZDFEntry));
                    // ultimateZDFEntry.Text += selectedJson[0].Text;
                    //ultimateZDFEntry.Name += selectedJson[0].Name;

                    //ZEV.docTextTxtBx1.Text = selectedJson[0].Text;

                }


            }

        }
    }
}
