using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zave.ZDFSource
{
    public class WordSourceFactory : SourceFactory
    {
        public WordSourceFactory() : base()
        {

        }
        protected override Source createSrc(string name, string page, string text)
        {
            return new WordSource(name, page, text);
        }
    }
}
