﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zave.ZDFSource
{
    public abstract class SourceFactory
    {       
        

        public Source produceSource(ZaveEvents.Data_Structures.SelectionData selDat)
        {
            Source Src = createSrc(selDat.SelectionDocName, selDat.SelectionPage, selDat.SelectionText);
            return Src;
        }

        protected abstract Source createSrc(string name, string page, string text);
    }
}
