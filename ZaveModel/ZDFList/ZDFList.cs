using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaveModel.ZDFList
{
    public class ZDFList : IZDFList
    {
        //Needs to be changed to Linq code when data access layer is implemented
        private List<ZDF.ZDF> zdfList { get; set; }

        public ZDFList()
        {
            zdfList = new List<ZDF.ZDF>();
        }

        public bool CreateZDFList(ZDF.ZDF zdf)
        {
            try
            {
                zdfList.Add(zdf);
                //zdfList.SaveChanges(); //Linq for Data Access
                return true;
            }
            catch
            {
                return false;
            }

        }

        public IEnumerable<ZDF.ZDF> ListZDFs()
        {
            return zdfList;
        }
    }
}
