﻿using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZaveController.ZDFSource;
using ZaveGlobalSettings.Data_Structures;
//using ZaveService.ZDFEntry;
using WordInterop = Microsoft.Office.Interop.Word;
using Microsoft.Win32.SafeHandles;
using Office = Microsoft.Office.Core;
using WordTools = Microsoft.Office.Tools.Word;
using System.Runtime.InteropServices;
using ZaveGlobalSettings.ZaveFile;
//using ZaveViewModel.Commands;
using System.IO;
using Newtonsoft.Json;
//using ZaveMo

namespace ZaveController.Global_Settings
{

    
    
    public sealed class EventInitSingleton : IDisposable
    {

        bool disposed = false;
        public ZaveModel.ZDF.ZDFSingleton activeZDF;
        private static DateTime lastRead;

        private static readonly EventInitSingleton instance = new EventInitSingleton();
        private FileSystemWatcher watcher;
        
        //public ZDFEntryHandler zdfEntryHandler { get; set; }

        //SafeHandle handle = new SafeFileHandle(, true);
        

        private EventInitSingleton()
        {
            
            activeZDF = ZaveModel.ZDF.ZDFSingleton.Instance;
            CreateFileWatcher(Path.GetTempPath());
            lastRead = DateTime.MinValue;
            //System.Windows.Forms.MessageBox.Show("EventInit Started!");
            //ThisAddIn.WordFired += new EventHandler<SrcEventArgs>(SrcHighlightEventHandler);
        }

        //private ~EventInitSingleton()
        //{
        //    Dispose();
        //}

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                watcher.Dispose();
            }

            disposed = true;
        }

        public static EventInitSingleton Instance
        {
            get
            {                
                return instance;
            }
        }

        public void CreateFileWatcher(string path)
        {
            // Create a new FileSystemWatcher and set its properties.
            watcher = new FileSystemWatcher();
            watcher.Path = path;
            /* Watch for changes in LastAccess and LastWrite times, and 
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.

            watcher.Filter = GuidGenerator.getGuid();

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            //watcher.Created += new FileSystemEventHandler(OnChanged);
            //watcher.Deleted += new FileSystemEventHandler(OnChanged);
            //watcher.Renamed += new RenamedEventHandler(OnRenamed);

            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }

        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e)
        {


            DateTime lastWriteTime = File.GetLastWriteTime(e.FullPath);
            if (lastWriteTime != lastRead)
            {
                // Specify what is done when a file is changed, created, or deleted.
                ZaveModel.ZDFEntry.ZDFEntry entry = new ZaveModel.ZDFEntry.ZDFEntry();

            //List<SelectionState> _selState = new List<SelectionState>();
            //SelectionState selState = new SelectionState();

            //_selState.Add(JsonConvert.DeserializeObject<SelectionState>(File.ReadAllText(e.FullPath)));
            //SelectionState temp = new SelectionState>();
                using (StreamReader sr = StreamReaderFactory.createStreamReader(e.FullPath))
                {
                    try
                    {
                        string json = sr.ReadToEnd();

                        var temp = JsonConvert.DeserializeObject<SelectionState[]>(json).ToList<SelectionState>();
                    
                        if (temp.Any<SelectionState>())
                        {
                            entry.Source = temp[0];

                            ZaveModel.ZDF.ZDFSingleton activeZDF = ZaveModel.ZDF.ZDFSingleton.Instance;

                            activeZDF.Add(entry);
                        }
                        sr.Close();
                    }
                    catch (IOException ex)
                    {
                        throw ex;
                    }
                }
            lastRead = lastWriteTime;

            }



        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        }


        //public void SrcHighlightEventHandler(Object o, SrcEventArgs e)
        //{



        //    ZaveModel.ZDFEntry.ZDFEntry entry = new ZaveModel.ZDFEntry.ZDFEntry();

        //    entry.Source = e.zSrc;
        //    activeZDF.Add(entry);
            //zdfEntryHandler = new ZaveService.ZDFEntry.DefaultZDFEntryHandler(e.zSrc, activeZDF);
            //zdfEntryHandler.CreateZDFEntry(new ZaveModel.ZDFEntry.ZDFEntry(e.zSrc));
//            entry.Source = e.zSrc;

//            _saveZDFEntryCommand = new RelayCommand(param => SaveZDFEntry(entry)
//, param => (entry != null));

           
           // activeZDF.Add(entry);

            
            //zevm.TxtDocName = activeZDF.EntryList[activeZDF.EntryList.Count - 1].Source.SelectionDocName;
            
            
            
        //}

        


    }

    

    
}
