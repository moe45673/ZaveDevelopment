using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ZaveGlobalSettings.Data_Structures.ZaveObservableCollection;
using ZaveGlobalSettings.Data_Structures;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Practices.Unity;
using Prism.Regions;
using Prism.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using ZaveModel.ZDF;
using ZaveModel.ZDFEntry;
using ZaveGlobalSettings.ZaveFile;
using ZaveService.IOService;
using Prism.Events;
using System.Windows.Input;

namespace ZaveViewModel.ViewModels
{
    public class MainContainerViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;
        private readonly IIOService _ioService;
        public static string ACTIVESORT = "TxtDocColor";

        public DelegateCommand SaveZDFDelegateCommand { get; set; }
        public DelegateCommand OpenZDFDelegateCommand { get; set; }

        public DelegateCommand NewZDFDelegateCommand { get; set; }

        public MainContainerViewModel(IRegionManager regionManager, IUnityContainer cont, IEventAggregator eventAgg, IOService ioService)
        {
            _container = cont;
            _regionManager = regionManager;
            _eventAggregator = eventAgg;
            SaveZDFDelegateCommand = new DelegateCommand(SaveZDF);
            OpenZDFDelegateCommand = new DelegateCommand(OpenZDF);
            NewZDFDelegateCommand = new DelegateCommand(NewZDF);
            _ioService = ioService;

        }

        [Conditional("DEBUG")]
        private void setIndented(JsonSerializer ser)
        {
            ser.Formatting = Formatting.Indented;
        }

