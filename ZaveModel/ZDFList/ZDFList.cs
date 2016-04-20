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
        private List<ZDF.ZDFSingleton> zdfList { get; set; }

        public ZDFList()
        {
            zdfList = new List<ZDF.ZDFSingleton>();
        }

        public bool CreateZDFList(ZDF.ZDFSingleton zdf)
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

        public IEnumerable<ZDF.ZDFSingleton> ListZDFs()
        {
            return zdfList;
        }
    }
}
