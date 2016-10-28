using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaveModel.ZDFEntry;
using ZaveGlobalSettings.Data_Structures;


namespace ZaveService.ZDFEntry
{
    
    public class ZDFEntryService : IZDFEntryService
    {

        public ZDFEntryService(ZaveModel.ZDFEntry.ZDFEntry entry = null)
        {
            if (entry != null)
                _activeZDFEntry = getZDFEntry(entry.ID) as ZaveModel.ZDFEntry.ZDFEntry;
            else
                _activeZDFEntry = new ZaveModel.ZDFEntry.ZDFEntry();
        }

        private ZaveModel.ZDFEntry.ZDFEntry _activeZDFEntry;
        public ZaveModel.ZDFEntry.ZDFEntry ActiveZDFEntry { get { return _activeZDFEntry; } set { _activeZDFEntry = value; } }

        public IZDFEntry getZDFEntry(string id)
        {
            if (string.IsNullOrEmpty(id)) return new ZaveModel.ZDFEntry.ZDFEntry();

            int idToPass;
            int.TryParse(id, out idToPass);
            return ZaveModel.ZDF.ZDFSingleton.GetInstance().ListEntries().FirstOrDefault(x => x.ID == idToPass);
        }

        public IZDFEntry getZDFEntry(int id)
        {
            if (id == 0) return new ZaveModel.ZDFEntry.ZDFEntry();

            
            return ZaveModel.ZDF.ZDFSingleton.GetInstance().ListEntries().FirstOrDefault(x => x.ID == id);
        }
    }
}
