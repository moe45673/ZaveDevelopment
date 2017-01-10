using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaveModel.ZDFEntry;
using ZaveGlobalSettings.Data_Structures;
using Prism.Events;

namespace ZaveService.ZDFEntry
{
    
    public class ZDFEntryService : IZDFEntryService
    {
        private IEventAggregator _eventAgg;

        public ZDFEntryService(IEventAggregator eventAgg)
        {
            _activeZDFEntryId = -1;

            _eventAgg = eventAgg;

            _eventAgg.GetEvent<EntrySelectedEvent>().Subscribe(SetZDFEntry);

        }

        private void SetZDFEntry(string id)
        {
            int.TryParse(id, out _activeZDFEntryId);
            _eventAgg.GetEvent<ActiveEntryUpdatedEvent>().Publish(id);
        }

        private int _activeZDFEntryId;
        public int ActiveZDFEntryId { get; set; }

        public T getZDFEntry<T>(string id) where T : IZDFEntry, new()
        {
            if (string.IsNullOrEmpty(id)) return new T();

            int idToPass;
            int.TryParse(id, out idToPass);
            //ActiveZDFEntryId = idToPass;
            T retValue = (T)ZaveModel.ZDF.ZDFSingleton.GetInstance().ListEntries().FirstOrDefault(x => x.ID == idToPass);
            return retValue;
        }

        public IZDFEntry getZDFEntry(int id)
        {

            //ActiveZDFEntryId = id;
            if (id == 0) return new ZaveModel.ZDFEntry.ZDFEntry();

            
            return ZaveModel.ZDF.ZDFSingleton.GetInstance().ListEntries().FirstOrDefault(x => x.ID == id);
        }
    }
}
