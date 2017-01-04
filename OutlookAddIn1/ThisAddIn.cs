using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
//using System.Text.RegularExpressions;
using System.ComponentModel;
using OutlookInterop = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using OutlookTools = Microsoft.Office.Tools.Outlook;
using ZaveGlobalSettings.Data_Structures;
using ZaveGlobalSettings.ZaveFile;

namespace OutlookAddIn1
{
    public partial class ThisAddIn
    {

        OutlookInterop.Inspectors _inspectors;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {

            _inspectors = this.Application.Inspectors;
            _inspectors.NewInspector += new Microsoft.Office.Interop.Outlook.InspectorsEvents_NewInspectorEventHandler(Inspectors_NewInspector);
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            // Note: Outlook no longer raises this event. If you have code that 
            //    must run when Outlook shuts down, see http://go.microsoft.com/fwlink/?LinkId=506785
        }


        private void Inspectors_NewInspector(OutlookInterop.Inspector inspector)
        {
            OutlookInterop.MailItem mailItem = inspector.CurrentItem as OutlookInterop.MailItem;

            if(mailItem != null)
            {
                if(mailItem.EntryID == null)
                {
                    mailItem.Subject = "This text was added by using code";
                    mailItem.Body = "This text was added by using code";

                    
                }
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
