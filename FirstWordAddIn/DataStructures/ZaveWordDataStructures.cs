using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using ZaveSrc = ZaveGlobalSettings.Data_Structures.SelectionState;
using ZaveController.ZDFSource;

namespace FirstWordAddIn.DataStructures
{


    public class WordEventArgs : ZaveController.Data_Structures.SrcEventArgs
    {

        


        public WordEventArgs(ZaveSrc sd) : base(sd)
        {


        }

    }

    public class WordSelectionState
    {

        ZaveGlobalSettings.Data_Structures.SrcType srcType { get; set; }

        public WordSelectionState(string name, string page, string text)
        {
            srcType = ZaveGlobalSettings.Data_Structures.SrcType.WORD;
        }

        ~WordSelectionState()
        {

        }

        public override string ToString()
        {
            return this.srcType.ToString();
        }



    }//end WordSource



    public class WordSourceFactory : SourceFactory
    {
        public WordSourceFactory() : base()
        {

        }
        protected override ZaveSrc createSrc(string name, string page, string text)
        {
            return new ZaveSrc(name, page, text);
            throw new NotImplementedException();
        }
    }
} //end namespace ZDFSource

