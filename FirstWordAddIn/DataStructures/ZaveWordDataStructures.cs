using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using ZaveSrc = ZaveModel.ZDFSource.Source;
using ZaveModel;
using ZaveSourceAdapter.ZDFSource;

namespace FirstWordAddIn.DataStructures
{


    public class WordEventArgs : ZaveSourceAdapter.Data_Structures.SrcEventArgs
    {

        


        public WordEventArgs(ZaveSrc sd) : base(sd)
        {


        }

    }

    public class WordSource : ZaveSrc
    {

        ZaveSourceAdapter.Data_Structures.SrcType srcType { get; }

        public WordSource(string name, string page, string text) : base(name, page, text)
        {
            srcType = ZaveSourceAdapter.Data_Structures.SrcType.WORD;
        }

        ~WordSource()
        {

        }



    }//end WordSource



    public class WordSourceFactory : SourceFactory
    {
        public WordSourceFactory() : base()
        {

        }
        protected override ZaveSrc createSrc(string name, string page, string text)
        {
            return new WordSource(name, page, text);
        }
    }
} //end namespace ZDFSource

