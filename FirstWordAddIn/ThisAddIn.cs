using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using WordInterop = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using WordTools = Microsoft.Office.Tools.Word;

namespace FirstWordAddIn
{
    public partial class ThisAddIn
    {
        public event EventHandler WordFired;
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            this.Application.DocumentOpen +=
            new WordInterop.ApplicationEvents4_DocumentOpenEventHandler(WorkWithDocument);

            ((WordInterop.ApplicationEvents4_Event)this.Application).NewDocument +=
                new WordInterop.ApplicationEvents4_NewDocumentEventHandler(WorkWithDocument);
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        void Application_DocumentBeforeSave(WordInterop.Document Doc, ref bool SaveAsUI, ref bool Cancel)
        {
            Doc.Paragraphs[1].Range.InsertParagraphBefore();
            Doc.Paragraphs[1].Range.Text = "This text was added by using code. ";
        }

        private void WorkWithDocument(Microsoft.Office.Interop.Word.Document Doc)
        {
            WordTools.Document vstoDoc = Globals.Factory.GetVstoObject(this.Application.ActiveDocument);
            vstoDoc.SelectionChange += new Microsoft.Office.Tools.Word.SelectionEventHandler(ThisDocument_SelectionChange);
        }

        private void DocumentSelectionChange()
        {
            WordTools.Document vstoDoc = Globals.Factory.GetVstoObject(this.Application.ActiveDocument);
            vstoDoc.SelectionChange += new Microsoft.Office.Tools.Word.SelectionEventHandler(ThisDocument_SelectionChange);
        }

        void ThisDocument_SelectionChange(object sender, Microsoft.Office.Tools.Word.SelectionEventArgs e)
        {
            try
            {
                WordTools.Document vstoDoc = Globals.Factory.GetVstoObject(this.Application.ActiveDocument);
                
                if (e.Selection.Text.Length >= 2)
                {
                    DataStructures.SelectionData selDat = new DataStructures.SelectionData();
                    selDat.SelectionDocName = e.Selection.Application.ActiveDocument.Name;
                    selDat.SelectionPage = e.Selection.Information[WordInterop.WdInformation.wdActiveEndAdjustedPageNumber];
                    selDat.SelectionText = e.Selection.Text;
                    
                }

            }
            
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.GetType().ToString() + '\n' + ex.Message);
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
