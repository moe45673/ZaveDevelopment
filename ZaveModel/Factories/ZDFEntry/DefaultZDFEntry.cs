using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZaveModel;

namespace ZaveModel.Factories.ZDFEntry
{
    public class DefaultZDFEntryFactory : ZDFEntryFactory
    {
        public DefaultZDFEntryFactory() : base(){
           
        }
        protected override IZDFEntry createZDFEntry(string name)
        {
            
            
            if (name == "")
            {
                return new ZaveModel.ZDFEntry.ZDFEntry();
            }


            throw new NotImplementedException();
        }
    }
}
