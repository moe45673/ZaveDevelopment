using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaveGlobalSettings.Data_Structures.AddInInterface
{
    public interface IZaveAddIn
    {

        IZaveAddInState CurrentState { get; set; }
        

        

    }

    public interface IZaveAddInState
    {
        void Dispose();
        void Start();
        void Stop();
        void SelectionChanged<SelStateArgs>(object sender, SelStateArgs e);
        
    }
}
