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
using Newtonsoft.Json.Linq;
using ZaveGlobalSettings.Data_Structures;
using ZaveGlobalSettings.Data_Structures.ZaveObservableCollection;
using ZaveModel.ZDFEntry;
using ZaveViewModel.ViewModels;
using Prism.Events;

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
            openFileDialog.DefaultExt = ".zdf"; // Default file extension
            openFileDialog.Filter = "ZDF documents (.zdf)|*.zdf"; // Filter files by extension
            if (openFileDialog.ShowDialog() == true)
            {

                //string fileContent = File.ReadAllText(openFileDialog.FileName);
                JsonSerializer deserial = new JsonSerializer();
                var filename = openFileDialog.FileName;

                using (var sr = new StreamReader(filename))
                {
                    using (JsonReader jr = new JsonTextReader(sr))
                    {
                        //deserial.NullValueHandling = NullValueHandling.Ignore;
                        JObject jObject = JObject.Load(jr);
                        //var output = "";
                        //foreach(JProperty prop in jObject.Properties())
                        //{
                        //    output += "PROPERTY 1 EQUALS " + prop.Name + "-" + prop.Value + '\r' + '\n';
                        //}

                         //System.Windows.Forms.MessageBox.Show(output);

                        ZaveModel.ZDF.ZDFSingleton activeZDF = ZaveModel.ZDF.ZDFSingleton.Instance;

                        activeZDF = JsonConvert.DeserializeObject<ZaveModel.ZDF.ZDFSingleton>(jObject.ToString());

                        JArray ja = (JArray)jObject["EntryList"]["_items"];

                        activeZDF.EntryList = new ObservableImmutableList<IZDFEntry>(ja.ToObject<List<IZDFEntry>>());
                        //activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance(eventAgg);
                        //foreach (var item in activeZDF.EntryList)
                        //{
                        //    activeZDF.Add(item);
                        //}



                        if (activeZDF.EntryList.Count > 0)
                        {
                            ObservableImmutableList<ZdfEntryItemViewModel> ZdfEntries = new ObservableImmutableList<ZdfEntryItemViewModel>();
                            //activeZDF.EntryList.Clear();
                            //ZaveModel.ZDF.ZDFSingleton activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance();
                            foreach (var item in activeZDF.EntryList)
                            {
                                //activeZDF.Add(item);

                                ZdfEntries.Add(new ZdfEntryItemViewModel(item as ZDFEntry));
                            }
                            //ZdfEntries.FirstOrDefault().TxtDocName = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                            //ZdfEntries.Select(w => w.TxtDocName == System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName));

                            List<SelectionState> selState = activeZDF.toSelectionStateList();

                        }

                    }
                }
            }

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnNewFile_Click(object sender, RoutedEventArgs e)
        {
            ZaveModel.ZDF.ZDFSingleton activeZDF;
            activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance();

            activeZDF.Add(new ZDFEntry());
            //SelectionState selState = new SelectionState(activeZDF.EntryList.LastOrDefault().ID, activeZDF.EntryList.LastOrDefault().Name, activeZDF.EntryList.LastOrDefault().Page, activeZDF.EntryList.LastOrDefault().Text, DateTime.Now, System.Drawing.Color.Yellow, SrcType.WORD, new List<SelectionComment>());
            //var activeNewZdf = activeZDF.EntryList[activeZDF.EntryList.Count - 1].toSelectionState();
            //activeNewZdf.Color = System.Drawing.Color.White;
            
        }
    }
}
