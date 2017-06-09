﻿///////////////////////////////////////////////////////////
//  App.cs
//  Implementation of the Class App
//  Generated by Enterprise Architect
//  Created on:      28-Mar-2016 9:30:09 AM
//  Original author: Moshe
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows;
//using ZaveViewModel.ViewModels;
using ZaveGlobalSettings.ZaveFile;
using Prism.Events;
using Microsoft.Practices.Unity;
using ZaveController;
using ZaveGlobalSettings.Data_Structures;
using System.Drawing;
using System.Threading.Tasks;
using ZaveService.IOService;
using Zave.Properties;

namespace Zave
{

    
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class ZaveApp : Application
    {


        //private Views.MainWindow win;
        //private bool mRequestClose = false;
        //public EventInitSingleton eventInit;


        /// <summary>
        /// Runs Init() Method
        /// </summary>
        public ZaveApp()
        {
            //Init();
            //MainWindow app = new MainWindow();
            //ZDFEntryViewModel viewModel = new ZDFEntryViewModel();
            //app.DataContext = viewModel;
            //app.Show();


        }

        
        /// <summary>
        /// 
        /// </summary>
        ~ZaveApp()
        {

            //if(eventInit != null)
            //    eventInit.Dispose();

            string projFile = System.IO.Path.GetTempPath() + APIFileNames.SourceToZave;
            string broadcastFile = Path.GetTempPath() + APIFileNames.ZaveToSource;
            //string projFile = System.IO.Path.GetTempPath() + "ZavePrototype";
            IOService.DeleteFile(projFile);
            IOService.DeleteFile(broadcastFile);
            Settings.Default.Save();
            
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {

            //var txt = "";
            
            var bs = new BootStrapper();
            
            bs.Run(e);

           // win = new Views.MainWindow();

            

            //var eventAgg = bs.Container.Resolve(typeof(IEventAggregator)) as EventAggregator;
            //var activeZDF = bs.Container.Resolve(typeof(ZaveModel.ZDF.ZDFSingleton));//
            //eventInit = EventInitSingleton.GetInstance(eventAgg, bs.Container);
            string projFile = System.IO.Path.GetTempPath() + APIFileNames.SourceToZave;
            string broadcastFile = Path.GetTempPath() + APIFileNames.ZaveToSource;
            //string projFile = System.IO.Path.GetTempPath() + "ZavePrototype";
            await IOService.CreateFileAsync(projFile);
            await IOService.CreateFileAsync(broadcastFile);
            

            

        }

       
    }//end App

   

}//end namespace ZaveProject