        private void SaveZDF()
        {
            var activeZDFVM = _container.Resolve(typeof(ZDFViewModel), "ZDFView") as ZDFViewModel;

            JsonSerializer serializer = new JsonSerializer();

            var filename = _ioService.SaveFileDialogService(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            if (filename != String.Empty)
            {

                using (var sw = _ioService.SaveFileService(filename))
                {
                    try
                    {
                        using (JsonWriter wr = new JsonTextWriter(sw))
                        {
                            try
                            {

                                setIndented(serializer);
                                serializer.Serialize(wr, activeZDFVM.GetModel());
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            finally
                            {
                                wr.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("Save File Error!");
                    }
                    finally
                    {
                        sw.Close();
                    }
                }
            }
        }

        #region New OPen
        private void OpenZDF()
        {
            //var activeZDFVM = _container.Resolve(typeof(ZDFViewModel), "ZDFView") as ZDFViewModel;

            JsonSerializer serializer = new JsonSerializer();
            ZDFSingleton activeZdf = ZDFSingleton.GetInstance();
            var filename = _ioService.OpenFileDialogService(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

            
            if (filename != String.Empty) //If the user presses cancel
            {
                using (var sr = _ioService.OpenFileService(filename))
                {
                    try
                    {
                        using (JsonReader wr = new JsonTextReader(sr))
                        {
                            try
                            {
                                JObject jObject = JObject.Load(wr);
                                //var output = "";
                                //foreach(JProperty prop in jObject.Properties())
                                //{
                                //    output += "PROPERTY 1 EQUALS " + prop.Name + "-" + prop.Value + '\r' + '\n';
                                //}

                                //System.Windows.Forms.MessageBox.Show(output);

                                //ZaveModel.ZDF.ZDFSingleton activeZDF = ZaveModel.ZDF.ZDFSingleton.Instance;

                                //activeZdf = JsonConvert.DeserializeObject<ZaveModel.ZDF.ZDFSingleton>(jObject.ToString());
                                activeZdf = JsonConvert.DeserializeObject<ZaveModel.ZDF.ZDFSingleton>(jObject.ToString());
                                activeZdf = ZDFSingleton.GetInstance(_eventAggregator);
                                JArray ja = (JArray)jObject["EntryList"]["_items"];

                                activeZdf.EntryList = new ObservableImmutableList<IZDFEntry>(ja.ToObject<List<ZDFEntry>>());

                                //activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance(eventAgg);
                                //foreach (var item in activeZDF.EntryList)
                                //{
                                //    activeZDF.Add(item);
                                //}



                                if (activeZdf.EntryList.Count > 0)
                                {
                                    ObservableImmutableList<ZdfEntryItemViewModel> ZdfEntries = new ObservableImmutableList<ZdfEntryItemViewModel>();
                                    ////activeZDF.EntryList.Clear();
                                    ZaveModel.ZDF.ZDFSingleton activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance();
                                    foreach (var item in activeZdf.EntryList)
                                    {
                                        //activeZdf.Add(item);

                                        ZdfEntries.Add(new ZdfEntryItemViewModel(item as ZDFEntry));
                                    }
                                    ////ZdfEntries.FirstOrDefault().TxtDocName = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                                    ////ZdfEntries.Select(w => w.TxtDocName == System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName));

                                    ////List<SelectionState> selState = activeZDF.toSelectionStateList();

                                    activeZdf.EntryList.LastOrDefault().Name = Path.GetFileName(filename);

                                    _eventAggregator.GetEvent<ZDFOpenedEvent>().Publish(activeZdf);

                                }



                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            finally
                            {
                                wr.Close();
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        sr.Close();
                    }
                }
            }
        }
        #endregion

        #region Old Open
        //private void OpenZDF()
        //{
        //    //var activeZDFVM = _container.Resolve(typeof(ZDFViewModel), "ZDFView") as ZDFViewModel;

        //    JsonSerializer serializer = new JsonSerializer();
        //    ZDFSingleton activeZdf = ZDFSingleton.GetInstance();
        //    var filename = _ioService.OpenFileDialogService(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        //    if (filename != null)
        //    {
        //        using (var sr = _ioService.OpenFileService(filename))
        //        {
        //            try
        //            {
        //                using (JsonReader wr = new JsonTextReader(sr))
        //                {
        //                    try
        //                    {
        //                        JObject jObject = JObject.Load(wr);
        //                        //var output = "";
        //                        //foreach(JProperty prop in jObject.Properties())
        //                        //{
        //                        //    output += "PROPERTY 1 EQUALS " + prop.Name + "-" + prop.Value + '\r' + '\n';
        //                        //}

        //                        //System.Windows.Forms.MessageBox.Show(output);

        //                        //ZaveModel.ZDF.ZDFSingleton activeZDF = ZaveModel.ZDF.ZDFSingleton.Instance;

        //                        //activeZdf = JsonConvert.DeserializeObject<ZaveModel.ZDF.ZDFSingleton>(jObject.ToString());
        //                        activeZdf = JsonConvert.DeserializeObject<ZaveModel.ZDF.ZDFSingleton>(jObject.ToString());
        //                        activeZdf = ZDFSingleton.GetInstance(_eventAggregator);
        //                        JArray ja = (JArray)jObject["EntryList"]["_items"];

        //                        activeZdf.EntryList = new ObservableImmutableList<IZDFEntry>(ja.ToObject<List<ZDFEntry>>());

        //                        if (activeZdf.EntryList.Count > 0)
        //                        {
        //                            activeZdf.EntryList.FirstOrDefault().Name = Path.GetFileName(filename);
        //                            _eventAggregator.GetEvent<ZDFOpenedEvent>().Publish(activeZdf);
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        throw ex;
        //                    }
        //                    finally
        //                    {
        //                        wr.Close();
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //            finally
        //            {
        //                sr.Close();
        //            }
        //        }
        //    }
        //}
        #endregion

        private void LV_EntryList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //IEventAggregator obj = new EventAggregator();
            //ObservableImmutableList<ZdfEntryItemViewModel> ZdfEntries = new ObservableImmutableList<ZdfEntryItemViewModel>();
            //ZaveModel.ZDFEntry.ZDFEntry entry = new ZaveModel.ZDFEntry.ZDFEntry();
            //ZaveModel.ZDF.ZDFSingleton activeZDF;
            //activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance();
            //activeZDF.EntryList.LastOrDefault().Text = activeZDF.EntryList.Count.ToString();
            //activeZDF.EntryList.LastOrDefault().Name = "from event123";
            //ZdfEntries.Add(new ZdfEntryItemViewModel(entry as ZDFEntry));
            //_eventAggregator.GetEvent<EntryReadEvent>().Publish(ZdfEntries.LastOrDefault());
        }
        
        private void NewZDF()
        {
            //ZaveModel.ZDF.ZDFSingleton activeZDF;
            //activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance();
            //ObservableImmutableList<ZdfEntryItemViewModel> ZdfEntries = new ObservableImmutableList<ZdfEntryItemViewModel>();
            //ZaveModel.ZDFEntry.ZDFEntry entry = new ZaveModel.ZDFEntry.ZDFEntry();
            ZDFSingleton activeZDF = ZDFSingleton.GetInstance();
            activeZDF.EntryList.Clear();
            //activeZDF.Add(new ZDFEntry());

            ////_eventAggregator.GetEvent<EntryCreatedEvent>().Publish(new ZDFEntry());
            //activeZDF.EntryList.LastOrDefault().Text = activeZDF.EntryList.Count.ToString();
            //activeZDF.EntryList.LastOrDefault().DateModified = DateTime.Now.Date;
            //activeZDF.EntryList.LastOrDefault().DateModified = DateTime.Now.Date;
            //activeZDF.EntryList.LastOrDefault().Name = "New ZDF Name";
            //ZdfEntries.Add(new ZdfEntryItemViewModel(entry as ZDFEntry));
           
            ////MouseButtonEventArgs ek = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);
            ////ek.RoutedEvent = System.Windows.Controls.TextBlock.MouseLeftButtonDownEvent;
            //ZDFViewModel objmodel = new ZDFViewModel(_eventAggregator,_container);
            
            ////_eventAggregator.GetEvent<EntryReadEvent>().Publish(ZdfEntries.LastOrDefault(x => x.TxtDocID == ZdfEntries.First<ZdfEntryItemViewModel>().TxtDocID));
            //MouseButtonEventArgs mm = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);
            
            //LV_EntryList_PreviewMouseLeftButtonDown(mm.Source, mm);
            ////ZdfEntries.Add(new ZdfEntryItemViewModel(entry as ZDFEntry));
            ////_eventAggregator.GetEvent<EntryReadEvent>().Publish(ZDFSingleton.GetInstance());
        }

        private string getSaveDirectory()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\ZDFs";
        }

        private string getSaveFileName()
        {
            return "\\SaveDoc";
        }

    }


}

//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using ZaveGlobalSettings.Data_Structures.ZaveObservableCollection;
//using ZaveGlobalSettings.Data_Structures;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Diagnostics;
//using Microsoft.Practices.Unity;
//using Prism.Regions;
//using Prism.Commands;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System.IO;
//using ZaveModel.ZDF;
//using ZaveModel.ZDFEntry;
//using ZaveGlobalSettings.ZaveFile;
//using ZaveService.IOService;
//using Prism.Events;

//namespace ZaveViewModel.ViewModels
//{
//    public class MainContainerViewModel
//    {
//        private readonly IEventAggregator _eventAggregator;
//        private readonly IRegionManager _regionManager;
//        private readonly IUnityContainer _container;
//        private readonly IIOService _ioService;

//        public DelegateCommand SaveZDFDelegateCommand { get; set; }
//        public DelegateCommand OpenZDFDelegateCommand { get; set; }

//        public MainContainerViewModel(IRegionManager regionManager, IUnityContainer cont, IEventAggregator eventAgg, IOService ioService)
//        {
//            _container = cont;
//            _regionManager = regionManager;
//            _eventAggregator = eventAgg;
//            SaveZDFDelegateCommand = new DelegateCommand(SaveZDF);
//            OpenZDFDelegateCommand = new DelegateCommand(OpenZDF);
//            _ioService = ioService;

//        }

//        [Conditional("DEBUG")]
//        private void setIndented(JsonSerializer ser)
//        {
//            ser.Formatting = Formatting.Indented;
//        }

//        private void SaveZDF()
//        {
//            //var activeZDFVM = _container.Resolve(typeof(ZDFViewModel), "ZDFView") as ZDFViewModel;
//            var activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance();
//            JsonSerializer serializer = new JsonSerializer();

//            var filename = _ioService.SaveFileDialogService(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
//            if (filename != String.Empty)
//            {

//                using (var sw = _ioService.SaveFileService(filename))
//                {
//                    try
//                    {
//                        using (JsonWriter wr = new JsonTextWriter(sw))
//                        {
//                            try
//                            {

//                                setIndented(serializer);
//                                serializer.Serialize(wr, activeZDF);
//                            }
//                            catch (Exception ex)
//                            {
//                                throw ex;
//                            }
//                            finally
//                            {
//                                wr.Close();
//                            }
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        System.Windows.Forms.MessageBox.Show("Save File Error!");
//                    }
//                    finally
//                    {
//                        sw.Close();
//                    }
//                }
//            }
//        }

//        private void OpenZDF()
//        {
//            //var activeZDFVM = _container.Resolve(typeof(ZDFViewModel), "ZDFView") as ZDFViewModel;

//            JsonSerializer serializer = new JsonSerializer();
//            ZDFSingleton activeZdf = ZDFSingleton.GetInstance();
//            var filename = _ioService.OpenFileDialogService(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
//            using (var sr = _ioService.OpenFileService(filename))
//            {
//                try
//                {
//                    using (JsonReader wr = new JsonTextReader(sr))
//                    {
//                        try
//                        {
//                            JObject jObject = JObject.Load(wr);
//                            //var output = "";
//                            //foreach(JProperty prop in jObject.Properties())
//                            //{
//                            //    output += "PROPERTY 1 EQUALS " + prop.Name + "-" + prop.Value + '\r' + '\n';
//                            //}

//                            //System.Windows.Forms.MessageBox.Show(output);

//                            //ZaveModel.ZDF.ZDFSingleton activeZDF = ZaveModel.ZDF.ZDFSingleton.Instance;

//                            activeZdf = JsonConvert.DeserializeObject<ZaveModel.ZDF.ZDFSingleton>(jObject.ToString());
//                            activeZdf = ZDFSingleton.GetInstance(_eventAggregator);
//                            JArray ja = (JArray)jObject["EntryList"]["_items"];

//                            activeZdf.EntryList = new ObservableImmutableList<IZDFEntry>(ja.ToObject<List<ZDFEntry>>());

//                            //activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance(eventAgg);
//                            //foreach (var item in activeZDF.EntryList)
//                            //{
//                            //    activeZDF.Add(item);
//                            //}



//                            if (activeZdf.EntryList.Count > 0)
//                            {
//                                //ObservableImmutableList<ZdfEntryItemViewModel> ZdfEntries = new ObservableImmutableList<ZdfEntryItemViewModel>();
//                                ////activeZDF.EntryList.Clear();
//                                ////ZaveModel.ZDF.ZDFSingleton activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance();
//                                //foreach (var item in activeZdf.EntryList)
//                                //{
//                                //    //activeZDF.Add(item);

//                                //    ZdfEntries.Add(new ZdfEntryItemViewModel(item as ZDFEntry));
//                                //}
//                                ////ZdfEntries.FirstOrDefault().TxtDocName = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);
//                                ////ZdfEntries.Select(w => w.TxtDocName == System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName));

//                                ////List<SelectionState> selState = activeZDF.toSelectionStateList();

//                                _eventAggregator.GetEvent<ZDFOpenedEvent>().Publish(activeZdf);

//                            }



//                        }
//                        catch (Exception ex)
//                        {
//                            throw ex;
//                        }
//                        finally
//                        {
//                            wr.Close();
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                }
//                finally
//                {
//                    sr.Close();
//                }
//            }
//        }

//        private string getSaveDirectory()
//        {
//            return Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\ZDFs";
//        }

//        private string getSaveFileName()
//        {
//            return "\\SaveDoc";
//        }

//    }


//}
