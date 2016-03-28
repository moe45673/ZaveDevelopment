using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zave.ZDFList
{
    public class ZDFList
    {
        public List<ZDF.ZDF> zdfList { get; set; }

        public ZDFList()
        {
            zdfList = new List<ZDF.ZDF>();
        }
    }
}
