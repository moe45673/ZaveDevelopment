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
using System.Runtime.InteropServices;
using ZaveGlobalSettings.ZaveResources;



//using ZaveSrc = ZaveGlobalSettings.Data_Structures.SelectionState;

namespace FirstWordAddIn
{
    
    using resourceCursor = ZaveGlobalSettings.Properties.Resources;

    



    public partial class ThisAddIn
    {


        RichTextBox rtb;
        IZaveLowLevelHook llHook;
        //Running under ZaveSourceAdapter, listener for all highlights from all possible sources
        //ZDFSingleton activeZDF = ZDFSingleton.Instance;

        

        

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="fileName"></param>
        ///// <returns></returns>
        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        //public static extern IntPtr LoadCursorFromFile(string fileName);

        //Cursor myCursor;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            //TODO
            llHook = new ZaveCursorHook();

            llHook.Init();
            llHook.Start();
            //MouseHook.MouseAction += new EventHandler(Captured);
            //only start listening for the event when a document is opened or created
            this.Application.DocumentOpen +=
            new WordInterop.ApplicationEvents4_DocumentOpenEventHandler(DocumentSelectionChange);

            ((WordInterop.ApplicationEvents4_Event)this.Application).NewDocument +=
                new WordInterop.ApplicationEvents4_NewDocumentEventHandler(DocumentSelectionChange);

            this.Application.WindowDeactivate += new WordInterop.ApplicationEvents4_WindowDeactivateEventHandler(Deactivated);
            this.Application.WindowActivate += new WordInterop.ApplicationEvents4_WindowActivateEventHandler(Activated);


            rtb = new RichTextBox();

            

        }

        
        
        
        





        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            llHook.Stop();
            llHook.Dispose(); 
        }
        
        /// <summary>
        /// Get the current doc and pass it to the event handler
        /// </summary>
        /// <param name="Doc"></param>
        private void DocumentSelectionChange(Microsoft.Office.Interop.Word.Document Doc)
        {

            WordTools.Document vstoDoc = Globals.Factory.GetVstoObject(this.Application.ActiveDocument);
            vstoDoc.SelectionChange += new Microsoft.Office.Tools.Word.SelectionEventHandler(ThisDocument_SelectionChange);
            //WordInterop.Application app = vstoDoc.Application;
            //WordInterop.WdCursorType oldCursor = app.System.Cursor;
            //WordInterop.Range range = vstoDoc.Range(ref missing, ref missing);

            //try
            //{
            //    app.System.Cursor = WordInterop.WdCursorType.wdCursorWait;
            //    Random r = new Random();
            //    for (int i = 1; i < 1000; i++)
            //    {
            //        range.Text = range.Text + r.NextDouble().ToString();
            //    }
            //}
            //finally
            //{
            //    app.System.Cursor = oldCursor;
            //}
        }

        

        private void Activated(WordInterop.Document Doc, WordInterop.Window Wn) { llHook.Start(); }
        private void Deactivated(WordInterop.Document Doc, WordInterop.Window Wn) { llHook.Stop(); }


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

        //public void OnWordFired(SelectionState selState)
        //{
        //    var handler = WordFired;
        //    if (handler != null)
        //        handler(this, new SrcEventArgs(selState));
        //}

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
