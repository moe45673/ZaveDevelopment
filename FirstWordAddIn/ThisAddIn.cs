using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
//using System.Text.RegularExpressions;
using System.ComponentModel;
using WordInterop = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using WordTools = Microsoft.Office.Tools.Word;
using FirstWordAddIn.DataStructures;
using ZaveGlobalSettings.Data_Structures;
using ZaveGlobalSettings.ZaveFile;
using System.Windows.Forms;
using System.Threading.Tasks;



//using ZaveSrc = ZaveGlobalSettings.Data_Structures.SelectionState;

namespace FirstWordAddIn
{

    
    public partial class ThisAddIn
    {

        RichTextBox rtb;

        //Running under ZaveSourceAdapter, listener for all highlights from all possible sources
        //ZDFSingleton activeZDF = ZDFSingleton.Instance;

        public static event EventHandler<SrcEventArgs> WordFired;


        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            //only start listening for the event when a document is opened or created
            this.Application.DocumentOpen +=
            new WordInterop.ApplicationEvents4_DocumentOpenEventHandler(DocumentSelectionChange);

            ((WordInterop.ApplicationEvents4_Event)this.Application).NewDocument +=
                new WordInterop.ApplicationEvents4_NewDocumentEventHandler(DocumentSelectionChange);

            rtb = new RichTextBox();
            
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


        async void ThisDocument_SelectionChange(object sender, Microsoft.Office.Tools.Word.SelectionEventArgs e)
        {
            try
            {
                WordTools.Document vstoDoc = Globals.Factory.GetVstoObject(Application.ActiveDocument);
                //_selStateList = new List<SelectionState>();
                
                if (e.Selection.Text.Length >= 2)
                {
                    List<SelectionState> _selStateList = new List<SelectionState>();

                    rtb.Clear();
                    e.Selection.Copy();
                    rtb.Paste();

                    _selStateList.Add(new SelectionState()
                    {
                        SelectionDocName = e.Selection.Application.ActiveDocument.Name,
                        SelectionPage = e.Selection.Information[WordInterop.WdInformation.wdActiveEndAdjustedPageNumber].ToString(),                        
                        SelectionText = rtb.Rtf,
                        SelectionDateModified = DateTime.Now,
                        srcType = SrcType.WORD
                    });

                    //System.Windows.Forms.MessageBox.Show(rtb.Rtf);

                    //#if DEBUG
                    //System.Windows.Forms.MessageBox.Show(default(DateTime).ToString());
                    //#endif

                    string projFile = System.IO.Path.GetTempPath() + GuidGenerator.getGuid();

                    WriteToJsonFile(projFile, _selStateList);
                    
                        
                   
                   

                }

            }
            
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.GetType().ToString() + '\n' + ex.Message);
            }
            
        }

        public void OnWordFired(SelectionState selState)
        {
            var handler = WordFired;
            if (handler != null)
                handler(this, new SrcEventArgs(selState));
        }

        private async Task WriteToJsonFile(string filename, List<SelectionState> selStateList)
        {

            await Task.Run(() =>
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(selStateList.ToArray());

                //#if DEBUG
                //                    System.Windows.Forms.MessageBox.Show("After Serialization");
                //#endif
                //OnWordFired(selState);         

                //string projFile = System.IO.Path.GetTempPath() + "ZavePrototype";
                //System.Windows.Forms.MessageBox.Show(projDir);


                using (StreamWriter sw = StreamWriterFactory.createStreamWriter(filename))
                {
                    try
                    {
                        //sw.BaseStream.Seek(0, SeekOrigin.End);
                        sw.Write(json);

                        sw.Close();
                    }
                    catch (IOException ex)
                    {
                        throw ex;
                    }
                }
            });

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
