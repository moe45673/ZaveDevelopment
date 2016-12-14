using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Common;
using Prism.Mvvm;
using Prism.Events;
using Microsoft.Practices.Unity;
using ZaveGlobalSettings.Data_Structures;
using ZaveGlobalSettings.Data_Structures.MostRecentlyUsedList;
using ZaveGlobalSettings.ZaveFile;
using System.IO;
using Westwind.Utilities;
using System.Runtime.InteropServices;
using ZaveGlobalSettings.Data_Structures.ZaveObservableCollection;

namespace ZaveViewModel.ViewModels
{
    public class ZDFListViewModel : BindableBase
    {

        IUnityContainer _container;
        IEventAggregator _aggregator;
        private static readonly int MAXLISTSIZE = 6;



        public ZDFListViewModel(IUnityContainer cont, IEventAggregator agg)
        {
            _container = cont;

            _aggregator = agg;

            _aggregator.GetEvent<ZDFOpenedEvent>().Subscribe(addToMRU);
            _aggregator.GetEvent<ZDFSavedEvent>().Subscribe(addToMRU);
            _aggregator.GetEvent<NewZDFCreatedEvent>().Subscribe(addToMRU);
            ResetListAsync();

        }


        public void addToMRU(string filename)
        {
            if (!filename.Equals(GuidGenerator.UNSAVEDFILENAME))
            {
                MostRecentlyUsedList.AddToRecentlyUsedDocs(Path.GetFullPath(filename));
                //_aggregator.GetEvent<MRUChangedEvent>().Publish(filename);
                ResetList();


            }
        }

        private async Task ResetList()
        {
            await Task.Run(() =>
            {
                if (_recentFileList != null)
                {
                    _recentFileList.Clear();
                }
                _recentFileList = new ObservableImmutableList<string>(MostRecentlyUsedList.GetMostRecentDocs("*.zdf"));
               
                

            });
            
        }

        private async Task ResetListAsync()
        {
            await (ResetList());
            OnPropertyChanged("RecentFiles");
           
        }
        


        public ObservableImmutableList<string> RecentFiles
        {
            get
            {
                try
                {
                    if(_recentFileList == null)
                    ResetListAsync();


                }
                catch
                {
                    _recentFileList = new ObservableImmutableList<string>();
                }

                return new ObservableImmutableList<string>(_recentFileList.Take(MAXLISTSIZE));

                        
             }
                 

                    
            
        }
        private ObservableImmutableList<string> _recentFileList;

        public string LastFileName
        {
            get { return _LastFileName; }
            set
            {
                SetProperty(ref _LastFileName, value);
                try
                {
                    MostRecentlyUsedList.AddToRecentlyUsedDocs(value);

                    // reload recent file list
                    
                }
                catch { }
            }
        }
        private string _LastFileName;

    }

}


