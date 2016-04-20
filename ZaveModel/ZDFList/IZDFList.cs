using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZaveModel.ZDF;

namespace ZaveModel.ZDFList
{
    public interface IZDFList
    {
        bool CreateZDFList(ZDF.ZDFSingleton zdf);
        IEnumerable<ZDF.ZDFSingleton> ListZDFs();
    }
}
