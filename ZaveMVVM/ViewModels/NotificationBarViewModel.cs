using Prism.Mvvm;
using Prism.Common;
using Prism.Events;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ZaveGlobalSettings.Data_Structures;

namespace ZaveViewModel.ViewModels
{
    public class NotificationBarViewModel : BindableBase
    {
        IEventAggregator _agg;

        public NotificationBarViewModel(IUnityContainer cont, IEventAggregator agg)
        {
            _notificationText = "";
            _agg = agg;
            _agg.GetEvent<ZDFExportedEvent>().Subscribe(ChangeNotificationText);
            //Begin();
        }

        private String _notificationText;
        public String NotificationText
        {
            get
            {
                return _notificationText;
            }
            set
            {
                SetProperty<String>(ref _notificationText, value);
            }
        }

        private void ChangeNotificationText(Object obj)
        {
            //NotificationText = InstanceNames.ExportedToWord;
            ChangeTextAsync();
        }

        private async Task ChangeTextAsync()
        {
            await Task.Run(() =>
            {
                NotificationText = "";
                NotificationText = InstanceNames.ExportedToWord;
            });
        }

        private async Task Begin()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    NotificationText = "Hello!";
                    Thread.Sleep(5000);
                    NotificationText = "GoodBye!";
                    Thread.Sleep(5000);
                }
            });



        }
    }
}


