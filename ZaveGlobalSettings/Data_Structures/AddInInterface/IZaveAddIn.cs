using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZaveGlobalSettings.Data_Structures.AddInInterface
{
    /// <summary>
    /// Extends the functionality of Zave Source plugins.
    /// </summary>
    public interface IZaveAddIn
    {

        /// <summary>
        /// Gets and sets the state of the plugin within the source, for example to add/disable zave functionality within it.  Should be rewritten to follow the State design pattern. 
        /// </summary>
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
