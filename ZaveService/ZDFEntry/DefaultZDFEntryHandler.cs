using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;
using ZaveModel.ZDF;
using ZaveGlobalSettings.Data_Structures;

namespace ZaveService.ZDFEntry
{
    public class DefaultZDFEntryHandler : ZDFEntryHandler
    {
        

        public DefaultZDFEntryHandler(SelectionState modelState, IZDF repository) : base(modelState, repository)
        {
            
        }

        
    }
}
