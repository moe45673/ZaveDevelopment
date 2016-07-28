using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Regions;
using Prism.Commands;
using Newtonsoft.Json;
using System.IO;
using ZaveModel.ZDF;
using ZaveGlobalSettings.ZaveFile;
using ZaveService.IOService;
using Prism.Events;

namespace ZaveViewModel.ViewModels
{
    public class MainContainerViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;
        private readonly IIOService _ioService;

        public DelegateCommand SaveZDFDelegateCommand { get; set; }
        public DelegateCommand OpenZDFDelegateCommand { get; set; }

        public MainContainerViewModel(IRegionManager regionManager, IUnityContainer cont, IEventAggregator eventAgg)
        {
            _container = cont;
            _regionManager = regionManager;
            _eventAggregator = eventAgg;
            //SaveZDFDelegateCommand = new DelegateCommand(SaveZDF);
            //OpenZDFDelegateCommand = new DelegateCommand(OpenZDF);
            //_ioService = ioService;
            
        }

        private void SaveZDF()
        {
            var activeZDFVM = _container.Resolve(typeof(ZDFViewModel), "ZDFView") as ZDFViewModel;
            
            JsonSerializer serializer = new JsonSerializer();

            var filename = _ioService.SaveFileDialogService(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            using (var sw = _ioService.SaveFileService(filename))
            {
                try
                {
                    using (JsonWriter wr = new JsonTextWriter(sw))
                    {
                        try
                        {
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
                }
                finally
                {
                    sw.Close();
                }
            }
        }

        private void OpenZDF()
        {
            var activeZDFVM = _container.Resolve(typeof(ZDFViewModel), "ZDFView") as ZDFViewModel;

            JsonSerializer serializer = new JsonSerializer();
            ZDFSingleton activeZdf = activeZDFVM.GetModel();
            var filename = _ioService.OpenFileDialogService(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            using (var sr = _ioService.OpenFileService(filename))
            {
                try
                {
                    using (JsonReader wr = new JsonTextReader(sr))
                    {
                        try
                        {
                            string json = sr.ReadToEnd();
                            activeZdf.EntryList = JsonConvert.DeserializeObject<ZDFSingleton>(json).EntryList;
                            
                            

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
