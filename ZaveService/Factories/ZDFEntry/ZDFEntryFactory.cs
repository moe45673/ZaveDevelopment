using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaveService.Factories.ZDFEntry
{
    public abstract class ZDFEntryFactory
    {
        public ZDFEntryFactory()
        {

        }

        public ZaveService.ZDFEntry.ZDFEntryHandler produceZDFEntry(string type)
        {
            return createZDFEntry(type); 
        }

        protected abstract ZaveService.ZDFEntry.ZDFEntryHandler createZDFEntry(string type);
        
    }
}
