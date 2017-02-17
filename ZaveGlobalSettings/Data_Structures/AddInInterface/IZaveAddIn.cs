using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaveGlobalSettings.Data_Structures.AddInInterface
{
    public interface IZaveAddIn
    {

        Boolean isEnabled { get; set; }

        void Dispose();
        void Start();
        void Stop();
    }
}
