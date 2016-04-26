using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using WordInterop = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using WordTools = Microsoft.Office.Tools.Word;
using FirstWordAddIn.DataStructures;
using ZaveGlobalSettings.Data_Structures;
using ZaveModel.ZDF;

using ZaveSrc = ZaveGlobalSettings.Data_Structures.SelectionState;

namespace FirstWordAddIn
{

    public partial class ThisAddIn
    {

        //Running under ZaveSourceAdapter, listener for all highlights from all possible sources
        ZDFSingleton activeZDF = ZDFSingleton.Instance;

        public static event EventHandler<WordEventArgs> WordFired;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            //only start listening for the event when a document is opened or created
            this.Application.DocumentOpen +=
            new WordInterop.ApplicationEvents4_DocumentOpenEventHandler(DocumentSelectionChange);

            ((WordInterop.ApplicationEvents4_Event)this.Application).NewDocument +=
                new WordInterop.ApplicationEvents4_NewDocumentEventHandler(DocumentSelectionChange);



            //Tie the highlight to the singleton handler
            ZDFSingleton.ModelPropertyChanged += new EventHandler<ModelEventArgs>(ZDFSingleton_ModelPropertyChanged);
        }

        private void ZDFSingleton_ModelPropertyChanged(object sender, ModelEventArgs e)
        {
            ZaveModel.ZDFEntry.ZDFEntry entry = new ZaveModel.ZDFEntry.ZDFEntry();
            //zdfEntryHandler = new ZaveService.ZDFEntry.DefaultZDFEntryHandler(e.zSrc, activeZDF);
            //zdfEntryHandler.CreateZDFEntry(new ZaveModel.ZDFEntry.ZDFEntry(e.zSrc));
            entry.Source = e.SelState;

            //            _saveZDFEntryCommand = new RelayCommand(param => SaveZDFEntry(entry)
            //, param => (entry != null));


            activeZDF.Add(entry);
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }
        
        /// <summary>
        /// Get the current doc and pass it to the event handler
        /// </summary>
        /// <param name="Doc"></param>
        private void DocumentSelectionChange(Microsoft.Office.Interop.Word.Document Doc)
        {

            WordTools.Document vstoDoc = Globals.Factory.GetVstoObject(this.Application.ActiveDocument);
            vstoDoc.SelectionChange += new Microsoft.Office.Tools.Word.SelectionEventHandler(ThisDocument_SelectionChange);
        }


        void ThisDocument_SelectionChange(object sender, Microsoft.Office.Tools.Word.SelectionEventArgs e)
        {
            try
            {
                WordTools.Document vstoDoc = Globals.Factory.GetVstoObject(Application.ActiveDocument);
                
                if (e.Selection.Text.Length >= 2)
                {
                    SelectionState selState = new SelectionState();
                    selState.SelectionDocName = e.Selection.Application.ActiveDocument.Name;
                    selState.SelectionPage = e.Selection.Information[WordInterop.WdInformation.wdActiveEndAdjustedPageNumber].ToString();
                    selState.SelectionText = e.Selection.Text;
                    selState.srcType = SrcType.WORD;
                    //ModelEventArgs mea = 
                    ZDFSingleton_ModelPropertyChanged(this, new ModelEventArgs("WordEntry", selState));
                    //System.Windows.Forms.MessageBox.Show("Thingie Fired");
                }

            }
            
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.GetType().ToString() + '\n' + ex.Message);
            }
            
        }

        private void OnWordFired(ZaveGlobalSettings.Data_Structures.SelectionState selDat)
        {

            ZaveSrc src = selDat;           


            EventHandler<WordEventArgs> handler = WordFired;
            if (handler != null)
            {
                WordEventArgs wea = new WordEventArgs(src);
                
                handler(this, wea);
            }
        }


        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
