using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaveGlobalSettings.Data_Structures;
using Zave.Properties;
using Prism.Events;

namespace Zave.Controllers
{
    
    public class AppSettingsController : IConfigProvider
    {

        IEventAggregator _eventAgg;
        Settings _settings;
        

        public AppSettingsController(IEventAggregator eventAgg)
        {
            _eventAgg = eventAgg;
            _settings = Settings.Default;
            _eventAgg.GetEvent<ActiveColorUpdatedEvent>().Subscribe(x => ActiveColor = x);
        }
        
        public Color ActiveColor
        {
            get
            {

                return _settings.ActiveColor;
            }

            set
            {
                _settings.ActiveColor = value;
                //_settings.Save();
            }
        }
    }
}
