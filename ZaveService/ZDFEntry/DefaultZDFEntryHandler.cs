using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZaveModel.ZDF;

namespace ZaveService.ZDFEntry
{
    public class DefaultZDFEntryHandler : ZDFEntryHandler
    {
        

        public DefaultZDFEntryHandler(ModelStateDictionary modelState, IZDF repository) : base(modelState, repository)
        {
            
        }

        
    }
}
